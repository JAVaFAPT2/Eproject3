import { Box, SimpleGrid, Text } from '@chakra-ui/react';
import { FaShoppingCart, FaShippingFast, FaLock, FaBlog } from 'react-icons/fa';
import { MotionFlex } from './MotionPrimitives';

export default function FeaturesSection({ bgColor, brandColor }) {
  const features = [
    { title: 'Easy Shopping', desc: 'Add to cart & checkout in just a few steps.', icon: FaShoppingCart },
    { title: 'Free Shipping', desc: 'Enjoy free delivery on all orders over 500k.', icon: FaShippingFast },
    { title: 'Secure Payment', desc: 'Safe checkout with multiple payment options.', icon: FaLock },
    { title: 'Fashion Blog', desc: 'Discover the latest trends & styling tips.', icon: FaBlog },
  ];

  return (
    <SimpleGrid columns={{ base: 1, md: 4 }} spacing={10} px={{ base: 6, md: 20 }} mb={20}>
      {features.map((f, i) => (
        <MotionFlex
          key={i}
          direction="column"
          align="center"
          p={6}
          borderRadius="xl"
          shadow="md"
          bg={bgColor}
          textAlign="center"
        >
          <Box fontSize="3xl" mb={3} color={brandColor}>
            <f.icon />
          </Box>
          <Text fontWeight="bold" fontSize="xl" mb={2}>
            {f.title}
          </Text>
          <Text color="gray.500">{f.desc}</Text>
        </MotionFlex>
      ))}
    </SimpleGrid>
  );
}
