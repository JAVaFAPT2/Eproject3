import React from 'react';
import { Navigate } from 'react-router-dom';
import AuthService from 'services/AuthService';

export default function PrivateRoute({ children }) {
  const token = AuthService.getAccessToken(); 

  if (!token) {
    return <Navigate to="/auth/sign-in" replace />;
  }

  return children;
}
