import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { Card, useColorModeValue } from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import UserService from 'services/UserService';
import List from './components/List';
import Columns from './components/Columns';
import { useAppToast } from 'utils/ToastHelper';
import Header from './components/Header';

function CustomerManagement() {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const bgColor = useColorModeValue('white', 'navy.800');
  const headerBg = useColorModeValue('gray.100', 'navy.800');

  const toast = useAppToast();
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);

  // ✅ Load danh sách khách hàng
  const loadUsers = useCallback(async () => {
    try {
      setLoading(true);
      const res = await UserService.get({ roleName: 'Customer' });
      setUsers(res.items || []);
    } catch (err) {
      console.error('❌ Failed to load customers:', err);
      toast.error('Failed to load customers');
    } finally {
      setLoading(false);
    }
  }, []); // Remove toast dependency to prevent infinite loop

  useEffect(() => {
    loadUsers();
  }, [loadUsers]);

  // ✅ Cấu hình cột hiển thị
  const columns = useMemo(() => Columns({ textColor }), [textColor]);

  const table = useReactTable({
    data: users,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <Card
      flexDirection="column"
      w="100%"
      borderRadius="16px"
      boxShadow="md"
      bg={bgColor}
    >
      <Header textColor={textColor} />
      <List
        table={table}
        textColor={textColor}
        borderColor={borderColor}
        bgColor={bgColor}
        headerBg={headerBg}
        loading={loading}
      />
    </Card>
  );
}

export default CustomerManagement;
