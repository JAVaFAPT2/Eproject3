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

// User Imports
import Home from 'views/user/home/Home';
import List from 'views/user/list/List';
import Detail from 'views/user/detail/Detail';
import PurchaseOrderPage from 'views/admin/purchase-order';

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
    name: 'Purchase Order Management',
    layout: '/admin',
    path: '/purchase-order-management',
    icon: (
      <Icon as={RiOrderPlayLine} width="20px" height="20px" color="inherit" />
    ),
    component: <PurchaseOrderPage />,
  },
  {
    name: 'Order Management',
    layout: '/admin',
    path: '/order-management',
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
  },
  {
    name: 'Sign Up',
    layout: '/auth',
    path: '/sign-up',
    component: <SignUp />,
  },
  {
    name: 'Forgot Password',
    layout: '/auth',
    path: '/forgot-password',
    component: <ForgotPassword />,
  },
  {
    name: 'Check Email',
    layout: '/auth',
    path: '/check-email',
    component: <CheckEmailNotice />,
  },
  {
    name: 'Reset Password',
    layout: '/auth',
    path: '/reset-password',
    component: <ResetPassword />,
  },

  //User Routes
  {
    name: 'Home',
    layout: '/user',
    path: '/home',
    component: <Home />,
  },
  {
    name: 'Models',
    layout: '/user',
    path: '/models',
    component: <List />,
  },
  {
    name: 'Models',
    layout: '/user',
    path: '/models/:model',
    component: <List />,
  },
  {
    name: 'Detail',
    layout: '/user',
    path: '/model/:id',
    component: <Detail />,
  },
];

export default routes;
