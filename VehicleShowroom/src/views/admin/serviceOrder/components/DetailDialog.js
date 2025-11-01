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
  Flex,
  Stack,
  Badge,
  Divider,
  useColorModeValue,
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
  const labelColor = useColorModeValue('gray.600', 'gray.300');
  const valueColor = useColorModeValue('gray.800', 'white');
  const cardBg = useColorModeValue('gray.50', 'navy.700');

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
    <Modal isOpen={isOpen} onClose={onClose} size="lg" isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="16px" bg={cardBg}>
        <ModalHeader fontSize="xl" fontWeight="700" textAlign="center">
          Service Order Detail
        </ModalHeader>
        <ModalCloseButton />

        <ModalBody>
          <Stack spacing={3} divider={<Divider />}>
            {/* ID */}
            <Flex justify="space-between" align="center">
              <Text color={labelColor} fontWeight="medium">
                Order ID
              </Text>
              <Text color={valueColor} fontWeight="semibold">
                {order.id}
              </Text>
            </Flex>

            {/* Created By */}
            <Flex justify="space-between" align="center">
              <Text color={labelColor} fontWeight="medium">
                Created By
              </Text>
              <Text color={valueColor}>{order.createdByName}</Text>
            </Flex>

            {/* Customer */}
            <Flex justify="space-between" align="center">
              <Text color={labelColor} fontWeight="medium">
                Customer
              </Text>
              <Text color={valueColor}>{order.customerName}</Text>
            </Flex>

            {/* Appointment Date */}
            <Flex justify="space-between" align="center">
              <Text color={labelColor} fontWeight="medium">
                Appointment
              </Text>
              <Text color={valueColor}>
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
            </Flex>

            {/* Type */}
            <Flex justify="space-between" align="center">
              <Text color={labelColor} fontWeight="medium">
                Type
              </Text>
              <Badge colorScheme={typeInfo.color}>{typeInfo.label}</Badge>
            </Flex>

            {/* Cost */}
            <Flex justify="space-between" align="center">
              <Text color={labelColor} fontWeight="medium">
                Cost
              </Text>
              <Text color="green.500" fontWeight="semibold">
                ${order.cost?.toLocaleString() || '0'}
              </Text>
            </Flex>

            {/* Description */}
            <Flex justify="space-between" align="start">
              <Text color={labelColor} fontWeight="medium">
                Description
              </Text>
              <Text
                color={valueColor}
                textAlign="right"
                whiteSpace="pre-wrap"
                maxW="60%"
              >
                {order.description || '—'}
              </Text>
            </Flex>

            {/* Status */}
            <Flex justify="space-between" align="center">
              <Text color={labelColor} fontWeight="medium">
                Status
              </Text>
              <Badge colorScheme={statusInfo.color}>{statusInfo.label}</Badge>
            </Flex>
          </Stack>
        </ModalBody>

        <ModalFooter justifyContent="end">
          <Button onClick={onClose}>Close</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
