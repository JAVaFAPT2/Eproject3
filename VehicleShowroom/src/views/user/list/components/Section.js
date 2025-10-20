import React from 'react';
import { Box, SimpleGrid } from '@chakra-ui/react';
import Card from './Card';

export default function Section({ vehicles }) {
  if (!vehicles || vehicles.length === 0) {
    return (
      <Box color="gray.500" fontStyle="italic" py={10}>
        No models available for the selected filters.
      </Box>
    );
  }
  return (
    <Box>
      <SimpleGrid columns={{ base: 1, xl: 3 }} spacing={8}>
        {vehicles.map((item) => (
          <Card key={item.modelNumber} item={item} />
        ))}
      </SimpleGrid>
    </Box>
  );
}
