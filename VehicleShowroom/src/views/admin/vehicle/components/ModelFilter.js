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

export default function ModelFilter({ modelFilter, setModelFilter, models }) {
  const bgColor = useColorModeValue('white', 'navy.800');
  const borderColor = useColorModeValue('gray.200', 'gray.700');
  const brandColor = useColorModeValue('brand.500', 'brand.400');

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
        MODEL
      </MenuButton>
      <MenuList
        bg={bgColor}
        borderColor={borderColor}
        maxH="250px"
        overflowY="auto"
      >
        <MenuItem onClick={() => setModelFilter(null)}>
          <Flex w="100%" justify="space-between" align="center">
            <Text fontWeight={!modelFilter ? 'bold' : 'normal'}>All</Text>
            {!modelFilter && (
              <Box h="36px" w="4px" bg={brandColor} borderRadius="5px" />
            )}
          </Flex>
        </MenuItem>

        {models.map((m) => (
          <MenuItem
            key={m.modelNumber}
            onClick={() => setModelFilter(m.modelNumber)}
            pl={m.level * 4}
            fontSize="md"
          >
            <Flex w="100%" justify="space-between" align="center">
              <Text
                fontWeight={modelFilter === m.modelNumber ? 'bold' : 'normal'}
              >
                {m.name}
              </Text>
              {modelFilter === m.modelNumber && (
                <Box h="36px" w="4px" bg={brandColor} borderRadius="5px" />
              )}
            </Flex>
          </MenuItem>
        ))}
      </MenuList>
    </Menu>
  );
}
