import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { useColorModeValue, Card, useDisclosure } from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';

import ConfirmDialog from 'components/dialog/ConfirmDialog';
import ImagePreview from 'components/images/ImagePreview';
import Pagination from 'components/pagination/Pagination';
import VehicleModelService from 'services/VehicleModelService';
import VehiclePhotoService from 'services/VehiclePhotoService';
import VehicleSpecService from 'services/VehicleSpecService';

import ModelForm from './ModelForm';
import SpecForm from './SpecForm';
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
  const [isPreviewOpen, setIsPreviewOpen] = useState(false);
  const [previewImages, setPreviewImages] = useState([]);
  const [previewIndex, setPreviewIndex] = useState(0);
  const [isSpecFormOpen, setIsSpecFormOpen] = useState(false);
  const [selectedSpecModel, setSelectedSpecModel] = useState(null);

  const { isOpen, onOpen, onClose } = useDisclosure();
  const {
    isOpen: isConfirmOpen,
    onOpen: onConfirmOpen,
    onClose: onConfirmClose,
  } = useDisclosure();

  const toggleExpand = useCallback(async (modelNumber) => {
    setExpandedRows((prev) => {
      const newState = { ...prev, [modelNumber]: !prev[modelNumber] };
      return newState;
    });

    const model = models.find((m) => m.modelNumber === modelNumber);
    if (model && model.level === 2 && !model.specs) {
      try {
        const specs = await VehicleSpecService.getByModelNumber(
          model.modelNumber,
        );
        setModels((prev) =>
          prev.map((m) =>
            m.modelNumber === model.modelNumber ? { ...m, specs } : m,
          ),
        );
      } catch (err) {
        console.error('Failed to load specs:', err);
        toast.error('Failed to load specifications');
      }
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handlePreview = useCallback(async (model, index = 0) => {
    try {
      const photos = await VehiclePhotoService.getByModelNumber(
        model.modelNumber,
      );
      const urls = photos.map((p) => p.url || p.photoUrl || p.path);
      setPreviewImages(urls);
      setPreviewIndex(index);
      setIsPreviewOpen(true);
    } catch (err) {
      console.error('Failed to load model photos:', err);
      toast.error('Failed to load images');
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const handleAddSpec = (model) => {
    setSelectedSpecModel(model);
    setIsSpecFormOpen(true);
  };

  const loadModels = useCallback(async (pageNum = 1) => {
    try {
      const res = await VehicleModelService.get({
        pageNumber: pageNum,
        pageSize: 50,
      });
      setModels(res.vehicleModels || []);
      setTotalPages(res.totalPages || 1);
    } catch (err) {
      console.error(err);
      toast.error('Failed to load vehicle models');
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

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
        onAddSpec: handleAddSpec,
        onPreview: handlePreview,
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
    [onOpen, onConfirmOpen, expandedRows, handlePreview, toggleExpand],
  );

  const table = useReactTable({
    data: treeData,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <>
      <ModelForm
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

      <SpecForm
        isOpen={isSpecFormOpen}
        onClose={() => setIsSpecFormOpen(false)}
        model={selectedSpecModel}
      />

      <ImagePreview
        isOpen={isPreviewOpen}
        onClose={() => setIsPreviewOpen(false)}
        images={previewImages}
        initialIndex={previewIndex}
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
          onAddSpec={handleAddSpec}
          onEdit={(m) => {
            setEditingModel(m);
            setParentModel(null);
            onOpen();
          }}
          onPreview={handlePreview}
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
