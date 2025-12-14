# Ngrok Setup Guide

## 📋 Tóm tắt

Dự án đã được deploy lên server (sử dụng hostname `DESKTOP-QEDCEJ1` hoặc IP `192.168.102.3`) qua Jenkins. Để expose ra internet qua ngrok:

## 🚀 Cách sử dụng

### 1️⃣ **Start Ngrok Tunnel**

Chạy lệnh này để start ngrok:

```powershell
.\start-ngrok.ps1
```

Script sẽ:

- Kiểm tra app container có chạy không
- Stop ngrok cũ (nếu có)
- Start ngrok mới
- Mở dashboard tự động

### 2️⃣ **Lấy Public URL**

Sau khi start, chạy:

```powershell
.\get-ngrok-url.ps1
```

Script sẽ:

- Lấy public URL từ ngrok
- Copy URL vào clipboard
- Hiển thị các endpoints
- Hỏi có muốn mở Swagger không

### 3️⃣ **Xem Dashboard**

Mở browser (thay `DESKTOP-QEDCEJ1` bằng hostname của server):

```
http://DESKTOP-QEDCEJ1:4040
```

Hoặc dùng IP:

```
http://192.168.102.3:4040
```

## 📝 Lưu ý

### ⚠️ URL thay đổi mỗi lần restart

Free plan của ngrok sẽ tạo URL mới mỗi lần restart container.

### 🔄 Restart Ngrok

Nếu muốn URL mới:

```powershell
.\start-ngrok.ps1
```

### 📱 Các URL sau khi có public URL

Giả sử ngrok URL là: `https://abc123.ngrok-free.app`

- **Swagger:** `https://abc123.ngrok-free.app/swagger`
- **API:** `https://abc123.ngrok-free.app/api`
- **Health:** `https://abc123.ngrok-free.app/health`

### 🛑 Stop Ngrok

```powershell
ssh jenkins@DESKTOP-QEDCEJ1 "docker stop ngrok-tunnel"
# Hoặc: ssh jenkins@192.168.102.3 "docker stop ngrok-tunnel"
```

## 🔧 Troubleshooting

### Container không chạy?

```powershell
ssh jenkins@DESKTOP-QEDCEJ1 "docker ps"
# Hoặc: ssh jenkins@192.168.102.3 "docker ps"
```

### App container bị stop?

```powershell
ssh jenkins@DESKTOP-QEDCEJ1 "docker start studentactivities"
# Hoặc: ssh jenkins@192.168.102.3 "docker start studentactivities"
```

### Xem logs ngrok?

```powershell
ssh jenkins@DESKTOP-QEDCEJ1 "docker logs ngrok-tunnel"
# Hoặc: ssh jenkins@192.168.102.3 "docker logs ngrok-tunnel"
```

### Xem logs app?

```powershell
ssh jenkins@DESKTOP-QEDCEJ1 "docker logs studentactivities"
# Hoặc: ssh jenkins@192.168.102.3 "docker logs studentactivities"
```

## 📚 Tham khảo

- Ngrok Dashboard: http://192.168.102.3:4040
- Jenkins: http://192.168.102.3:8088
- Local App: http://192.168.102.3
- Docker Registry: http://192.168.102.3:5443
