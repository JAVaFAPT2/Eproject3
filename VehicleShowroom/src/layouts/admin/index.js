import React, { useState } from 'react';
import { Box, Portal, useDisclosure } from '@chakra-ui/react';
import { Routes, Route, Navigate, useLocation } from 'react-router-dom';
import Sidebar from 'components/sidebar/Sidebar.js';
import Navbar from 'components/navbar/NavbarAdmin.js';
import Footer from 'components/footer/FooterAdmin.js';
import { SidebarContext } from 'contexts/SidebarContext';
import routes from 'routes.js';
import AuthService from 'services/AuthService';
import PrivateRoute from 'components/auth/PrivateRoute';

export default function Dashboard(props) {
  const { ...rest } = props;
  const [fixed] = useState(false);
  const [toggleSidebar, setToggleSidebar] = useState(false);
  const { onOpen } = useDisclosure();
  const location = useLocation();

  const role = AuthService.getRole();

  // Filter cho Sidebar
  const filteredRoutes = routes.filter((route) => {
    if (route.hideInSidebar) return false;
    if (route.role && route.role !== role) return false;
    return true;
  });

  // Hàm lấy route hiện tại
  const getActiveRoute = (routes, pathname) => {
    for (let route of routes) {
      if (route.collapse) {
        const active = getActiveRoute(route.items, pathname);
        if (active) return active;
      } else if (route.category) {
        const active = getActiveRoute(route.items, pathname);
        if (active) return active;
      } else {
        if (route.layout + route.path === pathname) return route;
      }
    }
    return null;
  };

  const activeRoute = getActiveRoute(routes, location.pathname);

  // Render routes
  const getRoutesComponents = (routes) =>
    routes.map((route, key) => {
      if (route.hideInSidebar) return null;
      if (route.role && route.role !== role) return null;

      if (route.layout === '/admin') {
        const element =
          route.path === '/profile' ? (
            <PrivateRoute>{route.component}</PrivateRoute>
          ) : (
            route.component
          );

        return <Route path={route.path} element={element} key={key} />;
      }
      if (route.collapse || route.category) {
        return getRoutesComponents(route.items);
      }
      return null;
    });

  return (
    <Box>
      <SidebarContext.Provider value={{ toggleSidebar, setToggleSidebar }}>
        <Sidebar routes={filteredRoutes} display="none" {...rest} />

        <Box
          float="right"
          minHeight="100vh"
          height="100%"
          overflow="auto"
          position="relative"
          maxHeight="100%"
          w={{ base: '100%', xl: 'calc( 100% - 290px )' }}
        >
          <Portal>
            <Box>
              <Navbar
                onOpen={onOpen}
                logoText="Horizon UI Dashboard PRO"
                brandText={activeRoute?.name || 'Default Brand Text'}
                secondary={activeRoute?.secondary || false}
                message={activeRoute?.messageNavbar || ''}
                fixed={fixed}
                {...rest}
              />
            </Box>
          </Portal>

          <Box
            mx="auto"
            p={{ base: '20px', md: '30px' }}
            pt="50px"
            minH="100vh"
          >
            <Routes>
              {getRoutesComponents(routes)}
              <Route
                path="/"
                element={<Navigate to="/admin/default" replace />}
              />
            </Routes>
          </Box>

          <Footer />
        </Box>
      </SidebarContext.Provider>
    </Box>
  );
}
