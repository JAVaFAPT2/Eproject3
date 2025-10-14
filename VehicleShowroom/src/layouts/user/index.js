import React, { useState } from 'react';
import { Box, Portal, useColorModeValue } from '@chakra-ui/react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Footer from 'components/footer/FooterAdmin.js';
import routes from 'routes.js';
import NavbarUser from 'components/navbar/NavbarUser';

export default function UserLayout() {
  const bgColor = useColorModeValue('white', 'navy.800');
  const [isCategoryOpen, setIsCategoryOpen] = useState(false);

  /** 🔹 Lấy danh sách Route trong layout='/user' */
  const renderUserRoutes = (routes) =>
    routes.flatMap((route, key) => {
      if (route.collapse || route.category) {
        return renderUserRoutes(route.items || []);
      }

      if (route.layout === '/user') {
        return <Route key={key} path={route.path} element={route.component} />;
      }

      return [];
    });

  return (
    <Box minH="100vh" bg={bgColor}>
      {/* 🔹 NavbarUser */}
      <Portal>
        <Box w="100%">
          <NavbarUser
            toggleCategory={() => setIsCategoryOpen((prev) => !prev)}
            isCategoryOpen={isCategoryOpen}
            modl={true}
          />
        </Box>
      </Portal>

      {/* 🔹 Main Content */}
      <Box mx="auto" pb="60px" minH="100vh">
        <Routes>
          {renderUserRoutes(routes)}
          {/* Redirect fallback */}
          <Route path="/" element={<Navigate to="/user/home" replace />} />
        </Routes>
      </Box>

      {/* 🔹 Footer chung */}
      <Footer />
    </Box>
  );
}
