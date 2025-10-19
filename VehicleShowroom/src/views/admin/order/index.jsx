import React, { useEffect, useMemo, useState, useCallback } from 'react';
import { useColorModeValue, Card, useDisclosure } from '@chakra-ui/react';
import { getCoreRowModel, useReactTable } from '@tanstack/react-table';
import { useAppToast } from 'utils/ToastHelper';
import { useUser } from 'contexts/UserContext';
import { LoadingState } from 'components/common/LoadingState';

import OrderService from 'services/OrderService';
import VehicleModelService from 'services/VehicleModelService';
import UserService from 'services/UserService';
import VehicleService from 'services/VehicleService';
import ServiceOrderService from 'services/ServiceOrderService';

import Header from './components/Header';
import List from './components/List';
import Columns from './components/Columns';
import Form from './components/Form';
import Pagination from 'components/pagination/Pagination';
import AssignForm from './components/AssignForm';
import ServiceForm from './components/ServiceForm'; // 🟢 form UI (UI only)

function OrderManagement() {
  const toast = useAppToast();
  const { user } = useUser();
  const bgColor = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('gray.200', 'navy.700');

  const [orders, setOrders] = useState([]);
  const [models, setModels] = useState([]);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false);

  const { isOpen, onOpen, onClose } = useDisclosure();
  const [assigningOrder, setAssigningOrder] = useState(null);
  const {
    isOpen: isAssignOpen,
    onOpen: onAssignOpen,
    onClose: onAssignClose,
  } = useDisclosure();

  const [selectedOrder, setSelectedOrder] = useState(null);
  const {
    isOpen: isServiceOpen,
    onOpen: onServiceOpen,
    onClose: onServiceClose,
  } = useDisclosure();

  // ✅ Load orders + createdBy name
  const loadOrders = useCallback(
    async (p = 1) => {
      try {
        setLoading(true);
        const res = await OrderService.get({
          pageNumber: p,
          pageSize: 10,
        });

        const orders = res.items || [];
        const userIds = [
          ...new Set(orders.map((o) => o.customerId).filter(Boolean)),
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
        models.forEach((m) => {
          modelMap[m.modelNumber] = m.name;
        });

        const withNames = orders.map((order) => ({
          ...order,
          customerName: userMap[order.customerId] || 'Unknown',
          modelName:
            modelMap[order.modelNumber] || order.modelNumber || 'Unknown',
        }));

        setOrders(withNames);
        setTotalPages(res.totalPages || 1);
      } catch (err) {
        toast.error('Failed to load orders');
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

  const handleAssignVehicle = (order) => {
    setAssigningOrder(order);
    onAssignOpen();
  };

  const handleAssigned = async (vehicle) => {
    try {
      setLoading(true);
      await OrderService.assignVehicle(
        assigningOrder.id,
        vehicle.vehicleId,
        user?.id,
      );
      await OrderService.updateStatus(assigningOrder.id, 2);
      await VehicleService.updateStatus(vehicle.vehicleId, 2);
      toast.success('Vehicle assigned successfully!');
      loadOrders(page);
    } catch (err) {
      toast.error('Failed to assign vehicle');
    } finally {
      setLoading(false);
    }
  };

  // 🟠 Mở form tạo Service Order
  const handleCreateService = (order) => {
    setSelectedOrder(order);
    onServiceOpen();
  };

  // ✅ Xử lý submit Service Order (POST)
  const handleSubmitService = async (formData) => {
    try {
      setLoading(true);
      const payload = {
        ...formData,
        orderId: selectedOrder.id,
        customerId: selectedOrder.customerId,
        createdBy: user?.id || '',
        cost: Number(formData.cost),
        appointmentDate: new Date(formData.appointmentDate).toISOString(),
      };

      await ServiceOrderService.create(payload);

      toast.success(`Service order created successfully`);
      onServiceClose();
      loadOrders(page);
    } catch (err) {
      console.error(err);
      toast.error('Failed to create service order');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadModels();
  }, []);

  useEffect(() => {
    if (models.length > 0) {
      loadOrders(page);
    }
  }, [models, page]);

  const columns = useMemo(
    () =>
      Columns({
        onAssign: handleAssignVehicle,
        onCreateService: handleCreateService,
      }),
    [handleAssignVehicle],
  );

  const table = useReactTable({
    data: orders,
    columns,
    getCoreRowModel: getCoreRowModel(),
  });

  return (
    <>
      {/* Create Order */}
      <Form
        isOpen={isOpen}
        onClose={onClose}
        reloadOrders={() => loadOrders(page)}
        models={models}
        bgColor={bgColor}
        textColor={textColor}
      />

      {/* Assign Vehicle */}
      <AssignForm
        isOpen={isAssignOpen}
        onClose={onAssignClose}
        order={assigningOrder}
        onAssigned={handleAssigned}
      />

      {/* Create Service Order */}
      <ServiceForm
        isOpen={isServiceOpen}
        onClose={onServiceClose}
        order={selectedOrder}
        onSubmit={handleSubmitService}
      />

      {/* Main List */}
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

export default OrderManagement;
