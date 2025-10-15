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
  const [employees, setEmployees] = useState([]);

  const loadEmployees = useCallback(async () => {
    try {
      const res = await UserService.getAll({
        roleName: 'Dealer',
      });
      setEmployees(res.data || res.content || []);
    } catch (err) {
      console.error('❌ Failed to load employees:', err);
      toast.error('Failed to load employees');
    }
    // eslint-disable-next-line
  }, []);

  useEffect(() => {
    loadEmployees();
  }, [loadEmployees]);

  const handleToggleActive = async (id, current) => {
    try {
      await UserService.toggleActive(id, !current);
      toast.success('Status updated');
      loadEmployees();
    } catch (err) {
      toast.error('Failed to update status');
    }
  };

  // 🔹 Định nghĩa cột
  const columns = useMemo(
    () =>
      Columns({
        onToggle: handleToggleActive,
      }),
    // eslint-disable-next-line
    [],
  );

  // 🔹 Khởi tạo table
  const table = useReactTable({
    data: employees,
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
