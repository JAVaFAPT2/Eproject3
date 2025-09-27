import { createColumnHelper } from '@tanstack/react-table';
import { Text, Flex, IconButton } from '@chakra-ui/react';
import { MdEdit, MdDelete } from 'react-icons/md';
import { RiEyeFill } from 'react-icons/ri';

const columnHelper = createColumnHelper();

export const getEmployeeColumns = ({ onShow, onEdit, onDelete, textColor }) => [
  columnHelper.accessor('email', {
    header: 'EMAIL',
    cell: (info) => (
      <Text fontSize="sm" fontWeight="600" color={textColor}>
        {info.getValue()}
      </Text>
    ),
  }),
  columnHelper.accessor('fullName', {
    header: 'FULL NAME',
    cell: (info) => <Text>{info.getValue() || '-'}</Text>,
  }),
  columnHelper.accessor('createdAt', {
    header: 'CREATED AT',
    cell: (info) => <Text>{new Date(info.getValue()).toLocaleString()}</Text>,
  }),
  columnHelper.display({
    id: 'actions',
    header: 'ACTIONS',
    cell: (info) => {
      const row = info.row.original;
      return (
        <Flex justify="flex-end" gap={2}>
          <IconButton
            aria-label="View"
            icon={<RiEyeFill style={{ fontSize: '20px' }} />}
            size="sm"
            borderRadius={10}
            colorScheme="purple"
            onClick={() => onShow(row)}
          />
          <IconButton
            aria-label="Edit"
            icon={<MdEdit style={{ fontSize: '20px' }} />}
            size="sm"
            borderRadius={10}
            colorScheme="blue"
            onClick={() => onEdit(row)}
          />
          <IconButton
            aria-label="Delete"
            icon={<MdDelete style={{ fontSize: '20px' }} />}
            size="sm"
            borderRadius={10}
            colorScheme="red"
            onClick={() => onDelete(row)}
          />
        </Flex>
      );
    },
  }),
];
