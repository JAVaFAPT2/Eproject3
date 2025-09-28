const BASE_URL =
  process.env.REACT_APP_API_BASE_URL || 'http://localhost:8080/api';

export const ApiUrl = {
  // 🔐 Auth
  AUTH: {
    LOGIN: `${BASE_URL}/auth/login`,
    FORGOT_PASSWORD: `${BASE_URL}/auth/forgot-password`,
    RESET_PASSWORD: `${BASE_URL}/auth/reset-password`,
    REFRESH_TOKEN: `${BASE_URL}/auth/refresh-token`,
    REVOKE_TOKEN: `${BASE_URL}/auth/revoke-token`,
  },

  // 👤 Profile
  PROFILE: {
    GET: `${BASE_URL}/profile`,
    UPDATE: `${BASE_URL}/profile`,
    CHANGE_PASSWORD: `${BASE_URL}/profile/change-password`,
  },

  // 👥 Employees
  EMPLOYEES: {
    BASE: `${BASE_URL}/employees`,
    BY_ID: (id) => `${BASE_URL}/employees/${id}`,
    PROFILE: `${BASE_URL}/employees/profile`,
  },

  // 👥 Customers
  CUSTOMERS: {
    BASE: `${BASE_URL}/customers`,
    ORDERS: (id) => `${BASE_URL}/customers/${id}/orders`,
  },

  // 🚗 Vehicles
  VEHICLES: {
    BASE: `${BASE_URL}/vehicles`,
    BY_ID: (id) => `${BASE_URL}/vehicles/${id}`,
    DELETE_MANY: `${BASE_URL}/vehicles`,
  },

  // 📦 Orders
  ORDERS: {
    BASE: `${BASE_URL}/orders`,
    BY_ID: (id) => `${BASE_URL}/orders/${id}`,
    PRINT: (id) => `${BASE_URL}/orders/${id}/print`,
  },

  // 📦 Purchase Orders
  PURCHASE_ORDERS: {
    BASE: `${BASE_URL}/purchase-orders`,
    BY_ID: (id) => `${BASE_URL}/purchase-orders/${id}`,
    APPROVE: (id) => `${BASE_URL}/purchase-orders/${id}/approve`,
  },

  // 📥 Goods Receipts
  GOODS_RECEIPTS: {
    BASE: `${BASE_URL}/goods-receipts`,
    ACCEPT: (id) => `${BASE_URL}/goods-receipts/${id}/accept`,
  },

  // 🚗 Vehicle Registrations
  VEHICLE_REGISTRATIONS: {
    BASE: `${BASE_URL}/vehicle-registrations`,
  },

  // 🔧 Service Orders
  SERVICE_ORDERS: {
    BASE: `${BASE_URL}/service-orders`,
    START: (id) => `${BASE_URL}/service-orders/${id}/start`,
  },

  // 🖼️ Image Upload
  IMAGES: {
    UPLOAD_VEHICLE: (id) => `${BASE_URL}/images/upload/vehicle/${id}`,
  },

  // 📊 Reports
  REPORTS: {
    STOCK: `${BASE_URL}/reports/stock-availability`,
    CUSTOMER: `${BASE_URL}/reports/customer-info`,
    VEHICLE_MASTER: `${BASE_URL}/reports/vehicle-master`,
    ALLOTMENT: `${BASE_URL}/reports/allotment-details`,
    WAITING_LIST: `${BASE_URL}/reports/waiting-list`,
  },

  // 📈 Excel Exports
  EXPORTS: {
    STOCK: `${BASE_URL}/reports/export/stock-availability`,
    CUSTOMER: `${BASE_URL}/reports/export/customer-info`,
    VEHICLE_MASTER: `${BASE_URL}/reports/export/vehicle-master`,
    ALLOTMENT: `${BASE_URL}/reports/export/allotment-details`,
    WAITING_LIST: `${BASE_URL}/reports/export/waiting-list`,
  },

  // 🔄 Returns
  RETURNS: {
    BASE: `${BASE_URL}/returns`,
  },

  // 📊 Dashboard
  DASHBOARD: {
    REVENUE: `${BASE_URL}/dashboard/revenue`,
    CUSTOMER: `${BASE_URL}/dashboard/customer`,
    TOP_VEHICLES: `${BASE_URL}/dashboard/top-vehicles`,
    RECENT_ORDERS: `${BASE_URL}/dashboard/recent-orders`,
  },
};

export default ApiUrl;
