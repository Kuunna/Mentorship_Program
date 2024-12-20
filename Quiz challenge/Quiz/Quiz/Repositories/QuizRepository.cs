﻿using QuizChallenge.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace QuizChallenge.Repositories
{
    public class QuizRepository
    {
        private readonly string _connectionString;

        public QuizRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddQuiz(Quiz quiz)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Quiz (Title, CreatedAt, Description, TimeLimit) VALUES (@Title, @CreatedAt, @Description, @TimeLimit)", connection);
                command.Parameters.AddWithValue("@Title", quiz.Title);
                command.Parameters.AddWithValue("@CreatedAt", quiz.CreatedAt);
                command.Parameters.AddWithValue("@Description", quiz.Description);
                command.Parameters.AddWithValue("@TimeLimit", quiz.TimeLimit);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Quiz GetQuizById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Quiz WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Quiz
                        {
                            Id = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            Description = reader["Description"].ToString(),
                            TimeLimit = (int)reader["TimeLimit"]
                        };
                    }
                }
            }
            return null;
        }

        public List<Quiz> GetAllQuizzes()
        {
            var Quiz = new List<Quiz>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Quiz", connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Quiz.Add(new Quiz
                        {
                            Id = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            Description = reader["Description"].ToString(),
                            TimeLimit = (int)reader["TimeLimit"]
                        });
                    }
                }
            }
            return Quiz;
        }

        public void AddQuestionToQuiz(int quizId, int questionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO QuizQuestions (QuizId, QuestionId) VALUES (@QuizId, @QuestionId)", connection);
                command.Parameters.AddWithValue("@QuizId", quizId);
                command.Parameters.AddWithValue("@QuestionId", questionId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void RemoveQuestionFromQuiz(int quizId, int questionId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM QuizQuestions WHERE QuizId = @QuizId AND QuestionId = @QuestionId", connection);
                command.Parameters.AddWithValue("@QuizId", quizId);
                command.Parameters.AddWithValue("@QuestionId", questionId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateQuiz(Quiz quiz)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("UPDATE Quiz SET Title = @Title, Description = @Description, TimeLimit = @TimeLimit WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", quiz.Id);
                command.Parameters.AddWithValue("@Title", quiz.Title);
                command.Parameters.AddWithValue("@Description", quiz.Description);
                command.Parameters.AddWithValue("@TimeLimit", quiz.TimeLimit);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteQuiz(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Quiz WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
