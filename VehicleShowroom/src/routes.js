import React from 'react';

import { Icon } from '@chakra-ui/react';
import { MdHome, MdCardTravel } from 'react-icons/md';

// Admin Imports
import Dashboard from 'views/admin/dashboard';
import VehiclePage from 'views/admin/vehicle';
import OrderPage from 'views/admin/order';

// Auth Imports
import SignIn from 'views/auth/signIn';
import SignUp from 'views/auth/signUp';
import CheckEmailNotice from 'views/auth/checkEmailNotice';
import ResetPassword from 'views/auth/resetPassword';
import ForgotPassword from 'views/auth/forgotPassword';
import { RiOrderPlayLine } from 'react-icons/ri';
import Home from 'views/user/home/home';

const routes = [
  // Admin Routes
  {
    name: 'Dashboard',
    layout: '/admin',
    path: '/dashboard',
    icon: <Icon as={MdHome} width="20px" height="20px" color="inherit" />,
    component: <Dashboard />,
  },
  {
    name: 'Vehicle Management',
    layout: '/admin',
    path: '/vehicle-management',
    icon: <Icon as={MdCardTravel} width="20px" height="20px" color="inherit" />,
    component: <VehiclePage />,
  },
  {
    name: 'Order Management',
    layout: '/admin',
    path: '/prder-management',
    icon: (
      <Icon as={RiOrderPlayLine} width="20px" height="20px" color="inherit" />
    ),
    component: <OrderPage />,
  },

  //Auth Routes
  {
    name: 'Sign In',
    layout: '/auth',
    path: '/sign-in',
    component: <SignIn />,
    hideInSidebar: true,
  },
  {
    name: 'Sign Up',
    layout: '/auth',
    path: '/sign-up',
    component: <SignUp />,
    hideInSidebar: true,
  },
  {
    name: 'Forgot Password',
    layout: '/auth',
    path: '/forgot-password',
    component: <ForgotPassword />,
    hideInSidebar: true,
  },
  {
    name: 'Check Email',
    layout: '/auth',
    path: '/check-email',
    component: <CheckEmailNotice />,
    hideInSidebar: true,
  },
  {
    name: 'Reset Password',
    layout: '/auth',
    path: '/reset-password',
    component: <ResetPassword />,
    hideInSidebar: true,
  },

  //User Routes
  {
    name: 'Home',
    layout: '/user',
    path: '/home',
    component: <Home />,
    hideInSidebar: true,
  },
];

export default routes;
