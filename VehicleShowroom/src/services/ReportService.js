import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constant/ApiUrl';

const ReportService = {
  stock: (params) => ApiClient.get(ApiUrl.REPORTS.STOCK, { params }),
  customer: (params) => ApiClient.get(ApiUrl.REPORTS.CUSTOMER, { params }),
  vehicleMaster: (params) => ApiClient.get(ApiUrl.REPORTS.VEHICLE_MASTER, { params }),
  allotment: (params) => ApiClient.get(ApiUrl.REPORTS.ALLOTMENT, { params }),
  waitingList: (params) => ApiClient.get(ApiUrl.REPORTS.WAITING_LIST, { params }),

  exportStock: (params) => ApiClient.get(ApiUrl.EXPORTS.STOCK, { params, responseType: 'blob' }),
  exportCustomer: (params) => ApiClient.get(ApiUrl.EXPORTS.CUSTOMER, { params, responseType: 'blob' }),
  exportVehicleMaster: (params) => ApiClient.get(ApiUrl.EXPORTS.VEHICLE_MASTER, { params, responseType: 'blob' }),
  exportAllotment: (params) => ApiClient.get(ApiUrl.EXPORTS.ALLOTMENT, { params, responseType: 'blob' }),
  exportWaitingList: (params) => ApiClient.get(ApiUrl.EXPORTS.WAITING_LIST, { params, responseType: 'blob' }),
};

export default ReportService;
