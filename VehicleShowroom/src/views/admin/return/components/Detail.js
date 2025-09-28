import React from 'react';
import { Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody, ModalCloseButton, Text } from '@chakra-ui/react';

export default function Detail({ isOpen, onClose, employee }) {
  if (!employee) return null;
  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Employee Detail</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <Text><strong>Email:</strong> {employee.email}</Text>
          <Text><strong>Name:</strong> {employee.fullName}</Text>
          <Text><strong>Hourly Rate:</strong> ${employee.hourlyRate}</Text>
          <Text><strong>Created:</strong> {employee.createdAt}</Text>
        </ModalBody>
      </ModalContent>
    </Modal>
  );
}
