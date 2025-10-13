import { documentOutputs } from '../mockData/documentOutputs.js';
import { simulateDelay } from './utils.js';

const DocumentOutputService = {
  getAll: () => simulateDelay(documentOutputs),

  generate: (data) => {
    const newDoc = {
      documentId: `d${documentOutputs.length + 1}`,
      ...data,
      createdAt: new Date().toISOString(),
    };
    documentOutputs.push(newDoc);
    return simulateDelay({ message: 'Document generated successfully', data: newDoc });
  },
};

export default DocumentOutputService;
