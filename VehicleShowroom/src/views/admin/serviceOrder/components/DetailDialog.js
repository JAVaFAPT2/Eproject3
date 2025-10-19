import React from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  ModalFooter,
  Button,
  Text,
  Stack,
  Badge,
} from '@chakra-ui/react';

// ✅ Map enum sang label + màu hiển thị
const STATUS_MAP = {
  1: { label: 'Scheduled', color: 'orange' },
  2: { label: 'In Progress', color: 'blue' },
  3: { label: 'Completed', color: 'green' },
  4: { label: 'Cancelled', color: 'red' },
};

// ✅ Map cho type
const TYPE_MAP = {
  1: { label: 'PreDelivery', color: 'purple' },
  2: { label: 'Maintenance', color: 'cyan' },
  3: { label: 'Repair', color: 'teal' },
};

export default function DetailDialog({ isOpen, onClose, order }) {
  if (!order) return null;

  const statusInfo = STATUS_MAP[order.status] || {
    label: 'Unknown',
    color: 'gray',
  };

  const typeInfo = TYPE_MAP[order.type] || {
    label: 'Unknown',
    color: 'gray',
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="md" isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Service Order Detail</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <Stack spacing={2}>
            <Text>
              <Text as="span" fontWeight="bold">
                Order ID:
              </Text>{' '}
              {order.id}
            </Text>
            <Text>
              <Text as="span" fontWeight="bold">
                Created By:
              </Text>{' '}
              {order.createdByName}
            </Text>
            <Text>
              <Text as="span" fontWeight="bold">
                Customer:
              </Text>{' '}
              {order.customerName}
            </Text>
            <Text>
              <Text as="span" fontWeight="bold">
                Appointment:
              </Text>{' '}
              {order.appointmentDate
                ? new Date(order.appointmentDate).toLocaleString('en-US', {
                    month: '2-digit',
                    day: '2-digit',
                    year: 'numeric',
                    hour: '2-digit',
                    minute: '2-digit',
                    hour12: true,
                  })
                : '—'}
            </Text>
            <Text>
              <Text as="span" fontWeight="bold">
                Type:
              </Text>{' '}
              <Badge colorScheme={typeInfo.color}>{typeInfo.label}</Badge>
            </Text>
            <Text>
              <Text as="span" fontWeight="bold">
                Cost:
              </Text>{' '}
              ${order.cost}
            </Text>
            <Text>
              <Text as="span" fontWeight="bold">
                Description:
              </Text>{' '}
              {order.description || '—'}
            </Text>
            <Text>
              <Text as="span" fontWeight="bold">
                Status:
              </Text>{' '}
              <Badge colorScheme={statusInfo.color}>{statusInfo.label}</Badge>
            </Text>
          </Stack>
        </ModalBody>
        <ModalFooter>
          <Button onClick={onClose}>Close</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
