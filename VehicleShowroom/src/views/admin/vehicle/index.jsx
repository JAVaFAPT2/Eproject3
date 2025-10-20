import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { useColorModeValue, Card, useDisclosure } from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import { useAppToast } from 'utils/ToastHelper';

import ConfirmDialog from 'components/dialog/ConfirmDialog';
import Pagination from 'components/pagination/Pagination';
import VehicleService from 'services/VehicleService';
import VehicleModelService from 'services/VehicleModelService';

import Form from './components/Form';
import Header from './components/Header';
import List from './components/List';
import Columns from './components/Columns';

function VehicleManagement() {
  const toast = useAppToast();
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');
  const borderColor = useColorModeValue('gray.200', 'navy.700');

  const [vehicles, setVehicles] = useState([]);
  const [models, setModels] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [searchInput, setSearchInput] = useState('');
  const [selectedToDelete, setSelectedToDelete] = useState(null);
  const [editingVehicle, setEditingVehicle] = useState(null);
  const [statusFilter, setStatusFilter] = useState(null);
  const [modelFilter, setModelFilter] = useState(null);

  const [loading, setLoading] = useState(false);
  const [modelsLoading, setModelsLoading] = useState(true);

  const { isOpen, onOpen, onClose } = useDisclosure();
  const {
    isOpen: isConfirmOpen,
    onOpen: onConfirmOpen,
    onClose: onConfirmClose,
  } = useDisclosure();

  // 🚀 Load danh sách model
  const loadModels = useCallback(async () => {
    try {
      setModelsLoading(true);
      const res = await VehicleModelService.get({
        pageNumber: 1,
        pageSize: 100,
      });
      setModels(res.items || []);
    } catch (err) {
      console.error('Failed to load models:', err);
      toast.error('Failed to load vehicle models');
    } finally {
      setModelsLoading(false);
    }
  }, []);

  // 🚗 Load danh sách vehicle (có filter)
  const loadVehicles = useCallback(
    async (p = 1) => {
      try {
        setLoading(true);
        const params = { pageNumber: p, pageSize: 10 };
        if (searchInput?.trim()) params.searchTerm = searchInput.trim();
        if (modelFilter) params.modelNumber = modelFilter;
        if (statusFilter) params.status = statusFilter;

        const res = await VehicleService.get(params);
        const list = res.items || [];

        const modelMap = {};
        models.forEach((m) => {
          modelMap[m.modelNumber] = m.name;
        });

        const withModelNames = list.map((v) => ({
          ...v,
          modelName: modelMap[v.modelNumber] || v.modelNumber || 'Unknown',
        }));

        setVehicles(withModelNames);
        setTotalPages(res.totalPages || 1);
      } catch (err) {
        toast.error('Failed to load vehicles');
      } finally {
        setLoading(false);
      }
    },
    [searchInput, modelFilter, statusFilter, models],
  );

  // 🕐 Debounce search + filter
  useEffect(() => {
    const delay = setTimeout(() => {
      if (models.length > 0) loadVehicles(page);
    }, 500);
    return () => clearTimeout(delay);
  }, [page, searchInput, modelFilter, statusFilter, loadVehicles, models]);

  useEffect(() => {
    loadModels();
  }, [loadModels]);

  const confirmDelete = async () => {
    if (!selectedToDelete) return;
    try {
      setLoading(true);
      await VehicleService.delete(selectedToDelete.vehicleId);
      await loadVehicles(page);
      toast.success('Vehicle deleted successfully');
    } catch {
      toast.error('Error deleting vehicle');
    } finally {
      setSelectedToDelete(null);
      onConfirmClose();
      setLoading(false);
    }
  };

  const columns = useMemo(
    () =>
      Columns({
        models,
        statusFilter,
        setStatusFilter,
        modelFilter,
        setModelFilter,
        onEdit: (v) => {
          setEditingVehicle(v);
          onOpen();
        },
        onDelete: (v) => {
          setSelectedToDelete(v);
          onConfirmOpen();
        },
      }),
    [models, statusFilter, modelFilter, onOpen, onConfirmOpen],
  );

  const table = useReactTable({
    data: vehicles,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  const isLoading = loading || modelsLoading;

  return (
    <>
      <Form
        isOpen={isOpen}
        onClose={() => {
          setEditingVehicle(null);
          onClose();
        }}
        reloadVehicles={() => loadVehicles(page)}
        vehicle={editingVehicle}
        models={models}
        bgColor={bgColor}
        textColor={textColor}
      />

      <Card
        flexDirection="column"
        w="100%"
        borderRadius="16px"
        boxShadow="md"
        bg={bgColor}
      >
        <Header
          searchInput={searchInput}
          setSearchInput={setSearchInput}
          onAdd={() => {
            setEditingVehicle(null);
            onOpen();
          }}
          textColor={textColor}
        />
        <List
          table={table}
          borderColor={borderColor}
          textColor={textColor}
          loading={isLoading}
        />
      </Card>

      {totalPages > 1 && (
        <Pagination
          page={page}
          totalPages={totalPages}
          onPageChange={setPage}
        />
      )}

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={onConfirmClose}
        onConfirm={confirmDelete}
        title="Delete Vehicle"
        message={
          selectedToDelete
            ? `Are you sure you want to delete vehicle "${selectedToDelete.vehicleId}"?`
            : 'Are you sure you want to delete this vehicle?'
        }
      />
    </>
  );
}

export default VehicleManagement;
