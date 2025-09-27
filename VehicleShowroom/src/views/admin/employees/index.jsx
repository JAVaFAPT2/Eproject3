import React from 'react';
import { Box } from '@chakra-ui/react';
import EmployeeTable from './components/EmployeeTable';

function EmployeePage() {
  return (
    <>
      <Box pt={{ base: '130px', md: '80px', xl: '80px' }}>
        <EmployeeTable />
      </Box>
    </>
  );
}

export default EmployeePage;
