import React, { useMemo } from 'react';
import {
  Box,
  Flex,
  Avatar,
  Text,
  VStack,
  useColorModeValue,
  Icon,
} from '@chakra-ui/react';
import { motion } from 'framer-motion';
import { FaTrophy, FaMedal } from 'react-icons/fa';

const MotionBox = motion(Box);

export default function EmployeeStanding({ employees = [] }) {
  const bgColor = useColorModeValue('white', 'navy.800');
  const borderColor = useColorModeValue('gray.200', 'gray.700');

  // 🏆 Màu podium
  const podiumColors = {
    1: useColorModeValue('yellow.300', 'yellow.600'),
    2: useColorModeValue('gray.400', 'gray.600'),
    3: useColorModeValue('orange.400', 'orange.600'),
  };

  // ✅ Sắp xếp giảm dần theo totalHours
  const sortedEmployees = useMemo(
    () => [...employees].sort((a, b) => (b.totalHours || 0) - (a.totalHours || 0)),
    [employees]
  );

  // ✅ Lấy top 3
  const top = sortedEmployees.slice(0, 3);
  while (top.length < 3) {
    top.push({
      id: Math.random(),
      fullName: 'N/A',
      totalHours: 0,
      avatarUrl: null,
    });
  }

  // 🏅 Icon theo rank
  const getIcon = (rank) => {
    if (rank === 1) return <Icon as={FaTrophy} w={7} h={7} color="yellow.500" />;
    if (rank === 2) return <Icon as={FaMedal} w={7} h={7} color="gray.500" />;
    if (rank === 3) return <Icon as={FaMedal} w={7} h={7} color="orange.500" />;
    return null;
  };

  // 🧱 Chiều cao podium
  const podiumHeights = { 1: 160, 2: 120, 3: 100 };
  const podiumOrder = [1, 0, 2]; // Hiển thị 2 - 1 - 3

  // 🗓️ Lấy tháng hiện tại
  const now = new Date();
  const currentMonth = now.toLocaleString('en-US', { month: 'long' });

  return (
    <Box
      bg={bgColor}
      border="1px solid"
      borderColor={borderColor}
      borderRadius="lg"
      p={6}
      shadow="md"
    >
      <Text fontSize="xl" fontWeight="bold" mb={6} textAlign="center">
        🏆 Top Employees – {currentMonth}
      </Text>

      <Flex justify="center" align="end" gap={8}>
        {podiumOrder.map((pos) => {
          const emp = top[pos];
          const rank = pos + 1;

          return (
            <VStack key={emp.id || pos} spacing={3}>
              {/* Avatar */}
              <Avatar size="lg" name={emp.fullName} src={emp.avatarUrl} mb={2} />

              {/* Podium */}
              <MotionBox
                w="100px"
                bg={podiumColors[rank]}
                borderRadius="md"
                display="flex"
                alignItems="center"
                justifyContent="center"
                initial={{ height: 0 }}
                animate={{ height: podiumHeights[rank] }}
                transition={{ duration: 0.8, ease: 'easeOut' }}
              >
                {getIcon(rank)}
              </MotionBox>

              {/* Info */}
              <Text fontWeight="semibold" textAlign="center" maxW="100px">
                {emp.fullName}
              </Text>
              <Text fontSize="sm" color="gray.500">
                {emp.totalHours && emp.totalHours > 0
                  ? `${emp.totalHours.toFixed(1)}h`
                  : '-'}
              </Text>
            </VStack>
          );
        })}
      </Flex>
    </Box>
  );
}
