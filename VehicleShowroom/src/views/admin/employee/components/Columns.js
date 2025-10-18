import { createColumnHelper } from '@tanstack/react-table';
import { Text, Switch, Flex } from '@chakra-ui/react';

const columnHelper = createColumnHelper();

export default function Columns({ onToggle }) {
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
    columnHelper.accessor('hireDate', {
      header: 'HIRE DATE',
      cell: (info) => (
        <Text>
          {info.getValue()
            ? new Date(info.getValue()).toLocaleDateString()
            : '-'}
        </Text>
      ),
    }),
    columnHelper.accessor('isActive', {
      header: <Text align="right">ACTIVE</Text>,
      cell: (info) => {
        const row = info.row.original;
        return (
          <Flex justify="center">
            <Switch
              colorScheme="green"
              isChecked={row.isActive}
              onChange={() => onToggle(row.id, row.isActive)}
            />
          </Flex>
        );
      },
    }),
  ];
}
