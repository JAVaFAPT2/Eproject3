import React from 'react';
import {
  Box,
  Flex,
  Button,
  IconButton,
  Image,
  useBreakpointValue,
} from '@chakra-ui/react';
import { useNavigate, useLocation } from 'react-router-dom';
import CategoryMenu from 'components/navbar/components/CategoryMenu';
import { CloseIcon } from '@chakra-ui/icons';

export default function NavbarUser({ toggleCategory, isCategoryOpen, modl }) {
  const navigate = useNavigate();
  const location = useLocation();
  const isDesktop = useBreakpointValue({ base: false, md: true });

  const isHome = location.pathname === '/user/home';

  return (
    <>
      <CategoryMenu isVisible={isCategoryOpen} closeHandler={toggleCategory} />

      <Box
        as="header"
        position={modl ? 'absolute' : 'relative'}
        top={0}
        w="100%"
        zIndex={100}
        bg={
          isHome
            ? 'linear-gradient(180deg, rgba(0,0,0,0.8) 0%, rgba(0,0,0,0) 100%)'
            : 'transparent'
        }
      >
        <Flex
          align="center"
          justify="space-between"
          px={{ base: 4, md: 10 }}
          h={'100px'}
          color={isHome ? 'white' : 'black'}
        >
          {/* Menu button */}
          <Button
            variant="ghost"
            color={isHome ? 'white' : 'black'}
            fontSize="md"
            leftIcon={
              <Image
                src="https://cdn.ui.porsche.com/porsche-design-system/icons/menu-lines.e332216.svg"
                w="24px"
                h="24px"
                filter={isHome ? 'invert(1)' : 'none'}
              />
            }
            onClick={toggleCategory}
            _hover={{ bg: 'transparent' }}
            gap={2}
          >
            {isDesktop && 'Menu'}
          </Button>

          {/* Logo */}
          <Box
            cursor="pointer"
            onClick={() => navigate('/')}
            color={isHome ? 'white' : 'black'}
            fontWeight={'600'}
            fontSize={'2xl'}
          >
            Car Showroom
          </Box>

          {/* Profile / Close icon */}
          <IconButton
            variant="ghost"
            aria-label="User Menu"
            onClick={toggleCategory}
            icon={
              isCategoryOpen ? (
                <CloseIcon
                  boxSize="24px"
                  filter={isHome ? 'invert(1)' : 'none'}
                  display={{ base: 'none', md: 'block' }}
                />
              ) : (
                <Image
                  src="https://cdn.ui.porsche.com/porsche-design-system/icons/user.c18dabe.svg"
                  w="24px"
                  h="24px"
                  filter={isHome ? 'invert(1)' : 'none'}
                />
              )
            }
          />
        </Flex>
      </Box>
    </>
  );
}
