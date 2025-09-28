import { createColumnHelper } from '@tanstack/react-table';
import { Text, Flex, IconButton } from '@chakra-ui/react';
import { MdEdit, MdDelete } from 'react-icons/md';

const col = createColumnHelper();

export const getColumns = ({ onEdit, onDelete, textColor }) => [
  col.accessor('orderNumber', {
    header: 'ORDER',
    cell: (i) => <Text color={textColor}>{i.getValue()}</Text>,
  }),
  col.accessor('customer', {
    header: 'CUSTOMER',
    cell: (i) => <Text>{i.getValue()?.fullName || '-'}</Text>,
  }),
  col.accessor('totalAmount', {
    header: 'TOTAL',
    cell: (i) => <Text>${i.getValue()}</Text>,
  }),
  col.accessor('status', {
    header: 'STATUS',
    cell: (i) => <Text>{i.getValue()}</Text>,
  }),
  col.display({
    id: 'actions',
    header: (
      <Text textAlign="right" w="full">
        ACTIONS
      </Text>
    ),
    cell: (i) => {
      const o = i.row.original;
      return (
        <Flex justify="flex-end" gap={2}>
          <IconButton
            icon={<MdEdit />}
            size="sm"
            colorScheme="blue"
            onClick={() => onEdit(o)}
          />
          <IconButton
            icon={<MdDelete />}
            size="sm"
            colorScheme="red"
            onClick={() => onDelete(o)}
          />
        </Flex>
      );
    },
  }),
];
