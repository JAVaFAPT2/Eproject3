import { createColumnHelper } from '@tanstack/react-table';
import {
  Text,
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  Button,
  Flex,
} from '@chakra-ui/react';
import { ChevronDownIcon } from '@chakra-ui/icons';
import { formatUSD } from 'utils/FormatHelper';

const columnHelper = createColumnHelper();

export default function Columns({ onComplete }) {
  return [
    columnHelper.display({
      id: 'index',
      header: () => <Text>#</Text>,
      cell: (info) => <Text>{info.row.index + 1}</Text>,
    }),

    columnHelper.accessor('createdByName', {
      header: 'Created By',
      cell: (info) => info.getValue() || 'Unknown',
    }),

    columnHelper.accessor('totalAmount', {
      header: 'TOTAL AMOUNT',
      cell: (info) => <Text>{formatUSD(info.getValue())}</Text>,
    }),

    columnHelper.accessor('status', {
      header: () => <Text textAlign="right">STATUS</Text>,
      cell: (info) => {
        const row = info.row.original;
        const current = info.getValue() || 'Pending';
        const isPending = current === 'Pending';

        return (
          <Flex justify="flex-end">
            <Menu isLazy>
              <MenuButton
                as={Button}
                rightIcon={<ChevronDownIcon />}
                colorScheme={isPending ? 'yellow' : 'green'}
                size="sm"
                variant="outline"
              >
                {current}
              </MenuButton>

              <MenuList>
                <MenuItem isDisabled color="yellow.700">
                  Pending
                </MenuItem>
                <MenuItem
                  isDisabled={!isPending}
                  color="green"
                  onClick={() => onComplete?.(row.id)}
                >
                  Complete
                </MenuItem>
              </MenuList>
            </Menu>
          </Flex>
        );
      },
    }),
  ];
}
