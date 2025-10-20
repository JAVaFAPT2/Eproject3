import axios from 'axios';

const BASE = 'https://eproject3.onrender.com/api';

const SAMPLE_MODELS = [
  {
    name: 'Porsche 911',
    price: 120000,
    description: 'High-performance sports car',
    level: 1,
  },
  {
    name: '911 Carrera',
    price: 130000,
    description: 'Classic Porsche sports model',
    parentId: 'Porsche 911',
    level: 2,
    slug: '911-carrera',
  },
  {
    name: 'Mercedes-Benz S-Class',
    price: 110000,
    description: 'Luxury sedan',
    level: 1,
  },
  {
    name: 'S 500',
    price: 120000,
    description: 'Premium S-Class variant',
    parentId: 'Mercedes-Benz S-Class',
    level: 2,
    slug: 's-500',
  },
  {
    name: 'BMW 7 Series',
    price: 100000,
    description: 'Premium sedan',
    level: 1,
  },
];

async function seedDatabase() {
  try {
    console.log('🌱 Seeding database...\n');

    // 1. Login as admin
    console.log('1️⃣  Logging in as admin...');
    const loginRes = await axios.post(`${BASE}/auth/login`, {
      username: 'admin',
      password: 'Admin123!',
    });

    const token = loginRes.data?.accessToken || loginRes.data?.token;
    if (!token) {
      throw new Error('Login failed: no token returned');
    }
    console.log('✅ Login successful\n');

    const headers = { Authorization: `Bearer ${token}` };

    // 2. Check if models exist
    console.log('2️⃣  Checking existing vehicle models...');
    const listRes = await axios.get(`${BASE}/VehicleModels?pageSize=1`);
    const existingCount = listRes.data?.totalCount || 0;

    if (existingCount > 0) {
      console.log(`ℹ️  Database already has ${existingCount} model(s). Skipping creation.\n`);
    } else {
      console.log('3️⃣  Creating sample vehicle models...');
      let createdCount = 0;
      for (const model of SAMPLE_MODELS) {
        try {
          await axios.post(`${BASE}/VehicleModels`, model, { headers });
          createdCount++;
          console.log(`   ✅ Created: ${model.name}`);
        } catch (err) {
          console.warn(`   ⚠️  Failed to create ${model.name}:`, err.response?.data?.message || err.message);
        }
      }
      console.log(`\n✅ Created ${createdCount}/${SAMPLE_MODELS.length} models\n`);
    }

    // 3. Run vehicle models migration
    console.log('4️⃣  Running vehicle models migration...');
    const migRes = await axios.post(
      `${BASE}/migrations/vehicle-models-v2`,
      {},
      { headers }
    );
    console.log('✅ Migration result:', migRes.data, '\n');

    console.log('🎉 Database seeding completed!');
  } catch (err) {
    console.error('❌ Seeding failed:', err.response?.data || err.message);
    process.exit(1);
  }
}

seedDatabase();
