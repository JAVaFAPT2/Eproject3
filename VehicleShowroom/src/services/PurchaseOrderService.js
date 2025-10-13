import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const PurchaseOrderService = {
  create({ createdBy, totalAmount, expectedDeliveryDate }) {
    return ApiClient.post(ApiUrl.PURCHASE_ORDERS.BASE, { createdBy, totalAmount, expectedDeliveryDate })
      .then(r => r.data);
  },
  addLine(poId, { modelNumber, quantity, pricePerUnit }) {
    return ApiClient.post(ApiUrl.PURCHASE_ORDERS.LINES(poId), { modelNumber, quantity, pricePerUnit })
      .then(r => r.data);
  },
  complete(poId) {
    return ApiClient.post(ApiUrl.PURCHASE_ORDERS.COMPLETE(poId)).then(r => r.data);
  },
};

export default PurchaseOrderService;
