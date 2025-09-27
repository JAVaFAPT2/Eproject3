import {
  Drawer,
  DrawerBody,
  DrawerHeader,
  DrawerOverlay,
  DrawerContent,
  DrawerCloseButton,
  Text,
  Flex,
  Icon,
  Spinner,
  Button,
} from '@chakra-ui/react';
import { IoMdCart } from 'react-icons/io';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';

export default function CartSidebar({ isOpen, onClose }) {
  const [cart, setCart] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    if (isOpen) {
      setLoading(true);
      setTimeout(() => {
        setCart({
          items: [
            { id: 1, name: 'T-Shirt', quantity: 2 },
            { id: 2, name: 'Jeans', quantity: 1 },
          ],
          totalPrice: 550000,
        });
        setLoading(false);
      }, 600);
    }
  }, [isOpen]);

  const handleGoToPayment = () => {
    onClose();
    navigate('/user/payment');
  };

  return (
    <Drawer placement="right" onClose={onClose} isOpen={isOpen} size="sm">
      <DrawerOverlay />
      <DrawerContent w="400px">
        <DrawerCloseButton />
        <DrawerHeader>Shopping Cart</DrawerHeader>
        <DrawerBody>
          {loading ? (
            <Flex justify="center" align="center" h="100%">
              <Spinner />
            </Flex>
          ) : !cart || !cart.items || cart.items.length === 0 ? (
            <Flex
              direction="column"
              align="center"
              justify="center"
              h="100%"
              gap={3}
              color="gray.500"
            >
              <Icon as={IoMdCart} w={12} h={12} />
              <Text fontSize="md">Your cart is empty</Text>
            </Flex>
          ) : (
            <Flex direction="column" gap={3}>
              {cart.items.map((item) => (
                <Flex key={item.id} justify="space-between" align="center">
                  <Text>{item.name}</Text>
                  <Text>{item.quantity}</Text>
                </Flex>
              ))}

              <Flex justify="space-between" mt={4} fontWeight="bold">
                <Text>Total:</Text>
                <Text>{cart.totalPrice.toLocaleString()}₫</Text>
              </Flex>

              <Button
                mt={6}
                colorScheme="brand"
                color="white"
                onClick={handleGoToPayment}
              >
                Go to Payment
              </Button>
            </Flex>
          )}
        </DrawerBody>
      </DrawerContent>
    </Drawer>
  );
}
