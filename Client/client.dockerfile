# Stage 1: Build the Angular application
FROM node:20.13.1-alpine AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build --prod

# Stage 2: Serve the application with Nginx
FROM nginx:alpine
COPY --from=build /app/dist/client /usr/share/nginx/html
COPY ssl/localhost.pem /etc/ssl/certs/localhost.pem
COPY ssl/localhost-key.pem /etc/ssl/private/localhost-key.pem

# Copy Nginx configuration
COPY nginx.conf /etc/nginx/nginx.conf

# Expose the port Nginx will run on
EXPOSE 4200

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]