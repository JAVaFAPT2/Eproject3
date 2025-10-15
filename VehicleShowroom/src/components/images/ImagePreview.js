import React, { useCallback, useEffect, useState } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalCloseButton,
  ModalBody,
  Image,
  IconButton,
  Flex,
  Box,
  HStack,
  useColorModeValue,
  Divider,
} from '@chakra-ui/react';
import { MdChevronLeft, MdChevronRight } from 'react-icons/md';

export default function ImagePreview({
  isOpen,
  onClose,
  images = [],
  initialIndex = 0,
}) {
  const [currentIndex, setCurrentIndex] = useState(initialIndex);

  const activeBorder = useColorModeValue('brand.500', 'brand.400');
  const bgColor = useColorModeValue('white', 'navy.800');
  const chevronBg = useColorModeValue('whiteAlpha.700', 'gray.700');
  const chevronColor = useColorModeValue('gray.700', 'white');
  const textColor = useColorModeValue('secondaryGray.900', 'white');

  useEffect(() => {
    if (isOpen) setCurrentIndex(initialIndex);
  }, [isOpen, initialIndex]);

  const prevImage = useCallback(() => {
    setCurrentIndex((prev) => (prev > 0 ? prev - 1 : images.length - 1));
  }, [images.length]);

  const nextImage = useCallback(() => {
    setCurrentIndex((prev) => (prev < images.length - 1 ? prev + 1 : 0));
  }, [images.length]);

  if (!images || images.length === 0) return null;

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="4xl" isCentered>
      <ModalOverlay bg="blackAlpha.600" backdropFilter="blur(6px)" />
      <ModalContent bg="transparent" boxShadow="none">
        <ModalCloseButton color={textColor} size="lg" zIndex={10} />
        <ModalBody bg={bgColor} borderRadius="20px">
          {/* ✅ Image viewer */}
          <Flex position="relative" align="center" justify="center" w="full">
            {/* Chevron Left */}
            {images.length > 1 && (
              <IconButton
                aria-label="Previous"
                icon={<MdChevronLeft size={36} />}
                position="absolute"
                left={4}
                top="50%"
                transform="translateY(-50%)"
                bg={chevronBg}
                color={chevronColor}
                _hover={{ bg: 'brand.500', color: 'white' }}
                boxShadow="lg"
                borderRadius="full"
                onClick={prevImage}
              />
            )}

            {/* Main Image */}
            <Box w="full" textAlign="center">
              <Image
                src={images[currentIndex]}
                maxH="70vh"
                objectFit="contain"
                mx="auto"
                borderRadius="lg"
                transition="all 0.3s ease"
              />
            </Box>

            {/* Chevron Right */}
            {images.length > 1 && (
              <IconButton
                aria-label="Next"
                icon={<MdChevronRight size={36} />}
                position="absolute"
                right={4}
                top="50%"
                transform="translateY(-50%)"
                bg={chevronBg}
                color={chevronColor}
                _hover={{ bg: 'brand.500', color: 'white' }}
                boxShadow="lg"
                borderRadius="full"
                onClick={nextImage}
              />
            )}
          </Flex>

          {/* ✅ Thumbnails */}
          {images.length > 1 && (
            <>
              <Divider my={4} />
              <HStack
                justify="center"
                spacing={3}
                py={2}
                px={3}
                overflowX="auto"
                w="full"
              >
                {images.map((img, idx) => (
                  <Box
                    key={idx}
                    borderRadius="md"
                    overflow="hidden"
                    borderWidth="2px"
                    borderColor={idx === currentIndex ? activeBorder : 'transparent'}
                    cursor="pointer"
                    onClick={() => setCurrentIndex(idx)}
                    transition="all 0.2s"
                    _hover={{ borderColor: activeBorder }}
                  >
                    <Image
                      src={img}
                      boxSize="60px"
                      objectFit="cover"
                      borderRadius="md"
                    />
                  </Box>
                ))}
              </HStack>
            </>
          )}
        </ModalBody>
      </ModalContent>
    </Modal>
  );
}
