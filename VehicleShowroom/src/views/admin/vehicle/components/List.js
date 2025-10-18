import React from 'react';
import { Table, Thead, Tbody, Tr, Th, Td, Box, Text } from '@chakra-ui/react';
import { flexRender } from '@tanstack/react-table';

export default function List({ table, borderColor, textColor, emptyMessage = 'No data available' }) {
  const rows = table.getRowModel().rows;
  const headers = table.getHeaderGroups()[0]?.headers || [];

  return (
    <Box minH="600px" overflowX="auto" p={3}>
      <Table variant="simple" color={textColor}>
        <Thead>
          {table.getHeaderGroups().map((headerGroup) => (
            <Tr key={headerGroup.id}>
              {headerGroup.headers.map((header) => (
                <Th key={header.id} borderColor={borderColor}>
                  {flexRender(header.column.columnDef.header, header.getContext())}
                </Th>
              ))}
            </Tr>
          ))}
        </Thead>

        <Tbody>
          {rows.length > 0 ? (
            rows.map((row) => (
              <Tr key={row.id}>
                {row.getVisibleCells().map((cell) => (
                  <Td key={cell.id} borderColor={borderColor}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </Td>
                ))}
              </Tr>
            ))
          ) : (
            <Tr>
              <Td
                colSpan={headers.length || 1}
                textAlign="center"
                borderColor={borderColor}
                py={10}
              >
                <Text color="gray.500" fontStyle="italic">
                  {emptyMessage}
                </Text>
              </Td>
            </Tr>
          )}
        </Tbody>
      </Table>
    </Box>
  );
}
