import { createColumnHelper } from '@tanstack/react-table';
import { Text, Flex, IconButton } from '@chakra-ui/react';
import { EditIcon, AddIcon, DeleteIcon } from '@chakra-ui/icons';

const columnHelper = createColumnHelper();

export default function CategoryColumns({ onEdit, onAdd, onDelete, toggleExpand, expandedRows }) {
  return [
    columnHelper.accessor('name', {
      header: 'NAME',
      cell: (info) => {
        const row = info.row.original;
        const isExpanded = expandedRows[row.id];
        const hasChildren = row.children && row.children.length > 0;

        return (
          <Flex align="center" gap={2}>
            {hasChildren && (
              <IconButton
                aria-label="Toggle expand"
                size="xs"
                variant="ghost"
                icon={<Text>{isExpanded ? '-' : '+'}</Text>}
                onClick={() => toggleExpand(row.id)}
              />
            )}
            <Text fontWeight="600">{info.getValue()}</Text>
          </Flex>
        );
      },
    }),
    columnHelper.accessor('description', {
      header: 'DESCRIPTION',
      cell: (info) => <Text>{info.getValue() || '-'}</Text>,
    }),
    columnHelper.accessor('createdAt', {
      header: 'CREATED AT',
      cell: (info) => (
        <Text fontSize="sm">
          {new Date(info.getValue()).toLocaleString()}
        </Text>
      ),
    }),
    columnHelper.display({
      id: 'actions',
      header: 'ACTIONS',
      cell: (info) => {
        const category = info.row.original;
        return (
          <Flex gap={2}>
            <IconButton
              aria-label="Add Sub"
              size="sm"
              icon={<AddIcon />}
              onClick={() => onAdd(category)}
            />
            <IconButton
              aria-label="Edit"
              size="sm"
              icon={<EditIcon />}
              onClick={() => onEdit(category)}
            />
            <IconButton
              aria-label="Delete"
              size="sm"
              colorScheme="red"
              icon={<DeleteIcon />}
              onClick={() => onDelete(category)}
            />
          </Flex>
        );
      },
    }),
  ];
}
