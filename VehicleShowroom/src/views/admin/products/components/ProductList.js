import React, { useState, useMemo, useEffect } from "react";
import {
  Flex,
  Box,
  Table,
  Tbody,
  Td,
  Th,
  Tr,
  Thead,
  Card,
  IconButton,
  useDisclosure,
  useToast,
  Spinner,
  useColorModeValue,
} from "@chakra-ui/react";
import { MdEdit, MdDelete, MdAdd } from "react-icons/md";
import { RiEyeFill } from "react-icons/ri";
import AddProduct from "./AddProduct";
import EditProduct from "./EditProduct";
import AddVariants from "./AddVariants";
import ConfirmDialog from "components/dialog/ConfirmDialog";
import ProductService from "services/ProductService";
import CategoryService from "services/CategoryService";
import ProductImageModal from "./ProductImageModal";

export default function ProductList({ searchInput }) {
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(false);

  const [editingProduct, setEditingProduct] = useState(null);
  const [addingVariantsProduct, setAddingVariantsProduct] = useState(null);
  const [imageProduct, setImageProduct] = useState(null);
  const [productToDelete, setProductToDelete] = useState(null);
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);

  const { isOpen: isAddOpen, onOpen: onAddOpen, onClose: onAddClose } = useDisclosure();
  const { isOpen: isEditOpen, onOpen: onEditOpen, onClose: onEditClose } = useDisclosure();
  const { isOpen: isVariantOpen, onOpen: onVariantOpen, onClose: onVariantClose } = useDisclosure();
  const { isOpen: isImageOpen, onOpen: onImageOpen, onClose: onImageClose } = useDisclosure();

  const toast = useToast();

  const textColor = useColorModeValue("secondaryGray.900", "white");
  const bgColor = useColorModeValue("white", "navy.800");
  const headerBg = useColorModeValue("gray.100", "navy.700");
  const borderColor = useColorModeValue("gray.200", "gray.600");

  const loadProducts = async () => {
    setLoading(true);
    try {
      const res = await ProductService.getProducts(0, 20);
      setProducts(res.content || res);
    } catch {
      toast({
        title: "Failed to load products",
        status: "error",
        duration: 3000,
        position: "bottom-right",
      });
    } finally {
      setLoading(false);
    }
  };

  const loadCategories = async () => {
    try {
      const res = await CategoryService.getCategories(0, 50);
      setCategories(res.content || res);
    } catch {
      toast({
        title: "Failed to load categories",
        status: "error",
        duration: 3000,
        position: "bottom-right",
      });
    }
  };

  useEffect(() => {
    loadProducts();
    loadCategories();
  }, []);

  const filteredProducts = useMemo(() => {
    let data = products;
    if (searchInput) {
      data = data.filter((p) =>
        p.name.toLowerCase().includes(searchInput.toLowerCase())
      );
    }
    return data;
  }, [products, searchInput]);

  const handleDelete = async () => {
    if (!productToDelete) return;
    try {
      await ProductService.deleteProduct(productToDelete.id);
      toast({
        title: "Deleted successfully",
        status: "success",
        duration: 2000,
        position: "bottom-right",
      });
      loadProducts();
    } catch {
      toast({
        title: "Failed to delete product",
        status: "error",
        duration: 2000,
        position: "bottom-right",
      });
    } finally {
      setProductToDelete(null);
      setIsConfirmOpen(false);
    }
  };

  return (
    <>
      {/* Modals */}
      <AddProduct
        isOpen={isAddOpen}
        onClose={onAddClose}
        reloadProducts={loadProducts}
        categories={categories}
      />

      <EditProduct
        isOpen={isEditOpen}
        onClose={() => {
          setEditingProduct(null);
          onEditClose();
        }}
        product={editingProduct}
        reloadProducts={loadProducts}
        categories={categories}
      />

      <AddVariants
        isOpen={isVariantOpen}
        onClose={() => {
          setAddingVariantsProduct(null);
          onVariantClose();
        }}
        product={addingVariantsProduct}
        onSave={loadProducts}
      />

      <ProductImageModal
        isOpen={isImageOpen}
        onClose={onImageClose}
        product={imageProduct}
      />

      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => setIsConfirmOpen(false)}
        onConfirm={handleDelete}
        title="Delete Product"
        message={`Are you sure you want to delete product ${productToDelete?.name}?`}
      />

      {/* Table only (no header here) */}
      <Card flexDirection="column" w="100%" borderRadius="16px" boxShadow="md" bg={bgColor}>
        <Box minH={"400px"} overflowX="auto" p={3}>
          {loading ? (
            <Flex justify="center" py={10}>
              <Spinner size="lg" />
            </Flex>
          ) : (
            <Table variant="simple" bg={bgColor}>
              <Thead bg={headerBg}>
                <Tr>
                  <Th color={textColor} borderColor={borderColor}>NAME</Th>
                  <Th color={textColor} borderColor={borderColor}>PRICE</Th>
                  <Th color={textColor} borderColor={borderColor}>STOCK</Th>
                  <Th color={textColor} borderColor={borderColor}>CREATED AT</Th>
                  <Th textAlign="right" color={textColor} borderColor={borderColor}>ACTIONS</Th>
                </Tr>
              </Thead>
              <Tbody>
                {filteredProducts.map((prod) => (
                  <Tr key={prod.id}>
                    <Td color={textColor} borderColor={borderColor}>{prod.name}</Td>
                    <Td color={textColor} borderColor={borderColor}>${prod.price}</Td>
                    <Td color={textColor} borderColor={borderColor}>{prod.stock}</Td>
                    <Td color={textColor} borderColor={borderColor}>
                      {new Date(prod.createdAt).toLocaleString()}
                    </Td>
                    <Td borderColor={borderColor}>
                      <Flex justify="flex-end" gap={2}>
                        <IconButton
                          aria-label="view-image"
                          size="sm"
                          icon={<RiEyeFill />}
                          colorScheme="purple"
                          onClick={() => {
                            setImageProduct(prod);
                            onImageOpen();
                          }}
                        />
                        <IconButton
                          aria-label="add-variants"
                          size="sm"
                          icon={<MdAdd />}
                          colorScheme="green"
                          onClick={() => {
                            setAddingVariantsProduct(prod);
                            onVariantOpen();
                          }}
                        />
                        <IconButton
                          aria-label="edit"
                          size="sm"
                          icon={<MdEdit />}
                          colorScheme="blue"
                          onClick={() => {
                            setEditingProduct(prod);
                            onEditOpen();
                          }}
                        />
                        <IconButton
                          aria-label="delete"
                          size="sm"
                          icon={<MdDelete />}
                          colorScheme="red"
                          onClick={() => {
                            setProductToDelete(prod);
                            setIsConfirmOpen(true);
                          }}
                        />
                      </Flex>
                    </Td>
                  </Tr>
                ))}
              </Tbody>
            </Table>
          )}
        </Box>
      </Card>
    </>
  );
}
