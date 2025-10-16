import React from 'react';

// Chakra imports
import { Flex, Image, Text } from '@chakra-ui/react';
import logo from 'assets/image/logo.png';
// Custom components
import { HSeparator } from 'components/separator/Separator';

export function SidebarBrand() {
  return (
    <Flex align="center" direction="column">
      <Image src={logo} w="100px" />
      <Text fontWeight={600} fontSize="xl" mb={5}>
        Car Showroom
      </Text>
      <HSeparator mb="20px" />
    </Flex>
  );
}

export default SidebarBrand;
