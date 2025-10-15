import { createColumnHelper } from '@tanstack/react-table';
import { Checkbox, Text, Flex, IconButton } from '@chakra-ui/react';
import { MdEdit, MdDelete } from 'react-icons/md';

const columnHelper = createColumnHelper();

export default function Columns({
  onEdit,
  onDelete,
  selectedBulk,
  setSelectedBulk,
}) {
  return [
    columnHelper.display({
      id: 'select',
      header: '',
      cell: (info) => {
        const row = info.row.original;
        const checked = selectedBulk.includes(row.vehicleId);
        return (
          <Checkbox
            isChecked={checked}
            onChange={(e) => {
              setSelectedBulk((prev) =>
                e.target.checked
                  ? [...prev, row.vehicleId]
                  : prev.filter((id) => id !== row.vehicleId),
              );
            }}
          />
        );
      },
    }),
    columnHelper.accessor('vin', {
      header: 'VIN',
      cell: (info) => <Text>{info.getValue()}</Text>,
    }),
    columnHelper.accessor('modelName', {
      header: 'MODEL NAME',
      cell: (info) => <Text>{info.getValue()}</Text>,
    }),
    columnHelper.accessor('purchasePrice', {
      header: 'PRICE',
      cell: (info) => <Text>${info.getValue()?.toLocaleString() || 0}</Text>,
    }),
    columnHelper.display({
      id: 'actions',
      header: 'ACTIONS',
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
