import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { useColorModeValue, Card, useDisclosure } from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import { useAppToast } from 'utils/ToastHelper';

import PurchaseOrderService from 'services/PurchaseOrderService';
import VehicleModelService from 'services/VehicleModelService';
import UserService from 'services/UserService';

import Header from './components/Header';
import List from './components/List';
import Columns from './components/Columns';
import Form from './components/Form';
import Pagination from 'components/pagination/Pagination';
import Detail from './components/Detail';

function PurchaseOrderManagement() {
  const toast = useAppToast();
  const bgColor = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'navy.700');

  const [orders, setOrders] = useState([]);
  const [models, setModels] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [statusFilter, setStatusFilter] = useState(null);
  const [loading, setLoading] = useState(false);

  const [selectedOrderId, setSelectedOrderId] = useState(null);
  const {
    isOpen: isDetailOpen,
    onOpen: openDetail,
    onClose: closeDetail,
  } = useDisclosure();

  const { isOpen, onOpen, onClose } = useDisclosure();

  // ✅ Load models
  const loadModels = useCallback(async () => {
    try {
      setLoading(true);
      const res = await VehicleModelService.get({
        pageNumber: 1,
        pageSize: 100,
      });
      setModels(res.items || []);
    } catch (err) {
      console.error('Failed to load models:', err);
    } finally {
      setLoading(false);
    }
  }, []);

  // ✅ Load orders
  const loadOrders = useCallback(
    async (p = 1) => {
      try {
        setLoading(true);
        const res = await PurchaseOrderService.get({
          pageNumber: p,
          pageSize: 10,
          status: statusFilter || null,
        });

        let orders = res.items || [];
        const userIds = [
          ...new Set(orders.map((o) => o.createdBy).filter(Boolean)),
        ];

        const userMap = {};
        await Promise.all(
          userIds.map(async (id) => {
            try {
              const userRes = await UserService.getById(id);
              userMap[id] = userRes?.name || userRes?.username || 'Unknown';
            } catch {
              userMap[id] = 'Unknown';
            }
          }),
        );

        const modelMap = {};
        models.forEach((m) => (modelMap[m.modelNumber] = m.name));

        const withNames = orders.map((order) => ({
          ...order,
          createdByName: userMap[order.createdBy] || 'Unknown',
          modelName:
            modelMap[order.modelNumber] || order.modelNumber || 'Unknown',
        }));

        setOrders(withNames);
        setTotalPages(res.totalPages || 1);
      } catch (err) {
        toast.error('Failed to load purchase orders');
      } finally {
        setLoading(false);
      }
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [models, statusFilter],
  );

  // ✅ Complete
  const handleCompleteOrder = useCallback(
    async (orderId) => {
      try {
        setLoading(true);
        await PurchaseOrderService.updateStatus(orderId, 2);
        toast.success('Purchase order marked as Completed');
        loadOrders(page);
      } catch {
        toast.error('Failed to update order status');
      } finally {
        setLoading(false);
      }
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [loadOrders, page],
  );

  // ✅ Cancel
  const handleCancelOrder = useCallback(
    async (orderId) => {
      try {
        setLoading(true);
        await PurchaseOrderService.updateStatus(orderId, 3);
        toast.success('Purchase order marked as Cancelled');
        loadOrders(page);
      } catch {
        toast.error('Failed to update order status');
      } finally {
        setLoading(false);
      }
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [loadOrders, page],
  );

  // ✅ Khởi tạo
  useEffect(() => {
    const init = async () => {
      setLoading(true);
      await loadModels();
      await loadOrders(page);
      setLoading(false);
    };
    init();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page, statusFilter]);

  // ✅ Mở modal chi tiết
  const handleViewDetail = useCallback(
    (id) => {
      setSelectedOrderId(id);
      openDetail();
    },
    [openDetail],
  );

  // ✅ Columns
  const columns = useMemo(
    () =>
      Columns({
        onComplete: handleCompleteOrder,
        onCancel: handleCancelOrder,
        statusFilter,
        setStatusFilter,
        onViewDetail: handleViewDetail,
      }),
    [handleCompleteOrder, handleCancelOrder, handleViewDetail, statusFilter],
  );

  const table = useReactTable({
    data: orders,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <>
      <Detail
        isOpen={isDetailOpen}
        onClose={closeDetail}
        orderId={selectedOrderId}
      />

      <Form
        isOpen={isOpen}
        onClose={onClose}
        reloadOrders={() => loadOrders(page)}
        models={models}
        bgColor={bgColor}
        textColor={textColor}
      />

      <Card bg={bgColor} boxShadow="md" borderRadius="16px">
        <Header onAdd={onOpen} textColor={textColor} />
        <List
          loading={loading}
          table={table}
          borderColor={borderColor}
          textColor={textColor}
        />
      </Card>

      {totalPages > 1 && (
        <Pagination
          page={page}
          totalPages={totalPages}
          onPageChange={setPage}
        />
      )}
    </>
  );
}

export default PurchaseOrderManagement;
