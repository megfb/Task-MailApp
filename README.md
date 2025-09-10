# TextboxMailApp – Lokal Çalıştırma

Projeyi Docker kullanmadan kendi bilgisayarınızda çalıştırmak için aşağıdaki adımları takip edebilirsiniz.

## Gereksinimler
- .NET 8 SDK
- Node.js ve npm
- Angular CLI
- PostgreSQL

## 1. Backend (API) Çalıştırma
1. `TextboxMailAppAPI` klasörüne gidin:
   ```bash
   cd TextboxMailAppAPI
   ```
2. Gerekli paketleri yükleyin ve projeyi çalıştırın:
   ```bash
   dotnet restore
   dotnet run
   ```


## 2. Frontend (Angular) Çalıştırma
1. `TextboxMailAppUI` klasörüne gidin:
   ```bash
   cd TextboxMailAppUI
   ```
2. Gerekli paketleri yükleyin:
   ```bash
   npm install
   ```
3. Angular uygulamasını çalıştırmadan önce **önemli bir nokta**:  
   - `service` sınıfları içerisinde yer alan API URL’lerini **http** yerine **https** olarak değiştirin.  
   - Örnek:  
     ```typescript
     // Önceki
     const apiUrl = 'http://localhost:...';
     // Güncel
     const apiUrl = 'https://localhost:...';
     ```
4. Uygulamayı başlatın:
   ```bash
   ng serve
   ```
5. Frontend varsayılan olarak **http://localhost:4200** portunda çalışacaktır.

## 3. Veritabanı Bağlantısı
- PostgreSQL çalışıyor olmalı ve `appsettings.json` veya environment değişkenleri backend ile uyumlu olmalıdır.
- Örnek connection string:
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=taskdb;Username=postgres;Password=1234"
  }
  ```
- Update-Database komutu ile migration işlemlerini tamamlayın.



# TextboxMailApp – Docker ile Çalıştırma

## Gereksinimler
- Docker
- Docker Compose

## Projeyi Çalıştırma
Projeyi Docker ile başlatmak için terminalde proje klasöründe şu komutu kullan:

```bash
docker compose up --build
```


## Servisler ve Portlar
- **Backend**: http://localhost:7135  
- **Frontend**: http://localhost:4200  
- **PostgreSQL**: localhost:5432 (kullanıcı: `postgres`, şifre: `1234`)  

## Projeyi Durdurma
Projeyi durdurmak için:

```bash
docker compose down
```

