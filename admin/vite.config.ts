import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    host: '100.105.115.119', // This is the key!
    port: 5172, // Your current port
  },
});
