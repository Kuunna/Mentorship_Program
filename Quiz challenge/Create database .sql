CREATE TABLE Quiz (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,           -- Tiêu đề của quiz
    CreatedAt DATETIME DEFAULT GETDATE(),    -- Thời gian tạo quiz
    [Description] NVARCHAR(MAX),                -- Mô tả về quiz
    TimeLimit INT                             -- Thời gian giới hạn cho quiz (phút)
);

CREATE TABLE Question (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    Content NVARCHAR(MAX) NOT NULL,          -- Nội dung câu hỏi
    [Format] NVARCHAR(50) NOT NULL,            -- Định dạng câu hỏi (HTML, Text, etc.)
    LevelId INT NOT NULL,                    -- Khóa ngoại tham chiếu đến bảng Level
    TopicId INT NOT NULL,                    -- Khóa ngoại tham chiếu đến bảng Topic
    TypeId INT NOT NULL               -- Loại câu hỏi (trắc nghiệm, tự luận, v.v.)
);

CREATE TABLE QuizQuestion (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    QuizId INT NOT NULL,                     -- Khóa ngoại tham chiếu đến bảng Quiz
    QuestionId INT NOT NULL,                 -- Khóa ngoại tham chiếu đến bảng Question
    [Order] INT NOT NULL                     -- Thứ tự của câu hỏi trong quiz
);

CREATE TABLE Answer (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    AnswerText NVARCHAR(MAX) NOT NULL,       -- Nội dung đáp án
    IsCorrect BIT NOT NULL,                   -- Đáp án có đúng hay không
    IsDynamic BIT NOT NULL,                   -- Đáp án có động không (có thể thay đổi)
    CanBeSuggested BIT NOT NULL               -- Đáp án có thể gợi ý không
);

CREATE TABLE QuestionAnswer (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    QuestionId INT NOT NULL,                 -- Khóa ngoại tham chiếu đến bảng Question
    AnswerId INT NOT NULL                     -- Khóa ngoại tham chiếu đến bảng Answer
);

CREATE TABLE Topic (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    TopicName NVARCHAR(255) NOT NULL,             -- Tên chủ đề
    ParentTopicId INT NULL                    -- Khóa ngoại tham chiếu đến chủ đề cha (nếu có)
);

CREATE TABLE [Type] (
    Id INT PRIMARY KEY IDENTITY,
    TypeName NVARCHAR(50) NOT NULL -- Loại câu hỏi (ví dụ: Trắc nghiệm, Tự luận, Điền vào chỗ trống, etc.)
);

CREATE TABLE Tag (
    Id INT PRIMARY KEY IDENTITY,
    TagName NVARCHAR(50) NOT NULL -- Tên thẻ cho quiz (ví dụ: SQL, C#, Algorithm, etc.)
);

CREATE TABLE QuizTag (
    Id INT PRIMARY KEY IDENTITY,
    QuizId INT NOT NULL,          -- Khóa ngoại đến bảng Quiz
    TagId INT NOT NULL,           -- Khóa ngoại đến bảng Tag
    FOREIGN KEY (QuizId) REFERENCES Quiz(Id),
    FOREIGN KEY (TagId) REFERENCES Tag(Id)
);


CREATE TABLE [Level] (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    LevelName NVARCHAR(50) NOT NULL,         -- Tên cấp độ (Easy, Medium, Hard)
    ScoreWeight FLOAT NOT NULL,               -- Trọng số điểm cho cấp độ này
    TimeConstraint INT NOT NULL                -- Thời gian ràng buộc cho cấp độ (phút)
);

CREATE TABLE [Role] (
    Id INT PRIMARY KEY IDENTITY(1,1), 
	RoleName NVARCHAR(50) NOT NULL
);

CREATE TABLE [User] (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    UserName NVARCHAR(50) NOT NULL,          -- Tên người dùng
    Email NVARCHAR(255) NOT NULL,            -- Địa chỉ email
    [Password] NVARCHAR(255) NOT NULL,         -- Mật khẩu
    CreatedAt DATETIME DEFAULT GETDATE(),    -- Thời gian tạo tài khoản
    UpdatedAt DATETIME DEFAULT GETDATE()     -- Thời gian cập nhật tài khoản
);

CREATE TABLE [UserRole] (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    UserId INT NOT NULL,
	RoleId INT NOT NULL
);

CREATE TABLE UserQuiz (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    UserId INT NOT NULL,                     -- Khóa ngoại tham chiếu đến bảng User
    QuizId INT NOT NULL,                     -- Khóa ngoại tham chiếu đến bảng Quiz
    CompletionTime INT NOT NULL,              -- Thời gian hoàn thành quiz (phút)
    AttemptAt DATETIME DEFAULT GETDATE(),      -- Thời gian thực hiện quiz
	Score INT NOT NULL DEFAULT 0,             -- Điểm của người dùng cho quiz
    TotalQuestions INT NOT NULL,              -- Tổng số câu hỏi trong quiz
    CorrectAnswers INT NOT NULL DEFAULT 0,    -- Số lượng câu trả lời đúng
    StartTime DATETIME DEFAULT GETDATE(),     -- Thời gian bắt đầu quiz
    EndTime DATETIME NULL;                    -- Thời gian kết thúc quiz
);

CREATE TABLE UserAnswer (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    UserQuizId INT NOT NULL,                 -- Khóa ngoại tham chiếu đến bảng UserQuiz
    QuestionId INT NOT NULL,                 -- Khóa ngoại tham chiếu đến bảng Question
    AnswerId INT NULL,                       -- Khóa ngoại tham chiếu đến bảng Answer (nếu chọn đáp án)
    [FreeText] NVARCHAR(MAX) NULL               -- Câu trả lời tự do của người dùng (nếu có)
);
