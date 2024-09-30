# Daily.dev Challenge

Viết code backend nhằm tạo trang tổng hợp thông tin (tương tự như trang daily.dev) từ nhiều nguồn khác nhau.

## Mô tả công việc:
- Tìm nguồn (RSS từ các báo như Báo Mới, Tuổi Trẻ, Dân Trí,...) của các tin tức.
- Lưu source vào database và hiển thị trên trang tổng hợp.
- Đăng ký nguồn tin mỗi khi mở trang, ứng dụng sẽ tìm từng nguồn và insert vào database.
- Hiển thị các tin tức dựa trên sở thích hoặc chủ đề mà người dùng chọn.

**Chú ý**: Hiện tại chỉ dừng lại ở mức độ backend và project nhỏ, chưa quan tâm đến bảo mật và phần giao diện (frontend).

## Mục tiêu:
- Tạo một nền tảng tổng hợp tin tức từ nhiều nguồn khác nhau.
- Lấy dữ liệu từ các nguồn RSS.
- Xử lý dữ liệu và lưu vào database.
- Hiển thị dữ liệu lên trang tổng hợp.
- Cho phép người dùng đăng ký theo dõi các nguồn tin yêu thích.
- Hiển thị tin tức theo chủ đề người dùng chọn.

## Công nghệ:
- SQL Server
- C#
- Các framework của C#

## Phân tích các thành phần:

### 1. Module lấy dữ liệu:
- Lấy danh sách các nguồn RSS.
- Đọc nội dung từ từng nguồn RSS.
- Xử lý dữ liệu (parse và extract thông tin cần thiết).

### 2. Module lưu trữ dữ liệu:
- Thiết kế cấu trúc database.
- Lưu trữ dữ liệu vào database.

### 3. Module logic nghiệp vụ:
- Xử lý các yêu cầu của người dùng (đăng ký, tìm kiếm).
- Lọc dữ liệu theo yêu cầu của người dùng.

### 4. Module giao tiếp với frontend:
- (Trong tương lai) Cung cấp API để frontend gọi.

## Design Database:

| Bảng               | Mô tả                                                       |
|--------------------|-------------------------------------------------------------|
| `Dim_Source`       | Chứa thông tin về các nguồn tin tức (RSS Feed)               |
| `Dim_User`         | Chứa thông tin về người dùng trên hệ thống                   |
| `Fact_News`        | Chứa thông tin về các bài viết                               |
| `Fact_Article_Interaction` | Quản lý tương tác của người dùng với bài viết (bookmark, lịch sử đọc, upvote, comment) |
| `Dim_Tag`          | Chứa các tags mà người dùng có thể theo dõi                  |
| `Dim_Category`     | Phân loại tin tức theo danh mục                              |
| `Fact_Bookmark`    | Chứa thông tin về các bài viết mà người dùng đã lưu          |
| `Fact_History`     | Chứa thông tin về lịch sử đọc bài viết của người dùng        |
| `Dim_Date`         | Phân tích các dữ liệu theo thời gian                         |
| `News_Tag`         | Mối quan hệ giữa bài viết và thẻ                             |
| `User_Source`      | Mối quan hệ giữa người dùng và nguồn tin                     |
| `User_Tag`         | Mối quan hệ giữa người dùng và thẻ                           |

## Quy trình hoạt động:

1. Khởi động ứng dụng: Đọc danh sách các nguồn RSS từ database.
2. Lấy dữ liệu:
    - Lặp qua từng nguồn RSS và gọi hàm lấy dữ liệu.
    - Xử lý dữ liệu và tạo đối tượng Article.
3. Lưu trữ dữ liệu:
    - Kiểm tra xem bài viết đã tồn tại chưa.
    - Nếu chưa, insert bài viết mới vào database.
4. Hiển thị dữ liệu:
    - Lấy dữ liệu từ database theo yêu cầu của người dùng (bài viết mới nhất, theo nguồn, theo chủ đề).
    - Trả về dữ liệu cho frontend.

## Các tính năng chính:

1. **My Feed (Trang chính)**:
   - Lấy danh sách bài viết theo sở thích người dùng.
   - Hỗ trợ phân trang và tải thêm bài viết.
   - Gợi ý bài viết từ các nguồn hoặc thẻ người dùng chưa theo dõi.

2. **Custom Feed**:
   - Người dùng thêm hoặc xóa các nguồn tin và thẻ.
   - Lưu cấu hình feed cá nhân hóa.

