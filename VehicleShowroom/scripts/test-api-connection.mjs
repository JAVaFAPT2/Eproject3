#!/usr/bin/env node

/**
 * API Connection Test Script
 * Tests backend connectivity and CORS configuration before starting the frontend
 */

// Use built-in fetch for Node.js 18+ or fallback to node-fetch
let fetch;
try {
  // Try to use built-in fetch (Node.js 18+)
  fetch = globalThis.fetch;
  if (!fetch) {
    throw new Error('Built-in fetch not available');
  }
} catch (e) {
  // Fallback to node-fetch for older Node.js versions
  const { default: nodeFetch } = await import('node-fetch');
  fetch = nodeFetch;
}

const API_BASE_URL = process.env.REACT_APP_API_URL || 'https://eproject3.onrender.com/api';

const tests = [
  {
    name: 'Health Check',
    url: `${API_BASE_URL.replace('/api', '')}/health`,
    method: 'GET',
    expectStatus: 200,
  },
  {
    name: 'Vehicle Models (Anonymous)',
    url: `${API_BASE_URL}/VehicleModels?pageNumber=1&pageSize=5`,
    method: 'GET',
    expectStatus: 200,
    checkCors: true,
  },
  {
    name: 'Auth Login Endpoint',
    url: `${API_BASE_URL}/auth/login`,
    method: 'POST',
    expectStatus: 400, // Should return 400 for missing body, not 404
    body: {},
  },
];

async function testEndpoint(test) {
  console.log(`\n🧪 Testing: ${test.name}`);
  console.log(`   URL: ${test.url}`);
  console.log(`   Method: ${test.method}`);

  try {
    const options = {
      method: test.method,
      headers: {
        'Content-Type': 'application/json',
        'Origin': 'http://localhost:3000', // Simulate frontend origin
      },
    };

    if (test.body) {
      options.body = JSON.stringify(test.body);
    }

    const response = await fetch(test.url, options);
    
    console.log(`   Status: ${response.status} ${response.statusText}`);
    
    // Check CORS headers
    if (test.checkCors) {
      const corsOrigin = response.headers.get('Access-Control-Allow-Origin');
      const corsMethods = response.headers.get('Access-Control-Allow-Methods');
      const corsHeaders = response.headers.get('Access-Control-Allow-Headers');
      
      console.log(`   CORS Origin: ${corsOrigin || 'NOT SET'}`);
      console.log(`   CORS Methods: ${corsMethods || 'NOT SET'}`);
      console.log(`   CORS Headers: ${corsHeaders || 'NOT SET'}`);
      
      if (!corsOrigin) {
        console.log('   ❌ CORS not configured - requests from localhost will fail');
        return false;
      } else {
        console.log('   ✅ CORS configured');
      }
    }

    // Check response status
    if (response.status === test.expectStatus) {
      console.log(`   ✅ Status matches expected (${test.expectStatus})`);
    } else {
      console.log(`   ⚠️  Status ${response.status} differs from expected ${test.expectStatus}`);
    }

    // Try to parse response body for additional info
    try {
      const data = await response.text();
      if (data) {
        console.log(`   Response preview: ${data.substring(0, 100)}${data.length > 100 ? '...' : ''}`);
      }
    } catch (e) {
      // Ignore parsing errors
    }

    return true;

  } catch (error) {
    console.log(`   ❌ Network Error: ${error.message}`);
    
    if (error.code === 'ENOTFOUND') {
      console.log('   💡 Check if the backend URL is correct and accessible');
    } else if (error.code === 'ECONNREFUSED') {
      console.log('   💡 Backend server might be down or not accessible');
    }
    
    return false;
  }
}

async function runTests() {
  console.log('🚀 Starting API Connection Tests');
  console.log(`📡 API Base URL: ${API_BASE_URL}`);
  console.log('=' .repeat(60));

  let passedTests = 0;
  let totalTests = tests.length;

  for (const test of tests) {
    const passed = await testEndpoint(test);
    if (passed) passedTests++;
    
    // Small delay between tests
    await new Promise(resolve => setTimeout(resolve, 500));
  }

  console.log('\n' + '=' .repeat(60));
  console.log(`📊 Test Results: ${passedTests}/${totalTests} tests passed`);
  
  if (passedTests === totalTests) {
    console.log('✅ All tests passed! Backend is ready for frontend connection.');
    process.exit(0);
  } else {
    console.log('❌ Some tests failed. Check backend configuration and connectivity.');
    console.log('\n💡 Common issues:');
    console.log('   - Backend server not running');
    console.log('   - CORS not configured for localhost:3000');
    console.log('   - Wrong API URL in environment variables');
    console.log('   - Network connectivity issues');
    process.exit(1);
  }
}

// Handle unhandled promise rejections
process.on('unhandledRejection', (reason, promise) => {
  console.error('Unhandled Rejection at:', promise, 'reason:', reason);
  process.exit(1);
});

runTests().catch(error => {
  console.error('Test runner failed:', error);
  process.exit(1);
});
