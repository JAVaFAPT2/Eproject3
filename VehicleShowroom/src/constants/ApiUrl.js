// =============================
// 🌐 API URL Constants
// =============================

const BASE_URL = process.env.REACT_APP_API_URL || 'https://localhost:8091/api';

export const ApiUrl = {
  // 🔐 Authentication
  AUTH: {
    LOGIN: `${BASE_URL}/auth/login`,
    REGISTER: `${BASE_URL}/auth/register`,
    FORGOT_PASSWORD: `${BASE_URL}/auth/forgot-password`,
    RESET_PASSWORD: `${BASE_URL}/auth/reset-password`,
    REFRESH_TOKEN: `${BASE_URL}/auth/refresh-token`,
    REVOKE_TOKEN: `${BASE_URL}/auth/revoke-token`,
  },

  // 👤 Profile Management
  PROFILE: {
    GET: `${BASE_URL}/profile`,
    UPDATE: `${BASE_URL}/profile`,
    CHANGE_PASSWORD: `${BASE_URL}/profile/change-password`,
  },

  // 👥 User Management
  USERS: {
    BASE: `${BASE_URL}/users`,
    BY_ID: (id) => `${BASE_URL}/users/${id}`,
  },

  // 🚗 Vehicle Models
  VEHICLE_MODELS: {
    BASE: `${BASE_URL}/vehicle-models`,
    BY_MODEL: (modelNumber) => `${BASE_URL}/vehicle-models/${modelNumber}`,
  },

  // 🚙 Vehicles
  VEHICLES: {
    BASE: `${BASE_URL}/vehicles`,
    SEARCH: `${BASE_URL}/vehicles/search`,
    BY_ID: (id) => `${BASE_URL}/vehicles/${id}`,
    STATUS: (id) => `${BASE_URL}/vehicles/${id}/status`,
    BULK_DELETE: `${BASE_URL}/vehicles/bulk-delete`,
  },

  // 📦 Purchase Orders
  PURCHASE_ORDERS: {
    BASE: `${BASE_URL}/purchase-orders`,
    LINES: (id) => `${BASE_URL}/purchase-orders/${id}/lines`,
    COMPLETE: (id) => `${BASE_URL}/purchase-orders/${id}/complete`,
  },

  // 🛒 Orders
  ORDERS: {
    BASE: `${BASE_URL}/orders`,
    BY_ID: (id) => `${BASE_URL}/orders/${id}`,
    ASSIGN_VEHICLE: (id) => `${BASE_URL}/orders/${id}/assign-vehicle`,
    CONFIRM: (id) => `${BASE_URL}/orders/${id}/confirm`,
    COMPLETE: (id) => `${BASE_URL}/orders/${id}/complete`,
  },

  // 🔧 Service Orders
  SERVICE_ORDERS: {
    BASE: `${BASE_URL}/service-orders`,
    STATUS: (id) => `${BASE_URL}/service-orders/${id}/status`,
  },

  // 💰 Billing Documents
  BILLING_DOCUMENTS: {
    BASE: `${BASE_URL}/billing-documents`,
  },

  // 📄 Document Outputs
  DOCUMENT_OUTPUTS: {
    GENERATE: `${BASE_URL}/document-outputs/generate`,
  },

  // 📊 Dashboard / Analytics
  DASHBOARD: {
    REVENUE: `${BASE_URL}/dashboard/revenue`,
    CUSTOMER: `${BASE_URL}/dashboard/customer`,
    TOP_VEHICLES: `${BASE_URL}/dashboard/top-vehicles`,
    RECENT_ORDERS: `${BASE_URL}/dashboard/recent-orders`,
  },
};

export default ApiUrl;
