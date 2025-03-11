# Git Convention - Lý Thuyết, Cách Sử Dụng và Ví Dụ

## 1. Giới Thiệu
Git Convention là tập hợp các quy tắc và chuẩn mực khi làm việc với Git, bao gồm việc đặt tên branch, viết commit message, đặt tên tag và quy trình làm việc với Pull Request (PR). Việc tuân thủ Git Convention giúp:
- **Đồng nhất:** Toàn bộ thành viên trong team đều làm việc theo cùng một chuẩn mực.
- **Dễ hiểu:** Lịch sử commit và cấu trúc branch rõ ràng giúp dễ dàng theo dõi thay đổi.
- **Bảo trì:** Hỗ trợ việc debug, review code và quản lý phiên bản hiệu quả.

## 2. Các Thành Phần Chính của Git Convention

### 2.1. Đặt Tên Branch
Thông thường, branch được chia thành các loại chính:
- **main/master:** Nhánh chính chứa code production.
- **develop:** Nhánh phát triển, nơi merge các tính năng mới.
- **feature:** Nhánh phát triển các tính năng mới.
- **bugfix:** Nhánh để sửa lỗi.
- **hotfix:** Nhánh để khắc phục lỗi khẩn cấp trên production.
- **release:** Nhánh chuẩn bị cho phiên bản phát hành.

**Ví dụ:**
- `feature/login-page`
- `bugfix/fix-login-error`
- `hotfix/critical-issue`

### 2.2. Commit Message Convention
Commit message giúp mô tả các thay đổi trong code một cách rõ ràng. Một số loại commit phổ biến:
- **feat:** Thêm tính năng mới.
- **fix:** Sửa lỗi.
- **docs:** Thay đổi tài liệu.
- **style:** Thay đổi định dạng, không ảnh hưởng logic.
- **refactor:** Cải tiến code mà không thêm tính năng hay sửa lỗi.
- **test:** Thêm/sửa test cases.
- **chore:** Công việc bảo trì, cập nhật cấu hình,...

**Ví dụ:**
```bash
git commit -m "feat: thêm chức năng đăng nhập bằng Google"
git commit -m "fix: sửa lỗi hiển thị trên mobile"
