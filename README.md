# Pottery Service

API `.NET 6` cho bài test fresher backend với chủ đề số hóa hệ thống quản lý cửa hàng gốm sứ.

Project triển khai theo hướng 4 layer:
- `Domain`
- `Application`
- `Infrastructure`
- `Api`

## Công nghệ sử dụng

- `.NET 6`
- `ASP.NET Core Web API`
- `Entity Framework Core 6`
- `PostgreSQL`
- `Swagger`
- `GitHub Actions`

## Cấu trúc thư mục

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

## Mô tả database

Hệ thống sử dụng 5 bảng chính và 1 bảng lưu lịch sử giá:

- `categories`: lưu danh mục sản phẩm như ấm trà, bát, đĩa, cốc.
- `products`: lưu thông tin sản phẩm, giá hiện tại, trạng thái hoạt động.
- `product_price_histories`: lưu lịch sử thay đổi giá của sản phẩm.
- `sales`: lưu thông tin hóa đơn bán hàng.
- `sale_items`: lưu chi tiết từng sản phẩm trong hóa đơn.

Quan hệ chính:
- `categories` 1-n `products`
- `products` 1-n `product_price_histories`
- `sales` 1-n `sale_items`
- `products` 1-n `sale_items`

Lưu ý nghiệp vụ:
- `products.current_price` là giá hiện tại của sản phẩm.
- `sale_items.unit_price` là giá tại thời điểm bán.
- Doanh thu lịch sử được tính từ `sale_items`, nên vẫn đúng ngay cả khi giá sản phẩm thay đổi về sau.

## File SQL trong repo

- [pottery-postgesql.sql](/d:/CODE/pottery-service/pottery-postgesql.sql): script tạo database và schema PostgreSQL.
- [pottery-data.sql](/d:/CODE/pottery-service/pottery-data.sql): dữ liệu mẫu để test nghiệp vụ và report.

Ghi chú:
- File `pottery-data.sql` đã tạo sẵn dữ liệu nền cho `categories`, `products`, `product_price_histories`, `sales` và `sale_items`.
- Sau khi import dữ liệu mẫu, có thể test nhanh trực tiếp các nghiệp vụ chính như bán hàng, thống kê số lượng bán và tra cứu doanh thu theo sản phẩm mà không cần nhập tay dữ liệu ban đầu.

## Cách chạy project

### 1. Tạo database và schema

Chạy script schema:

```bash
psql -U postgres -f pottery-postgesql.sql
```

Sau đó kết nối vào database `test-pv` và import dữ liệu mẫu:

```bash
psql -U postgres -d test-pv -f pottery-data.sql
```

### 2. Cấu hình connection string

File `appsettings*.json` đang được ignore khỏi Git, nên cần tự tạo file local:

`src/PotteryService.Api/appsettings.json`

Ví dụ:

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

Hoặc có thể dùng biến môi trường:

```powershell
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=test-pv;Username=your_username;Password=your_password"
```

### 3. Chạy API

```bash
dotnet restore
dotnet build PotteryService.sln
dotnet run --project src/PotteryService.Api
```
