import React from 'react';
import { Box } from '@chakra-ui/react';
import BlogTable from './components/BlogTable';

function BlogPage() {
  return (
    <Box pt={{ base: '130px', md: '80px', xl: '80px' }}>
      <BlogTable />
    </Box>
  );
}

export default BlogPage;
