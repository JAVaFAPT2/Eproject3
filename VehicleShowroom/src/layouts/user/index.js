import React, { useState } from 'react';
import { Box, useColorModeValue } from '@chakra-ui/react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Footer from 'components/footer/FooterAdmin.js';
import routes from 'routes.js';
import NavbarUser from 'components/navbar/NavbarUser';

export default function UserLayout() {
  const bgColor = useColorModeValue('white', 'navy.800');
  const [isCategoryOpen, setIsCategoryOpen] = useState(false);

  const toggleCategory = () => setIsCategoryOpen((prev) => !prev);

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
      <Box w="100%">
        <NavbarUser
          toggleCategory={toggleCategory}
          isCategoryOpen={isCategoryOpen}
        />
      </Box>

      {/* 🔹 Main Content */}
      <Box mx="auto" pb="60px" minH="100vh">
        <Routes>
          {renderUserRoutes(routes)}
          <Route path="/" element={<Navigate to="/user/home" replace />} />
        </Routes>
      </Box>

      {/* 🔹 Footer */}
      <Footer />
    </Box>
  );
}
