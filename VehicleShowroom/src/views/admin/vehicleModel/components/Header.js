import React from 'react';
import { Flex, Text, IconButton, useColorModeValue } from '@chakra-ui/react';
import { MdAdd } from 'react-icons/md';
import { SearchBar } from 'components/navbar/searchBar/SearchBar';

export default function Header({
  searchInput,
  setSearchInput,
  onAdd,
  textColor,
}) {
  return (
    <Flex px="25px" my="8px" justifyContent="space-between" align="center">
      <Text color={textColor} fontSize="22px" fontWeight="700">
        Vehicle Models
      </Text>
      <Flex gap={2}>
        <SearchBar
          placeholder="Search vehicle models..."
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
        />
        <IconButton
          aria-label="Add new"
          icon={<MdAdd />}
          colorScheme="green"
          size="md"
          borderRadius="2xl"
          onClick={onAdd}
        />
      </Flex>
    </Flex>
  );
}
