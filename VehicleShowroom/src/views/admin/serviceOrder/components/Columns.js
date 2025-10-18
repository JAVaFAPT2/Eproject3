import { createColumnHelper } from '@tanstack/react-table';
import { Text, Button, Flex, Badge, HStack } from '@chakra-ui/react';

const columnHelper = createColumnHelper();

export default function Columns({ onViewDetail, onUpdateStatus }) {
  // 🟩 Enum trạng thái hiển thị
  const statuses = {
    Scheduled: { label: 'Scheduled', color: 'blue' },
    InProgress: { label: 'In Progress', color: 'orange' },
    Completed: { label: 'Completed', color: 'green' },
    Cancelled: { label: 'Cancelled', color: 'red' },
  };

  return [
    // 🔹 STT
    columnHelper.display({
      id: 'index',
      header: '#',
      cell: (info) => info.row.index + 1,
    }),

    // 🔹 Created By
    columnHelper.accessor('createdByName', {
      header: 'CREATED BY',
      cell: (info) => info.getValue() || 'Unknown',
    }),

    // 🔹 Customer
    columnHelper.accessor('customerName', {
      header: 'CUSTOMER',
      cell: (info) => info.getValue() || 'Unknown',
    }),

    // 🔹 Appointment
    columnHelper.accessor('appointmentDate', {
      header: 'APPOINTMENT DATE',
      cell: (info) => {
        const date = info.getValue();
        return date ? new Date(date).toLocaleString() : '-';
      },
    }),

    // 🔹 Status — chỉ hiển thị Badge
    columnHelper.accessor('status', {
      header: 'STATUS',
      cell: (info) => {
        const value = info.getValue();
        const s = statuses[value] || {
          label: value || 'Unknown',
          color: 'gray',
        };
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

    // 🔹 Actions — có cả View và Update
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
