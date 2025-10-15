import React, { useState, useEffect } from 'react';
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
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  Box,
  Text,
  useColorModeValue,
} from '@chakra-ui/react';
import { ChevronDownIcon } from '@chakra-ui/icons';
import { useAppToast } from 'utils/ToastHelper';
import VehicleService from 'services/VehicleService';

export default function Form({
  isOpen,
  onClose,
  reloadVehicles,
  vehicle,
  models,
  bgColor,
  textColor
}) {
  const toast = useAppToast();
  const [formData, setFormData] = useState({
    vehicleId: '',
    modelNumber: '',
    purchasePrice: '',
    externalNumber: '',
    vin: '',
  });

  const [modelName, setModelName] = useState('');

  useEffect(() => {
    if (vehicle) {
      setFormData({
        vehicleId: vehicle.vehicleId || '',
        modelNumber: vehicle.modelNumber || '',
        purchasePrice: vehicle.purchasePrice || '',
        externalNumber: vehicle.externalNumber || '',
        vin: vehicle.vin || '',
      });
      setModelName(
        models.find((m) => m.modelNumber === vehicle.modelNumber)?.name || '',
      );
    } else {
      setFormData({
        vehicleId: '',
        modelNumber: '',
        purchasePrice: '',
        externalNumber: '',
        vin: '',
      });
      setModelName('');
    }
  }, [vehicle, models, isOpen]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    try {
      if (vehicle) {
        await VehicleService.update(vehicle.vehicleId, formData);
        toast.success('Vehicle updated successfully');
      } else {
        await VehicleService.create(formData);
        toast.success('Vehicle created successfully');
      }
      reloadVehicles();
      onClose();
    } catch (err) {
      toast.error('Error saving vehicle');
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px">
        <ModalHeader>{vehicle ? 'Edit Vehicle' : 'Add Vehicle'}</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <FormControl isRequired mb={4}>
            <FormLabel>Model</FormLabel>
            <Menu isLazy matchWidth>
              <MenuButton
                as={Button}
                rightIcon={<ChevronDownIcon />}
                w="full"
                variant="outline"
              >
                {modelName || 'Select model'}
              </MenuButton>
              <MenuList maxH="250px" overflowY="auto" bg={bgColor}>
                {models.length > 0 ? (
                  models.map((m) => (
                    <MenuItem
                      key={m.modelNumber}
                      onClick={() => {
                        setFormData((prev) => ({
                          ...prev,
                          modelNumber: m.modelNumber,
                        }));
                        setModelName(m.name);
                      }}
                    >
                      {m.name}
                    </MenuItem>
                  ))
                ) : (
                  <Box px={3} py={2}>
                    <Text fontSize="sm" color="gray.500">
                      No models available
                    </Text>
                  </Box>
                )}
              </MenuList>
            </Menu>
          </FormControl>

          <FormControl isRequired mb={4}>
            <FormLabel>VIN</FormLabel>
            <Input
              color={textColor}
              name="vin"
              value={formData.vin}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl mb={4}>
            <FormLabel>External Number</FormLabel>
            <Input
              color={textColor}
              name="externalNumber"
              value={formData.externalNumber}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl mb={4}>
            <FormLabel>Purchase Price</FormLabel>
            <Input
              color={textColor}
              name="purchasePrice"
              type="number"
              value={formData.purchasePrice}
              onChange={handleChange}
            />
          </FormControl>
        </ModalBody>
        <ModalFooter>
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button colorScheme="green" onClick={handleSubmit}>
            {vehicle ? 'Update' : 'Create'}
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
