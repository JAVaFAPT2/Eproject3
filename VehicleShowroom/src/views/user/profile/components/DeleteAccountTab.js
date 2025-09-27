import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  VStack,
  Text,
  useToast,
  useColorModeValue,
} from '@chakra-ui/react';
import UserService from 'services/UserService';

export default function DeleteAccountTab() {
  const toast = useToast();
  const bgColor = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const [code, setCode] = useState('');
  const [input, setInput] = useState('');

  useEffect(() => {
    const randomCode = Math.random().toString(36).substring(2, 8).toUpperCase();
    setCode(randomCode);
  }, []);

  const handleDelete = async () => {
    if (input !== code) {
      toast({
        title: 'Incorrect code',
        status: 'error',
        isClosable: true,
        duration: 3000,
        position: 'bottom-right',
      });
      return;
    }
    try {
      await UserService.deleteAccount();
      toast({
        title: 'Account deleted',
        status: 'success',
        isClosable: true,
        duration: 3000,
        position: 'bottom-right',
      });
      window.location.href = '/auth/sign-in';
    } catch (err) {
      console.error(err);
      toast({
        title: 'Delete failed',
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
        <Text>To delete your account, type the code below to confirm:</Text>
        <Text fontWeight="bold" fontSize="xl">
          {code}
        </Text>

        <FormControl>
          <FormLabel>Enter Code</FormLabel>
          <Input
            color={textColor}
            value={input}
            onChange={(e) => setInput(e.target.value)}
          />
        </FormControl>
      </VStack>
      <Button colorScheme="red" onClick={handleDelete} mt={5}>
        Delete Account
      </Button>
    </Box>
  );
}
