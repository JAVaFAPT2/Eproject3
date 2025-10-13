import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const DocumentOutputService = {
  generate({ entityType, entityId, fileType }) {
    return ApiClient.post(ApiUrl.DOCUMENT_OUTPUTS.GENERATE, { entityType, entityId, fileType })
      .then(r => r.data);
  },
};

export default DocumentOutputService;
