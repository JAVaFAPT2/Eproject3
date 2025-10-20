const BASE_URL =
  process.env.REACT_APP_API_URL || 'http://localhost:8090/api';

export const ApiUrl = {
  // 🔐 Auth
  AUTH: {
    BASE: `${BASE_URL}/auth`,
    REGISTER: `${BASE_URL}/auth/register`,
    LOGIN: `${BASE_URL}/auth/login`,
    FORGOT_PASSWORD: `${BASE_URL}/auth/forgot-password`,
    RESET_PASSWORD: `${BASE_URL}/auth/reset-password`,
    REFRESH_TOKEN: `${BASE_URL}/auth/refresh-token`,
    REVOKE_TOKEN: `${BASE_URL}/auth/revoke-token`,
  },

  // 📊 Dashboard
  DASHBOARD: {
    BASE: `${BASE_URL}/dashboard`,
    OVERVIEW: `${BASE_URL}/dashboard/overview`,
    REVENUE: `${BASE_URL}/dashboard/revenue`,
    CUSTOMER: `${BASE_URL}/dashboard/customer`,
    TOP_VEHICLES: `${BASE_URL}/dashboard/top-vehicles`,
    RECENT_ORDERS: `${BASE_URL}/dashboard/recent-orders`,
  },

  // 🛒 Orders
  ORDERS: {
    BASE: `${BASE_URL}/Orders`,
    BY_ID: (id) => `${BASE_URL}/Orders/${id}`,
    ASSIGN_VEHICLE: (id) => `${BASE_URL}/Orders/${id}/assign-vehicle`,
    STATUS: (id) => `${BASE_URL}/Orders/${id}/status`,
  },

  // 👤 Profile
  PROFILE: {
    BASE: `${BASE_URL}/profile`,
    CHANGE_PASSWORD: `${BASE_URL}/profile/change-password`,
  },

  // 📦 Purchase Orders
  PURCHASE_ORDERS: {
    BASE: `${BASE_URL}/PurchaseOrders`,
    LINES: (id) => `${BASE_URL}/PurchaseOrders/${id}/lines`,
    COMPLETE: (id) => `${BASE_URL}/PurchaseOrders/${id}/complete`,
  },

  // 📈 Reports
  REPORTS: {
    BASE: `${BASE_URL}/reports`,
    STOCK_AVAILABILITY: `${BASE_URL}/reports/stock-availability`,
    CUSTOMER_INFO: `${BASE_URL}/reports/customer-info`,
    VEHICLE_MASTER: `${BASE_URL}/reports/vehicle-master`,
    ALLOTMENT_DETAILS: `${BASE_URL}/reports/allotment-details`,
    WAITING_LIST: `${BASE_URL}/reports/waiting-list`,
    EXPORT_STOCK: `${BASE_URL}/reports/export/stock-availability`,
    EXPORT_CUSTOMER: `${BASE_URL}/reports/export/customer-info`,
    EXPORT_MASTER: `${BASE_URL}/reports/export/vehicle-master`,
    EXPORT_ALLOTMENT: `${BASE_URL}/reports/export/allotment-details`,
    EXPORT_WAITING: `${BASE_URL}/reports/export/waiting-list`,
  },

  // 🧰 Service Orders
  SERVICE_ORDERS: {
    BASE: `${BASE_URL}/ServiceOrders`,
    STATUS: (id) => `${BASE_URL}/ServiceOrders/${id}/status`,
  },

  // 👥 Users
  USERS: {
    BASE: `${BASE_URL}/Users`,
    BY_ID: (id) => `${BASE_URL}/Users/${id}`,
    PATCH: (id) => `${BASE_URL}/Users/${id}`,
    PROFILE: (id) => `${BASE_URL}/Users/${id}/profile`,
  },

  // 🚘 Vehicle Models
  VEHICLE_MODELS: {
    BASE: `${BASE_URL}/VehicleModels`,
    BY_ID: (modelNumber) => `${BASE_URL}/VehicleModels/${modelNumber}`,
    BY_MODEL: (modelNumber) => `${BASE_URL}/VehicleModels/${modelNumber}`,
    BY_SLUG: (slug) => `${BASE_URL}/VehicleModels/slug/${slug}`,
  },

  // 🖼 Vehicle Photos
  VEHICLE_PHOTOS: {
    BY_MODEL: (modelNumber) =>
      `${BASE_URL}/vehicle-models/${modelNumber}/photos`,
    UPLOAD: (modelNumber) =>
      `${BASE_URL}/vehicle-models/${modelNumber}/photos/upload`,
    BY_ID: (photoId) => `${BASE_URL}/photos/${photoId}`,
  },

  // 🚗 Vehicles
  VEHICLES: {
    BASE: `${BASE_URL}/Vehicles`,
    BY_ID: (id) => `${BASE_URL}/Vehicles/${id}`,
    STATUS: (id) => `${BASE_URL}/Vehicles/${id}/status`,
    LICENSE_PLATE: (id) => `${BASE_URL}/Vehicles/${id}/license-plate`,
  },

  // ⚙ Vehicle Specs
  VEHICLE_SPECS: {
    BY_MODEL: (modelNumber) =>
      `${BASE_URL}/vehicle-models/${modelNumber}/specs`,
    BY_ID: (specId) => `${BASE_URL}/specs/${specId}`,
  },
};

export default ApiUrl;
