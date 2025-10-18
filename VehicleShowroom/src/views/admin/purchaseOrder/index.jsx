import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { useColorModeValue, Card, useDisclosure } from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import { useAppToast } from 'utils/ToastHelper';
import { LoadingState } from 'components/common/LoadingState';

import PurchaseOrderService from 'services/PurchaseOrderService';
import VehicleModelService from 'services/VehicleModelService';
import UserService from 'services/UserService';

import Header from './components/Header';
import List from './components/List';
import Columns from './components/Columns';
import Form from './components/Form';
import Pagination from 'components/pagination/Pagination';

function PurchaseOrderManagement() {
  const toast = useAppToast();
  const bgColor = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'navy.700');

  const [orders, setOrders] = useState([]);
  const [models, setModels] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false); // ✅ Loading state
  const { isOpen, onOpen, onClose } = useDisclosure();

  // ✅ Load purchase orders + createdBy name + modelName
  const loadOrders = useCallback(
    async (p = 1) => {
      try {
        setLoading(true);
        const res = await PurchaseOrderService.get({
          pageNumber: p,
          pageSize: 10,
        });

        let orders = res.items || [];
        const userIds = [
          ...new Set(orders.map((o) => o.createdBy).filter(Boolean)),
        ];

        // 🔹 Lấy thông tin người tạo đơn
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

        // 🔹 Ánh xạ tên model
        const modelMap = {};
        models.forEach((m) => {
          modelMap[m.modelNumber] = m.name;
        });

        // 🔹 Gộp dữ liệu
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
        console.error(err);
      } finally {
        setLoading(false);
      }
    },
    [models],
  );

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

  // ✅ Đánh dấu đơn hàng là Complete
  const handleCompleteOrder = async (orderId) => {
    try {
      setLoading(true);
      await PurchaseOrderService.complete(orderId);
      toast.success('Purchase order marked as Complete');
      loadOrders(page);
    } catch (err) {
      console.error('❌ Failed to complete purchase order:', err);
      toast.error('Failed to update order status');
    } finally {
      setLoading(false);
    }
  };

  // ✅ Load models trước rồi mới load orders
  useEffect(() => {
    loadModels();
  }, []);

  useEffect(() => {
    if (models.length > 0) {
      loadOrders(page);
    }
  }, [models, page]);

  // ✅ Cột dữ liệu
  const columns = useMemo(
    () => Columns({ onComplete: handleCompleteOrder }),
    [handleCompleteOrder],
  );

  const table = useReactTable({
    data: orders,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  // ✅ Trạng thái hiển thị
  if (loading) return <LoadingState />;

  return (
    <>
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
        <List table={table} borderColor={borderColor} textColor={textColor} />
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
