import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { useColorModeValue, Card, useDisclosure } from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';

import ConfirmDialog from 'components/dialog/ConfirmDialog';
import Pagination from 'components/pagination/Pagination';
import VehicleService from 'services/VehicleService';
import VehicleModelService from 'services/VehicleModelService';

import Form from './Form';
import Header from './Header';
import List from './List';
import Columns from './Columns';

export default function Table() {
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
  const [selectedBulk, setSelectedBulk] = useState([]);
  const [editingVehicle, setEditingVehicle] = useState(null);

  const { isOpen, onOpen, onClose } = useDisclosure();
  const {
    isOpen: isConfirmOpen,
    onOpen: onConfirmOpen,
    onClose: onConfirmClose,
  } = useDisclosure();

  const loadVehicles = useCallback(
    async (p = 1) => {
      try {
        const res = await VehicleService.search({
          pageNumber: p,
          pageSize: 10,
          searchTerm: searchInput,
        });
        setVehicles(res.data || []);
        setTotalPages(res.totalPages || 1);
      } catch (err) {
        console.error(err);
        toast.error('Failed to load vehicles');
      }
    },
    [searchInput, toast],
  );

  const loadModels = useCallback(async () => {
    try {
      const res = await VehicleModelService.search({
        pageNumber: 1,
        pageSize: 100,
      });
      setModels(res.data || []);
    } catch (err) {
      console.error(err);
    }
  }, []);

  useEffect(() => {
    loadVehicles(page);
  }, [page, loadVehicles]);

  useEffect(() => {
    loadModels();
  }, [loadModels]);

  const confirmDelete = async () => {
    if (!selectedToDelete) return;
    try {
      await VehicleService.delete(selectedToDelete.vehicleId);
      loadVehicles(page);
      toast.success('Vehicle deleted successfully');
    } catch (err) {
      toast.error('Error deleting vehicle');
    } finally {
      setSelectedToDelete(null);
      onConfirmClose();
    }
  };

  const confirmBulkDelete = async () => {
    if (selectedBulk.length === 0) return;
    try {
      await VehicleService.bulkDelete(selectedBulk);
      loadVehicles(page);
      toast.success('Vehicles deleted successfully');
      setSelectedBulk([]);
    } catch {
      toast.error('Bulk delete failed');
    }
  };

  const columns = useMemo(
    () =>
      Columns({
        onEdit: (v) => {
          setEditingVehicle(v);
          onOpen();
        },
        onDelete: (v) => {
          setSelectedToDelete(v);
          onConfirmOpen();
        },
        selectedBulk,
        setSelectedBulk,
      }),
    [onOpen, onConfirmOpen, selectedBulk],
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
          onBulkDelete={confirmBulkDelete}
          hasSelected={selectedBulk.length > 0}
          textColor={textColor}
        />
        <List table={table} borderColor={borderColor} textColor={textColor} />
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
            ? `Are you sure you want to delete vehicle "${selectedToDelete.vin}"?`
            : 'Are you sure you want to delete this vehicle?'
        }
      />
    </>
  );
}
