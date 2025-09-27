import React from 'react';
import { Text, Flex, IconButton } from '@chakra-ui/react';
import { createColumnHelper } from '@tanstack/react-table';
import { MdEdit, MdDelete } from 'react-icons/md';

const columnHelper = createColumnHelper();

export const getCouponColumns = (onOpen, setEditingCoupon, setCouponToDelete, setIsConfirmOpen) => [
  columnHelper.accessor('code', {
    header: 'CODE',
    cell: (info) => <Text>{info.getValue() || '-'}</Text>,
  }),
  columnHelper.accessor('discountValue', {
    header: 'DISCOUNT VALUE',
    cell: (info) => <Text>{info.getValue() || 0} %</Text>,
  }),
  columnHelper.accessor('startDate', {
    header: 'START DATE',
    cell: (info) => {
      const value = info.getValue();
      return <Text>{value ? new Date(value).toLocaleDateString() : '-'}</Text>;
    },
  }),
  columnHelper.accessor('endDate', {
    header: 'END DATE',
    cell: (info) => {
      const value = info.getValue();
      return <Text>{value ? new Date(value).toLocaleDateString() : '-'}</Text>;
    },
  }),
  columnHelper.accessor('usedCount', {
    header: 'USED COUNT',
    cell: (info) => <Text>{info.getValue() || 0}</Text>,
  }),
  columnHelper.accessor('usageLimit', {
    header: 'USAGE LIMIT',
    cell: (info) => <Text>{info.getValue() || 0}</Text>,
  }),
  columnHelper.display({
    id: 'actions',
    header: () => <Text textAlign="right">ACTIONS</Text>,
    cell: (info) => (
      <Flex justify="flex-end" gap={2}>
        <IconButton
          aria-label="Edit"
          size="sm"
          icon={<MdEdit style={{ fontSize: '20px' }} />}
          colorScheme="blue"
          onClick={() => {
            setEditingCoupon(info.row.original);
            onOpen();
          }}
          borderRadius={10}
        />
        <IconButton
          aria-label="Delete"
          size="sm"
          icon={<MdDelete style={{ fontSize: '20px' }} />}
          colorScheme="red"
          onClick={() => {
            setCouponToDelete(info.row.original);
            setIsConfirmOpen(true);
          }}
          borderRadius={10}
        />
      </Flex>
    ),
  }),
];
