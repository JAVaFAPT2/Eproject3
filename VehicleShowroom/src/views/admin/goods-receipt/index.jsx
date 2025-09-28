import React, { useEffect, useMemo, useState } from 'react';
import { Box, Card, useDisclosure, useToast, useColorModeValue } from '@chakra-ui/react';
import GoodsReceiptService from 'services/GoodsReceiptService';
import Table from './components/Table';
import Header from './components/Header';
import Form from './components/Form';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import Pagination from 'components/pagination/Pagination';
import { getColumns } from './components/Columns';
import mockData from './variables/data.json';

export default function GoodsReceiptPage() {
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
      const res = await GoodsReceiptService.getAll();
      setData(res.data || mockData);
    } catch {
      setData(mockData);
    }
  };

  useEffect(() => { load(); }, []);

  const filtered = useMemo(() => 
    data.filter((v) =>
      v.receiptNumber.toLowerCase().includes(search.toLowerCase()) ||
      v.vehicleId.toLowerCase().includes(search.toLowerCase())
    ), 
  [data, search]);

  const handleDelete = async () => {
    try {
      await GoodsReceiptService.delete(confirm.target.id);
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
        onEdit: (v) => { setEditing(v); onOpen(); },
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
        title="Delete Receipt"
        message={`Delete ${confirm.target?.receiptNumber}?`}
      />
      <Card bg={bgColor}>
        <Header search={search} setSearch={setSearch} onAdd={onOpen} />
        <Table data={filtered} columns={columns} />
        <Pagination page={0} totalPages={1} />
      </Card>
    </Box>
  );
}
