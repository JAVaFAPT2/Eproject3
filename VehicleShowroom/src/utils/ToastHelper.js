import { useToast }  from '@chakra-ui/react'; 

export const useAppToast = () => {
  const toast = useToast();

  const show = (
    title,
    status = 'info',
    duration = 3000,
    position = 'bottom-right',
  ) => {
    toast({
      title,
      status,
      duration,
      isClosable: true,
      position,
    });
  };

  return {
    show,
    success: (title) => show(title, 'success'),
    error: (title) => show(title, 'error'),
    warning: (title) => show(title, 'warning'),
    info: (title) => show(title, 'info'),
  };
};
