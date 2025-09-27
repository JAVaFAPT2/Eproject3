import React, { useState, useEffect } from "react";
import {
  Modal, ModalOverlay, ModalContent, ModalHeader, ModalBody,
  ModalFooter, ModalCloseButton, Button, FormControl, FormLabel,
  Input, NumberInput, NumberInputField, Textarea,
  SimpleGrid, IconButton, Table, Thead, Tbody, Tr, Th, Td,
  Image, useToast, Box, useColorModeValue
} from "@chakra-ui/react";
import { MdDelete } from "react-icons/md";
import ImageUpload from "./ImageUpload";
import ProductService from "services/ProductService";

export default function EditProduct({ isOpen, onClose, product, reloadProducts }) {
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState(0);
  const [stock, setStock] = useState(0);
  const [variants, setVariants] = useState([]);
  const [images, setImages] = useState([]);
  const [loading, setLoading] = useState(false);
  const toast = useToast();

  const textColor = useColorModeValue("secondaryGray.900", "white");
  const bgColor = useColorModeValue("white", "navy.800");
  const headerBg = useColorModeValue("gray.100", "navy.800");

  useEffect(() => {
    if (product) {
      setName(product.name || "");
      setDescription(product.description || "");
      setPrice(product.price || 0);
      setStock(product.stock || 0);
      setVariants(product.variants || []);
      setImages(product.images || []);
    }
  }, [product]);

  const handleSave = async () => {
    const totalVariantStock = variants.reduce(
      (sum, v) => sum + (parseInt(v.stock) || 0),
      0
    );


    if (totalVariantStock !== stock) {
      toast({
        title: "Variant stock mismatch",
        description: `Variants (${totalVariantStock}) ≠ Product stock (${stock})`,
        status: "error",
        duration: 4000,
        isClosable: true,
        position: "bottom-right",
      });
      return;
    }

    try {
      setLoading(true);
      const updated = {
        ...product,
        name,
        description,
        price,
        stock,
        variants,
        images: images.map((img, i) => ({
          url: img.url,
          is_thumbnail: i === 0,
        })),
      };

      const res = await ProductService.updateProduct(product.id, updated);
      reloadProducts(res);
      toast({
        title: "Product updated!",
        status: "success",
        duration: 3000,
        position: "bottom-right",
      });
      onClose();
    } catch (err) {
      toast({
        title: "Update failed",
        status: "error",
        duration: 3000,
        position: "bottom-right",
      });
    } finally {
      setLoading(false);
    }
  };
const handleDeleteVariant = (index) => {
  setVariants((prev) => prev.filter((_, i) => i !== index));
};
  return (
    <Modal isOpen={isOpen} onClose={onClose} size="xl" isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor}>
        <ModalHeader bg={headerBg} borderTopRadius="20px">
          Edit Product
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody maxH="70vh" overflowY="auto">
          {/* Name */}
          <FormControl mb={3}>
            <FormLabel color={textColor}>Name</FormLabel>
            <Input
              value={name}
              onChange={(e) => setName(e.target.value)}
              color={textColor}
            />
          </FormControl>

          {/* Description */}
          <FormControl mb={3}>
            <FormLabel color={textColor}>Description</FormLabel>
            <Textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              color={textColor}
            />
          </FormControl>

          {/* Price + Stock */}
          <SimpleGrid columns={2} spacing={4} mb={3}>
            <FormControl>
              <FormLabel color={textColor}>Price</FormLabel>
              <NumberInput
                value={price}
                min={0}
                onChange={(_, valueNumber) => setPrice(valueNumber)}
              >
                <NumberInputField color={textColor} />
              </NumberInput>
            </FormControl>
            <FormControl>
              <FormLabel color={textColor}>Stock</FormLabel>
              <NumberInput
                value={stock}
                min={0}
                onChange={(_, valueNumber) => setStock(valueNumber)}
              >
                <NumberInputField color={textColor} />
              </NumberInput>
            </FormControl>
          </SimpleGrid>

          {/* Images */}
          <FormControl mb={3}>
            <FormLabel color={textColor}>Images</FormLabel>
            <ImageUpload images={images} setImages={setImages} />
            <SimpleGrid columns={3} spacing={3} mt={3}>
              {images.map((img, i) => (
                <Box key={i} border="1px solid #eee" borderRadius="md" p={1}>
                  <Image
                    src={img.url}
                    alt={`img-${i}`}
                    borderRadius="md"
                    objectFit="cover"
                    boxSize="100px"
                    mx="auto"
                  />
                </Box>
              ))}
            </SimpleGrid>
          </FormControl>

          {/* Variants */}
          <FormLabel color={textColor}>Variants</FormLabel>
          <Table size="sm" variant="simple">
            <Thead>
              <Tr>
                <Th color={textColor}>Color</Th>
                <Th color={textColor}>Size</Th>
                <Th color={textColor}>Stock</Th>
                <Th textAlign="center" color={textColor}>Action</Th>
              </Tr>
            </Thead>
            <Tbody>
              {variants.map((v, i) => (
                <Tr key={i}>
                  <Td>
                    <Input
                      value={v.color}
                      onChange={(e) =>
                        setVariants((prev) =>
                          prev.map((item, idx) =>
                            idx === i ? { ...item, color: e.target.value } : item
                          )
                        )
                      }
                      color={textColor}
                    />
                  </Td>
                  <Td>
                    <Input
                      value={v.size}
                      onChange={(e) =>
                        setVariants((prev) =>
                          prev.map((item, idx) =>
                            idx === i ? { ...item, size: e.target.value } : item
                          )
                        )
                      }
                      color={textColor}
                    />
                  </Td>
                  <Td>
                    <NumberInput
                      value={v.stock}
                      min={0}
                      onChange={(_, valueNumber) =>
                        setVariants((prev) =>
                          prev.map((item, idx) =>
                            idx === i ? { ...item, stock: valueNumber } : item
                          )
                        )
                      }
                    >
                      <NumberInputField color={textColor} />
                    </NumberInput>
                  </Td>
                  <Td textAlign="center">
                    <IconButton
                      aria-label="delete"
                      icon={<MdDelete />}
                      colorScheme="red"
                      size="sm"
                      onClick={() => handleDeleteVariant(i)}
                    />
                  </Td>
                </Tr>
              ))}
            </Tbody>
          </Table>
        </ModalBody>
        <ModalFooter bg={headerBg} borderBottomRadius="20px">
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button colorScheme="green" onClick={handleSave} isLoading={loading}>
            Save
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
