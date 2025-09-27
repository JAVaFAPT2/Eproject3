import React, { useState, useEffect } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalCloseButton,
  ModalBody,
  ModalFooter,
  FormControl,
  FormLabel,
  Input,
  Button,
  VStack,
  useToast,
  useColorModeValue,
} from '@chakra-ui/react';
import CategoryService from 'services/CategoryService';

export default function CategoryForm({
  isOpen,
  onClose,
  reloadCategories,
  category,
  parentCategory,
}) {
  const toast = useToast();

  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [loading, setLoading] = useState(false);

  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const bgColor = useColorModeValue('white', 'navy.800');
  const headerBg = useColorModeValue('gray.100', 'navy.800');

  useEffect(() => {
    if (category) {
      setName(category.name || '');
      setDescription(category.description || '');
    } else {
      setName('');
      setDescription('');
    }
  }, [category, isOpen]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      if (category) {
        await CategoryService.updateCategory(category.id, {
          name,
          description,
        });
        toast({
          title: 'Category updated successfully',
          status: 'success',
          duration: 3000,
          isClosable: true,
          position: 'bottom-right',
        });
      } else {
        await CategoryService.createCategory({
          name,
          description,
          parentId: parentCategory ? parentCategory.id : null,
        });
        toast({
          title: parentCategory
            ? 'Subcategory created successfully'
            : 'Category created successfully',
          status: 'success',
          duration: 3000,
          isClosable: true,
          position: 'bottom-right',
        });
      }

      if (reloadCategories) reloadCategories();
      onClose();
    } catch (err) {
      console.error(err);
      toast({
        title: 'Error saving category',
        status: 'error',
        duration: 3000,
        isClosable: true,
      });
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor}>
        <ModalHeader bg={headerBg} borderTopRadius="20px">
          {category
            ? 'Edit Category'
            : parentCategory
            ? `Add Subcategory to "${parentCategory.name}"`
            : 'Create New Category'}
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <VStack spacing={4} align="flex-start">
            <FormControl isRequired>
              <FormLabel>Name</FormLabel>
              <Input
                color={textColor}
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="Enter category name"
              />
            </FormControl>

            <FormControl>
              <FormLabel>Description</FormLabel>
              <Input
                color={textColor}
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                placeholder="Enter description"
              />
            </FormControl>
          </VStack>
        </ModalBody>

        <ModalFooter bg={headerBg} borderBottomRadius="20px">
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button
            colorScheme="green"
            type="submit"
            form="category-form"
            isLoading={loading}
            onClick={handleSubmit}
          >
            {category ? 'Update' : 'Create'}
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
