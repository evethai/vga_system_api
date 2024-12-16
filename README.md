# VGA - Career Guidance Application for High School Students  

## 🌟 Giới thiệu  
**VGA** là ứng dụng hướng nghiệp dành cho học sinh trung học, giúp học sinh xác định các ngành nghề phù hợp thông qua các bài kiểm tra tính cách và sở thích như MBTI và Holland Code.  
Đây là phần **backend** của ứng dụng, được viết bằng **C#** và triển khai trên Azure và VPS.  

---

## 🧩 Tính năng chính của API  
1. **Bài kiểm tra MBTI**  
   - Xử lý logic bài kiểm tra với 50-80 câu hỏi.  
   - Tính toán và trả về kết quả một trong 16 loại tính cách MBTI.  

2. **Bài kiểm tra Holland Code**  
   - Phân tích 60-120 câu hỏi để trả về mã Holland Code gồm 3 chữ cái.  

3. **Lưu trữ và quản lý kết quả**  
   - API cho phép lưu trữ, truy vấn, xem lại và xóa kết quả bài kiểm tra.  

4. **Caching và hiệu năng**  
   - Redis được sử dụng để caching dữ liệu, cải thiện tốc độ xử lý.  

---

## 🛠 Công nghệ sử dụng  
- **Ngôn ngữ lập trình**:  
  - C# (.NET 8.0).  
- **Cơ sở dữ liệu**:  
  - SQL Server.  
- **Caching**:  
  - Redis.  
- **Hosting**:  
  - Azure (môi trường sản xuất).  
  - VPS (môi trường thử nghiệm).  
- **Tài liệu API**:  
  - Swagger.  

---

## ⚙️ Cài đặt và triển khai  

### Yêu cầu hệ thống  
- .NET 8.0+  
- SQL Server 2019+  
- Redis  
