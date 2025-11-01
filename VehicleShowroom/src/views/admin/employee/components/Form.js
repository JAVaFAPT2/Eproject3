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
  SimpleGrid,
  VStack,
  useBreakpointValue,
} from '@chakra-ui/react';
import { DatePicker } from 'components/fields/DatePicker';
import { useAppToast } from 'utils/ToastHelper';
import UserService from 'services/UserService';

export default function Form({
  isOpen,
  onClose,
  reloadUsers,
  user,
  textColor,
  bgColor,
}) {
  const toast = useAppToast();
  const [loading, setLoading] = useState(false);

  const [formData, setFormData] = useState({
    username: '',
    email: '',
    password: '',
    name: '',
    phone: '',
    address: '',
    hireDate: '', 
  });

  const columns = useBreakpointValue({ base: 1, md: 2 });

  useEffect(() => {
    if (isOpen) {
      const today = new Date();
      const y = today.getFullYear();
      const m = String(today.getMonth() + 1).padStart(2, '0');
      const d = String(today.getDate()).padStart(2, '0');

      setFormData((prev) => ({
        ...prev,
        hireDate: `${y}-${m}-${d}`,
      }));
    }
  }, [isOpen]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      const payload = {
        username: formData.username,
        email: formData.email,
        password: formData.password,
        name: formData.name,
        phone: formData.phone,
        address: formData.address,
        hireDate: new Date(formData.hireDate).toISOString(),
      };

      await UserService.create(payload);
      toast.success('User created successfully');
      reloadUsers?.();
      onClose();
    } catch (err) {
      console.error(err);
      toast.error('Error creating user');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal isOpen={isOpen} onClose={onClose} isCentered size="xl">
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor}>
        <ModalHeader>Create New Employee</ModalHeader>
        <ModalCloseButton />

        <ModalBody>
          <VStack spacing={4} align="flex-start">
            {/* 🟩 Grid 2 cột */}
            <SimpleGrid columns={columns} spacing={4} w="100%">
              <FormControl isRequired>
                <FormLabel>Username</FormLabel>
                <Input
                  name="username"
                  color={textColor}
                  value={formData.username}
                  onChange={handleChange}
                  placeholder="Enter username"
                />
              </FormControl>

              <FormControl isRequired>
                <FormLabel>Email</FormLabel>
                <Input
                  name="email"
                  type="email"
                  color={textColor}
                  value={formData.email}
                  onChange={handleChange}
                  placeholder="user@example.com"
                />
              </FormControl>

              <FormControl isRequired>
                <FormLabel>Password</FormLabel>
                <Input
                  name="password"
                  type="password"
                  color={textColor}
                  value={formData.password}
                  onChange={handleChange}
                  placeholder="Enter password"
                />
              </FormControl>

              <FormControl isRequired>
                <FormLabel>Full Name</FormLabel>
                <Input
                  name="name"
                  color={textColor}
                  value={formData.name}
                  onChange={handleChange}
                  placeholder="Full name"
                />
              </FormControl>

              <FormControl>
                <FormLabel>Phone</FormLabel>
                <Input
                  name="phone"
                  color={textColor}
                  value={formData.phone}
                  onChange={handleChange}
                  placeholder="e.g. 0912345678"
                />
              </FormControl>

              <FormControl>
                <FormLabel>Address</FormLabel>
                <Input
                  name="address"
                  color={textColor}
                  value={formData.address}
                  onChange={handleChange}
                  placeholder="Address"
                />
              </FormControl>

              {/* 🗓️ Hire Date Picker */}
              <FormControl>
                <DatePicker
                  label="Hire Date"
                  value={formData.hireDate}
                  onChange={(val) =>
                    setFormData((prev) => ({ ...prev, hireDate: val }))
                  }
                />
              </FormControl>
            </SimpleGrid>
          </VStack>
        </ModalBody>

        <ModalFooter>
          <Button variant="ghost" mr={3} onClick={onClose}>
            Cancel
          </Button>
          <Button
            colorScheme="green"
            onClick={handleSubmit}
            isLoading={loading}
          >
            Create
          </Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
}
