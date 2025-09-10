# 1. Aşama: Derleme (Build Stage)
FROM node:20 AS build

# Çalışma dizinini belirliyoruz
WORKDIR /app

# package.json ve package-lock.json dosyalarını kopyalıyoruz
COPY package*.json ./

# Bağımlılıkları yüklüyoruz
RUN npm install

# Proje dosyalarını kopyalıyoruz
COPY . .

# Angular projesini production modunda derliyoruz
RUN npm run build -- --configuration production

# 2. Aşama: Nginx ile Servis (Serve Stage)
FROM nginx:alpine

# Nginx konfigürasyon dosyasını kopyalıyoruz
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Derlenmiş Angular uygulamasını Nginx'in varsayılan dizinine kopyalıyoruz
# angular.json'daki outputPath'e göre dist/TextboxMailAppUI olduğunu varsayıyoruz
COPY --from=build /app/dist/TextboxMailAppUI/browser /usr/share/nginx/html

# Port 80'i açığa çıkarıyoruz
EXPOSE 80

# Nginx'i başlatıyoruz
CMD ["nginx", "-g", "daemon off;"]