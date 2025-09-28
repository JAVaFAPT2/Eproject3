import React, { useEffect, useMemo, useState } from 'react';
import { Box, Card, useColorModeValue, useDisclosure } from '@chakra-ui/react';
import Pagination from 'components/pagination/Pagination';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import EmployeeService from 'services/EmployeeService';
import Table from 'views/admin/employee/components/Table';
import Header from 'views/admin/employee/components/Header';
import Form from 'views/admin/employee/components/Form';
import Detail from 'views/admin/employee/components/Detail';
import { getColumns } from 'views/admin/employee/components/Columns';
import mockData from 'views/admin/employee/variables/data.json';
import { useShowToast } from 'utils/helper';

export default function EmployeePage() {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');
  const { showToast } = useShowToast();
  const { isOpen, onOpen, onClose } = useDisclosure();
  const {
    isOpen: isDetailOpen,
    onOpen: onOpenDetail,
    onClose: onCloseDetail,
  } = useDisclosure();

  const [data, setData] = useState(mockData);
  const [search, setSearch] = useState('');
  const [page, setPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [editing, setEditing] = useState(null);
  const [selected, setSelected] = useState(null);
  const [confirm, setConfirm] = useState({ open: false, target: null });

  const load = async (query = '', p = 0) => {
    try {
      const res = await EmployeeService.getAll({
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
      await EmployeeService.delete(confirm.target.id);
      showToast('Deleted', 'success');
      load();
    } catch {
      showToast('Delete failed', 'error');
    } finally {
      setConfirm({ open: false, target: null });
    }
  };

  const columns = useMemo(
    () =>
      getColumns({
        onShow: (e) => {
          setSelected(e);
          onOpenDetail();
        },
        onEdit: (e) => {
          setEditing(e);
          onOpen();
        },
        onDelete: (e) => setConfirm({ open: true, target: e }),
        textColor,
      }),
    [textColor, onOpen, onOpenDetail],
  );

  return (
    <Box pt={{ base: '130px', md: '80px', xl: '80px' }}>
      <Form isOpen={isOpen} onClose={onClose} reload={load} editing={editing} />
      <Detail
        isOpen={isDetailOpen}
        onClose={onCloseDetail}
        employee={selected}
      />
      <ConfirmDialog
        isOpen={confirm.open}
        onClose={() => setConfirm({ open: false, target: null })}
        onConfirm={handleDelete}
        title="Delete Employee"
        message={`Delete ${confirm.target?.fullName}?`}
      />
      <Card bg={bgColor} borderRadius="16px">
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
