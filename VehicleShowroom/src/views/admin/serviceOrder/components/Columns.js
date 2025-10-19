import { createColumnHelper } from '@tanstack/react-table';
import { Text, Button, Flex, Badge, HStack } from '@chakra-ui/react';
import StatusFilter from './StatusFilter';

const columnHelper = createColumnHelper();

const STATUS_MAP = {
  1: { label: 'Scheduled', color: 'blue' },
  2: { label: 'In Progress', color: 'orange' },
  3: { label: 'Completed', color: 'green' },
  4: { label: 'Cancelled', color: 'red' },
};

const TYPE_MAP = {
  1: { label: 'PreDelivery', color: 'purple' },
  2: { label: 'Maintenance', color: 'cyan' },
  3: { label: 'Repair', color: 'teal' },
};

export default function Columns({
  onViewDetail,
  onUpdateStatus,
  statusFilter,
  setStatusFilter,
}) {
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
        const value = info.getValue();
        if (!value) return '-';
        const date = new Date(value);
        return date.toLocaleDateString('en-US', {
          month: '2-digit',
          day: '2-digit',
          year: 'numeric',
        });
      },
    }),

    // ✅ TYPE column
    columnHelper.accessor('type', {
      header: 'TYPE',
      cell: (info) => {
        const value = info.getValue();
        const t = TYPE_MAP[value] || { label: 'Unknown', color: 'gray' };
        return (
          <Badge
            colorScheme={t.color}
            variant="subtle"
            px={3}
            py={1}
            borderRadius="md"
          >
            {t.label}
          </Badge>
        );
      },
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
      header: <Text align="right">ACTIONS</Text>,
      cell: (info) => {
        const row = info.row.original;
        const status = Number(row.status); // đảm bảo kiểu number

        // 🔴 Nếu đã Completed hoặc Cancelled → ẩn hết nút
        if (status === 3 || status === 4) {
          return (
            <Flex justify="end">
              <Text color="gray.400" fontStyle="italic">
                {status === 3 ? 'Completed' : 'Cancelled'}
              </Text>
            </Flex>
          );
        }

        // ✅ Nếu chưa complete → hiển thị nút
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
