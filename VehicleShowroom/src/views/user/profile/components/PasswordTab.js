import React, { useState } from 'react';
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  VStack,
  useToast,
  useColorModeValue,
} from '@chakra-ui/react';
import UserService from 'services/UserService';

export default function PasswordTab() {
  const toast = useToast();
  const bgColor = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const [formData, setFormData] = useState({
    currentPassword: '',
    newPassword: '',
    confirmPassword: '',
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    try {
      if (formData.newPassword !== formData.confirmPassword) {
        toast({
          title: 'Passwords do not match',
          status: 'error',
          duration: 3000,
          isClosable: true,
          position: 'bottom-right',
        });
        return;
      }
      await UserService.changePassword({
        oldPassword: formData.currentPassword,
        newPassword: formData.newPassword,
      });

      toast({
        title: 'Password changed successfully',
        status: 'success',
        duration: 3000,
        isClosable: true,
        position: 'bottom-right',
      });
      setFormData({
        currentPassword: '',
        newPassword: '',
        confirmPassword: '',
      });
    } catch (err) {
      console.error(err);
      toast({
        title: 'Password change failed',
        status: 'error',
        isClosable: true,
        duration: 3000,
        position: 'bottom-right',
      });
    }
  };

  return (
    <Box bg={bgColor} p={6} borderRadius="16px" w={'50%'}>
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
      <Button colorScheme="brand" color="white" onClick={handleSubmit} mt={5}>
        Change Password
      </Button>
    </Box>
  );
}
