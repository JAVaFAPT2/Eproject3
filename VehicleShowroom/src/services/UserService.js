import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

class UserService {
  static async getProfile() {
    try {
      const res = await ApiClient.get(ApiUrl.PROFILE);
      const user = res.data;

      const addressParts = user.address
        ? user.address.split(',').map((p) => p.trim())
        : [];
      return {
        ...user,
        addressStreet: addressParts[0] || '',
        addressWard: addressParts[1] || '',
        addressDistrict: addressParts[2] || '',
        addressCity: addressParts[3] || '',
      };
    } catch (err) {
      console.error('Error fetching profile:', err);
      throw err;
    }
  }

  static async updateProfile(data) {
    try {
      const address = [
        data.addressStreet,
        data.addressWard,
        data.addressDistrict,
        data.addressCity,
      ]
        .filter(Boolean)
        .join(', ');
      const payload = { ...data, address };
      delete payload.addressStreet;
      delete payload.addressWard;
      delete payload.addressDistrict;
      delete payload.addressCity;

      const res = await ApiClient.put(ApiUrl.UPDATE_PROFILE, payload);
      return res.data;
    } catch (err) {
      console.error('Error updating profile:', err);
      throw err;
    }
  }

  static async updateAvatar(file) {
  try {
    const formData = new FormData();
    formData.append("file", file);

    const res = await ApiClient.put(ApiUrl.UPDATE_AVATAR, formData, {
      headers: { "Content-Type": "multipart/form-data" },
    });
    return res.data; 
  } catch (err) {
    console.error("Error updating avatar:", err);
    throw err;
  }
}


  static async changePassword({ oldPassword, newPassword }) {
    try {
      const res = await ApiClient.post(ApiUrl.CHANGE_PASSWORD, {
        oldPassword,
        newPassword,
      });
      return res.data;
    } catch (err) {
      console.error('Error changing password:', err);
      throw err;
    }
  }

  static async deleteAccount() {
    try {
      const res = await ApiClient.delete(ApiUrl.DELETE_ACCOUNT);
      return res.data;
    } catch (err) {
      console.error('Error deleting account:', err);
      throw err;
    }
  }

  static async getUsers({ page = 0, size = 20, keyword, roleId }) {
    try {
      const response = await ApiClient.get(ApiUrl.ADMIN_USERS, {
        params: {
          page,
          size,
          keyword,
          roleId,
        },
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching users:', error);
      throw error;
    }
  }
}

export default UserService;
