import React from 'react';
import {
  Box,
  Text,
  Flex,
  Stack,
  Badge,
  useColorModeValue,
  Divider,
} from '@chakra-ui/react';
import { motion } from 'framer-motion';

// Map enum cho ServiceOrderStatus
const SERVICE_STATUS_MAP = {
  1: 'Scheduled',
  2: 'In Progress',
  3: 'Completed',
  4: 'Cancelled',
};

const STATUS_COLOR = {
  1: 'yellow',
  2: 'blue',
  3: 'green',
  4: 'red',
};

const SERVICE_TYPE_MAP = {
  1: 'Pre-Delivery',
  2: 'Maintenance',
  3: 'Repair',
};

const MotionBox = motion(Box);

export default function ServiceHistoryTab({ services }) {
  const bgCard = useColorModeValue('white', 'navy.700');
  const borderColor = useColorModeValue('gray.200', 'gray.600');

  if (!services || services.length === 0)
    return <Text>No service history found.</Text>;

  const isValidDate = (dateStr) => {
    if (!dateStr) return false;
    const date = new Date(dateStr);
    return !isNaN(date.getTime());
  };

  return (
    <Box>
      <Text fontSize="xl" fontWeight="bold" mb={4}>
        Service History
      </Text>

      <Stack spacing={5}>
        {services.map((s) => {
          const validDate = isValidDate(s.appointmentDate);
          const appointmentLabel = validDate
            ? new Date(s.appointmentDate).toLocaleDateString()
            : 'Pending';

          return (
            <MotionBox
              key={s.id}
              p={5}
              borderWidth="1px"
              borderColor={borderColor}
              borderRadius="lg"
              bg={bgCard}
              shadow="sm"
              whileHover={{ scale: 1.01 }}
              transition="0.2s"
            >
              <Flex justify="space-between" align="center" mb={2}>
                <Text fontWeight="600">#{s.id}</Text>
                <Badge colorScheme={STATUS_COLOR[s.status] || 'gray'}>
                  {SERVICE_STATUS_MAP[s.status] || 'Unknown'}
                </Badge>
              </Flex>

              <Divider my={3} />

              <Flex justify="space-between" wrap="wrap" rowGap={2}>
                <Box>
                  <Text fontWeight="500">Type</Text>
                  <Text>{SERVICE_TYPE_MAP[s.type] || 'Unknown'}</Text>
                </Box>

                <Box>
                  <Text fontWeight="500">Cost</Text>
                  <Text>{s.cost ? `$${s.cost.toLocaleString()}` : '0'}</Text>
                </Box>

                <Box>
                  <Text fontWeight="500">Appointment Date</Text>
                  <Text>{appointmentLabel}</Text>
                </Box>
              </Flex>

              {s.description && (
                <Box mt={3}>
                  <Text fontWeight="500">Description</Text>
                  <Text fontSize="sm" color="gray.600">
                    {s.description}
                  </Text>
                </Box>
              )}
            </MotionBox>
          );
        })}
      </Stack>
    </Box>
  );
}
