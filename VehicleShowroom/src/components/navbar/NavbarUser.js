import React from 'react';
import {
  Box,
  Flex,
  IconButton,
  Text,
  useBreakpointValue,
} from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import { HamburgerIcon, CloseIcon } from '@chakra-ui/icons';
import CategoryMenu from 'components/categoryMenu/CategoryMenu';

function NavbarUser({ toggleCategory, isCategoryOpen }) {
  const navigate = useNavigate();

  return (
    <>
      {/* Drawer menu */}
      <CategoryMenu isVisible={isCategoryOpen} closeHandler={toggleCategory} />

      <Flex
        as="header"
        position="absolute"
        top={0}
        w="100%"
        h={{ base: '72px', md: '90px' }}
        align="center"
        justify="center"
        bg="transparent"
        color="white"
        zIndex={200}
      >
        <Flex
          w="100%"
          maxW="2560px"
          px={{ base: 4, md: 10 }}
          align="center"
          justify="space-between"
        >
          {/* 🔹 Menu button */}
          <IconButton
            icon={
              isCategoryOpen ? (
                <CloseIcon boxSize={5} />
              ) : (
                <HamburgerIcon boxSize={6} />
              )
            }
            variant="ghost"
            colorScheme="whiteAlpha"
            onClick={toggleCategory}
            aria-label="Toggle menu"
            _hover={{ bg: 'whiteAlpha.300' }}
          />

          {/* 🔹 Center title */}
          <Text
            fontSize={useBreakpointValue({ base: 'lg', md: '2xl' })}
            fontWeight="bold"
            cursor="pointer"
            onClick={() => navigate('/')}
          >
            Car Showroom
          </Text>

          {/* 🔹 Placeholder for right side (empty to balance layout) */}
          <Box w="40px" />
        </Flex>
      </Flex>
    </>
  );
}

export default NavbarUser;
