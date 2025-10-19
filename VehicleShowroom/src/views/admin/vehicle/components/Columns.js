import { createColumnHelper } from '@tanstack/react-table';
import { Text, Flex, IconButton, Badge } from '@chakra-ui/react';
import { MdEdit, MdDelete } from 'react-icons/md';
import { formatUSD } from 'utils/FormatHelper';
import ModelFilter from './ModelFilter';
import StatusFilter from './StatusFilter';

const columnHelper = createColumnHelper();

export default function Columns({
  models,
  statusFilter,
  setStatusFilter,
  modelFilter,
  setModelFilter,
  onEdit,
  onDelete,
}) {
  const STATUS_MAP = {
    1: { label: 'In Stock', color: 'green' },
    2: { label: 'Reserved', color: 'orange' },
    3: { label: 'Sold', color: 'red' },
  };

  return [
    columnHelper.display({
      id: 'index',
      header: () => <Text>#</Text>,
      cell: (info) => <Text>{info.row.index + 1}</Text>,
    }),

    columnHelper.accessor('vehicleId', {
      header: 'VEHICLE ID',
      cell: (info) => <Text>{info.getValue()}</Text>,
    }),

    columnHelper.accessor('modelName', {
      header: () => (
        <ModelFilter
          models={models}
          modelFilter={modelFilter}
          setModelFilter={setModelFilter}
        />
      ),
      cell: (info) => <Text>{info.getValue()}</Text>,
    }),

    // ✅ STATUS column with badge
    columnHelper.accessor('status', {
      header: () => (
        <StatusFilter
          statusFilter={statusFilter}
          setStatusFilter={setStatusFilter}
        />
      ),
      cell: (info) => {
        const val = Number(info.getValue());
        const s = STATUS_MAP[val] || { label: 'Unknown', color: 'gray' };

        return (
          <Badge
            colorScheme={s.color}
            px={3}
            py={1}
            borderRadius="md"
            variant="subtle"
          >
            {s.label}
          </Badge>
        );
      },
    }),

    columnHelper.accessor('purchasePrice', {
      header: 'PRICE',
      cell: (info) => <Text>{formatUSD(info.getValue())}</Text>,
    }),

    // ✅ ACTIONS
    columnHelper.display({
      id: 'actions',
      header: <Text align="right">ACTIONS</Text>,
      cell: (info) => {
        const row = info.row.original;
        return (
          <Flex justify="flex-end" gap={2}>
            <IconButton
              aria-label="Edit"
              icon={<MdEdit />}
              size="sm"
              colorScheme="blue"
              borderRadius="xl"
              onClick={() => onEdit(row)}
            />
            <IconButton
              aria-label="Delete"
              icon={<MdDelete />}
              size="sm"
              colorScheme="red"
              borderRadius="xl"
              onClick={() => onDelete(row)}
            />
          </Flex>
        );
      },
    }),
  ];
}
