import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

class CouponService{
  static async getCoupon(page = 0, size = 20, keyword = '') {
    try {
      const res = await ApiClient.get(ApiUrl.COUPONS, {
        params: { page, size, keyword },
      });
      return res.data;
    } catch (error) {
      console.error('Error fetching coupons:', error);
      throw error;
    }
  }

  static async createCoupon(data) {
    try {
      const res = await ApiClient.post(ApiUrl.ADMIN_COUPONS, data);
      return res.data;
    } catch (error) {
      console.error('Error creating Coupon:', error);
      throw error;
    }
  }

  static async updateCoupon(id, data) {
    try {
      const res = await ApiClient.put(`${ApiUrl.ADMIN_COUPONS}/${id}`, data);
      return res.data;
    } catch (error) {
      console.error('Error updating Coupon:', error);
      throw error;
    }
  }

  static async deleteCoupon(id) {
    try {
      const res = await ApiClient.delete(`${ApiUrl.ADMIN_COUPONS}/${id}`);
      return res.data;
    } catch (error) {
      console.error('Error deleting Coupon:', error);
      throw error;
    }
  }

  static async bulkDelete(ids) {
    try {
      const res = await ApiClient.delete(ApiUrl.ADMIN_COUPONS, {
        data: { ids },
      });
      return res.data;
    } catch (error) {
      console.error('Error bulk deleting coupons:', error);
      throw error;
    }
  }
}

export default CouponService;
