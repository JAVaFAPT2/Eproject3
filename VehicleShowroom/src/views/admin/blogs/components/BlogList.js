import React from 'react';
import {
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Box,
  useColorModeValue,
  Td,
} from '@chakra-ui/react';
import { flexRender } from '@tanstack/react-table';

export default function BlogList({ table }) {
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const headerBg = useColorModeValue('gray.100', 'navy.800');
  const bgColor = useColorModeValue('white', 'navy.800');

  return (
    <Box minH="400px" overflowX="auto" p={3}>
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
            <Tr key={row.id}>
              {row.getVisibleCells().map((cell) => (
                <Td key={cell.id} borderColor={borderColor}>
                  {flexRender(cell.column.columnDef.cell, cell.getContext())}
                </Td>
              ))}
            </Tr>
          ))}
        </Tbody>
      </Table>
    </Box>
  );
}
