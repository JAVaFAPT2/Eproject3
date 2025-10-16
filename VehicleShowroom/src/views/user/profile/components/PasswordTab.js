import React, { useState } from 'react';
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

export default function PasswordTab({ colors }) {
  const { bgColor, textColor, sectionBg } = colors;
  const toast = useAppToast();

  const [formData, setFormData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  });
  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    if (
      !formData.currentPassword ||
      !formData.newPassword ||
      !formData.confirmPassword
    ) {
      toast.error('Please fill in all fields');
      return;
    }

    if (formData.newPassword !== formData.confirmPassword) {
      toast.error('Passwords do not match');
      return;
    }

    setLoading(true);
    try {
      await ProfileService.changePassword({
        currentPassword: formData.currentPassword,
        newPassword: formData.newPassword,
      });
      toast.success('Password changed successfully');
      setFormData({
        currentPassword: '',
        newPassword: '',
        confirmPassword: '',
      });
    } catch (err) {
      console.error(err);
      const message =
        err.response?.data?.message ||
        'Password change failed. Please try again.';
      toast.error(message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box bg={bgColor} p={8} borderRadius="16px" shadow="md">
      {/* 🔹 Header */}
      <Flex direction="column" align="start" mb={6}>
        <Text fontSize="2xl" fontWeight="bold" color={textColor}>
          Change Password
        </Text>
        <Text fontSize="sm" color="gray.500">
          Update your account password below
        </Text>
      </Flex>

      <Divider my={6} />

      {/* 🔹 Password fields section */}
      <Box p={4} bg={sectionBg} borderRadius="12px" mb={6}>
        <Text fontSize="lg" fontWeight="bold" mb={4}>
          Security Details
        </Text>

        <SimpleGrid columns={{ base: 1, md: 2 }} spacing={6}>
          <FormControl gridColumn={{ md: 'span 2' }}>
            <FormLabel color={textColor}>Current Password</FormLabel>
            <Input
              color={textColor}
              type="password"
              name="currentPassword"
              value={formData.currentPassword}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl>
            <FormLabel color={textColor}>New Password</FormLabel>
            <Input
              color={textColor}
              type="password"
              name="newPassword"
              value={formData.newPassword}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl>
            <FormLabel color={textColor}>Confirm New Password</FormLabel>
            <Input
              color={textColor}
              type="password"
              name="confirmPassword"
              value={formData.confirmPassword}
              onChange={handleChange}
            />
          </FormControl>
        </SimpleGrid>
      </Box>

      {/* 🔹 Submit button */}
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
