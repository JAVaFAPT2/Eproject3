import React, { useState, useMemo, useEffect } from 'react';
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
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  Flex,
  Text,
  Box,
  useColorModeValue,
} from '@chakra-ui/react';
import { ChevronDownIcon } from '@chakra-ui/icons';
import { useAppToast } from 'utils/ToastHelper';
import VehicleSpecService from 'services/VehicleSpecService';

// 🟣 Danh sách nhóm và thông số
const SPEC_GROUPS = {
  General: ['Fuel Type', 'Seats'],
  'Power unit': [
    'Number of cylinders',
    'Bore',
    'Stroke',
    'Displacement',
    'Power (kW)',
    'Power (PS)',
    'Maximum engine speed',
    'Max. output per liter (kW/l)',
    'Max. output per liter (PS/l)',
  ],
  Performance: [
    'Top speed',
    'Acceleration 0 - 100 km/h',
    'Acceleration 0 - 100 km/h with Sport Chrono Package',
    'Acceleration 0 - 160 km/h',
    'Acceleration 0 - 160 km/h with Sport Chrono Package',
    'Acceleration 0 - 200 km/h',
    'Acceleration 0 - 200 km/h with Sport Chrono Package',
    'In-gear acceleration (80-120km/h) (50-75 mph)',
  ],
  'Consumption/Emissions (ECE)': [
    'Fuel consumption urban',
    'Fuel consumption extra-urban',
    'Fuel consumption combined',
    'CO2 emissions combined',
  ],
  'Sound level (type approved based on UN-R 51)': [
    'Sound level of stationary vehicle',
    'Sound level of stationary vehicle (rpm)',
    'Sound level of passing vehicle',
  ],
  Body: [
    'Length',
    'Width',
    'Width (with mirrors)',
    'Height',
    'Wheelbase',
    'Unladen weight (DIN)',
    'Unladen weight (EU)',
    'Permissible gross weight',
    'Maximum load',
  ],
  Capacities: [
    'Luggage compartment volume, front',
    'Open luggage compartment volume (behind front seats)',
    'Largest luggage compartment volume (behind front seats, up to roof)',
  ],
};

