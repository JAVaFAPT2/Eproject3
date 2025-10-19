import React, { useEffect, useState } from 'react';
import {
  Modal,
  ModalOverlay,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalFooter,
  ModalCloseButton,
  FormControl,
  FormLabel,
  Input,
  Textarea,
  Flex,
  useColorModeValue,
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  Button,
} from '@chakra-ui/react';
import { ChevronDownIcon } from '@chakra-ui/icons';
import { useUser } from 'contexts/UserContext';
import { DatePicker } from 'components/fields/DatePicker';

export default function ServiceForm({ isOpen, onClose, order, onSubmit }) {
  const { user } = useUser();
  const textColor = useColorModeValue('gray.800', 'white');

  const [formData, setFormData] = useState({
    type: 1,
    cost: 0,
    appointmentDate: '',
    description: '',
  });

  useEffect(() => {
    if (isOpen) {
      const today = new Date();
      const y = today.getFullYear();
      const m = String(today.getMonth() + 1).padStart(2, '0');
      const d = String(today.getDate()).padStart(2, '0');
      setFormData({
        type: 1,
        cost: 0,
        appointmentDate: `${y}-${m}-${d}`, 
        description: '',
      });
    }
  }, [isOpen]);

  const handleChange = (field, value) => {
    setFormData((prev) => ({ ...prev, [field]: value }));
  };

  const handleSubmit = () => {
    if (!formData.appointmentDate) return;
    console.log(formData);
    onSubmit?.(formData);
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} size="lg" isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px">
        <ModalHeader fontSize="xl" fontWeight="700">
          Create Service Order
        </ModalHeader>
        <ModalCloseButton />

        <ModalBody>
          {/* Hidden Order ID */}
          <Input type="hidden" value={order?.id || ''} readOnly />

          {/* Row 1: CreatedBy + Type */}
          <Flex gap={4} mb={4}>
            <FormControl flex="1">
              <FormLabel color={textColor}>Created By</FormLabel>
              <Input
                value={user?.name || user?.username || 'Unknown'}
                isReadOnly
              />
            </FormControl>

            <FormControl flex="0.7">
              <FormLabel color={textColor}>Type</FormLabel>
              <Menu isLazy>
                <MenuButton
                  as={Button}
                  w="full"
                  variant="outline"
                  rightIcon={<ChevronDownIcon />}
                  justifyContent="space-between"
                >
                  {formData.type === 1
                    ? 'PreDelivery'
                    : formData.type === 2
                    ? 'Maintenance'
                    : 'Repair'}
                </MenuButton>
                <MenuList>
                  <MenuItem onClick={() => handleChange('type', 1)}>
                    PreDelivery
                  </MenuItem>
                  <MenuItem onClick={() => handleChange('type', 2)}>
                    Maintenance
                  </MenuItem>
                  <MenuItem onClick={() => handleChange('type', 3)}>
                    Repair
                  </MenuItem>
                </MenuList>
              </Menu>
            </FormControl>
          </Flex>

          {/* Row 2: Cost + Appointment Date */}
          <Flex gap={4} mb={4}>
            <FormControl flex="0.6">
              <FormLabel color={textColor}>Cost</FormLabel>
              <Input
                type="number"
                value={formData.cost}
                onChange={(e) => handleChange('cost', e.target.value)}
                placeholder="Enter cost"
              />
            </FormControl>

            <Flex flex="1">
              <DatePicker
                label="Appointment Date"
                value={formData.appointmentDate}
                onChange={(val) => handleChange('appointmentDate', val)}
              />
            </Flex>
          </Flex>

          {/* Description */}
          <FormControl mb={2}>
            <FormLabel color={textColor}>Description</FormLabel>
            <Textarea
              value={formData.description}
              onChange={(e) => handleChange('description', e.target.value)}
              placeholder="Enter service description..."
            />
          </FormControl>
        </ModalBody>

        <ModalFooter>
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button colorScheme="green" onClick={handleSubmit}>
            Create
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
