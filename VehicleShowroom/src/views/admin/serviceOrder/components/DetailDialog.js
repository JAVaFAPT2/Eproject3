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
} from '@chakra-ui/react';

export default function DetailDialog({ isOpen, onClose, order }) {
  if (!order) return null;

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
              {new Date(order.appointmentDate).toLocaleString()}
            </Text>
            <Text>
              <Text as="span" fontWeight="bold">
                Type:
              </Text>{' '}
              {order.type}
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
              {order.status}
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
