import React from 'react';
import { Flex, Text, IconButton } from '@chakra-ui/react';
import { MdAdd } from 'react-icons/md';

export default function Header({ onAdd, textColor }) {
  return (
    <Flex px="25px" my="8px" justifyContent="space-between" align="center">
      <Text color={textColor} fontSize="22px" fontWeight="700">
        Purchase Orders
      </Text>
      <IconButton
        aria-label="Add new"
        icon={<MdAdd />}
        colorScheme="green"
        size="md"
        borderRadius="2xl"
        onClick={onAdd}
      />
    </Flex>
  );
}
