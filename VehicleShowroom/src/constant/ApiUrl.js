const BASE_URL = process.env.REACT_APP_API_URL || 'http://localhost:8080/api/';

const ApiUrl = {
  // Auth
  AUTH_REGISTER: `${BASE_URL}auth/register`,
  AUTH_LOGIN: `${BASE_URL}auth/login`,
  AUTH_SOCIAL_LOGIN: `${BASE_URL}auth/social-login`,
  AUTH_LOGOUT: `${BASE_URL}auth/logout`,
  AUTH_REFRESH: `${BASE_URL}auth/refresh`,
  AUTH_FORGOT_PASSWORD: `${BASE_URL}auth/forgot-password`,
  AUTH_RESET_PASSWORD: `${BASE_URL}auth/reset-password`,

  // Admin
  ADMIN_ROLES: `${BASE_URL}admin/roles`,
  ADMIN_USERS: `${BASE_URL}admin/users`,
  ADMIN_EMPLOYEES: `${BASE_URL}admin/employees`,
  ADMIN_CATEGORIES: `${BASE_URL}admin/categories`,
  ADMIN_COUPONS: `${BASE_URL}admin/coupons`,
  ADMIN_BLOGS: `${BASE_URL}admin/blogs`,
  ADMIN_PRODUCTS: `${BASE_URL}admin/products`,
  ADMIN_DASHBOARD_TOP_EMPLOYEES: `${BASE_URL}admin/dashboard/employees`,

  // Employee
  EMPLOYEE_CHECKIN: `${BASE_URL}employees/check-in`,
  EMPLOYEE_CHECKOUT: `${BASE_URL}employees/check-out`,

  // Profile
  PROFILE: `${BASE_URL}users/profile`,
  UPDATE_AVATAR: `${BASE_URL}users/update-avatar`,
  UPDATE_PROFILE: `${BASE_URL}users/update-profile`,
  CHANGE_PASSWORD: `${BASE_URL}users/change-password`,
  DELETE_ACCOUNT: `${BASE_URL}users/delete-account`,

  // User
  CARTS: `${BASE_URL}cart`,
  CATEGORIES: `${BASE_URL}user/categories`,
  PRODUCTS: `${BASE_URL}user/products`,
  COUPONS:  `${BASE_URL}user/coupons`,
  BLOGS: `${BASE_URL}user/blogs`,
};

export default ApiUrl;
