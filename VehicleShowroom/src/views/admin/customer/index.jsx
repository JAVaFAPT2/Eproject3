import React from 'react';
import Table from './components/Table';
import { Box }  from '@chakra-ui/react'; 

function CustomerPage() {
  return (
    <>
      <Box pt={{ base: '130px', md: '80px', xl: '80px' }}>
        <Table />
      </Box>
    </>
  );
}

export default CustomerPage;
