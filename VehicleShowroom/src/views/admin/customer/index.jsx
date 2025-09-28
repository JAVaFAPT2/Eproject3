import React, { useEffect, useMemo, useState } from 'react';
import { Box, Card, useDisclosure, useToast, useColorModeValue } from '@chakra-ui/react';
import CustomerService from 'services/CustomerService';
import Table from './components/Table';
import Header from './components/Header';
import Form from './components/Form';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import Pagination from 'components/pagination/Pagination';
import { getColumns } from './components/Columns';
import mockData from './variables/data.json';

export default function CustomerPage() {
  const [data, setData] = useState(mockData);
  const [search, setSearch] = useState('');
  const [editing, setEditing] = useState(null);
  const [confirm, setConfirm] = useState({ open: false, target: null });
  const { isOpen, onOpen, onClose } = useDisclosure();
  const toast = useToast();

  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');

  const load = async () => {
    try {
      const res = await CustomerService.getAll();
      setData(res.data || mockData);
    } catch {
      setData(mockData);
    }
  };

  useEffect(() => {
    load();
  }, []);

  const filtered = useMemo(
    () =>
      data.filter(
        (v) =>
          v.fullName.toLowerCase().includes(search.toLowerCase()) ||
          v.email.toLowerCase().includes(search.toLowerCase())
      ),
    [data, search],
  );

  const handleDelete = async () => {
    try {
      await CustomerService.delete(confirm.target.id);
      toast({ title: 'Deleted successfully', status: 'success' });
      load();
    } catch {
      toast({ title: 'Delete failed', status: 'error' });
    } finally {
      setConfirm({ open: false, target: null });
    }
  };

  const columns = useMemo(
    () =>
      getColumns({
        onEdit: (v) => {
          setEditing(v);
          onOpen();
        },
        onDelete: (v) => setConfirm({ open: true, target: v }),
        textColor,
      }),
    [textColor],
  );

  return (
    <Box pt={{ base: '130px', md: '80px' }}>
      <Form isOpen={isOpen} onClose={onClose} reload={load} editing={editing} />
      <ConfirmDialog
        isOpen={confirm.open}
        onClose={() => setConfirm({ open: false, target: null })}
        onConfirm={handleDelete}
        title="Delete Customer"
        message={`Delete ${confirm.target?.fullName}?`}
      />
      <Card bg={bgColor} borderRadius="16px">
        <Header search={search} setSearch={setSearch} onAdd={onOpen} />
        <Table data={filtered} columns={columns} />
        <Pagination page={0} totalPages={1} />
      </Card>
    </Box>
  );
}
