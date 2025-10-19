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
  const textColor = useColorModeValue('gray.700', 'gray.200');

  const options = [
    { label: 'All', value: null },
    { label: 'Scheduled', value: 1 },
    { label: 'In Progress', value: 2 },
    { label: 'Completed', value: 3 },
    { label: 'Cancelled', value: 4 },
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
            px={0}
          >
            <Flex w="100%" justify="space-between" align="center" px={3}>
              <Text
                color={textColor}
                fontWeight={statusFilter === option.value ? 'bold' : 'normal'}
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
