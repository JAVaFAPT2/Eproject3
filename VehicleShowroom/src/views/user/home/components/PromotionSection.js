import { Box, Button, Text } from '@chakra-ui/react';
import { MotionFlex, MotionText, MotionImage } from './MotionPrimitives';

export default function PromotionSection({ image, title, desc, buttonText, buttonIcon, colorScheme, bgColor, reverse, textColor }) {
  return (
    <MotionFlex
      bg={bgColor}
      shadow="md"
      direction={{ base: 'column', md: reverse ? 'row-reverse' : 'row' }}
      align="center"
      justify="space-between"
      gap={{ base: 10, md: 20 }}
      px={{ base: 6, md: 20 }}
      py={16}
      borderRadius="2xl"
      mx={{ base: 4, md: 20 }}
      mb={20}
    >
      <Box width="400px" textAlign="center">
        <MotionImage src={image} alt={title} borderRadius="2xl" objectFit="contain" w="100%" h="300px" />
      </Box>
      <Box flex={1} maxW="500px">
        <MotionText fontSize={{ base: '2xl', md: '4xl' }} fontWeight="bold" color={textColor} mb={4}>
          {title}
        </MotionText>
        <Text fontSize="lg" color="gray.500" mb={6}>
          {desc}
        </Text>
        <Button leftIcon={buttonIcon} size="lg" colorScheme={colorScheme} rounded="full" color={colorScheme === 'brand' ? 'white' : undefined}>
          {buttonText}
        </Button>
      </Box>
    </MotionFlex>
  );
}
