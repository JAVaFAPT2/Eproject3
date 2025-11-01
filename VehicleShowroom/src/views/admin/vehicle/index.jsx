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

  const { isOpen, onOpen, onClose } = useDisclosure();
  const {
    isOpen: isConfirmOpen,
    onOpen: onConfirmOpen,
    onClose: onConfirmClose,
  } = useDisclosure();

  // 🧠 Load models 1 lần, và vehicles mỗi khi filter/search/page thay đổi
  useEffect(() => {
    let abort = false;

    const loadData = async () => {
      try {
        setLoading(true);

        // 🔹 Load models 1 lần duy nhất
        let modelsData = models;
        if (modelsData.length === 0) {
          const resModels = await VehicleModelService.get({
            pageNumber: 1,
            pageSize: 100,
          });
          modelsData = resModels.items || [];
          if (!abort) setModels(modelsData);
        }

        // 🔹 Luôn load vehicles theo filter/search
        const params = { pageNumber: page, pageSize: 10 };
        if (searchInput?.trim()) params.searchTerm = searchInput.trim();
        if (modelFilter) params.modelNumber = modelFilter;
        if (statusFilter) params.status = statusFilter;

        const resVehicles = await VehicleService.get(params);
        const vehiclesData = resVehicles.items || [];

        const modelMap = {};
        modelsData.forEach((m) => {
          modelMap[m.modelNumber] = m.name;
        });

        const withModelNames = vehiclesData.map((v) => ({
          ...v,
          modelName: modelMap[v.modelNumber] || v.modelNumber || 'Unknown',
        }));

        if (!abort) {
          setVehicles(withModelNames);
          setTotalPages(resVehicles.totalPages || 1);
        }
      } catch (err) {
        if (!abort) {
          console.error('Failed to load vehicles:', err);
          toast.error('Failed to load vehicle data');
        }
      } finally {
        if (!abort) setLoading(false);
      }
    };

    loadData();

    return () => {
      abort = true;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page, searchInput, modelFilter, statusFilter]);

  // 🗑️ Confirm delete
  const confirmDelete = useCallback(async () => {
    if (!selectedToDelete) return;
    try {
      setLoading(true);
      await VehicleService.delete(selectedToDelete.vehicleId);

      const params = { pageNumber: page, pageSize: 10 };
      const res = await VehicleService.get(params);
      setVehicles(res.items || []);
      toast.success('Vehicle deleted successfully');
    } catch {
      toast.error('Error deleting vehicle');
    } finally {
      setSelectedToDelete(null);
      onConfirmClose();
      setLoading(false);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [selectedToDelete, page, onConfirmClose]);

  // 🧩 Columns config
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

  return (
    <>
      <Form
        isOpen={isOpen}
        onClose={() => {
          setEditingVehicle(null);
          onClose();
        }}
        reloadVehicles={() => {
          const params = { pageNumber: page, pageSize: 10 };
          VehicleService.get(params).then((res) =>
            setVehicles(res.items || []),
          );
        }}
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
          loading={loading}
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
