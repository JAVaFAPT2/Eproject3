import React from 'react';
import { Flex, Text, useColorModeValue } from '@chakra-ui/react';
import { SearchBar } from 'components/navbar/searchBar/SearchBar';

export default function Header({ search, setSearch }) {
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  return (
    <Flex px="25px" py="10px" justify="space-between" align="center">
      <Text fontSize="22px" fontWeight="700" color={textColor}>
        Reports
      </Text>
      <SearchBar
        placeholder="Search reports..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />
    </Flex>
  );
}
