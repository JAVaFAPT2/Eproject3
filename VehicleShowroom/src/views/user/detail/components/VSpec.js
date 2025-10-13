import { VStack, Text } from '@chakra-ui/react';

export default function Spec({ head, sub }) {
  return (
    <VStack align="start" w="full">
      <Text color="black" fontSize="7xl">
        {sub}
      </Text>
      <Text color="gray.700" fontSize="md">
        {head}
      </Text>
    </VStack>
  );
}
