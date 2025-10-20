import { createColumnHelper } from '@tanstack/react-table';
import { Text, Button, Flex, Badge } from '@chakra-ui/react';
import { MdAdd, MdCheck, MdBuild } from 'react-icons/md';
import StatusFilter from './StatusFilter';

const columnHelper = createColumnHelper();

const STATUS_MAP = {
  1: { label: 'Pending', color: 'orange' },
  2: { label: 'Confirmed', color: 'blue' },
  3: { label: 'Completed', color: 'green' },
  4: { label: 'Cancelled', color: 'red' },
};

export default function Columns({
  onAssign,
  onCreateService,
  statusFilter,
  setStatusFilter,
}) {
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

    // ✅ STATUS column
    columnHelper.accessor('status', {
      header: () => (
        <StatusFilter
          statusFilter={statusFilter}
          setStatusFilter={setStatusFilter}
        />
      ),
      cell: (info) => {
        const value = info.getValue();
        const s = STATUS_MAP[value] || { label: 'Unknown', color: 'gray' };
        return (
          <Badge
            colorScheme={s.color}
            variant="subtle"
            px={3}
            py={1}
            borderRadius="md"
          >
            {s.label}
          </Badge>
        );
      },
    }),

    // ✅ ACTIONS column
    columnHelper.display({
      id: 'actions',
      header: () => <Text textAlign="right">ACTIONS</Text>,
      cell: (info) => {
        const row = info.row.original;
        const value = Number(row.status);
        const isAssigned = !!row.vehicleId;
        const isCancelled = value === 4;

        if (isCancelled) {
          return (
            <Flex justify="flex-end">
              <Text color="gray.400" fontStyle="italic">
                Cancelled
              </Text>
            </Flex>
          );
        }

        return (
          <Flex justify="flex-end" gap={2}>
            {/* Assign / View */}
            <Button
              size="sm"
              leftIcon={isAssigned ? <MdCheck /> : <MdAdd />}
              colorScheme={isAssigned ? 'green' : 'blue'}
              onClick={() => onAssign?.(row)}
            >
              {isAssigned ? 'View Assigned' : 'Assign Vehicle'}
            </Button>

            {/* Create Service */}
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
