import { createColumnHelper } from '@tanstack/react-table';
import {
  Text,
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  Button,
  Flex,
  Badge,
} from '@chakra-ui/react';
import { ChevronDownIcon } from '@chakra-ui/icons';
import { formatUSD } from 'utils/FormatHelper';
import StatusFilter from './StatusFilter';

const columnHelper = createColumnHelper();

// ✅ Enum map cho PurchaseOrderStatus
const STATUS_MAP = {
  1: { label: 'Pending', color: 'yellow' },
  2: { label: 'Completed', color: 'green' },
  3: { label: 'Cancelled', color: 'red' },
};

export default function Columns({
  onComplete,
  onCancel,
  statusFilter,
  setStatusFilter,
}) {
  return [
    columnHelper.display({
      id: 'index',
      header: () => <Text>#</Text>,
      cell: (info) => <Text>{info.row.index + 1}</Text>,
    }),

    columnHelper.accessor('createdByName', {
      header: 'CREATED BY',
      cell: (info) => info.getValue() || 'Unknown',
    }),

    columnHelper.accessor('modelName', {
      header: 'MODEL NAME',
      cell: (info) => info.getValue() || 'Unknown',
    }),

    columnHelper.accessor('totalAmount', {
      header: 'TOTAL AMOUNT',
      cell: (info) => <Text>{formatUSD(info.getValue())}</Text>,
    }),

    columnHelper.accessor('orderDate', {
      header: 'ORDER DATE',
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

    // ✅ STATUS column có filter và action menu
    columnHelper.accessor('status', {
      header: () => (
        <Flex justify="flex-end">
          <StatusFilter
            statusFilter={statusFilter}
            setStatusFilter={setStatusFilter}
          />
        </Flex>
      ),
      cell: (info) => {
        const row = info.row.original;
        const status = Number(info.getValue());
        const s = STATUS_MAP[status] || { label: 'Unknown', color: 'gray' };
        const isCompleted = status === 2;
        const isCancelled = status === 3;

        // 🔴 Nếu Completed hoặc Cancelled → chỉ hiển thị badge
        if (isCompleted || isCancelled) {
          return (
            <Flex justify="flex-end">
              <Badge colorScheme={s.color} px={3} py={1} borderRadius="md">
                {s.label}
              </Badge>
            </Flex>
          );
        }

        // 🟡 Pending → hiển thị menu chọn Complete hoặc Cancel
        return (
          <Flex justify="flex-end">
            <Menu isLazy>
              <MenuButton
                as={Button}
                rightIcon={<ChevronDownIcon />}
                colorScheme={s.color}
                size="sm"
                variant="outline"
              >
                {s.label}
              </MenuButton>

              <MenuList>
                <MenuItem isDisabled color="yellow.700">
                  Pending
                </MenuItem>
                <MenuItem
                  color="green.600"
                  onClick={() => onComplete?.(row.id)}
                >
                  Mark Completed
                </MenuItem>
                <MenuItem color="red.500" onClick={() => onCancel?.(row.id)}>
                  Mark Cancelled
                </MenuItem>
              </MenuList>
            </Menu>
          </Flex>
        );
      },
    }),
  ];
}
