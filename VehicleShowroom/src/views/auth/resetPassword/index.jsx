import React, { useState } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import {
  Box,
  Button,
  Flex,
  FormControl,
  FormLabel,
  Heading,
  Input,
  Text,
  useColorModeValue,
} from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import DefaultAuth from 'layouts/auth/Default';
import illustration from 'assets/image/auth/banner.png';
import AuthService from 'services/AuthService';

function ResetPassword() {
  const textColor = useColorModeValue('navy.700', 'white');
  const textColorSecondary = 'gray.400';
  const toast = useAppToast();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const token = searchParams.get('token'); // lấy token từ query string

  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [loading, setLoading] = useState(false);

  const handleResetPassword = async () => {
    if (!password || !confirmPassword) {
      toast.error('Please fill in all fields');
      return;
    }

    if (password !== confirmPassword) {
      toast.error('Passwords do not match');
      return;
    }

    setLoading(true);
    try {
      // ✅ Đúng format body API: { token, newPassword }
      await AuthService.resetPassword({ token, newPassword: password });
      toast.success('Password reset successfully!');
      navigate('/auth/sign-in');
    } catch (error) {
      console.error(error);
      const msg =
        error.response?.data?.message ||
        'Failed to reset password. Please try again.';
      toast.error(msg);
    } finally {
      setLoading(false);
    }
  };

  return (
    <DefaultAuth illustrationBackground={illustration} image={illustration}>
      <Flex
        maxW={{ base: '100%', md: 'max-content' }}
        w="100%"
        mx={{ base: 'auto', lg: '0px' }}
        me="auto"
        h="100%"
        alignItems="center"
        justifyContent="center"
        flexDirection="column"
        px={{ base: '25px', md: '0px' }}
        mt={{ base: '40px', md: '10vh' }}
      >
        <Box w={{ base: '100%', md: '420px' }}>
          <Heading color={textColor} fontSize="32px" mb="10px">
            Reset Password
          </Heading>
          <Text color={textColorSecondary} mb="24px">
            Enter your new password below.
          </Text>

          <FormControl>
            <FormLabel color={textColor}>New Password</FormLabel>
            <Input
              type="password"
              placeholder="********"
              mb="16px"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />

            <FormLabel color={textColor}>Confirm Password</FormLabel>
            <Input
              type="password"
              placeholder="********"
              mb="24px"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
            />

            <Button
              w="100%"
              variant="brand"
              onClick={handleResetPassword}
              isLoading={loading}
            >
              Reset Password
            </Button>
          </FormControl>
        </Box>
      </Flex>
    </DefaultAuth>
  );
}

export default ResetPassword;
