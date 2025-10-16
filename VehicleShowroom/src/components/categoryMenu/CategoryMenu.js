import React from 'react';
import {
  Box,
  Flex,
  IconButton,
  Button,
  useColorModeValue,
  HStack,
  Text,
} from '@chakra-ui/react';
import { CloseIcon } from '@chakra-ui/icons';
import { motion, AnimatePresence } from 'framer-motion';
import { useNavigate } from 'react-router-dom';
import { MdLogin, MdPerson, MdLogout } from 'react-icons/md';
import IndividualCars from 'components/categoryMenu/components/IndividualCars';
import { useUser } from 'contexts/UserContext';

const MotionBox = motion(Box);

function CategoryMenu({ isVisible, closeHandler }) {
  const bgColor = useColorModeValue('white', 'gray.900');
  const textColor = useColorModeValue('gray.700', 'white');
  const borderColor = useColorModeValue('gray.200', 'gray.700');
  const navigate = useNavigate();
  const { isAuthenticated, user, logout } = useUser();

  // 🔹 Handle sign out
  const handleSignOut = async () => {
    await logout();
    closeHandler();
  };

  // 🔹 Handle navigation safely
  const handleNavigate = (path) => {
    closeHandler();
    navigate(path);
  };

  return (
    <>
      {/* 🔹 Drawer animation */}
      <AnimatePresence>
        {isVisible && (
          <MotionBox
            initial={{ x: '-100%', opacity: 0 }}
            animate={{ x: 0, opacity: 1 }}
            exit={{ x: '-100%', opacity: 0 }}
            transition={{ duration: 0.35, ease: 'easeInOut' }}
            position="fixed"
            top="0"
            left="0"
            h="100dvh"
            w={{ base: '100%', md: '30%' }}
            bg={bgColor}
            color={textColor}
            shadow="xl"
            display="flex"
            flexDirection="column"
            justifyContent="space-between"
            zIndex="1500"
          >
            {/* Header */}
            <Flex
              justify="space-between"
              align="center"
              p={6}
              borderColor={borderColor}
            >
              <Text fontSize="2xl" fontWeight="600">
                Models
              </Text>
              <IconButton
                icon={<CloseIcon />}
                aria-label="Close menu"
                onClick={closeHandler}
                variant="ghost"
                size="sm"
              />
            </Flex>

            {/* Content: Danh sách xe */}
            <Box flex="1" overflowY="auto" px="1rem" py="1rem">
              <IndividualCars />
            </Box>

            {/* Footer */}
            <Flex
              px="1rem"
              py="1rem"
              borderTop="1px solid"
              borderColor={borderColor}
              justify="space-between"
              align="center"
              gap={3}
            >
              {!isAuthenticated ? (
                <Flex w="full" justify="flex-end">
                  <Button
                    rightIcon={<MdLogin size={20} />}
                    onClick={() => handleNavigate('/auth/sign-in')}
                  >
                    Sign In
                  </Button>
                </Flex>
              ) : (
                <HStack w="full" justify="space-between">
                  <Flex gap={3} align="center">
                    {/* 👤 Profile Button */}
                    <Button
                      leftIcon={<MdPerson size={20} />}
                      onClick={() => handleNavigate('/user/profile')}
                    >
                      Profile
                    </Button>

                    {/* 🛠️ Admin Button (only for admins) */}
                    {user?.role === 'Admin' && (
                      <Button
                        leftIcon={<MdPerson size={20} />}
                        colorScheme="brand"
                        onClick={() => handleNavigate('/admin')}
                      >
                        Admin Panel
                      </Button>
                    )}
                  </Flex>
                  <Button
                    leftIcon={<MdLogout size={20} />}
                    onClick={handleSignOut}
                    color="red"
                  >
                    Sign Out
                  </Button>
                </HStack>
              )}
            </Flex>
          </MotionBox>
        )}
      </AnimatePresence>

      {/* 🔹 Overlay */}
      <AnimatePresence>
        {isVisible && (
          <MotionBox
            initial={{ opacity: 0 }}
            animate={{ opacity: 0.3 }}
            exit={{ opacity: 0 }}
            transition={{ duration: 0.25 }}
            position="fixed"
            top="0"
            left={{ base: '0', md: '30%' }}
            w={{ base: '100%', md: '70%' }}
            h="100dvh"
            bg="black"
            zIndex="1400"
            onClick={closeHandler}
          />
        )}
      </AnimatePresence>
    </>
  );
}

export default CategoryMenu;
