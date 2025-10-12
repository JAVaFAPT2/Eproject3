import ApiClient from 'api/ApiClient';
import  ApiUrl  from 'constant/ApiUrl';

const ProfileService = {
  getProfile: () => ApiClient.get(ApiUrl.PROFILE.GET),
  updateProfile: (data) => ApiClient.put(ApiUrl.PROFILE.UPDATE, data),
  changePassword: (data) => ApiClient.post(ApiUrl.PROFILE.CHANGE_PASSWORD, data),
};

export default ProfileService;
