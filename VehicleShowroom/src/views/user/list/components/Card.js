import React from 'react';
import {
  Box,
  Image,
  Text,
  VStack,
  Button,
  useColorModeValue,
  Flex,
  Spacer,
} from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import Spec from './Spec';
import { formatUSD } from 'utils/FormatHelper';

export default function Card({ item }) {
  const navigate = useNavigate();
  const cardBg = useColorModeValue('white', 'gray.800');

  const fuelType = item.specs?.find(
    (s) => s.specName === 'Fuel Type',
  )?.specValue;
  const photo = item.photo || '/placeholder-car.png';

  // 🔹 Lấy 3 thông số chính
  const accelSpec = item.specs?.find(
    (s) => s.specName === 'Acceleration 0 - 100 km/h',
  );
  const powerKW = item.specs?.find((s) => s.specName === 'Power (kW)');
  const powerPS = item.specs?.find((s) => s.specName === 'Power (PS)');
  const topSpeed = item.specs?.find((s) => s.specName === 'Top speed');

  const performanceSpecs = [
    accelSpec && {
      head: 'Acceleration 0 - 100 km/h',
      sub: accelSpec.specValue,
    },
    (powerKW || powerPS) && {
      head: 'Power (kW) / Power (PS)',
      sub:
        powerKW && powerPS
          ? `${powerKW.specValue} kW / ${powerPS.specValue} PS`
          : powerKW
          ? `${powerKW.specValue} kW`
          : powerPS
          ? `${powerPS.specValue} PS`
          : 'N/A',
    },
    topSpeed && { head: 'Top speed', sub: topSpeed.specValue },
  ].filter(Boolean);

  return (
    <Flex
      direction="column"
      justify="space-between"
      bg={cardBg}
      borderRadius="xl"
      p={5}
      shadow="md"
      transition="all 0.3s ease"
      _hover={{ transform: 'translateY(-5px)', shadow: '2xl' }}
      cursor="pointer"
      h="100%"
      onClick={() => navigate(`/user/detail/${item.slug}`)}
    >
      <VStack spacing={4} align="stretch" flex="1">
        {/* 🔹 Ảnh */}
        <Box position="relative" overflow="hidden" borderRadius="lg">
          <Image
            src={photo}
            alt={item.name}
            w="full"
            h="200px"
            objectFit="contain"
          />
          {fuelType && (
            <Text
              position="absolute"
              top={3}
              left={3}
              bg="gray.200"
              color="black"
              fontSize="sm"
              px={2}
              py={1}
              borderRadius="md"
            >
              {fuelType}
            </Text>
          )}
        </Box>

        {/* 🔹 Tên & Giá */}
        <Box>
          <Text fontSize="xl" fontWeight="600" mb={1}>
            {item.name}
          </Text>
          <Text color="gray.500" fontSize="sm">
            from {formatUSD(item.price) || 0}
          </Text>
        </Box>

        {/* 🔹 Thông số kỹ thuật */}
        <VStack align="start" spacing={2}>
          {performanceSpecs.length > 0 ? (
            performanceSpecs.map((spec, idx) => (
              <Spec key={idx} head={spec.head} sub={spec.sub} />
            ))
          ) : (
            <Text color="gray.400" fontSize="sm" fontStyle="italic">
              No specs available.
            </Text>
          )}
        </VStack>

        <Spacer />
      </VStack>

      {/* 🔹 Nút xem chi tiết ở dưới cùng */}
      <Button
        mt={4}
        bg="black"
        color="white"
        w="full"
        p={6}
        _hover={{ bg: 'gray.700' }}
        onClick={(e) => {
          e.stopPropagation();
          navigate(`/user/detail/${item.slug}`);
        }}
      >
        View Details
      </Button>
    </Flex>
  );
}
