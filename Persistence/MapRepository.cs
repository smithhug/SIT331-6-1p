// using Npgsql;
// using robot_controller_api.Models;

// namespace robot_controller_api.Persistence
// {
//     public class MapRepository : IMapDataAccess, IRepository
//     {
//         private IRepository _repo => this;

//         public void DeleteMap(int id)
//         {
//             var sql = "DELETE FROM map WHERE Id = @Id";
//             var sqlParams = new NpgsqlParameter[]
//             {
//                 new NpgsqlParameter("@Id", id)
//             };
//             _repo.ExecuteReader<Map>(sql, sqlParams); // ExecuteReader used instead
//         }

//         public void InsertMap(Map newMap)
//         {
//             var sql = "INSERT INTO map (\"Name\", Description, Columns, Rows, CreatedDate, ModifiedDate) " +
//                       "VALUES (@Name, @Description, @Columns, @Rows, @CreatedDate, @ModifiedDate)";
//             var sqlParams = new NpgsqlParameter[]
//             {
//                 new NpgsqlParameter("@Name", newMap.Name),
//                 new NpgsqlParameter("@Description", newMap.Description ?? (object)DBNull.Value),
//                 new NpgsqlParameter("@Columns", newMap.Columns),
//                 new NpgsqlParameter("@Rows", newMap.Rows),
//                 new NpgsqlParameter("@CreatedDate", newMap.CreatedDate),
//                 new NpgsqlParameter("@ModifiedDate", newMap.ModifiedDate)
//             };
//             _repo.ExecuteReader<Map>(sql, sqlParams); // ExecuteReader used instead
//         }

//         public List<Map> GetMaps()
//         {
//             var sql = "SELECT * FROM map";
//             return _repo.ExecuteReader<Map>(sql);
//         }

//         public void UpdateMap(Map updatedMap)
//         {
//             var sql = "UPDATE map SET \"Name\" = @Name, Description = @Description, " +
//                       "Columns = @Columns, Rows = @Rows, CreatedDate = @CreatedDate, " +
//                       "ModifiedDate = @ModifiedDate WHERE Id = @Id";
//             var sqlParams = new NpgsqlParameter[]
//             {
//                 new NpgsqlParameter("@Id", updatedMap.Id),
//                 new NpgsqlParameter("@Name", updatedMap.Name),
//                 new NpgsqlParameter("@Description", updatedMap.Description ?? (object)DBNull.Value),
//                 new NpgsqlParameter("@Columns", updatedMap.Columns),
//                 new NpgsqlParameter("@Rows", updatedMap.Rows),
//                 new NpgsqlParameter("@CreatedDate", updatedMap.CreatedDate),
//                 new NpgsqlParameter("@ModifiedDate", updatedMap.ModifiedDate)
//             };
//             _repo.ExecuteReader<Map>(sql, sqlParams); // ExecuteReader used instead
//         }
//     }
// }
