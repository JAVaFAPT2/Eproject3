import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { Card, useColorModeValue, useDisclosure } from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import UserService from 'services/UserService';
import List from './components/List';
import Columns from './components/Columns';
import { useAppToast } from 'utils/ToastHelper';
import Header from './components/Header';
import Form from './components/Form';

function EmployeeManagement() {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const bgColor = useColorModeValue('white', 'navy.800');
  const headerBg = useColorModeValue('gray.100', 'navy.800');

  const toast = useAppToast();
  const [employees, setEmployees] = useState([]);
  const [loading, setLoading] = useState(false);

  // 🟢 Disclosure cho modal thêm nhân viên
  const { isOpen, onOpen, onClose } = useDisclosure();

  // ✅ Load danh sách nhân viên (Dealer)
  const loadEmployees = useCallback(async () => {
    try {
      setLoading(true);
      const res = await UserService.get({ roleName: 'Employee' });
      setEmployees(res.items || []);
    } catch (err) {
      console.error('❌ Failed to load employees:', err);
      toast.error('Failed to load employees');
    } finally {
      setLoading(false);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []); // tránh dependency gây re-render

  useEffect(() => {
    loadEmployees();
  }, [loadEmployees]);

  // ✅ Toggle active/inactive
  const handleToggleActive = useCallback(
    async (id, current) => {
      try {
        setLoading(true);
        await UserService.toggleActive(id, !current);
        toast.success('Status updated');
        loadEmployees();
      } catch (err) {
        toast.error('Failed to update status');
      } finally {
        setLoading(false);
      }
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [loadEmployees],
  );

  // ✅ Cấu hình cột
  const columns = useMemo(
    () => Columns({ onToggle: handleToggleActive }),
    [handleToggleActive],
  );

  const table = useReactTable({
    data: employees,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <>
      {/* 🟢 Modal thêm mới */}
      <Form
        isOpen={isOpen}
        onClose={onClose}
        reloadUsers={loadEmployees}
        bgColor={bgColor}
        textColor={textColor}
      />

      {/* 🧩 Danh sách */}
      <Card
        flexDirection="column"
        w="100%"
        borderRadius="16px"
        boxShadow="md"
        bg={bgColor}
      >
        <Header textColor={textColor} onAdd={onOpen} />
        <List
          loading={loading}
          table={table}
          textColor={textColor}
          borderColor={borderColor}
          bgColor={bgColor}
          headerBg={headerBg}
        />
      </Card>
    </>
  );
}

export default EmployeeManagement;
