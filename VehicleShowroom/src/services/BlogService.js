import ApiClient from 'api/ApiClient';
import ApiUrl from 'constant/ApiUrl';

class BlogService {
  static async getBlogs(keyword = '', page = 0, size = 20) {
    const res = await ApiClient.get(ApiUrl.BLOGS, {
      params: { page, size, keyword },
    });
    return res.data;
  }

  static async createBlog(data) {
    const res = await ApiClient.post(ApiUrl.ADMIN_BLOGS, data);
    return res.data;
  }

  static async updateBlog(id, data) {
    const res = await ApiClient.put(`${ApiUrl.ADMIN_BLOGS}/${id}`, data);
    return res.data;
  }

  static async deleteBlog(id) {
    const res = await ApiClient.delete(`${ApiUrl.ADMIN_BLOGS}/${id}`);
    return res.data;
  }

  static async bulkDelete(ids) {
    const res = await ApiClient.delete(ApiUrl.ADMIN_BLOGS, {
      data: { ids },
    });
    return res.data;
  }
}

export default BlogService;
