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
  textColor,
}) {
  const toast = useAppToast();

  const [formData, setFormData] = useState({
    vehicleId: '',
    modelNumber: '',
    purchasePrice: '',
    externalNumber: '',
    vin: '',
    licensePlate: '',
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
        licensePlate: vehicle.licensePlate || '',
      });
      setModelName(
        models.find((m) => m.modelNumber === vehicle.modelNumber)?.name || ''
      );
    } else {
      setFormData({
        vehicleId: '',
        modelNumber: '',
        purchasePrice: '',
        externalNumber: '',
        vin: '',
        licensePlate: '',
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
      const payload = {
        modelNumber: formData.modelNumber,
        purchasePrice: Number(formData.purchasePrice),
        externalNumber: formData.externalNumber,
        vin: formData.vin,
      };

      if (vehicle) {
        // PUT /api/Vehicles/{id}
        const updatePayload = { ...payload, licensePlate: formData.licensePlate };
        await VehicleService.update(vehicle.vehicleId, updatePayload);
        toast.success('Vehicle updated successfully');
      } else {
        // POST /api/Vehicles
        const createPayload = { ...payload, vehicleId: formData.vehicleId };
        await VehicleService.create(createPayload);
        toast.success('Vehicle created successfully');
      }

      reloadVehicles();
      onClose();
    } catch (err) {
      console.error('❌ Error saving vehicle:', err);
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
          {/* MODEL */}
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
                  models.map((m) => {
                    const isParent = m.level === 1;
                    const isChild = m.level === 2;

                    return (
                      <MenuItem
                        key={m.modelNumber}
                        pl={m.level * 4}
                        fontWeight={isParent ? '700' : '500'}
                        color={isParent ? 'gray.900' : textColor}
                        _hover={
                          isParent
                            ? { bg: 'transparent', cursor: 'default' }
                            : { bg: 'gray.100', cursor: 'pointer' }
                        }
                        isDisabled={!isChild}
                        onClick={() => {
                          if (!isChild) return;
                          setFormData((prev) => ({
                            ...prev,
                            modelNumber: m.modelNumber,
                          }));
                          setModelName(m.name);
                        }}
                      >
                        {m.name}
                      </MenuItem>
                    );
                  })
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

          {/* VIN */}
          <FormControl isRequired mb={4}>
            <FormLabel>VIN</FormLabel>
            <Input
              color={textColor}
              name="vin"
              value={formData.vin}
              onChange={handleChange}
            />
          </FormControl>

          {/* External Number */}
          <FormControl mb={4}>
            <FormLabel>External Number</FormLabel>
            <Input
              color={textColor}
              name="externalNumber"
              value={formData.externalNumber}
              onChange={handleChange}
            />
          </FormControl>

          {/* Price */}
          <FormControl mb={4}>
            <FormLabel>Price</FormLabel>
            <Input
              color={textColor}
              name="purchasePrice"
              type="number"
              value={formData.purchasePrice}
              onChange={handleChange}
            />
          </FormControl>

          {/* License Plate (only edit mode) */}
          {vehicle && (
            <FormControl mb={4}>
              <FormLabel>License Plate</FormLabel>
              <Input
                color={textColor}
                name="licensePlate"
                value={formData.licensePlate}
                onChange={handleChange}
              />
            </FormControl>
          )}
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
