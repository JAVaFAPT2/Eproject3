const path = require('path');

module.exports = {
  webpack: {
    configure: (webpackConfig) => {
      // Optimize bundle splitting
      webpackConfig.optimization = {
        ...webpackConfig.optimization,
        splitChunks: {
          chunks: 'all',
          cacheGroups: {
            vendor: {
              test: /[\\/]node_modules[\\/]/,
              name: 'vendors',
              chunks: 'all',
              priority: 10,
            },
            chakra: {
              test: /[\\/]node_modules[\\/]@chakra-ui[\\/]/,
              name: 'chakra',
              chunks: 'all',
              priority: 20,
            },
            react: {
              test: /[\\/]node_modules[\\/](react|react-dom)[\\/]/,
              name: 'react',
              chunks: 'all',
              priority: 30,
            },
            framer: {
              test: /[\\/]node_modules[\\/]framer-motion[\\/]/,
              name: 'framer',
              chunks: 'all',
              priority: 15,
            },
            icons: {
              test: /[\\/]node_modules[\\/]react-icons[\\/]/,
              name: 'icons',
              chunks: 'all',
              priority: 15,
            },
          },
        },
      };

      // Add performance hints
      webpackConfig.performance = {
        hints: 'warning',
        maxEntrypointSize: 512000,
        maxAssetSize: 512000,
      };

      return webpackConfig;
    },
  },
  devServer: (devServerConfig) => {
    devServerConfig.proxy = {
      '/api': {
        target: 'http://localhost:5010',
        changeOrigin: true,
        secure: false,
      },
    };
    return devServerConfig;
  },
  babel: {
    plugins: [
      // Add lazy loading plugin
      ['@babel/plugin-syntax-dynamic-import'],
    ],
  },
};
