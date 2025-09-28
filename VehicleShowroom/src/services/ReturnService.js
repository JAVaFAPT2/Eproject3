import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constant/ApiUrl';

const ReturnService = {
  getAll: (params) => ApiClient.get(ApiUrl.RETURNS.BASE, { params }),
};

export default ReturnService;
