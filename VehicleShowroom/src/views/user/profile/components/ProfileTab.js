import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  SimpleGrid,
  Text,
  Divider,
  Flex,
} from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import ProfileService from 'services/ProfileService';

export default function ProfileTab({ user, colors }) {
  const toast = useAppToast();
  const { bgColor, textColor, sectionBg } = colors;

  const [formData, setFormData] = useState({
    username: '',
    name: '',
    email: '',
    phone: '',
    address: '',
    role: '',
  });
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (user) {
      setFormData({
        username: user.username || '',
        name: user.name || '',
        email: user.email || '',
        phone: user.phone || '',
        address: user.address || '',
        role: user.role || '',
      });
    }
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    setLoading(true);
    try {
      await ProfileService.updateProfile({
        name: formData.name,
        phone: formData.phone,
        address: formData.address,
      });
      toast.success('Profile updated successfully');
    } catch (error) {
      console.error(error);
      toast.error('Failed to update profile');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box bg={bgColor} p={8} borderRadius="16px" shadow="md">
      <Flex direction="column" align="start" mb={6}>
        <Text fontSize="2xl" fontWeight="bold" color={textColor}>
          My Profile
        </Text>
        <Text fontSize="sm" color="gray.500">
          View and edit your personal information
        </Text>
      </Flex>

      <Divider my={6} />

      <Box p={4} bg={sectionBg} borderRadius="12px" mb={6}>
        <Text fontSize="lg" fontWeight="bold" mb={4}>
          Personal Information
        </Text>

        <SimpleGrid columns={{ base: 1, md: 2 }} spacing={6}>
          <FormControl>
            <FormLabel>Username</FormLabel>
            <Input
              color={textColor}
              name="username"
              value={formData.username}
              isDisabled
            />
          </FormControl>

          <FormControl>
            <FormLabel>Full Name</FormLabel>
            <Input
              color={textColor}
              name="name"
              value={formData.name}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl>
            <FormLabel>Email</FormLabel>
            <Input
              color={textColor}
              name="email"
              value={formData.email}
              isDisabled
            />
          </FormControl>

          <FormControl>
            <FormLabel>Phone</FormLabel>
            <Input
              color={textColor}
              name="phone"
              value={formData.phone}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl gridColumn={{ md: 'span 2' }}>
            <FormLabel>Address</FormLabel>
            <Input
              color={textColor}
              name="address"
              value={formData.address}
              onChange={handleChange}
            />
          </FormControl>
        </SimpleGrid>
      </Box>

      <Button
        mt={8}
        colorScheme="brand"
        color="white"
        onClick={handleSubmit}
        isLoading={loading}
      >
        Save Changes
      </Button>
    </Box>
  );
}
