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

  // 🧠 Đồng bộ giá trị từ prop (nếu edit có sẵn thumbnail)
  useEffect(() => {
    if (value && value.length > 0) {
      setPreviews(value);
    } else {
      setPreviews([]);
    }
  }, [value]);

  // 📸 Xử lý chọn file
  const handleFiles = (selectedFiles) => {
    if (!selectedFiles || selectedFiles.length === 0) return;
    const fileArray = Array.from(selectedFiles);

    // Tạo preview cho mỗi ảnh
    const previewArray = fileArray.map((file) => URL.createObjectURL(file));

    // Hợp nhất nếu multiple
    let newFiles = multiple ? [...files, ...fileArray] : [fileArray[0]];
    let newPreviews = multiple
      ? [...previews, ...previewArray]
      : [previewArray[0]];

    setFiles(newFiles);
    setPreviews(newPreviews);
    onChange(newFiles, newPreviews);
  };

  // 🗑️ Xóa ảnh tại index
  const handleDelete = (index) => {
    const newFiles = [...files];
    const newPreviews = [...previews];

    newFiles.splice(index, 1);
    newPreviews.splice(index, 1);

    setFiles(newFiles);
    setPreviews(newPreviews);
    onChange(newFiles, newPreviews);
  };

  // 📤 Click vào vùng input để chọn ảnh
  const handleClick = () => fileInputRef.current?.click();

  return (
    <Box>
      {/* Input chọn file */}
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
              ? 'Click or drop multiple images (first will be thumbnail)'
              : 'Click or drop image to upload'}
          </Text>
        )}

        {/* Nếu có ảnh preview */}
        {previews.length > 0 &&
          (multiple ? (
            <SimpleGrid columns={[2, 3, 4]} spacing={3} mt={2}>
              {previews.map((src, idx) => (
                <Box key={idx} position="relative">
                  <Image
                    src={src}
                    boxSize="90px"
                    objectFit="cover"
                    borderRadius="md"
                    border={idx === 0 ? '2px solid' : '1px solid'}
                    borderColor={idx === 0 ? 'brand.400' : borderColor}
                    alt={`preview-${idx}`}
                  />
                  {/* 🏷️ Thumbnail tag */}
                  {idx === 0 && (
                    <Text
                      position="absolute"
                      bottom={1}
                      left={1}
                      bg="brand.400"
                      color="white"
                      fontSize="10px"
                      px={2}
                      py={0.5}
                      borderRadius="sm"
                    >
                      Thumbnail
                    </Text>
                  )}
                  {/* ❌ Nút xóa */}
                  <IconButton
                    aria-label="delete"
                    icon={<MdClose />}
                    size="xs"
                    colorScheme="red"
                    variant="solid"
                    position="absolute"
                    top={-2}
                    right={-1}
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
            // 🖼️ Single preview (ở giữa)
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
                  top={-3}
                  right={-3}
                  borderRadius="full"
                  onClick={(e) => {
                    e.stopPropagation();
                    handleDelete(0);
                  }}
                />
              </Box>
            </Flex>
          ))}
      </Box>
    </Box>
  );
}
