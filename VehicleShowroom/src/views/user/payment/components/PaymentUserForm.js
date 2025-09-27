import React from 'react';
import {
  Box,
  Heading,
  VStack,
  FormControl,
  FormLabel,
  Input,
  useColorModeValue,
  SimpleGrid,
} from '@chakra-ui/react';

export default function PaymentUserForm({ formData, setFormData }) {
  const sectionBg = useColorModeValue('gray.50', 'navy.700');
  const textColor = useColorModeValue('secondaryGray.900', 'white');

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

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
        Shipping Information
      </Heading>

      <SimpleGrid columns={{ base: 1, md: 2 }} spacing={10}>
        {/* Cột trái - Personal Info */}
        <VStack spacing={6} align="stretch">
          <FormControl isRequired>
            <FormLabel>Full Name</FormLabel>
            <Input
              color={textColor}
              name="fullName"
              value={formData.fullName}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl isRequired>
            <FormLabel>Email</FormLabel>
            <Input
              color={textColor}
              type="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl isRequired>
            <FormLabel>Phone</FormLabel>
            <Input
              color={textColor}
              name="phone"
              value={formData.phone}
              onChange={handleChange}
            />
          </FormControl>
        </VStack>

        {/* Cột phải - Address Info */}
        <VStack spacing={6} align="stretch">
          <FormControl isRequired>
            <FormLabel>Street</FormLabel>
            <Input
              color={textColor}
              name="addressStreet"
              value={formData.addressStreet}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl isRequired>
            <FormLabel>Ward</FormLabel>
            <Input
              color={textColor}
              name="addressWard"
              value={formData.addressWard}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl isRequired>
            <FormLabel>District</FormLabel>
            <Input
              color={textColor}
              name="addressDistrict"
              value={formData.addressDistrict}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl isRequired>
            <FormLabel>City</FormLabel>
            <Input
              color={textColor}
              name="addressCity"
              value={formData.addressCity}
              onChange={handleChange}
            />
          </FormControl>
        </VStack>
      </SimpleGrid>
    </Box>
  );
}
