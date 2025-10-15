import React from 'react';
import { Flex, Text, IconButton, Button, useColorModeValue } from '@chakra-ui/react';
import { MdAdd, MdDelete } from 'react-icons/md';
import { SearchBar } from 'components/navbar/searchBar/SearchBar';

export default function Header({
  searchInput,
  setSearchInput,
  onAdd,
  onBulkDelete,
  hasSelected,
  textColor
}) {
  return (
    <Flex px="25px" my="8px" justifyContent="space-between" align="center">
      <Text color={textColor} fontSize="22px" fontWeight="700">
        Vehicle List
      </Text>
      <Flex gap={2}>
        <SearchBar
          placeholder="Search by VIN or Model..."
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
        />
        {hasSelected && (
          <Button
            leftIcon={<MdDelete />}
            colorScheme="red"
            variant="outline"
            borderRadius="2xl"
            onClick={onBulkDelete}
          >
            Delete Selected
          </Button>
        )}
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
