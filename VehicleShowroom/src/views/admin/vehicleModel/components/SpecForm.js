// components/SpecForm.js
import React, { useState } from 'react';
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
} from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import VehicleSpecService from 'services/VehicleSpecService';

export default function SpecForm({ isOpen, onClose, model }) {
  const toast = useAppToast();
  const [formData, setFormData] = useState({
    specName: '',
    specValue: '',
    displayOrder: 0,
    groupName: '',
  });
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    if (!model?.modelNumber) return;
    try {
      setLoading(true);
      await VehicleSpecService.create(model.modelNumber, formData);
      toast.success(`Specification added to ${model.name}`);
      setFormData({
        specName: '',
        specValue: '',
        displayOrder: 0,
        groupName: '',
      });
      onClose();
    } catch (err) {
      console.error(err);
      toast.error('Failed to add specification');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered size="md">
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Add Specification for {model?.name}</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <VStack spacing={4}>
            <FormControl isRequired>
              <FormLabel>Spec Name</FormLabel>
              <Input
                name="specName"
                value={formData.specName}
                onChange={handleChange}
              />
            </FormControl>
            <FormControl isRequired>
              <FormLabel>Spec Value</FormLabel>
              <Input
                name="specValue"
                value={formData.specValue}
                onChange={handleChange}
              />
            </FormControl>
            <FormControl>
              <FormLabel>Display Order</FormLabel>
              <Input
                name="displayOrder"
                type="number"
                value={formData.displayOrder}
                onChange={handleChange}
              />
            </FormControl>
            <FormControl>
              <FormLabel>Group Name</FormLabel>
              <Input
                name="groupName"
                value={formData.groupName}
                onChange={handleChange}
              />
            </FormControl>
          </VStack>
        </ModalBody>
        <ModalFooter>
          <Button onClick={onClose} mr={3}>
            Cancel
          </Button>
          <Button
            colorScheme="green"
            onClick={handleSubmit}
            isLoading={loading}
          >
            Save
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
