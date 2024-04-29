using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class MapADO : IMapDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=postgres";

        public List<Map> GetMaps()
        {
            var maps = new List<Map>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);
            using var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                var id = dr.GetInt32(0);
                var name = dr.GetString(1);
                var description = dr.IsDBNull(2) ? null : dr.GetString(2);
                var columns = dr.GetInt32(3);
                var rows = dr.GetInt32(4);
                var createdDate = dr.GetDateTime(5);
                var modifiedDate = dr.GetDateTime(6);
                var isSquare = dr.GetBoolean(7);

                var map = new Map
                {
                    Id = id,
                    Name = name,
                    Description = description,
                    Rows = rows,
                    Columns = columns,
                    CreatedDate = createdDate,
                    ModifiedDate = modifiedDate
                };

                maps.Add(map);
            }
            return maps;
        }

        public void UpdateMap(Map map)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("UPDATE map SET \"Name\" = @Name, Description = @Description, Columns = @Columns, Rows = @Rows, CreatedDate = @CreatedDate, ModifiedDate = @ModifiedDate WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", map.Id);
            cmd.Parameters.AddWithValue("@Name", map.Name);
            cmd.Parameters.AddWithValue("@Description", map.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Columns", map.Columns);
            cmd.Parameters.AddWithValue("@Rows", map.Rows);
            cmd.Parameters.AddWithValue("@CreatedDate", map.CreatedDate);
            cmd.Parameters.AddWithValue("@ModifiedDate", map.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public void InsertMap(Map map)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("INSERT INTO map (\"Name\", Description, Columns, Rows, CreatedDate, ModifiedDate) VALUES (@Name, @Description, @Columns, @Rows, @CreatedDate, @ModifiedDate)", conn);
            cmd.Parameters.AddWithValue("@Name", map.Name);
            cmd.Parameters.AddWithValue("@Description", map.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Columns", map.Columns);
            cmd.Parameters.AddWithValue("@Rows", map.Rows);
            cmd.Parameters.AddWithValue("@CreatedDate", map.CreatedDate);
            cmd.Parameters.AddWithValue("@ModifiedDate", map.ModifiedDate);

            cmd.ExecuteNonQuery();
        }

        public void DeleteMap(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            conn.Open();

            using var cmd = new NpgsqlCommand("DELETE FROM map WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
