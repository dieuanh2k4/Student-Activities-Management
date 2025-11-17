# Jenkins CI/CD Setup Guide

## 📦 Custom Jenkins Image

Custom Jenkins image được định nghĩa trong `Dockerfile.jenkins` với các công cụ:

### Công cụ được cài đặt:

- ✅ **Jenkins LTS** - Phiên bản ổn định dài hạn
- ✅ **.NET 8 SDK** - Để build, test, publish .NET applications
- ✅ **Docker CLI** - Để build và chạy Docker containers
- ✅ **Docker Compose** - Để orchestrate multi-container applications
- ✅ **Git** - Version control
- ✅ **Jenkins Plugins**:
  - Git Plugin
  - Docker Pipeline
  - GitHub Integration
  - Credentials Binding
  - SSH Agent
  - Pipeline Stage View

---

## 🚀 Cách sử dụng

### 1. Build Custom Jenkins Image

```bash
# Build image
docker build -f Dockerfile.jenkins -t jenkins-dotnet:latest .

# Hoặc sử dụng docker-compose
docker-compose -f docker-compose.jenkins.yml build
```

### 2. Chạy Jenkins Container

```bash
# Sử dụng docker-compose (KHUYẾN NGHỊ)
docker-compose -f docker-compose.jenkins.yml up -d

# Hoặc chạy trực tiếp với docker run
docker run -d \
  --name jenkins-ci \
  --privileged \
  -p 8082:8080 \
  -p 50000:50000 \
  -v /var/run/docker.sock:/var/run/docker.sock \
  -v jenkins_home:/var/jenkins_home \
  jenkins-dotnet:latest
```

### 3. Truy cập Jenkins UI

- Mở browser: `http://localhost:8082`
- Lấy initial admin password:

```bash
docker exec jenkins-ci cat /var/jenkins_home/secrets/initialAdminPassword
```

### 4. Cấu hình Jenkins

1. **Install suggested plugins** hoặc chọn plugins theo nhu cầu
2. **Create First Admin User**
3. **Configure Jenkins URL**: `http://localhost:8082` (hoặc domain của bạn)

---

## ⚙️ Cấu hình Pipeline

### Tạo New Pipeline Job

1. Jenkins Dashboard → **New Item**
2. Nhập tên project: `StudentActivities-CI-CD`
3. Chọn **Pipeline** → OK
4. Trong **Pipeline** section:
   - Definition: **Pipeline script from SCM**
   - SCM: **Git**
   - Repository URL: `https://github.com/dieuanh2k4/Student-Activities-Management.git`
   - Branch: `*/main`
   - Script Path: `Jenkinsfile`
5. **Save**

### Cấu hình Credentials (nếu cần)

**Cho GitHub private repository:**

1. Dashboard → Manage Jenkins → Credentials
2. Add Credentials:
   - Kind: **Username with password**
   - Username: GitHub username
   - Password: GitHub Personal Access Token
   - ID: `github-credentials`

**Cho Docker Registry (nếu push images):**

1. Add Credentials:
   - Kind: **Username with password**
   - Username: Docker Hub username
   - Password: Docker Hub password
   - ID: `dockerhub-credentials`

---

## 🔧 Troubleshooting

### Lỗi: Docker permission denied

**Nguyên nhân:** Jenkins user không có quyền truy cập Docker socket

**Giải pháp:**

```bash
# Vào container
docker exec -it -u root jenkins-ci bash

# Kiểm tra docker group ID
stat -c '%g' /var/run/docker.sock

# Thêm jenkins user vào docker group với đúng GID
groupmod -g <DOCKER_GID> docker
usermod -aG docker jenkins

# Restart Jenkins
exit
docker restart jenkins-ci
```

### Lỗi: dotnet command not found

**Giải pháp:**

```bash
# Kiểm tra .NET đã cài đúng chưa
docker exec jenkins-ci dotnet --version

# Nếu chưa có, vào container và cài lại
docker exec -it -u root jenkins-ci bash
dotnet --version
```

### Lỗi: Cannot connect to Docker daemon

**Nguyên nhân:** Docker socket chưa được mount hoặc Docker service không chạy

**Giải pháp:**

```bash
# Trên Windows với WSL2/Docker Desktop
# Đảm bảo Docker Desktop đang chạy và expose daemon

# Trên Linux
# Kiểm tra Docker service
systemctl status docker

# Restart docker-compose
docker-compose -f docker-compose.jenkins.yml restart
```

---

## 📝 Customize Jenkins Image

### Thêm plugins khác

Sửa trong `Dockerfile.jenkins`:

```dockerfile
RUN jenkins-plugin-cli --plugins \
    git:latest \
    workflow-aggregator:latest \
    your-plugin-name:latest
```

### Thêm tools khác

```dockerfile
# Ví dụ: Cài Node.js
RUN curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs
```

### Thay đổi Java Opts

Sửa trong `docker-compose.jenkins.yml`:

```yaml
environment:
  - JAVA_OPTS=-Djenkins.install.runSetupWizard=false -Xmx2g -Xms512m
```

---

## 🎯 Best Practices

1. **Sử dụng Jenkins Shared Libraries** cho reusable pipeline code
2. **Store secrets trong Jenkins Credentials** (không hardcode trong Jenkinsfile)
3. **Sử dụng multi-stage builds** để tối ưu Docker images
4. **Enable backup** cho `/var/jenkins_home` volume
5. **Cấu hình Webhook** từ GitHub để auto-trigger builds khi có push
6. **Sử dụng parallel stages** để tăng tốc pipeline
7. **Clean workspace** sau mỗi build để tiết kiệm disk space

---

## 🔐 Security Recommendations

1. **Thay đổi default port** (8082) khi deploy production
2. **Sử dụng HTTPS** với SSL certificate
3. **Enable Matrix-based security** và tạo user riêng cho từng team
4. **Regular update Jenkins** và plugins
5. **Limit Jenkins user permissions** - không dùng root trong production
6. **Sử dụng secrets management** (HashiCorp Vault, AWS Secrets Manager)

---

## 📚 Tài liệu tham khảo

- [Jenkins Official Documentation](https://www.jenkins.io/doc/)
- [Docker in Jenkins](https://www.jenkins.io/doc/book/installing/docker/)
- [.NET on Jenkins](https://learn.microsoft.com/en-us/dotnet/core/tools/)
- [Pipeline Syntax](https://www.jenkins.io/doc/book/pipeline/syntax/)
