import React, { useState } from 'react';
import { Box, Portal, useDisclosure } from '@chakra-ui/react';
import { Routes, Route, Navigate, useLocation } from 'react-router-dom';
import Sidebar from 'components/sidebar/Sidebar.js';
import Navbar from 'components/navbar/NavbarAdmin.js';
import Footer from 'components/footer/FooterAdmin.js';
import { SidebarContext } from 'contexts/SidebarContext';
import routes from 'routes.js';

export default function Dashboard(props) {
  const { ...rest } = props;
  const [fixed] = useState(false);
  const [toggleSidebar, setToggleSidebar] = useState(false);
  const { onOpen } = useDisclosure();
  const location = useLocation();

  // ✅ Chỉ lấy các route có layout === '/admin'
  const adminRoutes = routes.filter((route) => route.layout === '/admin');

  // ✅ Lấy route đang active
  const getActiveRoute = (routes, pathname) => {
    for (let route of routes) {
      if (route.collapse || route.category) {
        const active = getActiveRoute(route.items, pathname);
        if (active) return active;
      } else if (route.layout + route.path === pathname) {
        return route;
      }
    }
    return null;
  };

  const activeRoute = getActiveRoute(adminRoutes, location.pathname);

  // ✅ Render các route admin
  const getRoutesComponents = (routes) =>
    routes
      .filter((r) => r.layout === '/admin')
      .map((route, key) => (
        <Route path={route.path} element={route.component} key={key} />
      ));

  return (
    <Box>
      <SidebarContext.Provider value={{ toggleSidebar, setToggleSidebar }}>
        {/* ✅ Sidebar chỉ hiện route admin */}
        <Sidebar routes={adminRoutes} display="none" {...rest} />

        <Box
          float="right"
          minHeight="100vh"
          height="100%"
          overflow="auto"
          position="relative"
          maxHeight="100%"
          w={{ base: '100%', xl: 'calc(100% - 290px)' }}
        >
          {/* ✅ Navbar */}
          <Portal>
            <Box>
              <Navbar
                onOpen={onOpen}
                logoText="Horizon UI Dashboard PRO"
                brandText={activeRoute?.name || 'Dashboard'}
                secondary={activeRoute?.secondary || false}
                message={activeRoute?.messageNavbar || ''}
                fixed={fixed}
                {...rest}
              />
            </Box>
          </Portal>

          {/* ✅ Nội dung chính */}
          <Box mx="auto" p={{ base: '20px', md: '30px' }} pt="50px" minH="100vh">
            <Routes>
              {getRoutesComponents(adminRoutes)}
              <Route
                path="/"
                element={<Navigate to="/admin/dashboard" replace />}
              />
            </Routes>
          </Box>

          <Footer />
        </Box>
      </SidebarContext.Provider>
    </Box>
  );
}
