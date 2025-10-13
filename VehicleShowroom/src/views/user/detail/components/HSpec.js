import { HStack, Text } from '@chakra-ui/react';

export default function HSpec({ head, sub }) {
  return (
    <HStack justify="space-between" align="center" w="full" mb={4}>
      <Text color="gray.700" fontSize="md">
        {head}
      </Text>
      <Text color="black" fontWeight="500" fontSize="md">
        {sub}
      </Text>
    </HStack>
  );
}
