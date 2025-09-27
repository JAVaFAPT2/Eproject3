import React from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalCloseButton,
  Image,
  Text,
} from '@chakra-ui/react';

import Success from 'assets/gif/success.gif';
import Error from 'assets/gif/error.gif';

function blogbackDialog({ isOpen, onClose, type = 'success', message }) {
  const gifUrl = type === 'success' ? Success : Error;

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent textAlign="center">
        <ModalHeader>{type === 'success' ? 'Success!' : 'Error!'}</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <Image src={gifUrl} alt={type} mx="auto" boxSize="150px" />
          <Text fontSize="lg" fontWeight="500">
            {message}
          </Text>
        </ModalBody>
      </ModalContent>
    </Modal>
  );
}

export default blogbackDialog;
