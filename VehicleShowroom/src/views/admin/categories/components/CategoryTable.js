import React, { useEffect, useMemo, useState, useCallback } from 'react';
import {
  useColorModeValue,
  Card,
  useDisclosure,
  useToast,
} from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import CategoryService from 'services/CategoryService';
import CategoryForm from './CategoryForm';
import Pagination from 'components/pagination/Pagination';
import CategoryHeader from './CategoryHeader';
import CategoryList from './CategoryList';
import CategoryColumns from './CategoryColumns';

export default function CategoryTable() {
  const bgColor = useColorModeValue('white', 'navy.800');
  const [categories, setCategories] = useState([]);
  const [page, setPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [searchInput, setSearchInput] = useState('');
  const [editingCategory, setEditingCategory] = useState(null);
  const [parentCategory, setParentCategory] = useState(null);
  const [selectedToDelete, setSelectedToDelete] = useState(null);
  const [expandedRows, setExpandedRows] = useState({});

  const toggleExpand = (id) => {
    setExpandedRows((prev) => ({ ...prev, [id]: !prev[id] }));
  };

  const toast = useToast();
  const { isOpen, onOpen, onClose } = useDisclosure();
  const {
    isOpen: isConfirmOpen,
    onOpen: onConfirmOpen,
    onClose: onConfirmClose,
  } = useDisclosure();

  const loadCategories = useCallback(async (p = 0) => {
    try {
      const res = await CategoryService.getCategories(p, 100);
      setCategories(res.content || []);
      setTotalPages(res.totalPages || 1);
    } catch (err) {
      console.error(err);
    }
  }, []);

  useEffect(() => {
    loadCategories(page);
  }, [page, loadCategories]);

  const buildTree = (flat) => {
    const map = {};
    flat.forEach((c) => (map[c.id] = { ...c, children: [] }));
    const roots = [];
    flat.forEach((c) => {
      if (c.parentId) map[c.parentId]?.children.push(map[c.id]);
      else roots.push(map[c.id]);
    });
    return roots;
  };

  const treeData = useMemo(() => {
    let data = categories;
    if (searchInput) {
      data = categories.filter((c) =>
        c.name.toLowerCase().includes(searchInput.toLowerCase()),
      );
    }
    return buildTree(data);
  }, [categories, searchInput]);

  const confirmDelete = async () => {
    if (!selectedToDelete) return;
    try {
      await CategoryService.deleteCategory(selectedToDelete.id);
      loadCategories(page);
      setSelectedToDelete(null);
      toast({
        title: 'Deleted successfully',
        status: 'success',
        duration: 3000,
        position: 'bottom-right',
      });
    } catch (err) {
      console.error(err);
      toast({
        title: 'Error deleting category',
        status: 'error',
        duration: 3000,
        isClosable: true,
      });
    } finally {
      onConfirmClose();
    }
  };

  // Columns tách riêng
  const columns = useMemo(
    () =>
      CategoryColumns({
        onEdit: (cat) => {
          setEditingCategory(cat);
          setParentCategory(null);
          onOpen();
        },
        onAdd: (cat) => {
          setParentCategory(cat);
          setEditingCategory(null);
          onOpen();
        },
        onDelete: (cat) => {
          setSelectedToDelete({ id: cat.id, name: cat.name });
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
      <CategoryForm
        isOpen={isOpen}
        onClose={() => {
          setEditingCategory(null);
          setParentCategory(null);
          onClose();
        }}
        reloadCategories={() => loadCategories(page)}
        category={editingCategory}
        parentCategory={parentCategory}
      />

      <Card
        flexDirection="column"
        w="100%"
        borderRadius="16px"
        boxShadow="md"
        bg={bgColor}
      >
        <CategoryHeader
          searchInput={searchInput}
          setSearchInput={setSearchInput}
          onAdd={() => {
            setParentCategory(null);
            setEditingCategory(null);
            onOpen();
          }}
        />
        <CategoryList
          table={table}
          treeData={treeData}
          expandedRows={expandedRows}
          toggleExpand={toggleExpand}
          onAdd={(category) => {
            setParentCategory(category);
            setEditingCategory(null);
            onOpen();
          }}
          onEdit={(category) => {
            setEditingCategory(category);
            setParentCategory(null);
            onOpen();
          }}
          onDelete={(category) => {
            setSelectedToDelete({ id: category.id, name: category.name });
            onConfirmOpen();
          }}
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
        title="Delete Category"
        message={
          selectedToDelete
            ? `Are you sure you want to delete category "${selectedToDelete.name}"?`
            : 'Are you sure you want to delete this category?'
        }
      />
    </>
  );
}
