# Student Activities Management

## I. GIỚI THIỆU ĐỀ TÀI

### 1. Mục tiêu tổng quan

Hệ thống được xây dựng để quản lý toàn bộ hoạt động ngoại khóa của sinh viên trong trường đại học, bao gồm:

- Quản lý sự kiện và câu lạc bộ
- Đăng ký tham gia hoạt động
- Điểm danh và tính điểm rèn luyện
- Thông báo và nhắc nhở tự động
- Báo cáo và thống kê

### 2. Phạm vi của đề tài

- **Phạm vi nghiệp vụ:** Tập trung vào các luồng nghiệp vụ chính bao gồm: Đăng ký tham gia sự kiện, quản lý Câu lạc bộ (CLB), quy trình Check-in tại chỗ, và phê duyệt điểm rèn luyện.
- **Phạm vi chức năng:**
  - Phát triển giao diện thân thiện cho Sinh viên xem lịch và điểm cá nhân.
  - Xây dựng hệ thống quản trị cho BTC và Admin để điều hành và thống kê.
  - Hệ thống quản lý điểm rèn luyện theo quy trình: BTC nhập/đề xuất -> Admin duyệt -> Sinh viên thụ hưởng.
- **Ràng buộc hệ thống:** Đề tài tập trung xây dựng nền tảng quản lý nội bộ, không yêu cầu liên kết trực tiếp với cổng thông tin đào tạo thực tế của trường.

## II. BỐI CẢNH

### 1. Đặt vấn đề

- Hệ thống được thiết kế theo yêu cầu thực tế của Phòng Công tác Sinh viên (CTSV) trường Đại học Xây dựng Hà Nội (HUCE).
- Nhu cầu cấp thiết là chuyển đổi số quy trình quản lý hoạt động để thay thế các phương thức truyền thống, giúp việc theo dõi dữ liệu trở nên chính xác và tức thời hơn.

### 2. Đối tượng sử dụng

Hệ thống hướng tới ba nhóm đối tượng tương tác trực tiếp:

- **Sinh viên:** Người dùng cuối tham gia đăng ký, theo dõi lịch trình sự kiện và kết quả điểm rèn luyện cá nhân.
- **Ban Tổ chức (BTC):** Các cá nhân hoặc nhóm được phân quyền để tạo sự kiện, thực hiện xác nhận tham gia (Check-in) và cập nhật điểm cho sinh viên.
- **Quản trị viên (Admin - Phòng CTSV):** Người có quyền cao nhất để giám sát toàn diện, quản lý người dùng, phân quyền cho BTC và phê duyệt báo cáo cuối cùng.

### 3. Các Actor

#### 3.1. Sinh viên (Students)

- **Vai trò:** Là đối tượng hưởng lợi và tương tác chính với hệ thống.
- **Trách nhiệm/Quyền hạn:**
  - Cập nhật và quản lý thông tin cá nhân.
  - Tìm kiếm, xem chi tiết các hoạt động, câu lạc bộ và sự kiện đang diễn ra.
  - Thực hiện đăng ký tham gia sự kiện trực tuyến.
  - Theo dõi tiến độ tích lũy và bảng điểm rèn luyện cá nhân theo từng kỳ.

#### 3.2. Ban tổ chức / Câu lạc bộ (Organizers / Clubs)

- **Vai trò:** Là những người trực tiếp vận hành và điều phối các hoạt động cụ thể.
- **Trách nhiệm/Quyền hạn:**
  - Khởi tạo và cập nhật nội dung sự kiện, tải lên hình ảnh thumbnail.
  - Quản lý danh sách sinh viên đã đăng ký tham gia.
  - Thực hiện công tác Check-in để xác nhận sự hiện diện của sinh viên tại sự kiện.
  - Đề xuất cộng điểm rèn luyện (Training Score) cho các sinh viên đủ điều kiện.

#### 3.3. Quản trị viên (Admin - Phòng CTSV)

- **Vai trò:** Là cơ quan quản lý cấp cao nhất, chịu trách nhiệm về tính chính xác của dữ liệu toàn hệ thống.
- **Trách nhiệm/Quyền hạn:**
  - Quản lý danh mục người dùng, cấp quyền cho các tài khoản Organizer.
  - Phê duyệt cuối cùng các yêu cầu cộng điểm rèn luyện từ Ban tổ chức.
  - Quản lý thông báo chung toàn trường.
  - Kết xuất các báo cáo, thống kê về tình hình hoạt động của sinh viên phục vụ công tác đánh giá thi đua.

## III. KỲ VỌNG KHÁCH HÀNG

Việc xây dựng hệ thống không chỉ nhằm mục đích tin học hóa các thủ tục, mà còn phải đáp ứng được các kỳ vọng cốt lõi về trải nghiệm người dùng, tính chính xác và sự ổn định của dữ liệu.

### 1. Admin (Phòng CTSV)

- **Chính xác và Minh bạch:** Kỳ vọng lớn nhất là hệ thống phải loại bỏ hoàn toàn các sai sót thủ công trong việc tính điểm rèn luyện. Dữ liệu phải được ghi nhận theo thời gian thực (Real-time) và có lịch sử đối soát rõ ràng.
- **Tối ưu hóa quy trình:** Giảm bớt các bước giấy tờ rườm rà. Việc phê duyệt danh sách cộng điểm phải được thực hiện nhanh chóng chỉ qua vài thao tác click chuột thay vì rà soát trên các file Excel rời rạc.
- **Báo cáo tổng hợp:** Hệ thống cần cung cấp các biểu đồ thống kê trực quan về mức độ tham gia hoạt động của sinh viên theo từng Khoa, Lớp hoặc học kỳ để phục vụ công tác đánh giá thi đua.

### 2. Organizer (Ban tổ chức CLB/sự kiện)

- **Công cụ quản lý hiệu quả:** BTC kỳ vọng có một công cụ hỗ trợ kiểm soát số lượng sinh viên đăng ký, các CLB, sự kiện.
- **Điểm danh thông minh:** Thay vì ký tên vào danh sách giấy, BTC kỳ vọng một cơ chế xác nhận tham gia (Check-in) nhanh chóng, chính xác, giúp tiết kiệm thời gian và tránh việc gian lận điểm danh hộ.
- **Truyền thông trực tiếp:** Thông tin về sự kiện phải được tiếp cận đúng đối tượng sinh viên mục tiêu một cách nhanh nhất thông qua hệ thống thông báo tích hợp.

### 3. Student (Sinh viên sử dụng)

- **Trải nghiệm mượt mà:** Sinh viên kỳ vọng một giao diện Web/Mobile dễ sử dụng, cho phép tìm kiếm và đăng ký sự kiện.
- **Theo dõi lộ trình cá nhân:** Kỳ vọng lớn nhất của sinh viên là có thể theo dõi "Ví điểm rèn luyện" của mình bất cứ lúc nào, biết rõ mình đang thiếu bao nhiêu điểm để đạt loại Khá/Giỏi trong kỳ.
- **Thông tin kịp thời:** Nhận được các thông báo nhắc nhở trước khi sự kiện bắt đầu để sắp xếp thời gian tham gia hợp lý.
