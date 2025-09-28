import { createColumnHelper } from '@tanstack/react-table';
import { Text, Flex, IconButton } from '@chakra-ui/react';
import { MdEdit, MdDelete } from 'react-icons/md';

const columnHelper = createColumnHelper();

export const getColumns = ({ onEdit, onDelete, textColor }) => [
  columnHelper.accessor('orderNumber', {
    header: 'ORDER NO',
    cell: (info) => <Text color={textColor}>{info.getValue()}</Text>,
  }),
  columnHelper.accessor('modelNumber', {
    header: 'MODEL',
    cell: (info) => <Text>{info.getValue()}</Text>,
  }),
  columnHelper.accessor('quantity', {
    header: 'QTY',
    cell: (info) => <Text>{info.getValue()}</Text>,
  }),
  columnHelper.accessor('status', {
    header: 'STATUS',
    cell: (info) => <Text>{info.getValue()}</Text>,
  }),
  columnHelper.display({
    id: 'actions',
    header: () => (
      <Text textAlign="right" w="full">
        ACTIONS
      </Text>
    ),
    cell: (info) => {
      const v = info.row.original;
      return (
        <Flex justify="flex-end" gap={2}>
          <IconButton aria-label="edit" icon={<MdEdit />} size="sm" colorScheme="blue" onClick={() => onEdit(v)} />
          <IconButton aria-label="delete" icon={<MdDelete />} size="sm" colorScheme="red" onClick={() => onDelete(v)} />
        </Flex>
      );
    },
  }),
];
