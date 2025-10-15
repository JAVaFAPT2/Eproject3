import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { Card, useColorModeValue } from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import UserService from 'services/UserService';
import List from './List';
import Columns from './Columns';
import { useAppToast } from 'utils/ToastHelper';

export default function Table() {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const bgColor = useColorModeValue('white', 'navy.800');
  const headerBg = useColorModeValue('gray.100', 'navy.800');

  const toast = useAppToast();
  const [users, setUsers] = useState([]);

  // ✅ Load danh sách khách hàng
  const loadUsers = useCallback(async () => {
    try {
      const res = await UserService.getAll({
        roleName: 'Customer',
      });
      setUsers(res.data || res.content || []);
    } catch (err) {
      console.error('❌ Failed to load customers:', err);
      toast.error('Failed to load customers');
    }
  }, [toast]);

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
      <List
        table={table}
        textColor={textColor}
        borderColor={borderColor}
        bgColor={bgColor}
        headerBg={headerBg}
      />
    </Card>
  );
}
