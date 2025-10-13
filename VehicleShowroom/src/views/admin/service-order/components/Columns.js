import { createColumnHelper } from '@tanstack/react-table';
import { Text, Flex, IconButton } from '@chakra-ui/react';
import { MdEdit, MdDelete } from 'react-icons/md';

const c = createColumnHelper();

export const getColumns = ({ onEdit, onDelete, textColor }) => [
  c.accessor('serviceId', { header: 'SERVICE ID', cell: (i) => <Text color={textColor}>{i.getValue()}</Text> }),
  c.accessor('vehicleId', { header: 'VEHICLE', cell: (i) => <Text>{i.getValue()}</Text> }),
  c.accessor('serviceType', { header: 'TYPE', cell: (i) => <Text>{i.getValue()}</Text> }),
  c.accessor('status', { header: 'STATUS', cell: (i) => <Text>{i.getValue()}</Text> }),
  c.display({
    id: 'actions',
    header: () => <Text textAlign="right" w="full">ACTIONS</Text>,
    cell: (i) => {
      const v = i.row.original;
      return (
        <Flex justify="flex-end" gap={2}>
          <IconButton aria-label="edit" icon={<MdEdit />} size="sm" colorScheme="blue" onClick={() => onEdit(v)} />
          <IconButton aria-label="delete" icon={<MdDelete />} size="sm" colorScheme="red" onClick={() => onDelete(v)} />
        </Flex>
      );
    },
  }),
];
