# Multi-stage build for React frontend
FROM node:18-alpine AS build

# Set working directory
WORKDIR /app

# Copy package files
COPY VehicleShowroom/package*.json ./

# Install dependencies
RUN npm ci --only=production

# Copy source code
COPY VehicleShowroom/ .

# Build the app
RUN npm run build

# Production stage with nginx
FROM nginx:alpine

# Copy built app to nginx
COPY --from=build /app/build /usr/share/nginx/html

# Copy nginx configuration
COPY VehicleShowroom/nginx.conf /etc/nginx/conf.d/default.conf

# Expose port
EXPOSE 80

# Start nginx
CMD ["nginx", "-g", "daemon off;"]