3. **Explore (Khám phá)**:
   - Hiển thị các bài viết nổi bật và gợi ý nguồn tin.
   - Phân loại bài viết theo danh mục.

4. **Discussions (Thảo luận)**:
   - Quản lý và tương tác với bình luận.
   - Người dùng có thể like hoặc reply bình luận.

5. **Tags**:
   - Quản lý và lọc bài viết theo thẻ.
   - Theo dõi hoặc bỏ theo dõi các thẻ.

6. **Sources**:
   - Hiển thị và quản lý danh sách các nguồn tin.
   - Người dùng có thể theo dõi hoặc bỏ theo dõi các nguồn tin.

7. **Leaderboard**:
   - Xếp hạng các bài viết và nguồn tin dựa trên số lượng tương tác.

8. **Submit a Link**:
   - Người dùng có thể gửi bài viết hoặc liên kết để quản trị viên kiểm duyệt và đăng tải.

9. **Bookmarks**:
   - Người dùng lưu và quản lý các bài viết đã bookmark.

10. **History**:
   - Quản lý lịch sử đọc bài viết của người dùng.

## Các role chính trong hệ thống:

1. **Guest (Người dùng chưa đăng nhập)**:
   - Xem danh sách bài viết mới nhất hoặc phổ biến.
   - Tìm kiếm bài viết và đăng ký tài khoản.

2. **Registered User (Người dùng đã đăng ký)**:
   - Xem và quản lý feed cá nhân.
   - Tương tác với bài viết, lưu bookmark, và quản lý lịch sử đọc.

3. **Admin (Quản trị viên)**:
   - Quản lý bài viết, người dùng, nguồn tin, thẻ, và tương tác.
   - Xử lý các báo cáo vi phạm từ người dùng.

## API Endpoints:

1. **NewsController**:
   - `GET /api/news`: Lấy danh sách bài viết.
   - `GET /api/news/{id}`: Lấy chi tiết bài viết theo ID.
   - `POST /api/news`: Tạo mới bài viết.
   - `PUT /api/news/{id}`: Cập nhật bài viết.
   - `DELETE /api/news/{id}`: Xóa bài viết.

2. **SourceController**:
   - `GET /api/source`: Lấy danh sách nguồn tin.
   - `GET /api/source/{id}`: Lấy chi tiết nguồn tin.
   - `POST /api/source`: Tạo mới nguồn tin.
   - `PUT /api/source/{id}`: Cập nhật nguồn tin.
   - `DELETE /api/source/{id}`: Xóa nguồn tin.

3. **UserController**:
   - `GET /api/user`: Lấy danh sách người dùng.
   - `GET /api/user/{id}`: Lấy chi tiết người dùng.
   - `POST /api/user`: Tạo mới người dùng.
   - `PUT /api/user/{id}`: Cập nhật người dùng.
   - `DELETE /api/user/{id}`: Xóa người dùng.

4. **TagController**:
   - `GET /api/tag`: Lấy danh sách thẻ.
   - `GET /api/tag/{id}`: Lấy chi tiết thẻ.
   - `POST /api/tag`: Tạo mới thẻ.
   - `PUT /api/tag/{id}`: Cập nhật thẻ.
   - `DELETE /api/tag/{id}`: Xóa thẻ.

5. **InteractionController**:
   - `GET /api/interaction`: Lấy danh sách tương tác.
   - `POST /api/interaction`: Tạo mới tương tác (bookmark, upvote, comment).
   - `PUT /api/interaction/{id}`: Cập nhật tương tác.
   - `DELETE /api/interaction/{id}`: Xóa tương tác.

6. **BookmarkController**:
   - `POST /api/bookmark`: Thêm bookmark mới.
   - `GET /api/bookmark/user/{userId}`: Lấy danh sách bài viết đã bookmark.
   - `DELETE /api/bookmark/{id}`: Xóa bookmark.

7. **HistoryController**:
   - `POST /api/history`: Thêm lịch sử đọc cho bài viết.
   - `GET /api/history/user/{userId}`: Lấy danh sách lịch sử đọc.

8. **CategoryController**:
   - `GET /api/category`: Lấy danh sách danh mục.
   - `POST /api/category`: Tạo danh mục mới.
   - `PUT /api/category/{id}`: Cập nhật danh mục.
   - `DELETE /api/category/{id}`: Xóa danh mục.
