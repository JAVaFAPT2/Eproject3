import React, { useEffect, useMemo, useState } from 'react';
import {
  Box,
  Card,
  useDisclosure,
  useToast,
  useColorModeValue,
} from '@chakra-ui/react';
import OrderService from 'services/OrderService';
import Header from 'views/admin/order/components/Header';
import Table from 'views/admin/order/components/Table';
import Form from 'views/admin/order/components/Form';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import Pagination from 'components/pagination/Pagination';
import { getColumns } from 'views/admin/order/components/Columns';
import mockData from 'views/admin/order/variables/data.json';

export default function OrderPage() {
  const [data, setData] = useState(mockData);
  const [search, setSearch] = useState('');
  const [page, setPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [editing, setEditing] = useState(null);
  const [confirm, setConfirm] = useState({ open: false, target: null });
  const { isOpen, onOpen, onClose } = useDisclosure();
  const toast = useToast();
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bg = useColorModeValue('white', 'navy.800');

  const load = async (query = '', p = 0) => {
    try {
      const res = await OrderService.getAll({
        searchTerm: query,
        pageNumber: p,
        pageSize: 10,
      });
      setData(res.data || mockData);
      setTotalPages(res.totalPages || 1);
    } catch {
      setData(mockData);
    }
  };

  useEffect(() => {
    load(search, page);
  }, [search, page]);

  const handleDelete = async () => {
    try {
      await OrderService.delete(confirm.target.id);
      toast({ title: 'Order deleted', status: 'success' });
      load();
    } catch {
      toast({ title: 'Failed to delete', status: 'error' });
    } finally {
      setConfirm({ open: false, target: null });
    }
  };

  const columns = useMemo(
    () =>
      getColumns({
        onEdit: (o) => {
          setEditing(o);
          onOpen();
        },
        onDelete: (o) => setConfirm({ open: true, target: o }),
        textColor,
      }),
    [textColor, onOpen],
  );

  return (
    <Box pt={{ base: '130px', md: '80px' }}>
      <Form isOpen={isOpen} onClose={onClose} reload={load} editing={editing} />
      <ConfirmDialog
        isOpen={confirm.open}
        onClose={() => setConfirm({ open: false })}
        onConfirm={handleDelete}
        title="Delete Order"
        message={`Delete ${confirm.target?.orderNumber}?`}
      />
      <Card bg={bg} borderRadius={20}>
        <Header search={search} setSearch={setSearch} onAdd={onOpen} />
        <Table data={data} columns={columns} />
      </Card>
      {totalPages > 1 && (
        <Pagination
          page={page}
          totalPages={totalPages}
          onPageChange={(p) => setPage(p)}
        />
      )}
    </Box>
  );
}
