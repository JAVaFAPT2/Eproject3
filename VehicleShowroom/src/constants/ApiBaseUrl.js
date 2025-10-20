// Centralized API base URL for both ApiClient and ApiUrl
// Trim whitespace and strip trailing slashes to avoid malformed URLs like "api%20/route" or double slashes
const rawBaseUrl = (process.env.REACT_APP_API_URL || 'https://eproject3.onrender.com/api').trim();
export const BASE_URL = rawBaseUrl.replace(/\/+$/, '');

