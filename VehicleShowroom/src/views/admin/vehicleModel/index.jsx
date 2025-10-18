import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { useColorModeValue, Card, useDisclosure } from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';

import { LoadingState } from 'components/common/LoadingState';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import ImagePreview from 'components/images/ImagePreview';
import Pagination from 'components/pagination/Pagination';
import VehicleModelService from 'services/VehicleModelService';
import VehiclePhotoService from 'services/VehiclePhotoService';
import VehicleSpecService from 'services/VehicleSpecService';

import ModelForm from './components/ModelForm';
import SpecForm from './components/SpecForm';
import Header from './components/Header';
import List from './components/List';
import Columns from './components/Columns';

function VehicleModelManagement() {
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const headerBg = useColorModeValue('gray.100', 'navy.800');
  const bgColor = useColorModeValue('white', 'navy.800');
  const toast = useAppToast();

  const [models, setModels] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [editingModel, setEditingModel] = useState(null);
  const [parentModel, setParentModel] = useState(null);
  const [selectedToDelete, setSelectedToDelete] = useState(null);
  const [expandedRows, setExpandedRows] = useState({});
  const [isPreviewOpen, setIsPreviewOpen] = useState(false);
  const [previewImages, setPreviewImages] = useState([]);
  const [previewIndex, setPreviewIndex] = useState(0);
  const [isSpecFormOpen, setIsSpecFormOpen] = useState(false);
  const [selectedSpecModel, setSelectedSpecModel] = useState(null);
  const [editingSpec, setEditingSpec] = useState(null);
  const [loading, setLoading] = useState(false);

  const { isOpen, onOpen, onClose } = useDisclosure();
  const {
    isOpen: isConfirmOpen,
    onOpen: onConfirmOpen,
    onClose: onConfirmClose,
  } = useDisclosure();

  const toggleExpand = useCallback(
    async (modelNumber) => {
      setExpandedRows((prev) => ({
        ...prev,
        [modelNumber]: !prev[modelNumber],
      }));

      const targetModel = models.find((m) => m.modelNumber === modelNumber);

      if (
        targetModel &&
        targetModel.level === 2 &&
        (!targetModel.specs || targetModel.specs.length === 0)
      ) {
        try {
          const specs = await VehicleSpecService.getByModelNumber(modelNumber);

          setModels((prev) =>
            prev.map((m) =>
              m.modelNumber === modelNumber ? { ...m, specs: specs.items } : m,
            ),
          );
        } catch (err) {
          console.error('Failed to load specs:', err);
          toast.error('Failed to load specifications');
        }
      }
    },
    [models],
  );

  // 🖼 Preview images
  const handlePreview = useCallback(
    async (model, index = 0) => {
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
    },
    [toast],
  );

  // ➕ Add spec
  const handleAddSpec = (model) => {
    setSelectedSpecModel(model);
    setEditingSpec(null);
    setIsSpecFormOpen(true);
  };

  // ✏️ Edit spec
  const handleEditSpec = (spec) => {
    const parentModel = models.find((m) =>
      m.specs?.some((s) => s.specId === spec.specId),
    );
    if (!parentModel) {
      toast.error('Parent model not found for this spec');
      return;
    }
    setSelectedSpecModel(parentModel);
    setEditingSpec(spec);
    setIsSpecFormOpen(true);
  };

  // ❌ Delete spec
  const handleDeleteSpec = async (spec) => {
    if (!window.confirm(`Delete spec "${spec.specName}"?`)) return;
    try {
      await VehicleSpecService.delete(spec.specId);
      toast.success('Specification deleted successfully');
      loadModels(page);
    } catch (err) {
      console.error(err);
      toast.error('Failed to delete spec');
    }
  };

  // 🧩 Load vehicle models
  const loadModels = useCallback(async (pageNum = 1, keepExpanded = false) => {
    try {
      setLoading(true);
      const res = await VehicleModelService.get({
        pageNumber: pageNum,
        pageSize: 50,
      });

      setModels((prev) => {
        if (keepExpanded) {
          return res.items.map((newModel) => {
            const old = prev.find(
              (m) => m.modelNumber === newModel.modelNumber,
            );
            return old?.specs ? { ...newModel, specs: old.specs } : newModel;
          });
        }
        return res.items;
      });
      setTotalPages(res.totalPages || 1);
    } catch (err) {
      console.error(err);
      toast.error('Failed to load vehicle models');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    loadModels(page);
  }, [page, loadModels]);

  // 🧱 Build tree by parentModel
  const buildTree = (flat) => {
    const map = {};
    flat.forEach((m) => (map[m.modelNumber] = { ...m, children: [] }));
    const roots = [];
    flat.forEach((m) => {
      if (m.parentModel) {
        map[m.parentModel]?.children.push(map[m.modelNumber]);
      } else {
        roots.push(map[m.modelNumber]);
      }
    });
    return roots;
  };

  const treeData = useMemo(() => {
    let data = models;
    return buildTree(data);
  }, [models]);

  // ❌ Delete model
  const confirmDelete = async () => {
    if (!selectedToDelete) return;
    try {
      await VehicleModelService.delete(selectedToDelete.modelNumber);
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

  if (loading) return <LoadingState />;

  return (
    <>
      {/* 🔹 Form Model */}
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

      {/* 🔹 Form Spec */}
      <SpecForm
        isOpen={isSpecFormOpen}
        onClose={() => setIsSpecFormOpen(false)}
        model={selectedSpecModel}
        reloadModels={() => loadModels(page)}
        editingSpec={editingSpec}
      />

      {/* 🔹 Preview Image */}
      <ImagePreview
        isOpen={isPreviewOpen}
        onClose={() => setIsPreviewOpen(false)}
        images={previewImages}
        initialIndex={previewIndex}
      />

      {/* 🔹 Main Table */}
      <Card
        flexDirection="column"
        w="100%"
        borderRadius="16px"
        boxShadow="md"
        bg={bgColor}
      >
        <Header
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
          onEditSpec={handleEditSpec}
          onDeleteSpec={handleDeleteSpec}
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

      {/* 🔹 Confirm Delete */}
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

export default VehicleModelManagement;
