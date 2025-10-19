import { createColumnHelper } from '@tanstack/react-table';
import { Text, Button, Flex, Badge, HStack } from '@chakra-ui/react';
import StatusFilter from './StatusFilter';

const columnHelper = createColumnHelper();

export default function Columns({
  onViewDetail,
  onUpdateStatus,
  statusFilter,
  setStatusFilter,
}) {
  const statuses = {
    1: { label: 'Scheduled', color: 'blue' },
    2: { label: 'In Progress', color: 'orange' },
    3: { label: 'Completed', color: 'green' },
    4: { label: 'Cancelled', color: 'red' },
  };

  return [
    columnHelper.display({
      id: 'index',
      header: '#',
      cell: (info) => info.row.index + 1,
    }),

    columnHelper.accessor('createdByName', {
      header: 'CREATED BY',
      cell: (info) => info.getValue() || 'Unknown',
    }),

    columnHelper.accessor('customerName', {
      header: 'CUSTOMER',
      cell: (info) => info.getValue() || 'Unknown',
    }),

    columnHelper.accessor('appointmentDate', {
      header: 'APPOINTMENT DATE',
      cell: (info) => {
        const date = info.getValue();
        return date ? new Date(date).toLocaleString() : '-';
      },
    }),

    // ✅ Header có menu filter
    columnHelper.accessor('status', {
      header: () => (
        <StatusFilter
          statusFilter={statusFilter}
          setStatusFilter={setStatusFilter}
        />
      ),
      cell: (info) => {
        const value = info.getValue();
        const s = statuses[value] || { label: 'Unknown', color: 'gray' };
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

    columnHelper.display({
      id: 'actions',
      header: <Text align="right">ACTIONS</Text>,
      cell: (info) => {
        const row = info.row.original;
        return (
          <Flex justify="end">
            <HStack spacing={2}>
              <Button
                size="sm"
                colorScheme="blue"
                onClick={() => onViewDetail(row)}
              >
                View
              </Button>
              <Button
                size="sm"
                colorScheme="green"
                onClick={() => onUpdateStatus(row)}
              >
                Update
              </Button>
            </HStack>
          </Flex>
        );
      },
    }),
  ];
}
