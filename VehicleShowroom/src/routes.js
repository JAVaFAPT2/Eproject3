import React from 'react';

import { Icon } from '@chakra-ui/react';

import {
  MdDashboard,
  MdPeopleAlt,
  MdBadge,
  MdCategory,
  MdDirectionsCarFilled,
  MdShoppingCartCheckout,
  MdAssignment,
  MdBuildCircle,
} from 'react-icons/md';

// Admin Imports
import Dashboard from 'views/admin/dashboard';
import CustomerManagement from 'views/admin/customer';
import EmployeeManagement from 'views/admin/employee';
import VehicleModelManagement from 'views/admin/vehicleModel';
import VehicleManagement from 'views/admin/vehicle';

// User Imports
import Home from 'views/user/home';
import Profile from 'views/user/profile';
import List from 'views/user/list';
import Detail from 'views/user/detail';

// Auth Imports
import SignIn from 'views/auth/signIn';
import SignUp from 'views/auth/signUp';
import CheckEmailNotice from 'views/auth/checkEmailNotice';
import ResetPassword from 'views/auth/resetPassword';
import ForgotPassword from 'views/auth/forgotPassword';
import PurchaseOrderManagement from 'views/admin/purchaseOrder';
import OrderManagement from 'views/admin/order';
import ServiceOrderManagement from 'views/admin/serviceOrder';

const routes = [
  // Admin Routes
  {
    name: 'Main Dashboard',
    layout: '/admin',
    path: '/dashboard',
    icon: <Icon as={MdDashboard} width="20px" height="20px" color="inherit" />,
    component: <Dashboard />,
  },
  {
    name: 'Customer Management',
    layout: '/admin',
    path: '/customer-management',
    icon: <Icon as={MdPeopleAlt} width="20px" height="20px" color="inherit" />,
    component: <CustomerManagement />,
  },
  {
    name: 'Employee Management',
    layout: '/admin',
    path: '/employee-management',
    icon: <Icon as={MdBadge} width="20px" height="20px" color="inherit" />,
    component: <EmployeeManagement />,
  },
  {
    name: 'Model Management',
    layout: '/admin',
    path: '/model-management',
    icon: <Icon as={MdCategory} width="20px" height="20px" color="inherit" />,
    component: <VehicleModelManagement />,
  },
  {
    name: 'Vehicle Management',
    layout: '/admin',
    path: '/vehicle-management',
    icon: (
      <Icon
        as={MdDirectionsCarFilled}
        width="20px"
        height="20px"
        color="inherit"
      />
    ),
    component: <VehicleManagement />,
  },
  {
    name: 'Purchase Management',
    layout: '/admin',
    path: '/purchase-management',
    icon: (
      <Icon
        as={MdShoppingCartCheckout}
        width="20px"
        height="20px"
        color="inherit"
      />
    ),
    component: <PurchaseOrderManagement />,
  },
  {
    name: 'Order Management',
    layout: '/admin',
    path: '/order-management',
    icon: <Icon as={MdAssignment} width="20px" height="20px" color="inherit" />,
    component: <OrderManagement />,
  },
  {
    name: 'Service Management',
    layout: '/admin',
    path: '/service-management',
    icon: (
      <Icon as={MdBuildCircle} width="20px" height="20px" color="inherit" />
    ),
    component: <ServiceOrderManagement />,
  },

  //User Routes
  {
    name: 'Home',
    layout: '/user',
    path: '/home',
    component: <Home />,
  },
  {
    name: 'Profile',
    layout: '/user',
    path: '/profile',
    component: <Profile />,
  },
  {
    name: 'List',
    layout: '/user',
    path: '/models',
    component: <List />,
  },
  {
    name: 'Detail',
    layout: '/user',
    path: '/detail',
    component: <Detail />,
  },
  {
    name: 'Detail',
    layout: '/user',
    path: '/detail/:slug',
    component: <Detail />,
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
];

export default routes;
