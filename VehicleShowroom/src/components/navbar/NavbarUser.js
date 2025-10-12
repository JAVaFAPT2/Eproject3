import React, { useEffect, useState } from 'react';
import {
  Box,
  Flex,
  Button,
  IconButton,
  Image,
  useBreakpointValue,
} from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import CategoryMenu from 'components/navbar/components/CategoryMenu';
import Svg from 'components/navbar/components/Svg';
import Picture from 'components/navbar/components/Picture';
import { CloseIcon } from '@chakra-ui/icons';

export default function NavbarUser({ toggleCategory, isCategoryOpen, styl, modl }) {
  const navigate = useNavigate();
  const isDesktop = useBreakpointValue({ base: false, md: true });
  const [profileIcon, setProfileIcon] = useState(null);

  useEffect(() => {
    const updateProfileIcon = () => {
      setProfileIcon(isDesktop ? <Svg styl={styl} /> : <Picture styl={styl} />);
    };
    updateProfileIcon();
  }, [isDesktop, styl]);

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
          styl
            ? 'transparent'
            : 'linear-gradient(180deg, rgba(0,0,0,0.8) 0%, rgba(0,0,0,0) 100%)'
        }
      >
        <Flex
          align="center"
          justify="space-between"
          px={{ base: 4, md: 10 }}
          h={{ base: '72px', md: '148px' }}
          color={styl ? 'black' : 'white'}
        >
          {/* Menu button */}
          <Button
            variant="ghost"
            color={styl ? 'black' : 'white'}
            fontSize="md"
            leftIcon={
              <Image
                src="https://cdn.ui.porsche.com/porsche-design-system/icons/menu-lines.e332216.svg"
                w="24px"
                h="24px"
                filter={!styl ? 'invert(1)' : 'none'}
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
            maxW={{ base: '30px', md: '100px' }}
            onClick={() => navigate('/')}
          >
            {profileIcon}
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
                  filter={modl ? 'invert(1)' : 'none'}
                  display={{ base: 'none', md: 'block' }}
                />
              ) : (
                <Image
                  src="https://cdn.ui.porsche.com/porsche-design-system/icons/user.c18dabe.svg"
                  w="24px"
                  h="24px"
                  filter={!styl ? 'invert(1)' : 'none'}
                />
              )
            }
          />
        </Flex>
      </Box>
    </>
  );
}