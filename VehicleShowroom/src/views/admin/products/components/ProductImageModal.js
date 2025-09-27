import React, { useState, useEffect } from "react";
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  Flex,
  Image,
  IconButton,
  Box,
  Text
} from "@chakra-ui/react";
import { MdNavigateBefore, MdNavigateNext, MdZoomIn, MdZoomOut } from "react-icons/md";

export default function ProductImageModal({ isOpen, onClose, product }) {
  const [currentIndex, setCurrentIndex] = useState(0);
  const [zoom, setZoom] = useState(1);

  useEffect(() => {
    if (product?.images?.length > 0) {
      const thumbIndex = product.images.findIndex((img) => img.is_thumbnail);
      setCurrentIndex(thumbIndex !== -1 ? thumbIndex : 0);
    } else {
      setCurrentIndex(0);
    }
    setZoom(1);
  }, [product]);

  if (!product) return null;

  const images = product.images?.length
    ? product.images
    : [{ url: "https://via.placeholder.com/500x500?text=No+Image" }];

  const handlePrev = () => {
    setCurrentIndex((prev) => (prev === 0 ? images.length - 1 : prev - 1));
    setZoom(1);
  };

  const handleNext = () => {
    setCurrentIndex((prev) => (prev === images.length - 1 ? 0 : prev + 1));
    setZoom(1);
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="xl" isCentered>
      <ModalOverlay />
      <ModalContent bg="black" color="white">
        <ModalHeader>{product.name} - Images</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <Flex direction="column" align="center" gap={4}>
            <Box
              position="relative"
              w="500px"
              h="500px"
              borderRadius="md"
              overflow="hidden"
              bg="gray.800"
            >
              <Image
                src={images[currentIndex]?.url}
                alt={`product-img-${currentIndex}`}
                boxSize="500px"
                objectFit="contain"
                transform={`scale(${zoom})`}
                transition="transform 0.2s ease"
                mx="auto"
              />

              {images.length > 1 && (
                <>
                  <IconButton
                    aria-label="prev"
                    icon={<MdNavigateBefore />}
                    onClick={handlePrev}
                    position="absolute"
                    top="50%"
                    left="10px"
                    transform="translateY(-50%)"
                    colorScheme="whiteAlpha"
                    size="lg"
                  />
                  <IconButton
                    aria-label="next"
                    icon={<MdNavigateNext />}
                    onClick={handleNext}
                    position="absolute"
                    top="50%"
                    right="10px"
                    transform="translateY(-50%)"
                    colorScheme="whiteAlpha"
                    size="lg"
                  />
                </>
              )}
            </Box>

            {/* Zoom Controls */}
            <Flex gap={3}>
              <IconButton
                aria-label="zoom-in"
                icon={<MdZoomIn />}
                onClick={() => setZoom((z) => Math.min(z + 0.2, 3))}
              />
              <IconButton
                aria-label="zoom-out"
                icon={<MdZoomOut />}
                onClick={() => setZoom((z) => Math.max(z - 0.2, 1))}
              />
            </Flex>

            <Text>
              {currentIndex + 1} / {images.length}
            </Text>
          </Flex>
        </ModalBody>
      </ModalContent>
    </Modal>
  );
}
