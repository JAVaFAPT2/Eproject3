import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const BillingDocumentService = {
  create(payload) {
    // { orderId, createdBy, amount, appointmentDate }
    return ApiClient.post(ApiUrl.BILLING_DOCUMENTS.BASE, payload).then(
      (r) => r.data,
    );
  },
};

export default BillingDocumentService;
