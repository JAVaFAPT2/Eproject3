import React, { useEffect, useMemo, useState } from 'react';
import {
  Card,
  useColorModeValue,
  useDisclosure,
  useToast,
} from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import EmployeeForm from './EmployeeForm';
import EmployeeDetail from './EmployeeDetail';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import Pagination from 'components/pagination/Pagination';
import EmployeeService from 'services/EmployeeService';
import EmployeeHeader from './EmployeeHeader';
import EmployeeList from './EmployeeList';
import { getEmployeeColumns } from './EmployeeColumns';

export default function EmployeeTable() {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const bgColor = useColorModeValue('white', 'navy.800');
  const headerBg = useColorModeValue('gray.100', 'navy.800');

  const [employees, setEmployees] = useState([]);
  const [page, setPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [searchInput, setSearchInput] = useState('');
  const [editingEmployee, setEditingEmployee] = useState(null);
  const [employeeToDelete, setEmployeeToDelete] = useState(null);
  const [selectedEmployee, setSelectedEmployee] = useState(null);
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);

  const toast = useToast();
  const { isOpen, onOpen, onClose } = useDisclosure();
  const {
    isOpen: isOpenDetails,
    onOpen: onOpenDetails,
    onClose: onCloseDetails,
  } = useDisclosure();

  useEffect(() => {
    loadEmployees(page);
  }, [page]);

  const loadEmployees = async (p = 0) => {
    try {
      const res = await EmployeeService.getEmployees(p, 20);
      setEmployees(res.content || []);
      setTotalPages(res.totalPages || 1);
    } catch (err) {
      console.error(err);
    }
  };

  const filteredData = useMemo(() => {
    let data = employees;
    if (searchInput) {
      data = data.filter((row) =>
        ['email', 'fullName'].some((key) =>
          String(row[key] || '')
            .toLowerCase()
            .includes(searchInput.toLowerCase()),
        ),
      );
    }
    return data;
  }, [employees, searchInput]);

  const handleDelete = async () => {
    try {
      await EmployeeService.deleteEmployee(employeeToDelete.id);
      toast({
        title: 'Employee deleted successfully',
        status: 'success',
        duration: 3000,
        position: 'bottom-right',
      });
      setEmployeeToDelete(null);
      loadEmployees(page);
    } catch (err) {
      toast({
        title: 'Failed to delete employee',
        status: 'error',
        duration: 3000,
        position: 'bottom-right',
      });
    }
  };

  // Columns
  const columns = useMemo(
    () =>
      getEmployeeColumns({
        onShow: (emp) => {
          setSelectedEmployee(emp);
          onOpenDetails();
        },
        onEdit: (emp) => {
          setEditingEmployee(emp);
          onOpen();
        },
        onDelete: (emp) => {
          setEmployeeToDelete(emp);
          setIsConfirmOpen(true);
        },
        textColor,
      }),
    [onOpen, onOpenDetails, textColor],
  );

  const table = useReactTable({
    data: filteredData,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <>
      <EmployeeForm
        isOpen={isOpen}
        onClose={() => {
          setEditingEmployee(null);
          onClose();
        }}
        reloadEmployees={() => loadEmployees(page)}
        editingEmployee={editingEmployee}
      />

      <EmployeeDetail
        isOpen={isOpenDetails}
        onClose={() => {
          setSelectedEmployee(null);
          onCloseDetails();
        }}
        employee={selectedEmployee}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => setIsConfirmOpen(false)}
        onConfirm={handleDelete}
        title="Delete Employee"
        message={`Are you sure you want to delete ${employeeToDelete?.fullName}?`}
      />

      <Card
        flexDirection="column"
        w="100%"
        borderRadius="16px"
        boxShadow="md"
        bg={bgColor}
      >
        <EmployeeHeader
          searchInput={searchInput}
          setSearchInput={setSearchInput}
          onAdd={onOpen}
        />
        <EmployeeList
          table={table}
          textColor={textColor}
          borderColor={borderColor}
          headerBg={headerBg}
          bgColor={bgColor}
        />
        {totalPages > 1 && (
          <Pagination
            page={page}
            totalPages={totalPages}
            onPageChange={setPage}
          />
        )}
      </Card>
    </>
  );
}
