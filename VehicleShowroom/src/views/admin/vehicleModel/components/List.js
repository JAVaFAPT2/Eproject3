import React from 'react';
import { Table, Thead, Tbody, Tr, Th, Box } from '@chakra-ui/react';
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
  textColor,
  bgColor,
  borderColor,
  headerBg,
}) {
  const renderRow = (m, depth = 0) => (
    <Row
      key={m.modelNumber}
      model={m}
      depth={depth}
      expandedRows={expandedRows}
      toggleExpand={toggleExpand}
      onAdd={onAdd}
      onAddSpec={onAddSpec}
      onPreview={onPreview}
      onEdit={onEdit}
      onDelete={onDelete}
      renderChildren={renderRow}
    />
  );

  return (
    <Box minH="600px" overflowX="auto" p={3}>
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
        <Tbody>{treeData.map((m) => renderRow(m))}</Tbody>
      </Table>
    </Box>
  );
}
