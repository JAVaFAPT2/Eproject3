import './assets/css/App.css';
import { Routes, Route, Navigate } from 'react-router-dom';
import { ChakraProvider } from '@chakra-ui/react';
import { useState } from 'react';

import initialTheme from './theme/theme';

// 🧩 Layouts
import AuthLayout from './layouts/auth';
import AdminLayout from './layouts/admin';
import UserLayout from 'layouts/user';

// 🧩 Providers
import { UserProvider } from 'contexts/UserContext';

export default function Main() {
  const [currentTheme, setCurrentTheme] = useState(initialTheme);

  return (
    <ChakraProvider theme={currentTheme}>
      <UserProvider>
        <Routes>
          <Route path="auth/*" element={<AuthLayout />} />
          <Route path="user/*" element={<UserLayout />} />
          <Route
            path="admin/*"
            element={
              <AdminLayout theme={currentTheme} setTheme={setCurrentTheme} />
            }
          />

          {/* Điều hướng mặc định */}
          <Route path="/" element={<Navigate to="/user" replace />} />
        </Routes>
      </UserProvider>
    </ChakraProvider>
  );
}
