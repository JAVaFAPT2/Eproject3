import React, { useState, useEffect, useMemo, useCallback } from 'react';
import {
  Card,
  useDisclosure,
  useToast,
  useColorModeValue,
} from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import { getBlogColumns } from './BlogColumns';
import BlogHeader from './BlogHeader';
import BlogList from './BlogList';
import Pagination from 'components/pagination/Pagination';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import BlogService from 'services/BlogService';
import BlogForm from './BlogForm';
import BlogDetailModal from './BlogDetailModal';
import BlogImageModal from './BlogImageModal';

export default function BlogTable() {
  const bgColor = useColorModeValue('white', 'navy.800');
  const [blogs, setBlogs] = useState([]);
  const [page, setPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [searchInput, setSearchInput] = useState('');
  const [editingBlog, setEditingBlog] = useState(null);
  const [selectedToDelete, setSelectedToDelete] = useState(null);
  const [detailBlog, setDetailBlog] = useState(null);
  const [previewImage, setPreviewImage] = useState(null);

  const toast = useToast();
  const { isOpen, onOpen, onClose } = useDisclosure();
  const {
    isOpen: isConfirmOpen,
    onOpen: onConfirmOpen,
    onClose: onConfirmClose,
  } = useDisclosure();
  const {
    isOpen: isDetailOpen,
    onOpen: onDetailOpen,
    onClose: onDetailClose,
  } = useDisclosure();
  const {
    isOpen: isImageOpen,
    onOpen: onImageOpen,
    onClose: onImageClose,
  } = useDisclosure();

  const loadBlogs = useCallback(
    async (p = 0) => {
      try {
        const res = await BlogService.getBlogs(searchInput, p, 10);
        setBlogs(res.content || []);
        setTotalPages(res.totalPages || 1);
      } catch (err) {
        console.error(err);
      }
    },
    [searchInput],
  );

  useEffect(() => {
    loadBlogs(page);
  }, [page, loadBlogs]);

  const confirmDelete = async () => {
    if (!selectedToDelete) return;
    try {
      await BlogService.deleteBlog(selectedToDelete.id);
      loadBlogs(page);
      toast({
        position: 'bottom-right',
        title: 'Deleted successfully',
        status: 'success',
        duration: 3000,
      });
    } catch (err) {
      toast({
        position: 'bottom-right',
        title: 'Error deleting blog',
        status: 'error',
        duration: 3000,
      });
    } finally {
      onConfirmClose();
    }
  };

  const columns = useMemo(
    () =>
      getBlogColumns({
        onShowDetails: (blog) => {
          setDetailBlog(blog);
          onDetailOpen();
        },
        onEdit: (blog) => {
          setEditingBlog(blog);
          onOpen();
        },
        onDelete: (blog) => {
          setSelectedToDelete({ id: blog.id, title: blog.title });
          onConfirmOpen();
        },
        onPreviewImage: (img) => {
          setPreviewImage(img);
          onImageOpen();
        },
      }),
    [onConfirmOpen, onDetailOpen, onImageOpen, onOpen],
  );

  const table = useReactTable({
    data: blogs,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <>
      <BlogForm
        isOpen={isOpen}
        onClose={() => {
          setEditingBlog(null);
          onClose();
        }}
        reloadBlogs={() => loadBlogs(page)}
        blog={editingBlog}
      />

      <BlogDetailModal
        isOpen={isDetailOpen}
        onClose={onDetailClose}
        blog={detailBlog}
      />

      <BlogImageModal
        isOpen={isImageOpen}
        onClose={onImageClose}
        previewImage={previewImage}
      />

      <Card
        flexDirection="column"
        w="100%"
        borderRadius="16px"
        boxShadow="md"
        bg={bgColor}
      >
        <BlogHeader
          title="Blog List"
          searchInput={searchInput}
          setSearchInput={setSearchInput}
          onAdd={() => {
            setEditingBlog(null);
            onOpen();
          }}
        />
        <BlogList table={table} data={blogs} />
        {totalPages > 1 && (
          <Pagination
            page={page}
            totalPages={totalPages}
            onPageChange={setPage}
          />
        )}
      </Card>

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={onConfirmClose}
        onConfirm={confirmDelete}
        title="Delete blog"
        message={
          selectedToDelete
            ? `Are you sure you want to delete blog "${selectedToDelete.title}"?`
            : 'Are you sure you want to delete this blog?'
        }
      />
    </>
  );
}
