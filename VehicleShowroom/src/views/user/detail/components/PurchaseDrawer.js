import React, { useState } from 'react';
import {
  Drawer,
  DrawerBody,
  DrawerHeader,
  DrawerOverlay,
  DrawerContent,
  DrawerCloseButton,
  VStack,
  Input,
  FormControl,
  FormLabel,
  Button,
  useToast,
  Box,
} from '@chakra-ui/react';

export default function PurchaseDrawer({ isOpen, onClose, vehicle }) {
  const toast = useToast();
  const [form, setForm] = useState({
    name: '',
    email: '',
    phone: '',
    address: '',
  });

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const payload = {
      vehicleId: vehicle?.vehicleId,
      ...form,
    };

    console.log('🚗 Purchase form payload:', payload);

    toast({
      title: 'Inquiry Sent',
      description: 'Your contact details have been submitted successfully!',
      status: 'success',
      duration: 4000,
      isClosable: true,
    });

    setForm({ name: '', email: '', phone: '', address: '' });
    onClose();
  };

  return (
    <Drawer isOpen={isOpen} placement="right" onClose={onClose} size="md">
      <DrawerOverlay />
      <DrawerContent display="flex" flexDirection="column">
        <DrawerCloseButton />
        <DrawerHeader fontSize="2xl" fontWeight="700">
          Contact for Purchase
        </DrawerHeader>

        {/* 🔹 Nội dung form */}
        <DrawerBody flex="1" overflowY="auto" pb="100px">
          <form onSubmit={handleSubmit}>
            <VStack spacing={5} align="stretch">
              <FormControl isRequired>
                <FormLabel>Name</FormLabel>
                <Input
                  name="name"
                  value={form.name}
                  onChange={handleChange}
                  placeholder="Your full name"
                />
              </FormControl>

              <FormControl isRequired>
                <FormLabel>Email</FormLabel>
                <Input
                  type="email"
                  name="email"
                  value={form.email}
                  onChange={handleChange}
                  placeholder="example@email.com"
                />
              </FormControl>

              <FormControl isRequired>
                <FormLabel>Phone</FormLabel>
                <Input
                  name="phone"
                  value={form.phone}
                  onChange={handleChange}
                  placeholder="Your phone number"
                />
              </FormControl>

              <FormControl>
                <FormLabel>Address</FormLabel>
                <Input
                  name="address"
                  value={form.address}
                  onChange={handleChange}
                  placeholder="Your address"
                />
              </FormControl>
            </VStack>
          </form>
        </DrawerBody>

        {/* 🔹 Nút Submit luôn ở dưới */}
        <Box
          p={4}
          borderTopWidth="1px"
          bg="white"
          position="sticky"
          bottom="0"
          zIndex="1"
        >
          <Button
            type="submit"
            bg="black"
            color="white"
            size="lg"
            w="full"
            _hover={{ bg: 'gray.700' }}
            onClick={handleSubmit}
          >
            Submit
          </Button>
        </Box>
      </DrawerContent>
    </Drawer>
  );
}
