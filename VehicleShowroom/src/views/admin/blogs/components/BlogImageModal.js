import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalBody,
  Image,
} from '@chakra-ui/react';

export default function BlogImageModal({ isOpen, onClose, previewImage }) {
  return (
    <Modal isOpen={isOpen} onClose={onClose} size="xl" isCentered>
      <ModalOverlay bg="blackAlpha.700" />
      <ModalContent bg="transparent" boxShadow="none" w="100%">
        <ModalBody
          p={0}
          display="flex"
          justifyContent="center"
          alignItems="center"
        >
          {previewImage && (
            <Image
              src={previewImage}
              alt="Preview"
              maxH="80vh"
              maxW="90vw"
              borderRadius="md"
              boxShadow="lg"
            />
          )}
        </ModalBody>
      </ModalContent>
    </Modal>
  );
}
