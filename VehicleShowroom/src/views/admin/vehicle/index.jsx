import React, { useEffect, useState } from 'react';
import { Box, Card, useColorModeValue, useDisclosure } from '@chakra-ui/react';
import VehicleService from 'services/VehicleService';
import Table from 'views/admin/vehicle/components/Table';
import Header from 'views/admin/vehicle/components/Header';
import Form from 'views/admin/vehicle/components/Form';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import Pagination from 'components/pagination/Pagination';
import { getColumns } from 'views/admin/vehicle/components/Columns';
import mockData from 'views/admin/vehicle/variables/data.json';
import { useShowToast } from 'utils/helper';

export default function VehiclePage() {
  const [data, setData] = useState(mockData);
  const [search, setSearch] = useState('');
  const [page, setPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);
  const [editing, setEditing] = useState(null);
  const [confirm, setConfirm] = useState({ open: false, target: null });
  const { isOpen, onOpen, onClose } = useDisclosure();
  const { showToast } = useShowToast();
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');

  const load = async (query = '', p = 0) => {
    try {
      const res = await VehicleService.getAll({
        searchTerm: query,
        pageNumber: p,
        pageSize: 10,
      });
      setData(res.content || []);
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
      await VehicleService.delete(confirm.target.id);
      showToast('Vehicle deleted', 'success');
      load(search, page);
    } catch {
      showToast('Delete failed', 'error');
    } finally {
      setConfirm({ open: false, target: null });
    }
  };

  const columns = getColumns({
    onEdit: (v) => {
      setEditing(v);
      onOpen();
    },
    onDelete: (v) => setConfirm({ open: true, target: v }),
    textColor,
  });

  return (
    <Box pt={{ base: '130px', md: '80px' }}>
      {/* Form Modal */}
      <Form
        isOpen={isOpen}
        onClose={onClose}
        reload={() => load(search, page)}
        editing={editing}
      />

      {/* Confirm Delete */}
      <ConfirmDialog
        isOpen={confirm.open}
        onClose={() => setConfirm({ open: false, target: null })}
        onConfirm={handleDelete}
        title="Delete Vehicle"
        message={`Delete ${confirm.target?.modelNumber}?`}
      />

      {/* Card with Table */}
      <Card bg={bgColor} borderRadius={20}>
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
