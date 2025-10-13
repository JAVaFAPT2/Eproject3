import React from 'react';
import { Flex, Button, Text, useColorModeValue } from '@chakra-ui/react';
import { SearchBar } from 'components/searchBar/SearchBar';

export default function Header({ search, setSearch, onAdd }) {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  return (
    <Flex px="25px" py="10px" justify="space-between" align="center">
      <Text fontSize="22px" fontWeight="700" color={textColor}>Service Orders</Text>
      <Flex gap={2}>
        <SearchBar placeholder="Search service orders..." value={search} onChange={(e) => setSearch(e.target.value)} />
        <Button colorScheme="green" onClick={onAdd}>+</Button>
      </Flex>
    </Flex>
  );
}
