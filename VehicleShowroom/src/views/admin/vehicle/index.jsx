import React from 'react';
import { Box } from '@chakra-ui/react';
import Table from './components/Table';

function VehiclePage() {
  return (
    <Box pt={{ base: '130px', md: '80px', xl: '80px' }}>
      <Table />
    </Box>
  );
}

export default VehiclePage;
