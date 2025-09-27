import { initializeApp } from 'firebase/app';
import { getAuth } from 'firebase/auth';

const firebaseConfig = {
  apiKey: 'AIzaSyD-OK_5AeBJygOS_fkhzexnh3i7_H_H3U4',
  authDomain: 'trendify-6c5c8.firebaseapp.com',
  projectId: 'trendify-6c5c8',
  storageBucket: 'trendify-6c5c8.firebasestorage.app',
  messagingSenderId: '557116082824',
  appId: '1:557116082824:web:9e816f97a4a13e08336d3d',
  measurementId: 'G-YC075H1BPT',
};

const app = initializeApp(firebaseConfig);

export const auth = getAuth(app);
