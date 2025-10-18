import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const ProfileService = {
  // 🟢 Lấy thông tin hồ sơ người dùng hiện tại
  get: async () => {
    const res = await ApiClient.get(ApiUrl.PROFILE.BASE);
    return res.data;
  },

  // 🟡 Cập nhật hồ sơ người dùng
  update: async (data) => {
    /**
     * data format:
     * {
     *   firstName: string,
     *   lastName: string,
     *   email: string,
     *   phone: string
     * }
     */
    const res = await ApiClient.put(ApiUrl.PROFILE.BASE, data);
    return res.data;
  },

  // 🔴 Đổi mật khẩu
  changePassword: async (data) => {
    /**
     * data format:
     * {
     *   currentPassword: string,
     *   newPassword: string
     * }
     */
    const res = await ApiClient.post(ApiUrl.PROFILE.CHANGE_PASSWORD, data);
    return res.data;
  },
};

export default ProfileService;
