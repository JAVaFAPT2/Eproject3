import React from 'react';
import { Flex, Text, Button, useColorModeValue } from '@chakra-ui/react';
import { SearchBar } from 'components/navbar/searchBar/SearchBar';

export default function BlogTableHeader({
  title,
  searchInput,
  setSearchInput,
  onAdd,
}) {
  const textColor = useColorModeValue('secondaryGray.900', 'white');

  return (
    <Flex px="25px" my="8px" justifyContent="space-between" align="center">
      <Text color={textColor} fontSize="22px" fontWeight="700">
        {title}
      </Text>
      <Flex gap={2}>
        <SearchBar
          placeholder="Search blogs..."
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
        />
        <Button colorScheme="green" onClick={onAdd} fontSize="24px">
          +
        </Button>
      </Flex>
    </Flex>
  );
}
