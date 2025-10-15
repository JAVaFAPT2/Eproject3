import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { useColorModeValue, Card, useDisclosure } from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';

import ConfirmDialog from 'components/dialog/ConfirmDialog';
import Pagination from 'components/pagination/Pagination';
import VehicleModelService from 'services/VehicleModelService';

import Form from './Form';
import Header from './Header';
import List from './List';
import Columns from './Columns';

export default function Table() {
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const headerBg = useColorModeValue('gray.100', 'navy.800');
  const bgColor = useColorModeValue('white', 'navy.800');
  const toast = useAppToast();

  const [models, setModels] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [searchInput, setSearchInput] = useState('');
  const [editingModel, setEditingModel] = useState(null);
  const [parentModel, setParentModel] = useState(null);
  const [selectedToDelete, setSelectedToDelete] = useState(null);
  const [expandedRows, setExpandedRows] = useState({});

  const { isOpen, onOpen, onClose } = useDisclosure();
  const {
    isOpen: isConfirmOpen,
    onOpen: onConfirmOpen,
    onClose: onConfirmClose,
  } = useDisclosure();

  const toggleExpand = (id) => {
    setExpandedRows((prev) => ({ ...prev, [id]: !prev[id] }));
  };

  const loadModels = useCallback(
    async (pageNum = 1) => {
      try {
        const res = await VehicleModelService.search({
          pageNumber: pageNum,
          pageSize: 50,
        });
        setModels(res.data || []);
        setTotalPages(res.totalPages || 1);
      } catch (err) {
        console.error(err);
        toast.error('Failed to load vehicle models');
      }
    },
    [toast],
  );

  useEffect(() => {
    loadModels(page);
  }, [page, loadModels]);

  const buildTree = (flat) => {
    const map = {};
    flat.forEach((m) => (map[m.modelNumber] = { ...m, children: [] }));
    const roots = [];
    flat.forEach((m) => {
      if (m.parentId) map[m.parentId]?.children.push(map[m.modelNumber]);
      else roots.push(map[m.modelNumber]);
    });
    return roots;
  };

  const treeData = useMemo(() => {
    let data = models;
    if (searchInput) {
      data = models.filter((m) =>
        m.name.toLowerCase().includes(searchInput.toLowerCase()),
      );
    }
    return buildTree(data);
  }, [models, searchInput]);

  const confirmDelete = async () => {
    if (!selectedToDelete) return;
    try {
      // nếu BE có softDelete, đổi lại ở đây
      await VehicleModelService.update(selectedToDelete.modelNumber, {
        active: false,
      });
      loadModels(page);
      setSelectedToDelete(null);
      toast.success('Model deleted successfully');
    } catch (err) {
      console.error(err);
      toast.error('Error deleting model');
    } finally {
      onConfirmClose();
    }
  };

  const columns = useMemo(
    () =>
      Columns({
        onEdit: (model) => {
          setEditingModel(model);
          setParentModel(null);
          onOpen();
        },
        onAdd: (model) => {
          setParentModel(model);
          setEditingModel(null);
          onOpen();
        },
        onDelete: (model) => {
          setSelectedToDelete({
            modelNumber: model.modelNumber,
            name: model.name,
          });
          onConfirmOpen();
        },
        toggleExpand,
        expandedRows,
      }),
    [onOpen, onConfirmOpen, expandedRows],
  );

  const table = useReactTable({
    data: treeData,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <>
      <Form
        isOpen={isOpen}
        onClose={() => {
          setEditingModel(null);
          setParentModel(null);
          onClose();
        }}
        reloadModels={() => loadModels(page)}
        model={editingModel}
        parentModel={parentModel}
        textColor={textColor}
        bgColor={bgColor}
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
            setParentModel(null);
            setEditingModel(null);
            onOpen();
          }}
          textColor={textColor}
        />
        <List
          table={table}
          treeData={treeData}
          expandedRows={expandedRows}
          toggleExpand={toggleExpand}
          onAdd={(m) => {
            setParentModel(m);
            setEditingModel(null);
            onOpen();
          }}
          onEdit={(m) => {
            setEditingModel(m);
            setParentModel(null);
            onOpen();
          }}
          onDelete={(m) => {
            setSelectedToDelete({ modelNumber: m.modelNumber, name: m.name });
            onConfirmOpen();
          }}
          textColor={textColor}
          bgColor={bgColor}
          borderColor={borderColor}
          headerBg={headerBg}
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
        title="Delete Vehicle Model"
        message={
          selectedToDelete
            ? `Are you sure you want to delete model "${selectedToDelete.name}"?`
            : 'Are you sure you want to delete this model?'
        }
      />
    </>
  );
}
