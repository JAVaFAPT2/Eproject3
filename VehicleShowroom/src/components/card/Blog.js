import { Box, Image, Text, Card, Flex } from '@chakra-ui/react';
import React from 'react';

export default function Blog({ thumbnail, title }) {
  return (
    <Card p="20px">
      <Flex direction={{ base: 'column' }} justify="center">
        {/* Thumbnail */}
        {thumbnail && (
          <Box mb={{ base: '20px', '2xl': '20px' }} position="relative">
            <Image
              src={thumbnail}
              alt={title}
              w="100%"
              h="400px"
              borderRadius="20px"
              objectFit="cover"
            />
          </Box>
        )}

        {/* Title */}
        <Text fontWeight="600" fontSize="lg" noOfLines={2}>
          {title}
        </Text>
      </Flex>
    </Card>
  );
}
