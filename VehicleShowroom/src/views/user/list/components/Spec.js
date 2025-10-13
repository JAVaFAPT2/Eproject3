import { VStack, Text } from '@chakra-ui/react';

export default function Spec({ head, sub }) {
  return (
    <VStack align="start" w="full">
      <Text color="black" fontWeight="600" fontSize="xl">
        {sub}
      </Text>
      <Text color="gray.700" fontSize="sm">
        {head}
      </Text>
    </VStack>
  );
}
