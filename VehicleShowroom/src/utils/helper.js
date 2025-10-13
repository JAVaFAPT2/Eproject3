import { useToast } from '@chakra-ui/react';

export function useShowToast() {
  const toast = useToast();

  const showToast = (title, status = 'info', description = '') => {
    toast({
      title,
      description,
      status,
      duration: 3000,
      isClosable: true,
      position: 'bottom-right',
    });
  };

  return { showToast };
}
