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
  Textarea,
  Button,
  VStack,
} from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import VehicleModelService from 'services/VehicleModelService';
import ImageUploader from 'components/images/ImageUploader';

export default function Form({
  isOpen,
  onClose,
  reloadModels,
  model,
  parentModel,
  textColor,
  bgColor,
}) {
  const toast = useAppToast();
  const [formData, setFormData] = useState({
    modelNumber: '',
    name: '',
    price: 0,
    description: '',
    parentId: '',
    level: 1,
    slug: '',
    files: [],
  });
  const [previews, setPreviews] = useState([]); // lưu preview images
  const [loading, setLoading] = useState(false);

  // 🧩 Khởi tạo hoặc reset khi mở modal
  useEffect(() => {
    if (model) {
      setFormData({
        ...model,
        files: [],
      });
      setPreviews(model.images || []); // nếu có ảnh sẵn (edit mode)
    } else {
      setFormData({
        modelNumber: '',
        name: '',
        price: 0,
        description: '',
        parentId: parentModel ? parentModel.modelNumber : '',
        level: parentModel ? (parentModel.level || 1) + 1 : 1,
        slug: '',
        files: [],
      });
      setPreviews([]);
    }
  }, [model, parentModel, isOpen]);

  // 🖊️ input change
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  // 📷 nhận file từ ImageUploader
  const handleImageChange = (files, previewUrls) => {
    setFormData((prev) => ({ ...prev, files }));
    setPreviews(previewUrls);
  };

  // 📨 submit
  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      // tạo formData object để gửi multi-form-data
      const data = new FormData();
      data.append('modelNumber', formData.modelNumber);
      data.append('name', formData.name);
      data.append('price', formData.price);
      data.append(
        'description',
        formData.description?.trim() || 'No description provided',
      );
      data.append('parentId', formData.parentId || '');
      data.append('level', formData.level);
      data.append('slug', formData.slug || '');

      if (formData.files && formData.files.length > 0) {
        formData.files.forEach((file) => {
          data.append('files', file);
        });
      }

      console.log('Submitting formData:', formData);

      if (model) {
        await VehicleModelService.update(model.modelNumber, data);
        toast.success('Vehicle model updated successfully');
      } else {
        await VehicleModelService.create(data);
        toast.success('Vehicle model created successfully');
      }

      reloadModels();
      onClose();
    } catch (err) {
      console.error(err);
      toast.error('Error saving vehicle model');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered size="lg">
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor}>
        <ModalHeader borderTopRadius="20px">
          {model
            ? 'Edit Vehicle Model'
            : parentModel
            ? `Add Variant to "${parentModel.name}"`
            : 'Create New Vehicle Model'}
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <VStack spacing={4} align="flex-start">
            <FormControl>
              <FormLabel>Upload Images</FormLabel>
              <ImageUploader
                multiple={true}
                value={previews}
                onChange={handleImageChange}
              />
            </FormControl>

            <FormControl isRequired>
              <FormLabel>Model Number</FormLabel>
              <Input
                name="modelNumber"
                color={textColor}
                value={formData.modelNumber}
                onChange={handleChange}
                disabled={!!model}
              />
            </FormControl>

            <FormControl isRequired>
              <FormLabel>Name</FormLabel>
              <Input
                name="name"
                color={textColor}
                value={formData.name}
                onChange={handleChange}
              />
            </FormControl>

            <FormControl>
              <FormLabel>Price</FormLabel>
              <Input
                name="price"
                type="number"
                color={textColor}
                value={formData.price}
                onChange={handleChange}
              />
            </FormControl>

            <FormControl isRequired>
              <FormLabel>Description</FormLabel>
              <Textarea
                name="description"
                color={textColor}
                value={formData.description}
                onChange={handleChange}
              />
            </FormControl>
          </VStack>
        </ModalBody>

        <ModalFooter borderBottomRadius="20px">
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button
            colorScheme="green"
            type="submit"
            isLoading={loading}
            onClick={handleSubmit}
          >
            {model ? 'Update' : 'Create'}
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
