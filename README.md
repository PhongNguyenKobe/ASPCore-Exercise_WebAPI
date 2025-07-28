# ASPCore-Exercise_WebAPI

# 💼 Đề tài: NIIEPay – Ứng dụng Web API Ngân hàng

## 🧾 Mô tả yêu cầu
Xây dựng ứng dụng ngân hàng NIIEPay với các chức năng sau:

- ✅ **Đăng ký tài khoản**: Lưu thông tin như số tài khoản, họ tên chủ thẻ, số điện thoại, CCCD, ngày hết hạn CCCD, số dư khả dụng,...
- 🔍 **Truy vấn thông tin tài khoản**: Hiển thị thông tin chi tiết của người dùng theo số tài khoản.
- 🔁 **Chuyển khoản**:
  - Tài khoản luôn duy trì tối thiểu **50.000 đ**
  - **Miễn phí chuyển khoản**
  - Hỗ trợ:
    - **Nội bộ** (cùng ngân hàng): qua số tài khoản hoặc số điện thoại
    - **Liên ngân hàng**: qua mã ngân hàng và số tài khoản
- 📜 **Xem lịch sử giao dịch**: Truy xuất danh sách giao dịch theo khoảng thời gian với thông tin gồm:
  - Tài khoản giao dịch
  - Số tiền
  - Thời gian
  - Số dư sau giao dịch
  - Nội dung, mã giao dịch
- 💰 **Gửi tiết kiệm có kỳ hạn**:
  - Các kỳ hạn: **1, 2, 3, 6, 9, 12, 18, 24, 36 tháng**
  - Lãi suất từ **3.5% đến 5.8%/năm** tùy kỳ hạn

---

📌 **Yêu cầu bổ sung**:  
Hãy **tự thiết kế database** đảm bảo lưu trữ đầy đủ các chức năng trên.
---

# 💳 NIIEPay – Banking Web API 

## 📘 Mô tả đề bài

Ứng dụng Web API mô phỏng hệ thống ngân hàng NIIEPay gồm **6 chức năng chính**, được phân loại theo nghiệp vụ thực tế:

---

## ✅ Chức năng chính

### 1. 🔐 Đăng ký tài khoản
- **API:** `POST /api/accounts/register`
- **Mô tả:** Người dùng tạo mới tài khoản ngân hàng với các thông tin cơ bản.
- **Ràng buộc:**
  - Số tài khoản không được trùng
  - Số dư ban đầu ≥ 100,000 VNĐ
  - CCCD còn hạn

---

### 2. 🔍 Truy vấn thông tin tài khoản
- **API:** `GET /api/accounts/{accountNumber}`
- **Mô tả:** Trả về thông tin cá nhân và số dư khả dụng của tài khoản.

---

### 3. 🔁 Chuyển khoản
- **API nội bộ:** `POST /api/transfers/internal`
- **API liên ngân hàng:** `POST /api/transfers/external`
- **Mô tả:** Thực hiện giao dịch chuyển tiền giữa các tài khoản.
- **Ràng buộc:**
  - Sau chuyển khoản, số dư phải ≥ 50,000 VNĐ
  - Không tính phí chuyển tiền

---

### 4. 📜 Lịch sử giao dịch
- **API:** `GET /api/transactions?accountNumber=...&fromDate=...&toDate=...`
- **Mô tả:** Tra cứu các giao dịch đã thực hiện trong một khoảng thời gian cụ thể.
- **Chức năng:**
  - Trả về các biến động số dư trong khoảng thời gian nhất định.
  - Bao gồm cả chiều gửi và nhận, có note, thời gian, số dư sau giao dịch.

---

### 5. 💰 Gửi tiết kiệm có kỳ hạn
- **API:** `POST /api/savings/open`
- **Mô tả:** Mở sổ tiết kiệm với lãi suất tùy theo kỳ hạn (1–36 tháng).
- **Ràng buộc:**
  - Sau khi gửi tiết kiệm, số dư vẫn ≥ 50,000 VNĐ
  - Kỳ hạn gửi phải hợp lệ

---

### 6. 📊 Xem bảng lãi suất tiết kiệm
- **API:** `GET /api/savings/rates`
- **Mô tả:** Trả về danh sách các kỳ hạn và lãi suất tương ứng (%/năm).

---

## 📊 Bảng tổng kết chức năng

| STT | Chức năng chính               | API chính                                   | Mô tả ngắn gọn                   |
| --- | ----------------------------- | ------------------------------------------- | -------------------------------- |
| 1   | Đăng ký tài khoản             | `POST /api/accounts/register`               | Tạo mới tài khoản ngân hàng      |
| 2   | Truy vấn tài khoản            | `GET /api/accounts/{accountNumber}`         | Xem thông tin tài khoản          |
| 3   | Chuyển khoản (nội bộ/liên NH) | `POST /api/transfers/internal` / `external` | Giao dịch chuyển tiền            |
| 4   | Lịch sử giao dịch             | `GET /api/transactions`                     | Xem biến động số dư              |
| 5   | Mở sổ tiết kiệm               | `POST /api/savings/open`                    | Gửi tiền kỳ hạn và tính lãi suất |
| 6   | Xem lãi suất theo kỳ hạn      | `GET /api/savings/rates`                    | Hiển thị bảng tra cứu lãi suất   |


