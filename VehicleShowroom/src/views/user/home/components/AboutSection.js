import { Box, Text } from '@chakra-ui/react';
import { MotionText } from './MotionPrimitives';

export default function AboutSection() {
  return (
    <Box px={{ base: 6, md: 20 }} mb={20} textAlign="center">
      <MotionText fontSize={{ base: '2xl', md: '4xl' }} fontWeight="bold" mb={4}>
        Why Trendify?
      </MotionText>
      <Text fontSize="lg" color="gray.500" maxW="700px" mx="auto">
        Trendify brings you the ultimate online shopping experience – from
        clothing and accessories to a lifestyle blog. We help you define your
        own unique style.
      </Text>
    </Box>
  );
}
