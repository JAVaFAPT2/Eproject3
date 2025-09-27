import React from "react";
import { Flex, Text, IconButton } from "@chakra-ui/react";
import { MdAdd } from "react-icons/md";
import { SearchBar } from "components/navbar/searchBar/SearchBar";

export default function ProductHeader({ searchInput, setSearchInput, onAddOpen }) {
  return (
    <Flex px="25px" my="8px" justifyContent="space-between" align="center">
      <Text fontSize="22px" fontWeight="700">
        Product List
      </Text>
      <Flex gap={2}>
        <SearchBar
          placeholder="Search products..."
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
        />
        <IconButton
          aria-label="add-product"
          icon={<MdAdd />}
          colorScheme="green"
          onClick={onAddOpen}
        />
      </Flex>
    </Flex>
  );
}
