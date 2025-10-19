import React, { useState, useEffect } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  ModalFooter,
  Button,
  Input,
  FormControl,
  FormLabel,
  Stack,
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  Flex,
  Text,
  Box,
} from '@chakra-ui/react';
import { ChevronDownIcon } from '@chakra-ui/icons';
import ServiceOrderService from 'services/ServiceOrderService';
import VehicleService from 'services/VehicleService';
import OrderService from 'services/OrderService';
import { useAppToast } from 'utils/ToastHelper';

const STATUS_OPTIONS = [
  { label: 'Scheduled', value: 1 },
  { label: 'In Progress', value: 2 },
  { label: 'Completed', value: 3 },
  { label: 'Cancelled', value: 4 },
];

export default function StatusForm({ isOpen, onClose, order, reload }) {
  const toast = useAppToast();
  const [status, setStatus] = useState(null);
  const [licensePlate, setLicensePlate] = useState('');
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (order) {
      setStatus(order.status || null);
      setLicensePlate('');
    }
  }, [order]);

  const handleSave = async () => {
    try {
      setLoading(true);

      const payload = { status, licensePlate: licensePlate.trim() || null  };
      const selected = STATUS_OPTIONS.find((s) => s.value === status);

      // 🔹 Gọi API update status của ServiceOrder
      await ServiceOrderService.updateStatus(order.id, payload);

      // 🔹 Nếu Completed → cần cập nhật cả Vehicle & Order
      if (selected?.label === 'Completed') {
        let vehicleId = null;

        // 👉 Gọi order theo orderId để lấy vehicleId
        if (order?.orderId) {
          const orderData = await OrderService.getById(order.orderId);
          vehicleId = orderData?.vehicleId;
        }

        if (vehicleId) {
          await VehicleService.updateStatus(vehicleId, 3);
        }

        await OrderService.updateStatus(order.orderId, 3);
      }

      toast.success('Status updated successfully');
      onClose();
      reload();
    } catch (err) {
      console.error(err);
      toast.error('Failed to update status');
    } finally {
      setLoading(false);
    }
  };

  const currentLabel =
    STATUS_OPTIONS.find((opt) => opt.value === status)?.label ||
    'Select status';

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Update Status</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <Stack spacing={4}>
            {/* 🔹 Menu thay cho Select */}
            <FormControl>
              <FormLabel>Status</FormLabel>
              <Menu>
                <MenuButton
                  as={Button}
                  rightIcon={<ChevronDownIcon />}
                  w="100%"
                  variant="outline"
                >
                  {currentLabel}
                </MenuButton>
                <MenuList>
                  {STATUS_OPTIONS.map((opt) => (
                    <MenuItem
                      key={opt.value}
                      onClick={() => setStatus(opt.value)}
                    >
                      <Flex
                        align="center"
                        justify="space-between"
                        w="full"
                        px={3}
                        position="relative"
                      >
                        <Text>{opt.label}</Text>
                        {status === opt.value && (
                          <Box
                            position="absolute"
                            right="0"
                            top="0"
                            bottom="0"
                            width="4px"
                            bg="brand.500"
                            borderTopRightRadius="md"
                            borderBottomRightRadius="md"
                          />
                        )}
                      </Flex>
                    </MenuItem>
                  ))}
                </MenuList>
              </Menu>
            </FormControl>

            {/* 🔹 Chỉ hiển thị nếu Completed */}
            {status === 3 && (
              <FormControl>
                <FormLabel>License Plate</FormLabel>
                <Input
                  placeholder="Enter new license plate"
                  value={licensePlate}
                  onChange={(e) => setLicensePlate(e.target.value)}
                />
              </FormControl>
            )}
          </Stack>
        </ModalBody>
        <ModalFooter>
          <Button onClick={onClose} mr={3}>
            Cancel
          </Button>
          <Button colorScheme="green" isLoading={loading} onClick={handleSave}>
            Save
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
