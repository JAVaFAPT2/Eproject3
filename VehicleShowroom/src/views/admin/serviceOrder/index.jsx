import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { useColorModeValue, Card, useDisclosure } from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import { useAppToast } from 'utils/ToastHelper';
import { LoadingState } from 'components/common/LoadingState';

import ServiceOrderService from 'services/ServiceOrderService';
import UserService from 'services/UserService';

import Header from './components/Header';
import List from './components/List';
import Columns from './components/Columns';
import DetailDialog from './components/DetailDialog';
import StatusForm from './components/StatusForm';
import Pagination from 'components/pagination/Pagination';

function ServiceOrderManagement() {
  const toast = useAppToast();
  const bgColor = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'navy.700');

  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);

  const [selectedOrder, setSelectedOrder] = useState(null);
  const [updatingOrder, setUpdatingOrder] = useState(null);

  const {
    isOpen: isDetailOpen,
    onOpen: onDetailOpen,
    onClose: onDetailClose,
  } = useDisclosure();

  const {
    isOpen: isStatusOpen,
    onOpen: onStatusOpen,
    onClose: onStatusClose,
  } = useDisclosure();

  // ✅ Load orders (mapping createdBy & customerName)
  const loadOrders = useCallback(async (p = 1) => {
    try {
      setLoading(true);
      const res = await ServiceOrderService.get({
        pageNumber: p,
        pageSize: 10,
      });
      const orders = res.items || [];

      const ids = [
        ...new Set(
          orders.flatMap((o) => [o.createdBy, o.customerId]).filter(Boolean),
        ),
      ];

      const userMap = {};
      await Promise.all(
        ids.map(async (id) => {
          try {
            const userRes = await UserService.getById(id);
            userMap[id] = userRes?.name || userRes?.username || 'Unknown';
          } catch {
            userMap[id] = 'Unknown';
          }
        }),
      );

      const mapped = orders.map((o) => ({
        ...o,
        createdByName: userMap[o.createdBy] || 'Unknown',
        customerName: userMap[o.customerId] || 'Unknown',
      }));

      setOrders(mapped);
      setTotalPages(res.totalPages || 1);
    } catch (err) {
      toast.error('Failed to load service orders');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    loadOrders(page);
  }, [page]);

  const handleViewDetail = (order) => {
    setSelectedOrder(order);
    onDetailOpen();
  };

  const handleStatusChange = (order) => {
    setUpdatingOrder(order);
    onStatusOpen();
  };

  const columns = useMemo(
    () =>
      Columns({
        onViewDetail: handleViewDetail,
        onUpdateStatus: handleStatusChange,
      }),
    [],
  );

  const table = useReactTable({
    data: orders,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  if (loading) return <LoadingState />;

  return (
    <>
      <Card bg={bgColor} boxShadow="md" borderRadius="16px">
        <Header textColor={textColor} />
        <List table={table} borderColor={borderColor} textColor={textColor} />
      </Card>

      {totalPages > 1 && (
        <Pagination
          page={page}
          totalPages={totalPages}
          onPageChange={setPage}
        />
      )}

      <DetailDialog
        isOpen={isDetailOpen}
        onClose={onDetailClose}
        order={selectedOrder}
      />

      <StatusForm
        isOpen={isStatusOpen}
        onClose={onStatusClose}
        order={updatingOrder}
        reload={() => loadOrders(page)}
      />
    </>
  );
}

export default ServiceOrderManagement;
