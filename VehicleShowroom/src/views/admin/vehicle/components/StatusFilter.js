import {
  Menu,
  MenuButton,
  MenuList,
  MenuItem,
  Button,
  Flex,
  Text,
  Box,
  useColorModeValue,
} from '@chakra-ui/react';
import { MdArrowDropDown } from 'react-icons/md';

export default function StatusFilter({ statusFilter, setStatusFilter }) {
  const bgColor = useColorModeValue('white', 'navy.800');
  const borderColor = useColorModeValue('gray.200', 'gray.700');
  const brandColor = useColorModeValue('brand.500', 'brand.400');

  const options = [
    { label: 'All', value: null },
    { label: 'In Stock', value: 1 },
    { label: 'Sold', value: 2 },
    { label: 'Reserved', value: 3 },
    { label: 'In Service', value: 4 },
  ];

  return (
    <Menu isLazy>
      <MenuButton
        as={Button}
        size="sm"
        variant="ghost"
        rightIcon={<MdArrowDropDown />}
        _hover={{ bg: 'transparent' }}
        _active={{ bg: 'transparent' }}
        p={0}
      >
        STATUS
      </MenuButton>
      <MenuList bg={bgColor} borderColor={borderColor}>
        {options.map((option) => (
          <MenuItem
            key={option.label}
            onClick={() => setStatusFilter(option.value)}
          >
            <Flex w="100%" justify="space-between" align="center">
              <Text
                fontWeight={statusFilter === option.value ? 'bold' : 'normal'}
                fontSize="md"
              >
                {option.label}
              </Text>
              {statusFilter === option.value && (
                <Box h="36px" w="4px" bg={brandColor} borderRadius="5px" />
              )}
            </Flex>
          </MenuItem>
        ))}
      </MenuList>
    </Menu>
  );
}
