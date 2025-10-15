// utils/CartHelper.js
import AuthService from 'services/AuthService';
import CartService from 'services/CartService';

const CART_KEY = 'guest_cart';

export const CartHelper = {
  async addItem(product) {
    const user = AuthService.getUser();

    if (user) {
      return CartService.addItem({
        productId: product.productId,
        quantity: product.quantity || 1,
        variantId: product.variantId,
      });
    }

    const stored = sessionStorage.getItem(CART_KEY);
    const cart = stored ? JSON.parse(stored) : [];

    const index = cart.findIndex(
      (i) =>
        i.productId === product.id &&
        i.color === product.color &&
        i.size === product.size,
    );
    if (index !== -1) {
      cart[index].quantity += product.quantity || 1;
    } else {
      cart.push({
        productId: product.productId,
        variantId: product.variantId,
        quantity: product.quantity || 1,
        productName: product.name,
        price: product.price,
        thumbnailUrl: product.thumbnailUrl,
        color: product.color || null,
        size: product.size || null,
      });
    }

    sessionStorage.setItem(CART_KEY, JSON.stringify(cart));
    return { local: true };
  },

  async updateQuantity(productId, color, size, quantity) {
    const user = AuthService.getUser();
    if (user) {
      return CartService.updateItem({ productId, quantity });
    }
    const stored = sessionStorage.getItem(CART_KEY);
    if (!stored) return;
    const cart = JSON.parse(stored);
    const idx = cart.findIndex(
      (i) => i.productId === productId && i.color === color && i.size === size,
    );
    if (idx !== -1) {
      cart[idx].quantity = quantity;
      sessionStorage.setItem(CART_KEY, JSON.stringify(cart));
    }
  },

  async removeItem(productId, color, size) {
    const user = AuthService.getUser();
    if (user) {
      return CartService.removeItem(productId);
    }

    const stored = sessionStorage.getItem(CART_KEY);
    if (!stored) return;
    const cart = JSON.parse(stored).filter(
      (i) =>
        !(i.productId === productId && i.color === color && i.size === size),
    );
    sessionStorage.setItem(CART_KEY, JSON.stringify(cart));
  },

  async getCart() {
    const user = AuthService.getUser();
    if (user) {
      const { data } = await CartService.getAll();
      const totalPrice = data.reduce((sum, i) => sum + i.price * i.quantity, 0);
      const totalQuantity = data.reduce((sum, i) => sum + i.quantity, 0);
      return { items: data, totalPrice, totalQuantity };
    }
    const stored = sessionStorage.getItem(CART_KEY);
    const cart = stored ? JSON.parse(stored) : [];
    const totalPrice = cart.reduce((sum, i) => sum + i.price * i.quantity, 0);
    const totalQuantity = cart.reduce((sum, i) => sum + i.quantity, 0);
    return { items: cart, totalPrice, totalQuantity };
  },

  clearGuest() {
    sessionStorage.removeItem(CART_KEY);
  },
};
