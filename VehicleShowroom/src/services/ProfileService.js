import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const ProfileService = {
  get() {
    return ApiClient.get(ApiUrl.PROFILE.GET).then((r) => r.data);
  },
  update(payload) {
    return ApiClient.put(ApiUrl.PROFILE.UPDATE, payload).then((r) => r.data);
  },
  changePassword({ currentPassword, newPassword }) {
    return ApiClient.post(ApiUrl.PROFILE.CHANGE_PASSWORD, {
      currentPassword,
      newPassword,
    }).then((r) => r.data);
  },
};

export default ProfileService;
