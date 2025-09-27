import React, { useState } from 'react';
import {
  Box,
  Heading,
  VStack,
  Flex,
  Image,
  Text,
  Divider,
  Button,
  useColorModeValue,
  useToast,
  Input,
  HStack,
  Badge,
} from '@chakra-ui/react';

export default function PaymentSummary({ cart, formData }) {
  const sectionBg = useColorModeValue('gray.50', 'navy.700');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const toast = useToast();

  const [couponCode, setCouponCode] = useState('');
  const [discount, setDiscount] = useState(0);
  const [applied, setApplied] = useState(false);

  const handleApplyCoupon = () => {
    const code = couponCode.trim().toUpperCase();
    if (!code) {
      toast({
        title: 'Please enter a coupon code.',
        status: 'info',
        duration: 2000,
        isClosable: true,
        position: 'bottom-right',
      });
      return;
    }
    if (code === 'DISCOUNT10') {
      setDiscount(0.1);
      setApplied(true);
      toast({
        title: 'Coupon applied! 10% discount.',
        status: 'success',
        duration: 2000,
        isClosable: true,
        position: 'bottom-right',
      });
    } else {
      setDiscount(0);
      setApplied(false);
      toast({
        title: 'Invalid coupon code.',
        status: 'warning',
        duration: 2000,
        isClosable: true,
        position: 'bottom-right',
      });
    }
  };

  const handlePlaceOrder = () => {
    const requiredFields = Object.values(formData).every(
      (v) => v && v.trim() !== '',
    );
    if (!requiredFields) {
      toast({
        title: 'Please fill all required fields.',
        status: 'warning',
        duration: 3000,
        isClosable: true,
        position: 'bottom-right',
      });
      return;
    }
    toast({
      title: 'Order placed successfully!',
      status: 'success',
      duration: 3000,
      isClosable: true,
      position: 'bottom-right',
    });
  };

  // 💲 Format USD
  const formatUSD = (value) =>
    `$${Number(value).toLocaleString('en-US', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    })}`;

  const subtotal = cart.totalPrice;
  const discountValue = subtotal * discount;
  const finalTotal = subtotal - discountValue;

  return (
    <Box
      flex="1"
      h="100%"
      w={{ base: '100%', md: 'auto' }}
      bg={sectionBg}
      p={6}
      borderRadius="12px"
    >
      <Heading size="md" mb={6} color={textColor}>
        Order Summary
      </Heading>

      {/* 🛍️ Product List */}
      <VStack spacing={4} align="stretch" divider={<Divider />}>
        {cart.items.map((item) => (
          <Flex key={item.id} align="center" justify="space-between" gap={4}>
            <Box position="relative">
              <Image
                src={item.imageUrl}
                boxSize="80px"
                borderRadius="8px"
                objectFit="cover"
              />
              {/* 🔢 Quantity badge */}
              <Badge
                position="absolute"
                top="-10px"
                right="-10px"
                bg="brand.500"
                color="white"
                borderRadius="full"
                px={2}
                fontSize="xs"
              >
                {item.quantity}
              </Badge>
            </Box>

            <Box flex="1">
              <Text fontWeight="bold">{item.productName}</Text>
              <Text fontSize="sm" color="gray.500">
                {item.variantName}
              </Text>
            </Box>

            {/* 💰 Total price per product */}
            <Text fontWeight="semibold">
              {formatUSD(item.price * item.quantity)}
            </Text>
          </Flex>
        ))}
      </VStack>

      <Divider my={6} />

      {/* 🧾 Coupon Input */}
      <Box mb={4}>
        <Text fontWeight="semibold" mb={2} color={textColor}>
          Coupon Code 
        </Text>
        <HStack>
          <Input
            placeholder="Enter coupon code"
            value={couponCode}
            onChange={(e) => setCouponCode(e.target.value)}
            disabled={applied}
            color={textColor}
          />
          <Button
            colorScheme="brand"
            onClick={handleApplyCoupon}
            isDisabled={applied}
            color={'white'}
          >
            {applied ? 'Applied' : 'Apply'}
          </Button>
        </HStack>
        {applied && (
          <Text mt={2} fontSize="sm" color="green.500">
            ✅ 10% discount applied
          </Text>
        )}
      </Box>

      {/* 💵 Totals */}
      <VStack spacing={2} align="stretch" mb={4}>
        <Flex justify="space-between">
          <Text color={textColor}>Subtotal</Text>
          <Text color={textColor}>{formatUSD(subtotal)}</Text>
        </Flex>

        {applied && (
          <Flex justify="space-between" color="green.500">
            <Text>Discount (10%)</Text>
            <Text>-{formatUSD(discountValue)}</Text>
          </Flex>
        )}

        <Divider />

        <Flex justify="space-between" fontWeight="bold">
          <Text color={textColor}>Total</Text>
          <Text color={textColor}>{formatUSD(finalTotal)}</Text>
        </Flex>
      </VStack>

      <Button
        colorScheme="brand"
        color="white"
        w="full"
        onClick={handlePlaceOrder}
      >
        Place Order
      </Button>
    </Box>
  );
}
