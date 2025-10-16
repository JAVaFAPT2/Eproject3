import React, { useState, useEffect, useRef } from 'react';
import {
  Box,
  Image,
  Text,
  SimpleGrid,
  IconButton,
  Flex,
  useColorModeValue,
} from '@chakra-ui/react';
import { MdClose } from 'react-icons/md';

export default function ImageUploader({
  multiple = false,
  value = [],
  onChange,
}) {
  const borderColor = useColorModeValue('gray.300', 'gray.600');
  const textColor = useColorModeValue('gray.500', 'gray.400');
  const fileInputRef = useRef(null);

  const [files, setFiles] = useState([]);
  const [previews, setPreviews] = useState([]);

  // 🧠 Sync with parent value (for edit mode)
  useEffect(() => {
    if (Array.isArray(value) && value.length > 0) {
      setPreviews(value);
    } else {
      setPreviews([]);
    }
  }, [value]);

  // 🧹 Cleanup URLs on unmount
  useEffect(() => {
    return () => previews.forEach((p) => URL.revokeObjectURL(p));
  }, [previews]);

  // 📸 Handle file select
  const handleFiles = (selectedFiles) => {
    if (!selectedFiles || selectedFiles.length === 0) return;

    const fileArray = Array.from(selectedFiles);
    const previewArray = fileArray.map((file) => URL.createObjectURL(file));

    // ✅ If multiple = true → merge all
    const newFiles = multiple ? [...files, ...fileArray] : [fileArray[0]];
    const newPreviews = multiple
      ? [...previews, ...previewArray]
      : [previewArray[0]];

    setFiles(newFiles);
    setPreviews(newPreviews);
    onChange?.(newFiles, newPreviews);

    fileInputRef.current.value = ''; // allow reselecting same file
  };

  // 🗑️ Delete image by index
  const handleDelete = (index) => {
    const newFiles = [...files];
    const newPreviews = [...previews];

    URL.revokeObjectURL(newPreviews[index]);

    newFiles.splice(index, 1);
    newPreviews.splice(index, 1);

    setFiles(newFiles);
    setPreviews(newPreviews);
    onChange?.(newFiles, newPreviews);
  };

  // 📤 Open file dialog
  const handleClick = () => fileInputRef.current?.click();

  return (
    <Box>
      <Box
        border="2px dashed"
        borderColor={borderColor}
        borderRadius="md"
        cursor="pointer"
        textAlign="center"
        p={4}
        color={textColor}
        onClick={handleClick}
        transition="all 0.2s"
        _hover={{
          borderColor: 'blue.400',
          bg: useColorModeValue('gray.50', 'navy.700'),
        }}
      >
        <input
          ref={fileInputRef}
          type="file"
          accept="image/*"
          multiple={multiple}
          hidden
          onChange={(e) => handleFiles(e.target.files)}
        />

        {/* Nếu chưa có ảnh */}
        {previews.length === 0 && (
          <Text fontWeight="semibold">
            {multiple
              ? 'Click to select multiple images'
              : 'Click to upload an image'}
          </Text>
        )}

        {/* Nếu có ảnh */}
        {previews.length > 0 &&
          (multiple ? (
            <SimpleGrid columns={[2, 3, 4]} spacing={3} mt={3}>
              {previews.map((src, idx) => (
                <Box key={idx} position="relative">
                  <Image
                    src={src}
                    boxSize="90px"
                    objectFit="cover"
                    borderRadius="md"
                    border="1px solid"
                    borderColor={borderColor}
                    alt={`preview-${idx}`}
                  />

                  {/* 🔢 Display order number */}
                  <Text
                    position="absolute"
                    top={1}
                    left={1}
                    bg="blackAlpha.700"
                    color="white"
                    fontSize="xs"
                    fontWeight="bold"
                    px={2}
                    py={0.5}
                    borderRadius="md"
                  >
                    {idx + 1}
                  </Text>

                  {/* ❌ Delete button */}
                  <IconButton
                    aria-label="delete"
                    icon={<MdClose />}
                    size="xs"
                    colorScheme="red"
                    variant="solid"
                    position="absolute"
                    top={1}
                    right={1}
                    borderRadius="full"
                    onClick={(e) => {
                      e.stopPropagation();
                      handleDelete(idx);
                    }}
                  />
                </Box>
              ))}
            </SimpleGrid>
          ) : (
            <Flex justify="center" mt={3}>
              <Box position="relative" display="inline-block">
                <Image
                  src={previews[0]}
                  boxSize="160px"
                  objectFit="cover"
                  borderRadius="md"
                  border="2px solid"
                  borderColor="blue.400"
                  alt="preview"
                />
                <IconButton
                  aria-label="delete"
                  icon={<MdClose />}
                  size="sm"
                  colorScheme="red"
                  variant="solid"
                  position="absolute"
                  top={1}
                  right={1}
                  borderRadius="full"
                  onClick={(e) => {
                    e.stopPropagation();
                    handleDelete(0);
                  }}
                />
                {/* 🔢 Display order for single */}
                <Text
                  position="absolute"
                  top={1}
                  left={1}
                  bg="blackAlpha.700"
                  color="white"
                  fontSize="xs"
                  fontWeight="bold"
                  px={2}
                  py={0.5}
                  borderRadius="md"
                >
                  1
                </Text>
              </Box>
            </Flex>
          ))}
      </Box>
    </Box>
  );
}
