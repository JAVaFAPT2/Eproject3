import { createColumnHelper } from '@tanstack/react-table';
import { Text, Flex, IconButton } from '@chakra-ui/react';
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
  const statusMap = {
    1: 'In Stock',
    2: 'Sold',
    3: 'Reserved',
    4: 'In Service',
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

    columnHelper.accessor('status', {
      header: () => (
        <StatusFilter
          statusFilter={statusFilter}
          setStatusFilter={setStatusFilter}
        />
      ),
      cell: (info) => {
        const val = info.getValue();
        return (
          <Text
            color={
              val === 1
                ? 'green.400'
                : val === 2
                ? 'red.400'
                : val === 3
                ? 'orange.400'
                : 'blue.400'
            }
          >
            {statusMap[val] || 'Unknown'}
          </Text>
        );
      },
    }),
    columnHelper.accessor('purchasePrice', {
      header: 'PRICE',
      cell: (info) => <Text>{formatUSD(info.getValue())}</Text>,
    }),
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
