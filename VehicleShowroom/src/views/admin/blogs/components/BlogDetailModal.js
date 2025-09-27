import React from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Button,
  Box,
  useColorModeValue,
} from '@chakra-ui/react';

export default function BlogDetailModal({ isOpen, onClose, blog }) {
  const textColor = useColorModeValue('secondaryGray.900', 'white');

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="xl" scrollBehavior="inside">
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>{blog?.title}</ModalHeader>
        <ModalBody>
          <Box
            dangerouslySetInnerHTML={{ __html: blog?.content || '' }}
            sx={{
              color: textColor,
              lineHeight: 1.6,
              img: { maxW: '100%', borderRadius: 'md', my: 2 },
            }}
          />
        </ModalBody>
        <ModalFooter>
          <Button onClick={onClose}>Close</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
