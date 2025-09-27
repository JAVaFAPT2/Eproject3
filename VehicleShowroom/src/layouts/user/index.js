import React from 'react';
import { Box, Portal, useDisclosure, useColorModeValue } from '@chakra-ui/react';
import { Routes, Route, Navigate } from 'react-router-dom';
import NavbarUser from 'components/navbar/NavbarUser';
import Footer from 'components/footer/FooterAdmin.js';
import routes from 'routes.js';
import PrivateRoute from 'components/auth/PrivateRoute';

export default function UserLayout() {
  const bgColor = useColorModeValue('white', 'navy.800');
  const { isOpen, onToggle } = useDisclosure();

  const getRoutesComponents = (routes) =>
    routes.map((route, key) => {
      if (route.role === 'ADMIN') return null;
      if (route.collapse || route.category) {
        return getRoutesComponents(route.items);
      }
      if (route.layout === '/user' || !route.role) {
        return (
          <Route
            key={key}
            path={route.path}
            element={
              route.private ? (
                <PrivateRoute>{route.component}</PrivateRoute>
              ) : (
                route.component
              )
            }
          />
        );
      }
      return null;
    });

  return (
    <Box minH="100vh" bg={bgColor}>
      {/* Navbar */}
      <Portal>
        <Box>
          <NavbarUser isOpen={isOpen} onToggle={onToggle} />
        </Box>
      </Portal>

      {/* Nội dung chính */}
      <Box mx="auto" pt="75px" minH="100vh">
        <Routes>
          {getRoutesComponents(routes)}
          <Route path="/" element={<Navigate to="/user/home" replace />} />
        </Routes>
      </Box>

      {/* Footer */}
      <Footer />
    </Box>
  );
}
