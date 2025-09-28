import {
  Avatar,
  Button,
  Flex,
  Icon,
  Popover,
  PopoverTrigger,
  PopoverContent,
  PopoverBody,
  PopoverHeader,
  Text,
  useColorMode,
  useColorModeValue,
} from '@chakra-ui/react';
import { IoMdMoon, IoMdSunny, IoMdCart } from 'react-icons/io';
import React, { useState } from 'react';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import { useNavigate } from 'react-router-dom';
import AuthService from 'services/AuthService';
import { useUser } from 'contexts/UserContext';
import CartSidebar from 'components/sidebar/CartSidebar';
import { MdLogin } from 'react-icons/md';
import { SidebarResponsive } from 'components/sidebar/Sidebar';
import routes from 'routes.js';
import { useShowToast } from 'utils/helper';

export default function NavbarLinks() {
  const { colorMode, toggleColorMode } = useColorMode();
  const navbarIcon = useColorModeValue('gray.600', 'white');
  const menuBg = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('#E6ECFA', 'rgba(135, 140, 189, 0.3)');

  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [isCartOpen, setIsCartOpen] = useState(false);

  const { user } = useUser();
  const navigate = useNavigate();
  const { showToast } = useShowToast();

  const handleLogout = async () => {
    showToast('Logout success!', 'success');
    AuthService.logout();
    navigate('/auth/sign-in');
  };

  const filteredRoutes = routes.filter((route) => !route.hideInSidebar);

  return (
    <Flex align="center" gap="10px">
      {/* Sidebar toggle for mobile */}
      <SidebarResponsive routes={filteredRoutes} />

      {/* Cart toggle */}
      <Button
        variant="ghost"
        p="0"
        me={2}
        minW="unset"
        onClick={() => setIsCartOpen(true)}
        _hover={{ bg: 'transparent' }}
      >
        <Icon h="20px" w="20px" color={navbarIcon} as={IoMdCart} />
      </Button>

      <CartSidebar isOpen={isCartOpen} onClose={() => setIsCartOpen(false)} />

      {/* Dark/Light toggle */}
      <Button
        variant="ghost"
        p="0"
        minW="unset"
        onClick={toggleColorMode}
        _hover={{ bg: 'transparent' }}
      >
        <Icon
          h="20px"
          w="20px"
          color={navbarIcon}
          as={colorMode === 'light' ? IoMdMoon : IoMdSunny}
        />
      </Button>

      {/* User / Login */}
      {!user ? (
        <Button
          variant="ghost"
          p="0"
          me={2}
          minW="unset"
          onClick={() => navigate('/auth/sign-in')}
        >
          <Icon h="30px" w="30px" color={navbarIcon} as={MdLogin} />
        </Button>
      ) : (
        <Popover placement="bottom-end">
          <PopoverTrigger>
            <Avatar
              _hover={{ cursor: 'pointer' }}
              name={user?.fullName || user?.email?.split('@')[0] || 'User'}
              src={user?.avatarUrl || ''}
              size="sm"
              w="40px"
              h="40px"
              bg={menuBg}
              p={2}
            />
          </PopoverTrigger>
          <PopoverContent
            mt="10px"
            borderRadius="10px"
            bg={menuBg}
            border="none"
            w={{ base: '100%', md: '180px' }}
            shadow={'lg'}
          >
            <PopoverHeader borderBottom="1px solid" borderColor={borderColor}>
              <Text fontSize="sm" fontWeight="700" color={textColor}>
                👋 Hey, {user?.fullName || user?.email?.split('@')[0]}
              </Text>
            </PopoverHeader>
            <PopoverBody>
              <Flex direction="column" gap={2}>
                <Button
                  variant="ghost"
                  justifyContent="flex-start"
                  size="sm"
                  onClick={() => navigate('/user/profile')}
                >
                  Your Profile
                </Button>
                <Button
                  variant="ghost"
                  justifyContent="flex-start"
                  size="sm"
                  color="red.400"
                  onClick={() => setIsConfirmOpen(true)}
                >
                  Log out
                </Button>
              </Flex>
            </PopoverBody>
          </PopoverContent>
        </Popover>
      )}

      {/* Confirm Dialog */}
      <ConfirmDialog
        isOpen={isConfirmOpen}
        onClose={() => setIsConfirmOpen(false)}
        onConfirm={handleLogout}
        title="Confirm Logout"
        message="Are you sure you want to log out?"
      />
    </Flex>
  );
}
