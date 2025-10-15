import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  SimpleGrid,
  useColorModeValue,
  Text,
  Divider,
  Flex,
} from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import ProfileService from 'services/ProfileService';

export default function ProfileTab() {
  const toast = useAppToast();
  const bgColor = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const sectionBg = useColorModeValue('gray.50', 'navy.700');

  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
  });
  const [loading, setLoading] = useState(false);

  // 🧩 Lấy profile từ API khi mount
  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const data = await ProfileService.getProfile();
        setFormData({
          firstName: data.firstName || '',
          lastName: data.lastName || '',
          email: data.email || '',
          phone: data.phone || '',
        });
      } catch (error) {
        console.error(error);
        toast.error('Failed to load profile information');
      }
    };

    fetchProfile();
  }, []);

  // 📝 Cập nhật state khi người dùng thay đổi input
  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  // 💾 Gửi cập nhật lên server
  const handleSubmit = async () => {
    setLoading(true);
    try {
      await ProfileService.updateProfile(formData);
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

      {/* Personal Information */}
      <Box p={4} bg={sectionBg} borderRadius="12px" mb={6}>
        <Text fontSize="lg" fontWeight="bold" mb={4}>
          Personal Information
        </Text>

        <SimpleGrid columns={{ base: 1, md: 2 }} spacing={6}>
          <FormControl>
            <FormLabel>First Name</FormLabel>
            <Input
              color={textColor}
              name="firstName"
              value={formData.firstName}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl>
            <FormLabel>Last Name</FormLabel>
            <Input
              color={textColor}
              name="lastName"
              value={formData.lastName}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl>
            <FormLabel>Email</FormLabel>
            <Input
              color={textColor}
              name="email"
              value={formData.email}
              onChange={handleChange}
              disabled
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
