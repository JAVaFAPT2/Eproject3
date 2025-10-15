import React, { useState } from 'react';
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  VStack,
  useColorModeValue,
} from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import ProfileService from 'services/ProfileService';

export default function PasswordTab() {
  const toast = useAppToast();
  const bgColor = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');

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
    <Box bg={bgColor} p={6} borderRadius="16px" w={{ base: '100%', md: '50%' }}>
      <VStack spacing={4} align="stretch">
        <FormControl>
          <FormLabel>Current Password</FormLabel>
          <Input
            color={textColor}
            type="password"
            name="currentPassword"
            value={formData.currentPassword}
            onChange={handleChange}
          />
        </FormControl>

        <FormControl>
          <FormLabel>New Password</FormLabel>
          <Input
            color={textColor}
            type="password"
            name="newPassword"
            value={formData.newPassword}
            onChange={handleChange}
          />
        </FormControl>

        <FormControl>
          <FormLabel>Confirm New Password</FormLabel>
          <Input
            color={textColor}
            type="password"
            name="confirmPassword"
            value={formData.confirmPassword}
            onChange={handleChange}
          />
        </FormControl>
      </VStack>

      <Button
        mt={5}
        colorScheme="brand"
        color="white"
        onClick={handleSubmit}
        isLoading={loading}
      >
        Change Password
      </Button>
    </Box>
  );
}
