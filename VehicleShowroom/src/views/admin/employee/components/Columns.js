import { createColumnHelper } from '@tanstack/react-table';
import { Text, Flex, IconButton } from '@chakra-ui/react';
import { MdEdit, MdDelete } from 'react-icons/md';
import { RiEyeFill } from 'react-icons/ri';

const columnHelper = createColumnHelper();

export const getColumns = ({ onShow, onEdit, onDelete, textColor }) => [
  columnHelper.accessor('email', {
    header: 'EMAIL',
    cell: (info) => <Text color={textColor}>{info.getValue()}</Text>,
  }),
  columnHelper.accessor('fullName', {
    header: 'FULL NAME',
    cell: (info) => <Text>{info.getValue()}</Text>,
  }),
  columnHelper.accessor('hourlyRate', {
    header: 'HOURLY RATE',
    cell: (info) => <Text>${info.getValue()}</Text>,
  }),
  columnHelper.display({
    id: 'actions',
    header: (
      <Text textAlign="right" w="full">
        ACTIONS
      </Text>
    ),
    cell: (info) => {
      const e = info.row.original;
      return (
        <Flex justify="flex-end" gap={2}>
          <IconButton
            aria-label="view"
            icon={<RiEyeFill />}
            size="sm"
            colorScheme="purple"
            onClick={() => onShow(e)}
          />
          <IconButton
            aria-label="edit"
            icon={<MdEdit />}
            size="sm"
            colorScheme="blue"
            onClick={() => onEdit(e)}
          />
          <IconButton
            aria-label="delete"
            icon={<MdDelete />}
            size="sm"
            colorScheme="red"
            onClick={() => onDelete(e)}
          />
        </Flex>
      );
    },
  }),
];
