import React from 'react';
import { Flex, Text, IconButton } from '@chakra-ui/react';
import { MdAdd } from 'react-icons/md';

export default function Header({ textColor, onAdd }) {
  return (
    <Flex
      px="25px"
      my="8px"
      justifyContent="space-between"
      align="center"
      flexWrap="wrap"
    >
      <Text color={textColor} fontSize="22px" fontWeight="700">
        Employees
      </Text>

      {/* 🟢 Nút Add Employee */}
      <IconButton
        icon={<MdAdd />}
        colorScheme="green"
        size="sm"
        borderRadius={10}
        onClick={onAdd}
      />
    </Flex>
  );
}
