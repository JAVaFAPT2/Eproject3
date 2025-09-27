import React, { useState, useEffect } from "react";
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalFooter,
  ModalCloseButton,
  Button,
  FormControl,
  FormLabel,
  Input,
  NumberInput,
  NumberInputField,
  IconButton,
  useToast,
  Divider,
  Text,
  Grid,
  useColorModeValue
} from "@chakra-ui/react";
import { MdDelete } from "react-icons/md";
import ProductService from "services/ProductService";

export default function AddVariants({ isOpen, onClose, product, onSave }) {
  const [variants, setVariants] = useState([]);
  const [loading, setLoading] = useState(false);
  const toast = useToast();

  useEffect(() => {
    setVariants(product?.variants || []);
  }, [product]);

  const handleAdd = () => {
    setVariants((prev) => [...prev, { color: "", size: "", stock: 0 }]);
  };

  const handleDelete = (index) => {
    setVariants((prev) => prev.filter((_, i) => i !== index));
  };

  const handleSave = async () => {
    const totalVariantStock = variants.reduce(
      (sum, v) => sum + (parseInt(v.stock) || 0),
      0
    );

    if (totalVariantStock !== product.stock) {
      toast({
        title: "Variant stock mismatch",
        description: `Tổng stock của variants (${totalVariantStock}) phải bằng stock của product (${product.stock})`,
        status: "error",
        duration: 4000,
        isClosable: true,
        position: "bottom-right",
      });
      return;
    }

    try {
      setLoading(true);
      const updated = await ProductService.updateProduct(product.id, {
        ...product,
        variants,
      });
      toast({
        title: "Variants updated",
        status: "success",
        duration: 3000,
        position: "bottom-right",
      });
      onSave(updated);
      onClose();
    } catch (err) {
      toast({
        title: "Failed to update variants",
        status: "error",
        duration: 3000,
        position: "bottom-right",
      });
    } finally {
      setLoading(false);
    }
  };
  const numbercolor = useColorModeValue("secondaryGray.900", "white");
  const textColor = useColorModeValue("secondaryGray.900", "white");
  const bgColor = useColorModeValue("white", "navy.800");
  const headerBg = useColorModeValue("gray.100", "navy.800");
  return (
    <Modal isOpen={isOpen} onClose={onClose} size="xl" isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor} >
        <ModalHeader bg={headerBg} borderTopRadius="20px">
          Variants cho {product?.name}
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          {variants.length === 0 && (
            <Text color={textColor} fontSize="sm"  mb={3}>
              Chưa có variant nào, hãy thêm mới bên dưới
            </Text>
          )}

          {variants.map((v, i) => (
            <Grid
              key={i}
              color={textColor}
              templateColumns="repeat(3, 1fr) 40px"
              gap={3}
              mb={3}
              alignItems="end"
            >
              <FormControl>
                <FormLabel color={textColor}>Color</FormLabel>
                <Input
                  color={textColor}
                  value={v.color}
                  onChange={(e) =>
                    setVariants((prev) =>
                      prev.map((item, idx) =>
                        idx === i ? { ...item, color: e.target.value } : item
                      )
                    )
                  }
                />
              </FormControl>

              <FormControl>
                <FormLabel>Size</FormLabel>
                <Input
                  color={textColor}
                  value={v.size}
                  onChange={(e) =>
                    setVariants((prev) =>
                      prev.map((item, idx) =>
                        idx === i ? { ...item, size: e.target.value } : item
                      )
                    )
                  }
                />
              </FormControl>

              <FormControl>
                <FormLabel color={textColor}>Stock</FormLabel>
                <NumberInput
                  color={numbercolor}
                  value={v.stock}
                  min={0}
                  onChange={(_, val) =>
                    setVariants((prev) =>
                      prev.map((item, idx) =>
                        idx === i ? { ...item, stock: val || 0 } : item
                      )
                    )
                  }
                >
                  <NumberInputField color={numbercolor} />
                </NumberInput>
              </FormControl>

              <IconButton
                aria-label="delete"
                icon={<MdDelete />}
                colorScheme="red"
                onClick={() => handleDelete(i)}
                mt="auto"
              />
            </Grid>
          ))}

          <Divider my={4} />
          <Button onClick={handleAdd} colorScheme="blue" size="sm">
            + Add Variant
          </Button>
        </ModalBody>
        <ModalFooter>
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button
            colorScheme="green"
            onClick={handleSave}
            isLoading={loading}
            loadingText="Saving..."
          >
            Save
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
