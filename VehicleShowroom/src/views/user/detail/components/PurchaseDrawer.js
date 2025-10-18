import React, { useState, useEffect } from 'react';
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
  Box,
} from '@chakra-ui/react';
import { useUser } from 'contexts/UserContext';
import OrderService from 'services/OrderService';
import ProfileService from 'services/ProfileService';
import { useAppToast } from 'utils/ToastHelper';

export default function PurchaseDrawer({ isOpen, onClose, vehicle }) {
  const toast = useAppToast();
  const { user, refreshUser } = useUser();

  const [form, setForm] = useState({
    name: '',
    email: '',
    phone: '',
    address: '',
  });

  useEffect(() => {
    if (user) {
      setForm({
        name: user.name || '',
        email: user.email || '',
        phone: user.phone || '',
        address: user.address || '',
      });
    }
  }, [user]);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!user) {
      toast.warning('Please sign in before making a purchase inquiry.');
      return;
    }

    try {
      await ProfileService.update({
        name: form.name,
        email: form.email,
        phone: form.phone,
        address: form.address,
      });

      await refreshUser();

      const payload = {
        customerId: user.id,
        modelNumber: vehicle?.modelNumber,
        salePrice: vehicle?.price || 0,
      };

      await OrderService.create(payload);

      toast.success('Your order has been successfully submitted!');

      onClose();
    } catch (err) {
      toast.error(
        err.response?.data?.message ||
          'Something went wrong while processing your order please try again.',
      );
    }
  };

  return (
    <Drawer isOpen={isOpen} placement="right" onClose={onClose} size="md">
      <DrawerOverlay />
      <DrawerContent display="flex" flexDirection="column">
        <DrawerCloseButton />
        <DrawerHeader fontSize="2xl" fontWeight="700">
          Contact for Purchase
        </DrawerHeader>

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

              <FormControl isRequired>
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
