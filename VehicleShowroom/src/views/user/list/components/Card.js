import React from 'react';
import {
  Box,
  Image,
  Text,
  VStack,
  Button,
  useColorModeValue,
  Divider,
} from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import Spec from 'views/user/list/components/Spec';

export default function Card({ item }) {
  const navigate = useNavigate();
  const cardBg = useColorModeValue('white', 'gray.800');

  // 🔹 Specs
  const performanceSpecs =
    item.specs?.filter((s) => s.groupName === 'GeneralPerformance') || [];

  const fuelType = item.specs?.find(
    (s) => s.specName === 'Fuel Type',
  )?.specValue;

  // 🔹 Ảnh đầu tiên (ưu tiên vehicle-specific, fallback model photo)
  const mainPhoto = item.photos?.[0]?.url || '/placeholder-car.png';

  return (
    <Box
      bg={cardBg}
      borderRadius="xl"
      p={5}
      shadow="md"
      transition="all 0.3s ease"
      _hover={{ transform: 'translateY(-5px)', shadow: '2xl' }}
      cursor="pointer"
      onClick={() => navigate(`/user/model/${item.vehicleId}`)}
    >
      <VStack spacing={4} align="stretch">
        {/* 🔹 Ảnh xe */}
        <Box position="relative" overflow="hidden" borderRadius="lg">
          <Image
            src={mainPhoto}
            alt={item.name}
            w="full"
            h="200px"
            objectFit="contain"
            transition="transform 0.4s ease"
            _hover={{ transform: 'translateX(10px)' }}
          />

          {fuelType && (
            <Text
              position="absolute"
              top={3}
              left={3}
              bg="blackAlpha.700"
              color="white"
              fontSize="sm"
              px={2}
              py={1}
              borderRadius="md"
            >
              {fuelType}
            </Text>
          )}
        </Box>

        {/* 🔹 Tên xe và giá */}
        <Box>
          <Text fontSize="xl" fontWeight="600" mb={1}>
            {item.name}
          </Text>
          <Text color="gray.500" fontSize="sm">
            from ${item.purchasePrice?.toLocaleString()}
          </Text>
        </Box>

        <Divider />

        {/* 🔹 Các thông số kỹ thuật */}
        <VStack align="start" spacing={2}>
          {performanceSpecs.length > 0 ? (
            performanceSpecs.map((spec) => (
              <Spec
                key={spec.specId}
                head={spec.specName}
                sub={spec.specValue}
              />
            ))
          ) : (
            <Text color="gray.400" fontSize="sm" fontStyle="italic">
              No specs available.
            </Text>
          )}
        </VStack>

        {/* 🔹 Nút hành động */}
        <Button
          mt={3}
          bg="black"
          color="white"
          w="full"
          p={6}
          _hover={{ bg: 'gray.700' }}
          onClick={() => navigate(`/user/model/${item.vehicleId}`)}
        >
          View Details
        </Button>
      </VStack>
    </Box>
  );
}
