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
} from '@chakra-ui/react';
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
  });

  // Reset khi mở modal
  useEffect(() => {
    if (user) {
      setFormData({
        username: user.username || '',
        email: user.email || '',
        password: '',
        name: user.name || '',
        phone: user.phone || '',
        address: user.address || '',
      });
    } else {
      setFormData({
        username: '',
        email: '',
        password: '',
        name: '',
        phone: '',
        address: '',
      });
    }
  }, [user, isOpen]);

  // Xử lý input change
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  // Submit form
  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);

    try {
      // BE sẽ tự thêm hireDate & roleName theo người đăng nhập
      const payload = {
        ...formData,
        hireDate: new Date().toISOString(),
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
    <Modal isOpen={isOpen} onClose={onClose} isCentered>
      <ModalOverlay />
      <ModalContent borderRadius="20px" bg={bgColor} color={textColor}>
        <ModalHeader borderTopRadius="20px">Create New Employee</ModalHeader>
        <ModalCloseButton />

        <ModalBody>
          <VStack spacing={4} align="flex-start">
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
          </VStack>
        </ModalBody>

        <ModalFooter borderBottomRadius="20px">
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
