import ApiClient from 'api/ApiClient';
import { ApiUrl } from 'constants/ApiUrl';

const ProfileService = {
  /**
   * 👤 Get current user profile
   * GET /api/profile
   */
  async getProfile() {
    const res = await ApiClient.get(ApiUrl.PROFILE.GET);
    return res.data;
  },

  /**
   * ✏️ Update current user profile
   * PUT /api/profile
   * @param {Object} payload { firstName, lastName, email, phone, ... }
   */
  async updateProfile(payload) {
    const res = await ApiClient.put(ApiUrl.PROFILE.UPDATE, payload);
    return res.data;
  },

  /**
   * 🔑 Change current user password
   * POST /api/profile/change-password
   * @param {Object} payload { currentPassword, newPassword }
   */
  async changePassword(payload) {
    const res = await ApiClient.post(ApiUrl.PROFILE.CHANGE_PASSWORD, payload);
    return res.data;
  },
};

export default ProfileService;
