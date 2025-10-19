import React from 'react';
import {
  Box,
  Flex,
  IconButton,
  Text,
  useBreakpointValue,
} from '@chakra-ui/react';
import { useNavigate, useLocation } from 'react-router-dom';
import { HamburgerIcon } from '@chakra-ui/icons';
import CategoryMenu from 'components/categoryMenu/CategoryMenu';

function NavbarUser({ toggleCategory, isCategoryOpen }) {
  const navigate = useNavigate();
  const location = useLocation();

  // 🧭 Kiểm tra có đang ở trang home không
  const isHome =
    location.pathname === '/' ||
    location.pathname === '/user' ||
    location.pathname === '/user/home';

  // 🎨 Màu chữ và icon phụ thuộc location
  const textColor = isHome ? 'white' : 'black';
  const iconColor = isHome ? 'white' : 'black';

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
        color={textColor}
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
            icon={<HamburgerIcon boxSize={6} color={iconColor} />}
            variant="ghost"
            onClick={toggleCategory}
            aria-label="Toggle menu"
          />

          {/* 🔹 Center title */}
          <Text
            fontSize={useBreakpointValue({ base: 'lg', md: '2xl' })}
            fontWeight="bold"
            cursor="pointer"
            onClick={() => navigate('/')}
            color={textColor}
            transition="color 0.2s ease"
          >
            Car Showroom
          </Text>

          {/* 🔹 Placeholder for right side (balance layout) */}
          <Box w="40px" />
        </Flex>
      </Flex>
    </>
  );
}

export default NavbarUser;
