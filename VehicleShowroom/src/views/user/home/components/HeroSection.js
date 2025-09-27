import { Box, Button } from '@chakra-ui/react';
import Welcome from 'assets/img/home/welcome.png';
import { MotionFlex, MotionText, MotionImage } from './MotionPrimitives';

export default function HeroSection() {
  return (
    <MotionFlex
      direction={{ base: 'column', md: 'row' }}
      align="center"
      justify="space-between"
      gap={{ base: 10, md: 20 }}
      px={{ base: 6, md: 20 }}
      mb={20}
      initial={{ opacity: 0, y: 40 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.8 }}
    >
      <Box flex={1} maxW="500px">
        <MotionText
          fontSize={{ base: '3xl', md: '5xl' }}
          fontWeight="bold"
          mb={4}
          bgGradient="linear(to-r, #4facfe, #7366ff, #d633ff, #ff6a00)"
          bgClip="text"
        >
          Welcome to <span>Trendify</span>
        </MotionText>
        <MotionText fontSize={{ base: 'md', md: 'lg' }} color="gray.500" mb={6}>
          Discover the latest fashion trends, exclusive deals, and unique
          accessories that define your style.
        </MotionText>
        <Button size="lg" color="white" colorScheme="brand" rounded="full">
          Shop Now
        </Button>
      </Box>

      <Box width="400px" textAlign="center">
        <MotionImage
          src={Welcome}
          alt="Welcome"
          borderRadius="2xl"
          objectFit="contain"
          w="100%"
          h="300px"
        />
      </Box>
    </MotionFlex>
  );
}
