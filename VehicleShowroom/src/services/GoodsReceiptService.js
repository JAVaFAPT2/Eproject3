import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constant/ApiUrl';

const GoodsReceiptService = {
  getAll: (params) => ApiClient.get(ApiUrl.GOODS_RECEIPTS.BASE, { params }),
  create: (data) => ApiClient.post(ApiUrl.GOODS_RECEIPTS.BASE, data),
  accept: (id) => ApiClient.put(ApiUrl.GOODS_RECEIPTS.ACCEPT(id)),
};

export default GoodsReceiptService;
