import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
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

function ForgotPassword() {
  const textColor = useColorModeValue('navy.700', 'white');
  const textColorSecondary = 'gray.400';
  const toast = useAppToast();
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [loading, setLoading] = useState(false);

  const handleForgotPassword = async () => {
    if (!email) {
      toast.error('Please enter your email address');
      return;
    }

    setLoading(true);
    try {
      // ✅ Gọi API từ AuthService
      await AuthService.forgotPassword(email);

      toast.success('Password reset email sent!');
      navigate('/auth/check-email'); // điều hướng sang trang thông báo gửi thành công
    } catch (error) {
      console.error(error);
      const msg =
        error.response?.data?.message ||
        'Failed to send reset email. Please try again.';
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
        h="70vh"
        alignItems="center"
        justifyContent="flex-start"
        flexDirection="column"
        px={{ base: '25px', md: '0px' }}
        mt={{ base: '40px', md: '10vh' }}
      >
        <Box w={{ base: '100%', md: '420px' }}>
          <Heading color={textColor} fontSize="32px" mb="10px">
            Forgot Password
          </Heading>
          <Text color={textColorSecondary} mb="24px">
            Enter your email address and we’ll send you a link to reset your
            password.
          </Text>

          <FormControl>
            <FormLabel color={textColor}>Email address</FormLabel>
            <Input
              type="email"
              placeholder="mail@example.com"
              mb="24px"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
            <Button
              w="100%"
              variant="brand"
              onClick={handleForgotPassword}
              isLoading={loading}
            >
              Send Reset Link
            </Button>
          </FormControl>
        </Box>
      </Flex>
    </DefaultAuth>
  );
}

export default ForgotPassword;
