import axios from 'axios';

const BASE = process.env.REACT_APP_API_URL || process.env.API_URL || 'http://localhost:5010/api';

async function main() {
  const failures = [];

  const tryCall = async (name, fn) => {
    try {
      await fn();
      console.log(`[OK] ${name}`);
    } catch (e) {
      console.error(`[FAIL] ${name}:`, e.response?.status, e.response?.data || e.message);
      failures.push(name);
    }
  };

  // Optional: attempt login if env creds provided
  const { SMOKE_USER, SMOKE_PASS } = process.env;
  let token = null;
  if (SMOKE_USER && SMOKE_PASS) {
    await tryCall('auth/login', async () => {
      const res = await axios.post(`${BASE}/auth/login`, { username: SMOKE_USER, password: SMOKE_PASS });
      token = res.data?.accessToken || res.data?.token || null;
    });
  }

  const client = axios.create({ baseURL: BASE, headers: token ? { Authorization: `Bearer ${token}` } : {} });

  await tryCall('VehicleModels list', async () => {
    await client.get('/VehicleModels?pageSize=1');
  });

  if (token) {
    await tryCall('Orders list', async () => {
      await client.get('/Orders?pageSize=1');
    });
  } else {
    console.log('[SKIP] Orders list (no auth token provided)');
  }

  if (failures.length) {
    console.error(`Smoke failed: ${failures.join(', ')}`);
    process.exit(1);
  } else {
    console.log('Smoke passed');
  }
}

main();


