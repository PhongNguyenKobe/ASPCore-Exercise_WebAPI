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
