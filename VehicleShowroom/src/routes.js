import React from 'react';

import { Icon } from '@chakra-ui/react';

import {
  MdPerson,
  MdHome,
  MdAccountCircle,
  MdCategory,
  MdOutlineDirectionsCar,
} from 'react-icons/md';

// Admin Imports
import MainDashboard from 'views/admin/default';
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

const routes = [
  // Admin Routes
  {
    name: 'Main Dashboard',
    layout: '/admin',
    path: '/default',
    icon: <Icon as={MdHome} width="20px" height="20px" color="inherit" />,
    component: <MainDashboard />,
  },
  {
    name: 'Customer Management',
    layout: '/admin',
    path: '/customer-management',
    icon: (
      <Icon as={MdAccountCircle} width="20px" height="20px" color="inherit" />
    ),
    component: <CustomerManagement />,
  },
  {
    name: 'Employee Management',
    layout: '/admin',
    path: '/employee-management',
    icon: (
      <Icon as={MdAccountCircle} width="20px" height="20px" color="inherit" />
    ),
    component: <EmployeeManagement />,
  },
  {
    name: 'Vehicle Model Management',
    layout: '/admin',
    path: '/vehicle-model-management',
    icon: <Icon as={MdCategory} width="20px" height="20px" color="inherit" />,
    component: <VehicleModelManagement />,
  },
  {
    name: 'Vehicle Management',
    layout: '/admin',
    path: '/vehicle-management',
    icon: (
      <Icon
        as={MdOutlineDirectionsCar}
        width="20px"
        height="20px"
        color="inherit"
      />
    ),
    component: <VehicleManagement />,
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
    icon: <Icon as={MdPerson} width="20px" height="20px" color="inherit" />,
    component: <Profile />,
  },
  {
    name: 'List',
    layout: '/user',
    path: '/list',
    component: <List />,
  },
  {
    name: 'Detail',
    layout: '/user',
    path: '/detail',
    icon: <Icon as={MdPerson} width="20px" height="20px" color="inherit" />,
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
