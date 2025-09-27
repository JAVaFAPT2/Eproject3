import React from 'react';
import { Box } from '@chakra-ui/react';
import CouponTable from './components/CouponTable';

function CouponPage() {
  return (
    <>
      <Box pt={{ base: '130px', md: '80px', xl: '80px' }}>
        <CouponTable />
      </Box>
    </>
  );
}

export default CouponPage;
