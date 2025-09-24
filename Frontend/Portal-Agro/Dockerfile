# # 🔹 Etapa de build (compila Angular)
# FROM node:22-alpine AS build

# WORKDIR /app
# COPY package*.json ./
# RUN npm install
# COPY . .
# RUN npm run build -- --configuration production

# # 🔹 Etapa de runtime (sirve con NGINX)
# FROM nginx:alpine

# COPY nginx.conf /etc/nginx/conf.d/default.conf

# # 👇 FIX: Copiar desde la carpeta correcta
# COPY --from=build /app/dist/portal-agro /usr/share/nginx/html


# EXPOSE 80
# CMD ["nginx", "-g", "daemon off;"]
# Build
# Build Angular
FROM node:22-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build -- --configuration=production

# Runtime NGINX
FROM nginx:alpine
# Sobrescribe el default server
COPY nginx.conf /etc/nginx/conf.d/default.conf
# COPIA DESDE /browser (clave)
COPY --from=build /app/dist/portal-agro/browser /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]

