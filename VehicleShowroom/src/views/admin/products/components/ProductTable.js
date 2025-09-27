import React, { useState, useMemo, useEffect } from "react";
import {
  Box, Card, Flex, Spinner, useDisclosure, useToast, 
  useColorModeValue
} from "@chakra-ui/react";
import ProductHeader from "./ProductHeader";
import ProductImageModal from "./ProductImageModal";
import ProductList from "./ProductList";
import AddProduct from "./AddProduct";
import EditProduct from "./EditProduct";
import AddVariants from "./AddVariants";
import ConfirmDialog from "components/dialog/ConfirmDialog";
import ProductService from "services/ProductService";
import CategoryService from "services/CategoryService";

export default function ProductTable() {
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(0);
  const [totalPages, setTotalPages] = useState(1);

  const [searchInput, setSearchInput] = useState("");
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

  // màu theo theme
  const bgColor = useColorModeValue("white", "navy.800");

  const loadProducts = async (p = 0) => {
    setLoading(true);
    try {
      const res = await ProductService.getProducts(p, 10);
      setProducts(res.content || res);
      setTotalPages(res.totalPages || 1);
    } catch {
      toast({ title: "Failed to load products", status: "error", position: "bottom-right" });
    } finally {
      setLoading(false);
    }
  };

  const loadCategories = async () => {
    try {
      const res = await CategoryService.getCategories(0, 50);
      setCategories(res.content || res);
    } catch {
      toast({ title: "Failed to load categories", status: "error", position: "bottom-right" });
    }
  };

  useEffect(() => {
    loadProducts(page);
    loadCategories();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [page]);

  const filteredProducts = useMemo(
    () => products.filter(p => p.name?.toLowerCase().includes(searchInput.toLowerCase())),
    [products, searchInput]
  );

  const handleDelete = async () => {
    if (!productToDelete) return;
    try {
      await ProductService.deleteProduct(productToDelete.id);
      toast({ title: "Deleted successfully", status: "success", position: "bottom-right" });
      loadProducts(page);
    } catch {
      toast({ title: "Failed to delete product", status: "error", position: "bottom-right" });
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
        reloadProducts={() => loadProducts(page)}
        categories={categories}
      />
      <EditProduct
        isOpen={isEditOpen}
        onClose={() => { setEditingProduct(null); onEditClose(); }}
        product={editingProduct}
        reloadProducts={() => loadProducts(page)}
        categories={categories}
      />
      <AddVariants
        isOpen={isVariantOpen}
        onClose={() => { setAddingVariantsProduct(null); onVariantClose(); }}
        product={addingVariantsProduct}
        onSave={() => loadProducts(page)}
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

      {/* Card chỉ có 1 header duy nhất */}
      <Card flexDirection="column" w="100%" borderRadius="16px" boxShadow="md" bg={bgColor}>
        <ProductHeader
          searchInput={searchInput}
          setSearchInput={setSearchInput}
          onAddOpen={onAddOpen}
        />

        <Box minH="400px" overflowX="auto" p={3}>
          {loading ? (
            <Flex justify="center" py={10}><Spinner size="lg" /></Flex>
          ) : (
            <ProductList
              products={filteredProducts}
              page={page}
              totalPages={totalPages}
              onPageChange={setPage}
              onViewImage={(p) => { setImageProduct(p); onImageOpen(); }}
              onAddVariants={(p) => { setAddingVariantsProduct(p); onVariantOpen(); }}
              onEdit={(p) => { setEditingProduct(p); onEditOpen(); }}
              onDelete={(p) => { setProductToDelete(p); setIsConfirmOpen(true); }}
            />
          )}
        </Box>
      </Card>
    </>
  );
}
