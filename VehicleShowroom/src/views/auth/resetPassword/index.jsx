// views/auth/ResetPassword.jsx
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
import DefaultAuth from 'layouts/auth/Default';
import illustration from 'assets/img/auth/auth.png';
import AuthService from 'services/AuthService';
import { useShowToast } from 'utils/helper';

function ResetPassword() {
  const textColor = useColorModeValue('navy.700', 'white');
  const textColorSecondary = 'gray.400';
  const { showToast } = useShowToast();
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const token = searchParams.get('token');

  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');

  const handleResetPassword = async () => {
    if (!password || !confirmPassword) {
      showToast('Please fill in all fields', 'warning');
      return;
    }

    if (password !== confirmPassword) {
      showToast('Passwords do not match', 'error');
      return;
    }

    try {
      await AuthService.resetPassword({ token, password });
      showToast('Password reset successfully!', 'success');
      navigate('/auth/sign-in');
    } catch (error) {
      console.error(error);
      showToast('Failed to reset password', 'error');
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
        <Box>
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

            <Button w="100%" variant="brand" onClick={handleResetPassword}>
              Reset Password
            </Button>
          </FormControl>
        </Box>
      </Flex>
    </DefaultAuth>
  );
}

export default ResetPassword;
