import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

class CategoryService {
  static async getCategories(page = 0, size = 100) {
    try {
      const res = await ApiClient.get(ApiUrl.CATEGORIES, {
        params: { page, size },
      });
      return res.data;
    } catch (error) {
      console.error('Error fetching categories:', error);
      throw error;
    }
  }

  static async createCategory(data) {
    return (await ApiClient.post(ApiUrl.ADMIN_CATEGORIES, data)).data;
  }

  static async updateCategory(id, data) {
    return (await ApiClient.put(`${ApiUrl.ADMIN_CATEGORIES}/${id}`, data)).data;
  }

  static async deleteCategory(id) {
    return (await ApiClient.delete(`${ApiUrl.ADMIN_CATEGORIES}/${id}`)).data;
  }

  static async bulkDelete(ids) {
    return (await ApiClient.delete(ApiUrl.ADMIN_CATEGORIES, { data: { ids } })).data;
  }
}

export default CategoryService;
