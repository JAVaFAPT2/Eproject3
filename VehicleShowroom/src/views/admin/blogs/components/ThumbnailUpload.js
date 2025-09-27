import { Box, Image, Text, useColorModeValue } from '@chakra-ui/react';

export default function ThumbnailUploader({ thumbnail, onChange }) {
  const borderColor = useColorModeValue('gray.300', 'gray.600');
  const textColor = useColorModeValue('gray.500', 'gray.400');

  const handleFile = (file) => {
    if (!file) return;
    const reader = new FileReader();
    reader.onloadend = () => {
      onChange(reader.result);
    };
    reader.readAsDataURL(file);
  };

  return (
    <Box
      onClick={() => document.getElementById('thumbnail-input').click()}
      onDrop={(e) => {
        e.preventDefault();
        handleFile(e.dataTransfer.files[0]);
      }}
      onDragOver={(e) => e.preventDefault()}
      border="2px dashed"
      borderColor={borderColor}
      borderRadius="md"
      cursor="pointer"
      textAlign="center"
      p={6}
      minH="50px"
      display="flex"
      alignItems="center"
      justifyContent="center"
      flexDirection="column"
      color={textColor}
    >
      <input
        id="thumbnail-input"
        type="file"
        accept="image/*"
        onChange={(e) => handleFile(e.target.files[0])}
        hidden
      />
      {thumbnail ? (
        <Image
          src={thumbnail}
          alt="Thumbnail Preview"
          maxH="140px"
          objectFit="contain"
          borderRadius="md"
        />
      ) : (
        <Text fontSize="lg" fontWeight="semibold">
          Click or drag file to upload
        </Text>
      )}
    </Box>
  );
}
