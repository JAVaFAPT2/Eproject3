// src/utils/SlugHelper.js

/**
 * 🔹 Tạo slug thân thiện SEO từ chuỗi tên (VD: "718 Cayman GT4 RS" → "718-cayman-gt4-rs")
 * Loại bỏ ký tự đặc biệt, chuyển chữ thường, thay khoảng trắng bằng "-"
 */
export const generateSlug = (text) => {
  if (!text) return '';
  return text
    .toString()
    .normalize('NFD') // loại bỏ dấu tiếng Việt
    .replace(/[\u0300-\u036f]/g, '') // xóa tổ hợp dấu
    .toLowerCase()
    .trim()
    .replace(/[^a-z0-9\s-]/g, '') // bỏ ký tự đặc biệt
    .replace(/\s+/g, '-') // thay khoảng trắng bằng -
    .replace(/-+/g, '-'); // bỏ trùng dấu -
};
