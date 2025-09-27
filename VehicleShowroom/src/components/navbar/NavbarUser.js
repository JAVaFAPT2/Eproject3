import {
  Box,
  Flex,
  useColorModeValue,
  Image,
  IconButton,
  useDisclosure,
} from '@chakra-ui/react';
import React, { useState, useEffect } from 'react';
import { SearchBar } from 'components/navbar/searchBar/SearchBar';
import NavbarLinks from 'components/navbar/NavbarLinks';
import { SearchIcon, CloseIcon } from '@chakra-ui/icons';
import logo from 'assets/img/auth/auth.png';
import { motion, AnimatePresence } from 'framer-motion';
import { MdMenu } from 'react-icons/md';
import MegaMenu from 'components/navbar/megaMenu/MegaMenu';
import CategoryService from 'services/CategoryService';
import { useNavigate } from 'react-router-dom';

const MotionFlex = motion(Flex);
const MotionBox = motion(Box);

export default function NavbarUser() {
  const navbarBg = useColorModeValue('white', 'navy.800');
  const navbarBorder = useColorModeValue('rgba(11,20,55,0.1)', 'navy.600');

  const navigate = useNavigate();

  const { isOpen, onOpen, onClose } = useDisclosure();
  const [isMegaMenuOpen, setMegaMenuOpen] = useState(false);

  const [categories, setCategories] = useState([]);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const res = await CategoryService.getCategories(0, 50, '');
        setCategories(res.content || []);
      } catch (err) {
        console.error('Failed to load categories:', err);
      }
    };
    fetchCategories();
  }, []);

  return (
    <Flex
      position="fixed"
      top="0"
      left="0"
      w="100%"
      minH="75px"
      bg={navbarBg}
      borderBottom="1px solid"
      borderColor={navbarBorder}
      align="center"
      justify="space-between"
      px={6}
      zIndex="99"
      borderBottomLeftRadius={isMegaMenuOpen ? '0' : '20px'}
      borderBottomRightRadius={isMegaMenuOpen ? '0' : '20px'}
    >
      {/* Items (logo + links + desktop search) */}
      <AnimatePresence>
        {!isOpen && (
          <MotionFlex
            key="nav-items"
            align="center"
            justify={{
              base: 'space-between',
              md: 'center',
            }}
            w="100%"
            initial={{ x: 0, opacity: 1 }}
            exit={{ x: -100, opacity: 0 }}
            animate={{ x: 0, opacity: 1 }}
            transition={{ duration: 0.3 }}
          >
            <Flex align="center">
              <IconButton
                aria-label="Menu"
                icon={<MdMenu style={{ display: 'block', margin: 'auto' }} />}
                bg="transparent"
                me={4}
                onClick={() => setMegaMenuOpen(!isMegaMenuOpen)}
                display={isMegaMenuOpen ? 'none' : 'block'}
                fontSize="28px"
              />

              {/* Logo */}
              <Image
                src={logo}
                alt="Logo"
                h="60px"
                onClick={() => navigate('/')}
                _hover={{ cursor: 'pointer' }}
              />
            </Flex>

            <Box flex="1" />

            {/* Desktop Search */}
            <Box
              display={{ base: 'none', md: 'block' }}
              mx={6}
              border="1px solid"
              borderColor={navbarBorder}
              borderRadius="30px"
              py="2"
              px="4"
              w={{
                md: isMegaMenuOpen ? '50%' : '200px',
                lg: isMegaMenuOpen ? '50%' : '300px',
              }}
            >
              <SearchBar
                placeholder="Search..."
                borderRadius="30px"
                background="transparent"
                w="100%"
              />
            </Box>

            <Box flex="1" display={isMegaMenuOpen ? 'block' : 'none'} />

            <Flex align="center">
              {/* Mobile search icon */}
              <Box display={{ base: 'block', md: 'none' }}>
                <IconButton
                  aria-label="Open search"
                  icon={<SearchIcon />}
                  onClick={onOpen}
                  bg="transparent"
                />
              </Box>

              {/* Links */}
              <NavbarLinks />
            </Flex>
          </MotionFlex>
        )}
      </AnimatePresence>

      {/* Searchbar overlay (mobile) */}
      <AnimatePresence>
        {isOpen && (
          <MotionBox
            key="searchbar"
            position="absolute"
            top="0"
            left="0"
            w="100%"
            h="100%"
            initial={{ x: '100%' }}
            animate={{ x: 0 }}
            exit={{ x: '100%' }}
            transition={{ duration: 0.35, ease: 'easeInOut' }}
            zIndex="90"
          >
            <Flex
              w="100%"
              h="75px"
              align="center"
              justify="space-between"
              bg={navbarBg}
              px={4}
              borderBottom="1px solid"
              borderColor={navbarBorder}
            >
              <SearchBar
                placeholder="Search..."
                borderRadius="30px"
                background="transparent"
                w="100%"
              />
              <IconButton
                aria-label="Close search"
                icon={<CloseIcon />}
                ml={2}
                onClick={onClose}
                bg="transparent"
              />
            </Flex>
          </MotionBox>
        )}
      </AnimatePresence>

      {/* Mega menu */}
      <AnimatePresence>
        {isMegaMenuOpen && (
          <MotionBox
            key="megamenu"
            position="absolute"
            top="75px"
            left="0"
            w="100%"
            bg={navbarBg}
            borderBottom="1px solid"
            borderColor={navbarBorder}
            initial={{ opacity: 0, y: 0 }}
            animate={{ opacity: 1, y: 0 }}
            exit={{ opacity: 0, y: 0 }}
            transition={{ duration: 0.2 }}
          >
            <MegaMenu
              categories={categories}
              onClose={() => setMegaMenuOpen(false)}
            />
          </MotionBox>
        )}
      </AnimatePresence>
    </Flex>
  );
}
