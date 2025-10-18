import { createColumnHelper } from '@tanstack/react-table';
import { Text } from '@chakra-ui/react';

const columnHelper = createColumnHelper();

export default function Columns() {
  return [
    columnHelper.display({
      id: 'index',
      header: () => <Text>#</Text>,
      cell: (info) => <Text>{info.row.index + 1}</Text>,
    }),

    columnHelper.accessor('email', {
      header: 'EMAIL',
      cell: (info) => (
        <Text fontSize="sm" fontWeight="600">
          {info.getValue()}
        </Text>
      ),
    }),
    columnHelper.accessor('username', {
      header: 'USER NAME',
      cell: (info) => <Text>{info.getValue() || '-'}</Text>,
    }),
    columnHelper.accessor('name', {
      header: 'FULL NAME',
      cell: (info) => <Text>{info.getValue() || '-'}</Text>,
    }),
    columnHelper.accessor('phone', {
      header: 'PHONE NUMBER',
      cell: (info) => <Text>{info.getValue() || '-'}</Text>,
    }),
    columnHelper.accessor('createdAt', {
      header: 'JOINED DATE',
      cell: (info) => (
        <Text>
          {info.getValue()
            ? new Date(info.getValue()).toLocaleDateString()
            : '-'}
        </Text>
      ),
    }),
  ];
}
