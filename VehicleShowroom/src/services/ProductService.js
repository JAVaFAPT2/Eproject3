// src/services/ProductService.js
import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

class ProductService {
  static async getProducts(page = 0, size = 20) {
    try {
      const res = await ApiClient.get(ApiUrl.PRODUCTS, {
        params: { page, size },
      });
      return res.data;
    } catch (error) {
      console.error('Error fetching products:', error);
      throw error;
    }
  }

  static async createProduct(data) {
    return (await ApiClient.post(ApiUrl.ADMIN_PRODUCTS, data)).data;
  }

  static async updateProduct(id, data) {
    return (await ApiClient.put(`${ApiUrl.ADMIN_PRODUCTS}/${id}`, data)).data;
  }

  static async deleteProduct(id) {
    return (await ApiClient.delete(`${ApiUrl.ADMIN_PRODUCTS}/${id}`)).data;
  }

  static async bulkDelete(ids) {
    return (await ApiClient.delete(ApiUrl.ADMIN_PRODUCTS, { data: { ids } }))
      .data;
  }
}

export default ProductService;
