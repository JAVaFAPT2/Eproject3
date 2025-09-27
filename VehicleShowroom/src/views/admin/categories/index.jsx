import React from 'react';
import { Box } from '@chakra-ui/react';
import CategoryTable from './components/CategoryTable';

function CategoryPage() {
  return (
    <Box pt={{ base: '130px', md: '80px', xl: '80px' }}>
      <CategoryTable />
    </Box>
  );
}

export default CategoryPage;
