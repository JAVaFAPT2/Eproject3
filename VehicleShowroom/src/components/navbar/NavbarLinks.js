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
  useColorModeValue,
} from '@chakra-ui/react';
import { useAppToast } from 'utils/ToastHelper';
import React, { useState } from 'react';
import ConfirmDialog from 'components/dialog/ConfirmDialog';
import { useNavigate, useLocation } from 'react-router-dom';
import AuthService from 'services/AuthService';
import { useUser } from 'contexts/UserContext';
import { MdLogin } from 'react-icons/md';
import { SidebarResponsive } from 'components/sidebar/Sidebar';
import routes from 'routes.js';

export default function NavbarLinks() {
  const navbarIcon = useColorModeValue('gray.600', 'white');
  const menuBg = useColorModeValue('white', 'navy.800');
  const textColor = useColorModeValue('secondaryGray.900', 'white');
  const borderColor = useColorModeValue('#E6ECFA', 'rgba(135, 140, 189, 0.3)');

  const [isConfirmOpen, setIsConfirmOpen] = useState(false);

  const { user } = useUser();
  const navigate = useNavigate();
  const toast = useAppToast();

  const location = useLocation();
  const isAdminRoute = location.pathname.startsWith('/admin');

  const handleToggleDashboard = () => {
    if (isAdminRoute) {
      navigate('/user');
    } else {
      navigate('/admin');
    }
  };

  const handleLogout = async () => {
    toast.success('Logout success!');
    AuthService.logout();
    setTimeout(() => {
      navigate('/user');
      window.location.reload();
    }, 1000);
  };

  const filteredRoutes = routes.filter((route) => {
    if (route.hideInSidebar) return false;
    if (route.role && route.role !== user?.roleName) return false;
    return true;
  });

  return (
    <Flex align="center" gap="10px">
      {isAdminRoute && <SidebarResponsive routes={filteredRoutes} />}

      {/* User / Login */}
      {!user ? (
        <Button
          variant="ghost"
          p="0"
          me={2}
          minW="unset"
          onClick={() => navigate('/auth/sign-in')}
          _hover={{ backgroundColor: 'none' }}
        >
          <Icon h="24px" w="24px" color={navbarIcon} as={MdLogin} />
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
              bg="transparent"
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
                {(user?.roleName === 'ADMIN' ||
                  user?.roleName === 'EMPLOYEE') && (
                  <Button
                    variant="ghost"
                    justifyContent="flex-start"
                    size="sm"
                    onClick={handleToggleDashboard}
                  >
                    {isAdminRoute ? 'Back to Website' : 'Go to Dashboard'}
                  </Button>
                )}

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
