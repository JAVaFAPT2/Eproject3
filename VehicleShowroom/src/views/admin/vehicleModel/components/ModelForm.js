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
import VehiclePhotoService from 'services/VehiclePhotoService';
import ImageUploader from 'components/images/ImageUploader';
import { generateSlug } from 'utils/SlugHelper'; 

export default function ModelForm({
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
    parentModel: '',
    level: 1,
    slug: '',
    files: [],
  });
  const [previews, setPreviews] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const loadModelPhotos = async () => {
      if (model?.modelNumber) {
        try {
          const photos = await VehiclePhotoService.getByModelNumber(
            model.modelNumber,
          );
          setPreviews(photos.map((p) => p.url || p.photoUrl));
        } catch (err) {
          console.error('Error loading photos:', err);
        }
      }
    };

    if (model) {
      setFormData({
        modelNumber: model.modelNumber,
        name: model.name || '',
        price: model.price || 0,
        description: model.description || '',
        parentModel: model.parentModel || '',
        level: model.level || 1,
        slug: model.slug || '',
        files: [],
      });
      loadModelPhotos();
    } else {
      setFormData({
        modelNumber: '',
        name: '',
        price: 0,
        description: '',
        parentModel: parentModel?.modelNumber || '',
        level: parentModel ? (parentModel.level || 1) + 1 : 1,
        slug: '',
        files: [],
      });
      setPreviews([]);
    }
  }, [model, parentModel, isOpen]);

  // 🖊️ handle input
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => {
      // ✅ tự sinh slug khi thay đổi name
      if (name === 'name') {
        return { ...prev, name: value, slug: generateSlug(value) };
      }
      return { ...prev, [name]: value };
    });
  };

  const handleImageChange = (files, previewUrls) => {
    setFormData((prev) => ({ ...prev, files }));
    setPreviews(previewUrls);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      let modelNumber = formData.modelNumber;

      // 🔹 1. Create or update model
      if (model) {
        await VehicleModelService.update(modelNumber, {
          modelNumber,
          name: formData.name,
          price: parseFloat(formData.price) || 0,
          description: formData.description,
          parentId: formData.parentModel || null,
          level: formData.level,
          slug: generateSlug(formData.name),
        });
        toast.success('Vehicle model updated successfully');
      } else {
        const created = await VehicleModelService.create({
          modelNumber,
          name: formData.name,
          price: parseFloat(formData.price) || 0,
          description: formData.description,
          parentId: formData.parentModel || null,
          level: formData.level,
          slug: generateSlug(formData.name),
        });
        modelNumber = created.modelNumber || formData.modelNumber;
        toast.success('Vehicle model created successfully');
      }

      // 🔹 2. Upload photos (nếu có)
      if (formData.files.length > 0 && modelNumber) {
        await VehiclePhotoService.upload(modelNumber, formData.files);
        toast.success('Photos uploaded successfully');
      }

      reloadModels();
      onClose();
    } catch (err) {
      console.error('Error saving vehicle model:', err);
      toast.error('Failed to save vehicle model');
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
                multiple
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
