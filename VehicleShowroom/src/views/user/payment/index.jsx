import React, { useState, useEffect } from 'react';
import {
  Box,
  Flex,
  Icon,
  Text,
  useColorModeValue,
} from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import { FaChevronLeft } from 'react-icons/fa';
import { useUser } from 'contexts/UserContext';
import PaymentUserForm from 'views/user/payment/components/PaymentUserForm';
import PaymentSummary from 'views/user/payment/components/PaymentSummary';

export default function PaymentPage() {
  const { user } = useUser();
  const navigate = useNavigate();
  const bgColor = useColorModeValue('white', 'navy.800');

  const [formData, setFormData] = useState({
    fullName: '',
    email: '',
    phone: '',
    addressStreet: '',
    addressWard: '',
    addressDistrict: '',
    addressCity: '',
  });

  const [cart, setCart] = useState({
    items: [
      {
        id: 1,
        productName: 'Denim Jacket',
        variantName: 'Blue / M',
        price: 65,
        quantity: 2,
        imageUrl:
          'https://images.unsplash.com/photo-1526170375885-4d8ecf77b99f?w=400',
      },
      {
        id: 2,
        productName: 'Sneakers',
        variantName: 'White / 42',
        price: 12,
        quantity: 1,
        imageUrl:
          'https://images.unsplash.com/photo-1606813902779-9f6e6e848d7b?w=400',
      },
    ],
    totalPrice: 65 * 2 + 12,
  });

  useEffect(() => {
    if (user) {
      setFormData({
        fullName: user.fullName || '',
        email: user.email || '',
        phone: user.phone || '',
        addressStreet: user.addressStreet || '',
        addressWard: user.addressWard || '',
        addressDistrict: user.addressDistrict || '',
        addressCity: user.addressCity || '',
      });
    }
  }, [user]);

  return (
    <Box bg={bgColor} p={8}>
      <Flex direction={'column'}>
        <Flex
          align="center"
          gap={2}
          mb={6}
          w="fit-content"
          _hover={{ cursor: 'pointer', color: 'brand.500' }}
          transition="color 0.2s"
          onClick={() => navigate(-1)}
        >
          <Icon as={FaChevronLeft} boxSize={4} />
          <Text fontWeight="medium" fontSize="md">
            Back
          </Text>
        </Flex>
        <Flex direction={{ base: 'column', md: 'row' }} gap={8}>
          <PaymentUserForm formData={formData} setFormData={setFormData} />
          <PaymentSummary cart={cart} formData={formData} />
        </Flex>
      </Flex>
    </Box>
  );
}
