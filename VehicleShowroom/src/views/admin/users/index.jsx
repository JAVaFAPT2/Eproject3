import React from 'react';
import UserTable from './components/UserTable';
import { Box } from '@chakra-ui/react';
import RoleStats from './components/RoleStats';

function UserPage() {
  return (
    <>
      <Box pt={{ base: '130px', md: '80px', xl: '80px' }}>
        <RoleStats />
        <UserTable />
      </Box>
    </>
  );
}

export default UserPage;
