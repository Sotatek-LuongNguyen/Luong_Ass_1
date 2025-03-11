1. Order Management Backend


- Orders Application: Quản lý đơn hàng và trạng thái của chúng.
- Payments Application: Xử lý thanh toán cho đơn hàng.

## 🛠 Công nghệ sử dụng

- **.NET 8**
- **Entity Framework**
- **Serilog** (Logging)
- **Docker** 

## 📌 Kiến trúc hệ thống

### 1. Orders Application
- Tạo đơn hàng với các trạng thái: `created`, `confirmed`, `delivered`, `cancelled`.
- Sau khi đơn hàng được tạo, gọi **Payments Application** để xử lý thanh toán.
- Cung cấp các API chính:
  - `POST /orders` - Tạo đơn hàng mới.
  - `POST /orders/{id}/cancel` - Hủy đơn hàng.
  - `GET /orders/{id}` - Kiểm tra trạng thái đơn hàng.

### 2. Payments Application
- Nhận yêu cầu xử lý thanh toán từ **Orders Application**.
- Trả về kết quả xác nhận (`confirmed`) hoặc từ chối (`declined`) ngẫu nhiên.
- API chính:
  - `POST /payments/process` - Xử lý thanh toán cho đơn hàng.

### 3. Luồng hoạt động

1. Người dùng gửi yêu cầu tạo đơn hàng qua **Orders Application**.
2. Đơn hàng được lưu với trạng thái `created`.
3. **Orders Application** gọi **Payments Application** để xác nhận thanh toán.
4. **Payments Application** trả về trạng thái `confirmed` hoặc `declined`:
   - Nếu `declined` → Đơn hàng bị chuyển thành `cancelled`.
   - Nếu `confirmed` → Đơn hàng chuyển thành `confirmed` và sau X giây sẽ tự động cập nhật thành `delivered`.

## 🔧 Cài đặt & Chạy ứng dụng

1. **Clone repository**:

   git clone https://github.com/yourusername/order-management-backend.git
   Cài Đặt CSDL SQL Sever và thay đổi connectionstrings cho phù hợp.
   cd order-management-backend
   