import React from 'react';
import { Flex, Text } from '@chakra-ui/react';

export default function Header({ textColor }) {
  return (
    <Flex px="25px" my="8px" justifyContent="start" align="center">
      <Text color={textColor} fontSize="22px" fontWeight="700">
        Customers
      </Text>
    </Flex>
  );
}
