import { Center, Spinner } from '@chakra-ui/react';

export function LoadingState() {
  return (
    <Center py={20}>
      <Spinner size="xl" color="brand.500" thickness="4px" />
    </Center>
  );
}