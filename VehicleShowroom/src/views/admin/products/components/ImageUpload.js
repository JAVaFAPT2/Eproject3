import React from "react";
import { Box, Text, Image, IconButton, useToast } from "@chakra-ui/react";
import { MdDelete } from "react-icons/md";

export default function ImageUpload({ images, setImages }) {
  const toast = useToast();

  const fileToBase64 = (file) =>
    new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => resolve(reader.result);
      reader.onerror = (err) => reject(err);
    });

  const handleFiles = async (files) => {
    for (let file of files) {
      if (!file.type.startsWith("image/")) {
        toast({ title: "File must be an image", status: "error" });
        continue;
      }
      const base64 = await fileToBase64(file);
      setImages((prev) => [
        ...prev,
        { url: base64, is_thumbnail: prev.length === 0 },
      ]);
    }
  };

  const handleDrop = (e) => {
    e.preventDefault();
    handleFiles(e.dataTransfer.files);
  };

  const handleSelect = (e) => {
    handleFiles(e.target.files);
  };

  const removeImage = (index) => {
    setImages((prev) => prev.filter((_, i) => i !== index));
  };

  return (
    <Box>
      {/* Drag area */}
      <Box
        border="2px dashed"
        borderColor="gray.300"
        borderRadius="md"
        p={6}
        textAlign="center"
        cursor="pointer"
        onDrop={handleDrop}
        onDragOver={(e) => e.preventDefault()}
        onClick={() => document.getElementById("fileInput").click()}
        mb={4}
      >
        <Text color="gray.500">Click or drag file to upload</Text>
        <input
          id="fileInput"
          type="file"
          accept="image/*"
          multiple
          style={{ display: "none" }}
          onChange={handleSelect}
        />
      </Box>

      {/* Preview */}
      <Box display="flex" gap={3} flexWrap="wrap">
        {images.map((img, index) => (
          <Box key={index} position="relative" boxSize="120px">
            <Image
              src={img.url}
              alt={`preview-${index}`}
              boxSize="100%"
              objectFit="cover"
              borderRadius="md"
              border={img.is_thumbnail ? "2px solid green" : "1px solid gray"}
            />
            <IconButton
              aria-label="delete"
              icon={<MdDelete />}
              size="sm"
              colorScheme="red"
              position="absolute"
              top="2px"
              right="2px"
              onClick={() => removeImage(index)}
            />
          </Box>
        ))}
      </Box>
    </Box>
  );
}
