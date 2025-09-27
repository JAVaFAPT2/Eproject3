import React from 'react';

import { Icon } from '@chakra-ui/react';
import {
  MdPerson,
  MdHome,
  MdAccountCircle,
  MdCategory,
  MdProductionQuantityLimits,
  MdNewspaper,
} from 'react-icons/md';
import { FaTicketAlt } from 'react-icons/fa';

// Admin Imports
import MainDashboard from 'views/admin/default';
import UserPage from 'views/admin/users';
import EmployeePage from 'views/admin/employees';
import CategoryPage from 'views/admin/categories';
import CouponPage from 'views/admin/coupon';
import ProductPage from 'views/admin/products';

// User Imports
import Home from 'views/user/home';
import Profile from 'views/user/profile';
import Blog from 'views/user/blog';
import BlogSearchPage from "views/user/blog/search";


// Auth Imports
import SignIn from 'views/auth/signIn';
import SignUp from 'views/auth/signUp';
import CheckEmailNotice from 'views/auth/checkEmailNotice';
import ResetPassword from 'views/auth/resetPassword';
import ForgotPassword from 'views/auth/forgotPassword';
import BlogPage from 'views/admin/blogs';
import PaymentPage from 'views/user/payment';

const routes = [
  // Admin Routes
  {
    name: 'Main Dashboard',
    layout: '/admin',
    path: '/default',
    icon: <Icon as={MdHome} width="20px" height="20px" color="inherit" />,
    component: <MainDashboard />,
    role: 'ADMIN',
  },
  {
    name: 'User Management',
    layout: '/admin',
    path: '/user-management',
    icon: (
      <Icon as={MdAccountCircle} width="20px" height="20px" color="inherit" />
    ),
    component: <UserPage />,
    role: 'ADMIN',
  },
  {
    name: 'Employee Management',
    layout: '/admin',
    path: '/employee-management',
    icon: <Icon as={MdPerson} width="20px" height="20px" color="inherit" />,
    component: <EmployeePage />,
    role: 'ADMIN',
  },
  {
    name: 'Category Management',
    layout: '/admin',
    path: '/category-management',
    icon: <Icon as={MdCategory} width="20px" height="20px" color="inherit" />,
    component: <CategoryPage />,
    role: 'ADMIN',
  },
  {
    name: 'Product Management',
    layout: '/admin',
    path: '/product-management',
    icon: <Icon as={MdProductionQuantityLimits} width="20px" height="20px" color="inherit" />,
    component: <ProductPage />,
    role: 'ADMIN',
  },
  {
    name: 'Coupon Management',
    layout: '/admin',
    path: '/coupon-management',
    icon: <Icon as={FaTicketAlt} width="20px" height="20px" color="inherit" />,
    component: <CouponPage />,
    role: 'ADMIN',
  },
  {
    name: 'Blog Management',
    layout: '/admin',
    path: '/blog-management',
    icon: <Icon as={MdNewspaper} width="20px" height="20px" color="inherit" />,
    component: <BlogPage />,
    role: 'ADMIN',
  },

  //User Routes
  {
    name: 'Home',
    layout: '/user',
    path: '/home',
    component: <Home />,
    hideInSidebar: true,
  },
  {
    name: 'Profile',
    layout: '/user',
    path: '/profile',
    icon: <Icon as={MdPerson} width="20px" height="20px" color="inherit" />,
    component: <Profile />,
  },
  {
    name:'Blog',
    layout:'/user',
    path:'/blog',
    component:<Blog/>,
    hideInSidebar: true,
  },
  { 
    name:'Blog Search',
    path: "blog/search",
    component: <BlogSearchPage />,
    layout: "/user"
  },
 { 
    name:'Payment',
    path: "Payment",
    component: <PaymentPage />,
    layout: "/user"
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
];

export default routes;
