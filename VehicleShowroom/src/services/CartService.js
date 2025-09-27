import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

class CartService {
  static async getCart() {
    try {
      const res = await ApiClient.get(ApiUrl.CARTS);
      return res.data;
    } catch (error) {
      console.error('Error fetching cart:', error);
      throw error;
    }
  }

  static async addItem(item) {
    return (await ApiClient.post(ApiUrl.CARTS, item)).data;
  }

  static async removeItem(itemId) {
    return (await ApiClient.delete(`${ApiUrl.CARTS}/${itemId}`)).data;
  }

  static async clearCart() {
    return (await ApiClient.delete(ApiUrl.CARTS)).data;
  }
}

export default CartService;
