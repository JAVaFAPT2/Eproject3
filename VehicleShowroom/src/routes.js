import React, { lazy } from 'react';
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

// Admin Imports - Lazy loaded for better performance
const Dashboard = lazy(() => import('views/admin/dashboard'));
const CustomerManagement = lazy(() => import('views/admin/customer'));
const EmployeeManagement = lazy(() => import('views/admin/employee'));
const VehicleModelManagement = lazy(() => import('views/admin/vehicleModel'));
const VehicleManagement = lazy(() => import('views/admin/vehicle'));
const PurchaseOrderManagement = lazy(() => import('views/admin/purchaseOrder'));
const OrderManagement = lazy(() => import('views/admin/order'));
const ServiceOrderManagement = lazy(() => import('views/admin/serviceOrder'));

// User Imports - Lazy loaded for better performance
const Home = lazy(() => import('views/user/home'));
const Profile = lazy(() => import('views/user/profile'));
const List = lazy(() => import('views/user/list'));
const Detail = lazy(() => import('views/user/detail'));

// Auth Imports - Lazy loaded for better performance
const SignIn = lazy(() => import('views/auth/signIn'));
const SignUp = lazy(() => import('views/auth/signUp'));
const CheckEmailNotice = lazy(() => import('views/auth/checkEmailNotice'));
const ResetPassword = lazy(() => import('views/auth/resetPassword'));
const ForgotPassword = lazy(() => import('views/auth/forgotPassword'));

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
