using System.Data.SqlClient;

namespace QuizChallenge.Repositories
{
    public class LevelRepository
    {
        private readonly string _connectionString;

        public LevelRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public float GetScoreWeightByLevelName(string levelName)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT ScoreWeight FROM Level WHERE LevelName = @LevelName", connection);
            command.Parameters.AddWithValue("@LevelName", levelName);
            connection.Open();

            var scoreWeight = command.ExecuteScalar();
            return scoreWeight != null ? Convert.ToSingle(scoreWeight) : 0f;
        }

        public float GetScoreWeightByLevelId(int levelId)
        {
            using var connection = new SqlConnection(_connectionString);
            var command = new SqlCommand("SELECT ScoreWeight FROM Level WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", levelId);
            connection.Open();

            var scoreWeight = command.ExecuteScalar();
            return scoreWeight != null ? Convert.ToSingle(scoreWeight) : 0f;
        }
    }

}
