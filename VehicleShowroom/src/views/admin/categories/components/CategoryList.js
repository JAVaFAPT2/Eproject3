import React from 'react';
import { Table, Thead, Tbody, Tr, Th, Box, useColorModeValue } from '@chakra-ui/react';
import { flexRender } from '@tanstack/react-table';
import CategoryRow from './CategoryRow';

export default function CategoryList({
  table,
  treeData,
  expandedRows,
  toggleExpand,
  onAdd,
  onEdit,
  onDelete,
}) {
  const borderColor = useColorModeValue('gray.200', 'navy.700');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const headerBg = useColorModeValue('gray.100', 'navy.800');
  const bgColor = useColorModeValue('white', 'navy.800');

  const renderRow = (cat, depth = 0) => (
    <CategoryRow
      key={cat.id}
      cat={cat}
      depth={depth}
      expandedRows={expandedRows}
      toggleExpand={toggleExpand}
      onAdd={onAdd}
      onEdit={onEdit}
      onDelete={onDelete}
      renderChildren={renderRow}
    />
  );

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
                  {flexRender(header.column.columnDef.header, header.getContext())}
                </Th>
              ))}
            </Tr>
          ))}
        </Thead>
        <Tbody>{treeData.map((cat) => renderRow(cat))}</Tbody>
      </Table>
    </Box>
  );
}
