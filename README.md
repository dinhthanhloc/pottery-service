# Pottery Service

API `.NET 6` cho bai test fresher backend voi chu de so hoa he thong quan ly cua hang gom su.

Project trien khai theo huong 4 layer:
- `Domain`
- `Application`
- `Infrastructure`
- `Api`

## Cong nghe su dung

- `.NET 6`
- `ASP.NET Core Web API`
- `Entity Framework Core 6`
- `PostgreSQL`
- `Swagger`
- `GitHub Actions`

## Cau truc thu muc

```text
src
|-- PotteryService.Domain
|   |-- Common
|   `-- Entities
|-- PotteryService.Application
|   |-- Common
|   |-- Features
|   `-- DependencyInjection.cs
|-- PotteryService.Infrastructure
|   |-- Persistence
|   |-- Repositories
|   `-- DependencyInjection.cs
`-- PotteryService.Api
    |-- Controllers
    |-- Middleware
    `-- Program.cs
```

## Mo ta database

He thong su dung 5 bang chinh va 1 bang luu lich su gia:

- `categories`: luu danh muc san pham nhu am tra, bat, dia, coc.
- `products`: luu thong tin san pham, gia hien tai, trang thai hoat dong.
- `product_price_histories`: luu lich su thay doi gia cua san pham.
- `sales`: luu thong tin hoa don ban hang.
- `sale_items`: luu chi tiet tung san pham trong hoa don.

Quan he chinh:
- `categories` 1-n `products`
- `products` 1-n `product_price_histories`
- `sales` 1-n `sale_items`
- `products` 1-n `sale_items`

Luu y nghiep vu:
- `products.current_price` la gia hien tai cua san pham.
- `sale_items.unit_price` la gia tai thoi diem ban.
- Doanh thu lich su duoc tinh tu `sale_items`, nen van dung ngay ca khi gia san pham thay doi ve sau.

## File SQL trong repo

- [pottery-postgesql.sql](/d:/CODE/pottery-service/pottery-postgesql.sql): script tao database va schema PostgreSQL.
- [pottery-data.sql](/d:/CODE/pottery-service/pottery-data.sql): du lieu mau de test nghiep vu va report.

## Cach chay project

### 1. Tao database va schema

Chay script schema:

```bash
psql -U postgres -f pottery-postgesql.sql
```

Sau do ket noi vao database `test-pv` va import du lieu mau:

```bash
psql -U postgres -d test-pv -f pottery-data.sql
```

### 2. Cau hinh connection string

File `appsettings*.json` dang duoc ignore khoi Git, nen can tu tao file local:

`src/PotteryService.Api/appsettings.json`

Vi du:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=test-pv;Username=your_username;Password=your_password"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Hoac co the dung bien moi truong:

```powershell
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=test-pv;Username=your_username;Password=your_password"
```

### 3. Chay API

```bash
dotnet restore
dotnet build PotteryService.sln
dotnet run --project src/PotteryService.Api
```