export default function SpecForm({
  isOpen,
  onClose,
  model,
  reloadModels,
  editingSpec,
}) {
  const toast = useAppToast();
  const [formData, setFormData] = useState({
    specName: '',
    specValue: '',
    groupName: '',
  });
  const [loading, setLoading] = useState(false);

  const menuBg = useColorModeValue('white', 'gray.700');
  const hoverBg = useColorModeValue('gray.100', 'gray.600');
  const activeColor = useColorModeValue('brand.500', 'brand.300');

  // 🧩 Danh sách spec theo group
  const availableSpecs = useMemo(() => {
    if (!formData.groupName) return [];
    return SPEC_GROUPS[formData.groupName] || [];
  }, [formData.groupName]);

  // 🧠 Khi mở modal để edit spec
  useEffect(() => {
    if (editingSpec) {
      setFormData({
        specName: editingSpec.specName,
        specValue: editingSpec.specValue,
        groupName: editingSpec.groupName || '',
      });
    } else {
      setFormData({
        specName: '',
        specValue: '',
        groupName: '',
      });
    }
  }, [editingSpec, isOpen]);

  const handleSelectGroup = (group) => {
    setFormData((prev) => ({
      ...prev,
      groupName: group,
      specName: '', 
    }));
  };

  const handleSelectSpec = (spec) => {
    const detectedGroup = Object.keys(SPEC_GROUPS).find((g) =>
      SPEC_GROUPS[g].includes(spec),
    );
    setFormData((prev) => ({
      ...prev,
      specName: spec,
      groupName: prev.groupName || detectedGroup || 'General',
    }));
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    if (!model?.modelNumber) return;
    if (!formData.specName || !formData.specValue) {
      toast.error('Please fill all required fields');
      return;
    }

    try {
      setLoading(true);
      const payload = {
        specName: formData.specName,
        specValue: formData.specValue,
        groupName: formData.groupName || 'General',
      };

      if (editingSpec?.specId) {
        await VehicleSpecService.update(editingSpec.specId, payload);
        toast.success('Specification updated successfully');
      } else {
        await VehicleSpecService.create(model.modelNumber, payload);
        toast.success(`Specification added to ${model.name}`);
      }

      if (typeof reloadModels === 'function') reloadModels();
      onClose();
    } catch (err) {
      console.error(err);
      toast.error('Failed to save specification');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered size="md">
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>
          {editingSpec
            ? `Edit Specification`
            : `Add Specification for ${model?.name}`}
        </ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <VStack spacing={4}>
            {/* 🟣 Menu chọn Group */}
            <FormControl isRequired>
              <FormLabel>Group Name</FormLabel>
              <Menu placement="bottom-start" autoSelect={false}>
                <MenuButton
                  as={Button}
                  rightIcon={<ChevronDownIcon />}
                  w="full"
                  justifyContent="space-between"
                >
                  {formData.groupName || 'Select group'}
                </MenuButton>
                <MenuList bg={menuBg}>
                  {Object.keys(SPEC_GROUPS).map((group) => {
                    const isActive = formData.groupName === group;
                    return (
                      <MenuItem
                        key={group}
                        onClick={() => handleSelectGroup(group)}
                        _hover={{ bg: hoverBg }}
                        px={3}
                        py={2}
                      >
                        <Flex
                          align="center"
                          justify="space-between"
                          w="full"
                          position="relative"
                        >
                          <Text fontWeight={isActive ? '600' : 'normal'}>
                            {group}
                          </Text>
                          {isActive && (
                            <Box
                              position="absolute"
                              right={0}
                              top={0}
                              bottom={0}
                              w="4px"
                              bg={activeColor}
                              borderRadius="full"
                            />
                          )}
                        </Flex>
                      </MenuItem>
                    );
                  })}
                </MenuList>
              </Menu>
            </FormControl>

            {/* 🟢 Menu chọn Spec */}
            <FormControl isRequired>
              <FormLabel>Specification</FormLabel>
              <Menu placement="bottom-start" autoSelect={false}>
                <MenuButton
                  as={Button}
                  rightIcon={<ChevronDownIcon />}
                  w="full"
                  justifyContent="space-between"
                >
                  {formData.specName || 'Select specification'}
                </MenuButton>
                <MenuList maxH="250px" overflowY="auto" bg={menuBg}>
                  {formData.groupName
                    ? availableSpecs.map((spec) => {
                        const isActive = formData.specName === spec;
                        return (
                          <MenuItem
                            key={spec}
                            onClick={() => handleSelectSpec(spec)}
                            _hover={{ bg: hoverBg }}
                            px={3}
                            py={2}
                          >
                            <Flex
                              align="center"
                              justify="space-between"
                              w="full"
                              position="relative"
                            >
                              <Text fontWeight={isActive ? '600' : 'normal'}>
                                {spec}
                              </Text>
                              {isActive && (
                                <Box
                                  position="absolute"
                                  right={0}
                                  top={0}
                                  bottom={0}
                                  w="4px"
                                  bg={activeColor}
                                  borderRadius="full"
                                />
                              )}
                            </Flex>
                          </MenuItem>
                        );
                      })
                    : Object.entries(SPEC_GROUPS).map(([group, specs]) => (
                        <React.Fragment key={group}>
                          <Text
                            px={3}
                            py={1}
                            fontSize="sm"
                            fontWeight="bold"
                            color="gray.500"
                          >
                            {group}
                          </Text>
                          {specs.map((spec) => {
                            const isActive = formData.specName === spec;
                            return (
                              <MenuItem
                                key={spec}
                                onClick={() => handleSelectSpec(spec)}
                                _hover={{ bg: hoverBg }}
                                px={3}
                                py={2}
                              >
                                <Flex
                                  align="center"
                                  justify="space-between"
                                  w="full"
                                  position="relative"
                                >
                                  <Text
                                    fontWeight={isActive ? '600' : 'normal'}
                                  >
                                    {spec}
                                  </Text>
                                  {isActive && (
                                    <Box
                                      position="absolute"
                                      right={0}
                                      top={0}
                                      bottom={0}
                                      w="4px"
                                      bg={activeColor}
                                      borderRadius="full"
                                    />
                                  )}
                                </Flex>
                              </MenuItem>
                            );
                          })}
                        </React.Fragment>
                      ))}
                </MenuList>
              </Menu>
            </FormControl>

            {/* Giá trị */}
            <FormControl isRequired>
              <FormLabel>Value</FormLabel>
              <Input
                name="specValue"
                value={formData.specValue}
                onChange={handleChange}
                placeholder="Enter value (e.g. 275 km/h)"
              />
            </FormControl>
          </VStack>
        </ModalBody>

        <ModalFooter>
          <Button onClick={onClose} mr={3}>
            Cancel
          </Button>
          <Button
            colorScheme={editingSpec ? 'blue' : 'green'}
            onClick={handleSubmit}
            isLoading={loading}
          >
            {editingSpec ? 'Update' : 'Save'}
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
