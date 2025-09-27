import React from 'react';
import { Flex, Text } from '@chakra-ui/react';
import { SearchBar } from 'components/navbar/searchBar/SearchBar';

export default function UserHeader({ searchInput, setSearchInput, textColor }) {
  return (
    <Flex px="25px" my="8px" justifyContent="space-between" align="center">
      <Text color={textColor} fontSize="22px" mb="4px" fontWeight="700">
        User List
      </Text>
      <Flex gap={2}>
        <SearchBar
          placeholder="Search users..."
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
        />
      </Flex>
    </Flex>
  );
}
