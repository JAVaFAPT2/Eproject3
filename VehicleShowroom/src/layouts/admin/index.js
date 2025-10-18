import React, { useState, useEffect } from 'react';
import { Box, Portal, useDisclosure } from '@chakra-ui/react';
import {
  Routes,
  Route,
  Navigate,
  useLocation,
  useNavigate,
} from 'react-router-dom';
import Sidebar from 'components/sidebar/Sidebar.js';
import Navbar from 'components/navbar/NavbarAdmin.js';
import Footer from 'components/footer/FooterAdmin.js';
import { SidebarContext } from 'contexts/SidebarContext';
import routes from 'routes.js';
import { useUser } from 'contexts/UserContext';
import { useAppToast } from 'utils/ToastHelper';

export default function Dashboard(props) {
  const { ...rest } = props;
  const [fixed] = useState(false);
  const [toggleSidebar, setToggleSidebar] = useState(false);
  const { onOpen } = useDisclosure();
  const location = useLocation();
  const navigate = useNavigate();
  const toast = useAppToast();
  const { user, loading } = useUser();

  useEffect(() => {
    if (user === null && !loading) {
      toast.warning('Please sign in first.');
      navigate('/auth/sign-in', { replace: true });
      return;
    }

    if (!loading && user && !['Admin'].includes(user.role)) {
      toast.error(
        'Access denied. You do not have permission to access the admin panel.',
      );
      navigate('/user', { replace: true });
    }
  }, [user, loading, navigate, toast]);

  // ✅ Lọc route cho sidebar
  const filteredRoutes = routes.filter((route) => {
    if (route.hideInSidebar) return false;
    return true;
  });

  // ✅ Lấy route hiện tại
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

  const activeRoute = getActiveRoute(routes, location.pathname);

  // ✅ Render route component
  const getRoutesComponents = (routes) =>
    routes.map((route, key) => {
      if (route.hideInSidebar) return null;
      if (route.role && route.role !== user?.roleName) return null;

      if (route.layout === '/admin') {
        return <Route path={route.path} element={route.component} key={key} />;
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
          w={{ base: '100%', xl: 'calc(100% - 290px)' }}
        >
          <Portal>
            <Box>
              <Navbar
                onOpen={onOpen}
                logoText="Trendify Admin"
                brandText={activeRoute?.name || 'Dashboard'}
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
            pt={{ base: '200px', md: '120px' }}
            minH="100vh"
          >
            <Routes>
              {getRoutesComponents(routes)}
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
