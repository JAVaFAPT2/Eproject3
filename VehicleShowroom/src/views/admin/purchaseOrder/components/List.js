import React from 'react';
import {
  Table,
  Thead,
  Tbody,
  Tr,
  Th,
  Td,
  Box,
  Spinner,
  Flex,
  Text,
} from '@chakra-ui/react';
import { flexRender } from '@tanstack/react-table';

export default function List({
  table,
  borderColor,
  textColor,
  loading = false,
}) {
  const rows = table.getRowModel().rows;
  const headers = table.getHeaderGroups()[0]?.headers || [];

  return (
    <Box minH="600px" overflowX="auto" p={3}>
      <Table variant="simple" color={textColor}>
        {/* ✅ Header luôn hiển thị */}
        <Thead>
          {table.getHeaderGroups().map((headerGroup) => (
            <Tr key={headerGroup.id}>
              {headerGroup.headers.map((header) => (
                <Th key={header.id} borderColor={borderColor}>
                  {flexRender(
                    header.column.columnDef.header,
                    header.getContext(),
                  )}
                </Th>
              ))}
            </Tr>
          ))}
        </Thead>

        {/* ✅ Body hiển thị theo trạng thái */}
        <Tbody>
          {loading ? (
            <Tr>
              <Td colSpan={headers.length || 1} py={10}>
                <Flex
                  direction="column"
                  align="center"
                  justify="center"
                  gap={2}
                >
                  <Spinner size="lg" color="brand.500" />
                  <Text color="gray.500" fontSize="sm">
                    Loading purchase orders...
                  </Text>
                </Flex>
              </Td>
            </Tr>
          ) : rows.length === 0 ? (
            <Tr>
              <Td
                colSpan={headers.length || 1}
                textAlign="center"
                borderColor={borderColor}
                py={10}
              >
                <Text color="gray.500" fontStyle="italic">
                  No purchase orders found
                </Text>
              </Td>
            </Tr>
          ) : (
            rows.map((row) => (
              <Tr key={row.id}>
                {row.getVisibleCells().map((cell) => (
                  <Td key={cell.id} borderColor={borderColor}>
                    {flexRender(cell.column.columnDef.cell, cell.getContext())}
                  </Td>
                ))}
              </Tr>
            ))
          )}
        </Tbody>
      </Table>
    </Box>
  );
}
