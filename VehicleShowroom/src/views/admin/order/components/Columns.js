import { createColumnHelper } from '@tanstack/react-table';
import { Text, Button, Flex } from '@chakra-ui/react';
import { MdAdd, MdCheck, MdBuild } from 'react-icons/md';

const columnHelper = createColumnHelper();

export default function Columns({ onAssign, onCreateService }) {
  return [
    columnHelper.display({
      id: 'index',
      header: () => <Text>#</Text>,
      cell: (info) => <Text>{info.row.index + 1}</Text>,
    }),

    columnHelper.accessor('customerName', {
      header: 'CUSTOMER NAME',
      cell: (info) => info.getValue() || 'Unknown',
    }),

    columnHelper.accessor('modelName', {
      header: 'MODEL NAME',
      cell: (info) => info.getValue() || 'Unknown',
    }),

    columnHelper.accessor('status', {
      header: () => <Text textAlign="right">ACTIONS</Text>,
      cell: (info) => {
        const row = info.row.original;
        const isAssigned = !!row.vehicleId;

        return (
          <Flex justify="flex-end" gap={2}>
            <Button
              size="sm"
              leftIcon={isAssigned ? <MdCheck /> : <MdAdd />}
              colorScheme={isAssigned ? 'green' : 'blue'}
              onClick={() => onAssign?.(row)}
            >
              {isAssigned ? 'View Assigned' : 'Assign Vehicle'}
            </Button>

            <Button
              size="sm"
              leftIcon={<MdBuild />}
              colorScheme="orange"
              onClick={() => onCreateService?.(row)}
            >
              Create Service
            </Button>
          </Flex>
        );
      },
    }),
  ];
}
