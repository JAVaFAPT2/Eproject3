import { Box, Text, InputGroup, Input, InputRightElement, Button } from '@chakra-ui/react';
import { MotionText } from './MotionPrimitives';

export default function SubscribeSection({ bgColor, textColor }) {
  return (
    <Box shadow="md" textAlign="center" px={{ base: 6, md: 20 }} py={16} bg={bgColor} borderRadius="2xl" mx={{ base: 4, md: 20 }}>
      <MotionText fontSize={{ base: '2xl', md: '4xl' }} fontWeight="bold" mb={4}>
        Join the Trendify Community
      </MotionText>
      <Text fontSize="lg" color="gray.500" mb={6}>
        Subscribe to get the latest deals, trends, and fashion news delivered to your inbox.
      </Text>
      <Box maxW="500px" mx="auto">
        <InputGroup size="lg">
          <Input placeholder="Enter your email" bg="transparent" borderRadius="full" pr="120px" color={textColor} />
          <InputRightElement width="100px">
            <Button h="full" size="md" color="white" colorScheme="brand" rounded="full">
              Subscribe
            </Button>
          </InputRightElement>
        </InputGroup>
      </Box>
    </Box>
  );
}
