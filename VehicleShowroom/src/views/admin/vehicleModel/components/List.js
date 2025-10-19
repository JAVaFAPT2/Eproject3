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
import Row from 'views/admin/vehicleModel/components/Row';

export default function List({
  table,
  treeData,
  expandedRows,
  toggleExpand,
  onAdd,
  onAddSpec,
  onPreview,
  onEdit,
  onDelete,
  onEditSpec,
  onDeleteSpec,
  textColor,
  bgColor,
  borderColor,
  headerBg,
  loading,
}) {
  const renderRow = (m, index, depth = 0, prefix = '') => {
    const displayIndex = prefix ? `${prefix}.${index + 1}` : `${index + 1}`;
    return (
      <Row
        key={m.modelNumber}
        index={displayIndex}
        model={m}
        depth={depth}
        expandedRows={expandedRows}
        toggleExpand={toggleExpand}
        onAdd={onAdd}
        onAddSpec={onAddSpec}
        onPreview={onPreview}
        onEdit={onEdit}
        onDelete={onDelete}
        onEditSpec={onEditSpec}
        onDeleteSpec={onDeleteSpec}
        renderChildren={(child, childIndex) =>
          renderRow(child, childIndex, depth + 1, displayIndex)
        }
      />
    );
  };

  const headers = table.getHeaderGroups()[0]?.headers || [];

  return (
    <Box minH="600px" overflowX="auto" p={3}>
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

        {/* ✅ Body hiển thị loading riêng */}
        <Tbody>
          {loading ? (
            <Tr>
              <Td colSpan={table.getAllColumns().length}>
                <Flex
                  py={10}
                  align="center"
                  justify="center"
                  direction="column"
                  gap={2}
                >
                  <Spinner size="lg" color="brand.500" />
                  <Text color={textColor} fontSize="sm">
                    Loading vehicle models...
                  </Text>
                </Flex>
              </Td>
            </Tr>
          ) : treeData.length === 0 ? (
            <Tr>
              <Td colSpan={headers.length || 1} py={10}>
                <Flex align="center" justify="center">
                  <Text color="gray.500" fontStyle="italic">
                    No vehicle models found
                  </Text>
                </Flex>
              </Td>
            </Tr>
          ) : (
            treeData.map((m, i) => renderRow(m, i, 0))
          )}
        </Tbody>
      </Table>
    </Box>
  );
}
