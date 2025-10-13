import { billingDocuments } from '../mockData/billingDocuments.js';
import { simulateDelay } from './utils.js';

const BillingDocumentService = {
  getAll: () => simulateDelay(billingDocuments),

  create: (data) => {
    const newDoc = { ...data, billId: `b${billingDocuments.length + 1}` };
    billingDocuments.push(newDoc);
    return simulateDelay({ message: 'Billing document created successfully', data: newDoc });
  },

  updateStatus: (id, status) => {
    const b = billingDocuments.find((x) => x.billId === id);
    if (b) b.status = status;
    return simulateDelay({ message: 'Billing document updated successfully' });
  },
};

export default BillingDocumentService;
