import React, { useEffect, useState } from 'react';
import {
  Box,
  Flex,
  IconButton,
  Image,
  useBreakpointValue,
  useColorModeValue,
} from '@chakra-ui/react';
import { useNavigate, useLocation } from 'react-router-dom';
import { HamburgerIcon } from '@chakra-ui/icons';
import CategoryMenu from 'components/categoryMenu/CategoryMenu';

import logo from 'assets/image/logo.png';

function NavbarUser({ toggleCategory, isCategoryOpen }) {
  const navigate = useNavigate();
  const location = useLocation();

  const [isHome, setIsHome] = useState(false);

  // 🧭 Recompute when route changes
  useEffect(() => {
    setIsHome(
      location.pathname === '/' ||
        location.pathname === '/user' ||
        location.pathname === '/user/home'
    );
  }, [location.pathname]);

  // 🎨 Auto switch color based on current route
  const textColor = useColorModeValue(isHome ? 'white' : 'black', isHome ? 'white' : 'gray.100');
  const iconColor = textColor;

  return (
    <>
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
        transition="color 0.3s ease"
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
            _hover={{ bg: 'transparent' }}
          />

          {/* 🔹 Center logo */}
          <Box
            cursor="pointer"
            onClick={() => navigate('/user/')}
            transition="transform 0.2s ease"
            _hover={{ transform: 'scale(1.05)' }}
          >
            <Image
              src={logo}
              alt="Car Showroom Logo"
              height={useBreakpointValue({ base: '48px', md: '60px' })}
              objectFit="contain"
              filter={isHome ? 'invert(1)' : 'invert(0)'} // 👈 auto đổi màu theo nền
              transition="filter 0.3s ease"
            />
          </Box>

          {/* 🔹 Placeholder for symmetry */}
          <Box w="40px" />
        </Flex>
      </Flex>
    </>
  );
}

export default NavbarUser;
