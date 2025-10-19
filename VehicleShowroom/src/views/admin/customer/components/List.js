import React from 'react';
import {
  Box,
  Table,
  Thead,
  Tr,
  Th,
  Td,
  Tbody,
  Spinner,
  Flex,
  Text,
} from '@chakra-ui/react';
import { flexRender } from '@tanstack/react-table';

export default function List({
  table,
  textColor,
  borderColor,
  headerBg,
  bgColor,
  loading = false,
}) {
  const rows = table.getRowModel().rows;
  const headers = table.getHeaderGroups()[0]?.headers || [];

  return (
    <Box minH="600px" overflowX="auto" bg={bgColor} borderRadius="10px" p={3}>
      <Table variant="simple" bg={bgColor}>
        {/* ✅ Header luôn hiển thị */}
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

        {/* ✅ Body hiển thị 3 trạng thái */}
        <Tbody>
          {loading ? (
            <Tr>
              <Td colSpan={headers.length || 1}>
                <Flex
                  direction="column"
                  align="center"
                  justify="center"
                  py={10}
                  gap={2}
                >
                  <Spinner size="lg" color="brand.500" />
                  <Text color="gray.500" fontSize="sm">
                    Loading customers...
                  </Text>
                </Flex>
              </Td>
            </Tr>
          ) : rows.length === 0 ? (
            <Tr>
              <Td
                colSpan={headers.length || 1}
                textAlign="center"
                py={10}
                borderColor={borderColor}
              >
                <Text color="gray.500" fontStyle="italic">
                  No customers found
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
