import React, { useState } from "react";
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalFooter,
  ModalBody,
  ModalCloseButton,
  Button,
  FormControl,
  FormLabel,
  Input,
  NumberInput,
  NumberInputField,
  Select,
  Textarea,
  Flex,
  useToast,
  useColorModeValue,
} from "@chakra-ui/react";
import ImageUpload from "./ImageUpload";
import ProductService from "services/ProductService";

export default function AddProduct({
  isOpen,
  onClose,
  reloadProducts,
  categories = [],
}) {
  const [categoryId, setCategoryId] = useState("");
  const [name, setName] = useState("");
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState(0);
  const [stock, setStock] = useState(0);
  const [images, setImages] = useState([]);

  const toast = useToast();
  const textColor = useColorModeValue("secondaryGray.900", "white");
  const bgColor = useColorModeValue("white", "navy.800");
  const headerBg = useColorModeValue("gray.100", "navy.800");
  const selectBorder = useColorModeValue("gray.300", "gray.600");
  const handleSave = async () => {
    if (!categoryId || !name || !price || !stock) {
      toast({
        title: "Please fill all required fields",
        status: "warning",
        duration: 3000,
        position: "bottom-right",
      });
      return;
    }

    const newProduct = {
      category_id: parseInt(categoryId),
      name,
      description,
      price: parseFloat(price),
      stock: parseInt(stock),
      images: images.map((img, i) => ({
        url: img.url,
        is_thumbnail: i === 0,
      })),
      variants: [],
    };

    try {
      await ProductService.createProduct(newProduct);
      toast({
        title: "Product added!",
        status: "success",
        duration: 3000,
        position: "bottom-right",
      });
      reloadProducts();
      // reset form
      setCategoryId("");
      setName("");
      setDescription("");
      setPrice(0);
      setStock(0);
      setImages([]);
      onClose();
    } catch (err) {
      toast({
        title: "Failed to add product",
        status: "error",
        duration: 3000,
        position: "bottom-right",
      });
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="lg" isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor}>
        <ModalHeader bg={headerBg} borderTopRadius="20px">
          Add Product
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          {/* Name + Category */}
          <Flex gap={4} mb={3}>
            <FormControl flex="6" isRequired>
              <FormLabel color={textColor}>Name</FormLabel>
              <Input
              
                value={name}
                onChange={(e) => setName(e.target.value)}

                placeholder="Product name"
                bg={bgColor}
                borderColor={selectBorder}
                color={textColor}
              _placeholder={{ color: useColorModeValue("gray.500", "gray.400") }}
              />
            </FormControl>

            <FormControl flex="4" isRequired>
  <FormLabel color={textColor}>Category</FormLabel>
  <Select
    placeholder="Category"
    value={categoryId}
    onChange={(e) => setCategoryId(e.target.value)}
    bg={bgColor}
    borderColor={selectBorder}
    color={textColor}
    _placeholder={{ color: useColorModeValue("gray.500", "gray.400") }}
  >
    {categories.map((cat) => (
      <option key={cat.id} value={cat.id}>
        {cat.name}
      </option>
    ))}
  </Select>
</FormControl>
          </Flex>

          {/* Description */}
          <FormControl mb={3}>
            <FormLabel color={textColor}>Description</FormLabel>
            <Textarea
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              placeholder="Description"
              color={textColor}
            />
          </FormControl>

          {/* Price + Stock */}
          <Flex gap={4} mb={3}>
            <FormControl isRequired>
              <FormLabel color={textColor}>Price</FormLabel>
              <NumberInput
                value={price}
                min={0}
                onChange={(valString, valNum) => setPrice(valNum)}
              >
                <NumberInputField color={textColor} />
              </NumberInput>
            </FormControl>

            <FormControl isRequired>
              <FormLabel color={textColor}>Stock</FormLabel>
              <NumberInput
                value={stock}
                min={0}
                onChange={(valString, valNum) => setStock(valNum)}
              >
                <NumberInputField color={textColor} />
              </NumberInput>
            </FormControl>
          </Flex>

          {/* Images */}
          <FormControl mb={3}>
            <FormLabel color={textColor}>Images</FormLabel>
            <ImageUpload images={images} setImages={setImages} />
          </FormControl>
        </ModalBody>

        <ModalFooter bg={headerBg} borderBottomRadius="20px">
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button colorScheme="green" onClick={handleSave}>
            Save
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
