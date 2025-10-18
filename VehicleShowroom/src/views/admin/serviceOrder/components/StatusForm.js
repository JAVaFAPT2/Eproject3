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
  Select,
  Input,
  FormControl,
  FormLabel,
  useToast,
  Stack,
} from '@chakra-ui/react';
import ServiceOrderService from 'services/ServiceOrderService';

export default function StatusForm({ isOpen, onClose, order, reload }) {
  const toast = useToast();
  const [status, setStatus] = useState('');
  const [licensePlate, setLicensePlate] = useState('');
  const [loading, setLoading] = useState(false);

  // ✅ Đồng bộ lại khi order thay đổi
  useEffect(() => {
    if (order) {
      setStatus(order.status || '');
      setLicensePlate('');
    }
  }, [order]);

  const handleSave = async () => {
    try {
      setLoading(true);

      const payload = { status };
      if (status === 'Completed' && licensePlate.trim()) {
        payload.licensePlate = licensePlate.trim();
      }

      await ServiceOrderService.updateStatus(order.id, payload);
      toast({
        title: 'Status updated successfully',
        status: 'success',
        duration: 2000,
        isClosable: true,
      });
      onClose();
      reload();
    } catch (err) {
      toast({
        title: 'Failed to update status',
        status: 'error',
        duration: 2000,
        isClosable: true,
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Update Status</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <Stack spacing={4}>
            <FormControl>
              <FormLabel>Status</FormLabel>
              <Select
                value={status}
                onChange={(e) => setStatus(e.target.value)}
              >
                <option value="Scheduled">Scheduled</option>
                <option value="InProgress">In Progress</option>
                <option value="Completed">Completed</option>
                <option value="Cancelled">Cancelled</option>
              </Select>
            </FormControl>

            {status === 'Completed' && (
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
