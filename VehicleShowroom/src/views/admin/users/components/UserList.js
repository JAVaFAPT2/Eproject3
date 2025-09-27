import React from 'react';
import {
  Box,
  Table,
  Thead,
  Tr,
  Th,
  Tbody,
  useColorModeValue,
} from '@chakra-ui/react';
import { flexRender } from '@tanstack/react-table';
import UserRow from './UserRow';

export default function UserList({ table, textColor }) {
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const headerBg = useColorModeValue('gray.100', 'navy.800');
  const bgColor = useColorModeValue('white', 'navy.800');

  return (
    <Box minH="400px" overflowX="auto" bg={bgColor} borderRadius="10px" p={3}>
      <Table variant="simple" bg={bgColor}>
        <Thead bg={headerBg}>
          {table.getHeaderGroups().map((headerGroup) => (
            <Tr key={headerGroup.id}>
              {headerGroup.headers.map((header) => (
                <Th
                  key={header.id}
                  borderColor={borderColor}
                  fontSize="12px"
                  color={textColor}
                >
                  {flexRender(
                    header.column.columnDef.header,
                    header.getContext(),
                  )}
                </Th>
              ))}
            </Tr>
          ))}
        </Thead>
        <Tbody>
          {table.getRowModel().rows.map((row) => (
            <UserRow key={row.id} row={row} textColor={textColor} />
          ))}
        </Tbody>
      </Table>
    </Box>
  );
}
