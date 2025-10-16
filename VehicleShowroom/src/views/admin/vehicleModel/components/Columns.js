import { createColumnHelper } from '@tanstack/react-table';
import { Text, Flex } from '@chakra-ui/react';

const columnHelper = createColumnHelper();

export default function Columns({ toggleExpand, expandedRows }) {
  return [
    columnHelper.accessor('modelNumber', {
      header: 'MODEL NUMBER',
      cell: (info) => <Text fontWeight="600">{info.getValue()}</Text>,
    }),
    columnHelper.accessor('name', {
      header: 'NAME',
      cell: (info) => {
        const row = info.row.original;
        const isExpanded = expandedRows[row.modelNumber];
        const hasChildren = row.children && row.children.length > 0;

        return (
          <Flex align="center" gap={2}>
            {hasChildren && (
              <Text
                cursor="pointer"
                fontWeight="bold"
                onClick={() => toggleExpand(row.modelNumber)}
              >
                {isExpanded ? '−' : '+'}
              </Text>
            )}
            <Text>{info.getValue()}</Text>
          </Flex>
        );
      },
    }),
    columnHelper.accessor('price', {
      header: 'PRICE',
      cell: (info) => <Text>${info.getValue()?.toLocaleString() || 0}</Text>,
    }),
    columnHelper.display({
      id: 'actions',
      header: <Text align="right">ACTIONS</Text>,
    }),
  ];
}
