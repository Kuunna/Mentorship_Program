﻿using System.Data.SqlClient;
using QuizChallenge.Models;

namespace QuizChallenge.Repositories
{
    public class TopicRepository
    {
        private readonly string _connectionString;

        public TopicRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddTopic(Topic topic)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("INSERT INTO Topic (TopicName) VALUES (@TopicName)", connection);
            command.Parameters.AddWithValue("@TopicName", topic.TopicName);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public Topic GetTopicById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM Topic WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Topic
                {
                    Id = (int)reader["Id"],
                    TopicName = reader["TopicName"].ToString()
                };
            }
            return null;
        }

        public List<Topic> GetAllTopics()
        {
            var topics = new List<Topic>();
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT * FROM Topic", connection);
            connection.Open();

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                topics.Add(new Topic
                {
                    Id = (int)reader["Id"],
                    TopicName = reader["TopicName"].ToString()
                });
            }
            return topics;
        }

        public void UpdateTopic(Topic topic)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("UPDATE Topic SET TopicName = @TopicName WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", topic.Id);
            command.Parameters.AddWithValue("@TopicName", topic.TopicName);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void DeleteTopic(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("DELETE FROM Topic WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}