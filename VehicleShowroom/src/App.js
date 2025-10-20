import './assets/css/App.css';
import { Routes, Route, Navigate } from 'react-router-dom';
import { ChakraProvider } from '@chakra-ui/react';
import { useState, Suspense, lazy } from 'react';

import initialTheme from './theme/theme';
import useWebVitals from './hooks/useWebVitals';

// 🧩 Layouts - Lazy loaded for better performance
const AuthLayout = lazy(() => import('./layouts/auth'));
const AdminLayout = lazy(() => import('./layouts/admin'));
const UserLayout = lazy(() => import('layouts/user'));

// 🧩 Providers
import { UserProvider } from 'contexts/UserContext';

// Loading component
const LoadingSpinner = () => (
  <div style={{ 
    display: 'flex', 
    justifyContent: 'center', 
    alignItems: 'center', 
    height: '100vh',
    fontSize: '18px'
  }}>
    Loading...
  </div>
);

export default function Main() {
  const [currentTheme, setCurrentTheme] = useState(initialTheme);
  
  // Monitor web vitals
  useWebVitals();

  return (
    <ChakraProvider theme={currentTheme}>
      <UserProvider>
        <Suspense fallback={<LoadingSpinner />}>
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
        </Suspense>
      </UserProvider>
    </ChakraProvider>
  );
}
