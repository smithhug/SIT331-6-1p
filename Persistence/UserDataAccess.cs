using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class UserDataAccess : IUserDataAccess
    {
        private const string CONNECTION_STRING = "Host=localhost;Username=postgres;Password=password;Database=postgres";

        public List<UserModel> GetUsers()
        {
            var users = new List<UserModel>();

            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM public.user", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new UserModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                                LastName = reader.GetString(reader.GetOrdinal("lastname")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("passwordhash")),
                                Role = reader.GetString(reader.GetOrdinal("role")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("createddate")),
                                ModifiedDate = reader.GetDateTime(reader.GetOrdinal("modifieddate"))
                            };

                            users.Add(user);
                        }
                    }
                }
            }

            return users;
        }

        public void AddUser(UserModel newUser)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(
                    "INSERT INTO public.user (email, firstname, lastname, passwordhash, role, createddate, modifieddate) VALUES (@Email, @FirstName, @LastName, @PasswordHash, @Role, @CreatedDate, @ModifiedDate) RETURNING id", connection))
                {
                    command.Parameters.AddWithValue("@Email", newUser.Email);
                    command.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", newUser.LastName);
                    command.Parameters.AddWithValue("@PasswordHash", newUser.PasswordHash);
                    command.Parameters.AddWithValue("@Role", newUser.Role);
                    command.Parameters.AddWithValue("@CreatedDate", newUser.CreatedDate);
                    command.Parameters.AddWithValue("@ModifiedDate", newUser.ModifiedDate);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateUser(UserModel updatedUser)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("UPDATE public.user SET email = @Email, firstname = @FirstName, lastname = @LastName, passwordhash = @PasswordHash, role = @Role, modifieddate = @ModifiedDate WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Email", updatedUser.Email);
                    command.Parameters.AddWithValue("@FirstName", updatedUser.FirstName);
                    command.Parameters.AddWithValue("@LastName", updatedUser.LastName);
                    command.Parameters.AddWithValue("@PasswordHash", updatedUser.PasswordHash);
                    command.Parameters.AddWithValue("@Role", updatedUser.Role);
                    command.Parameters.AddWithValue("@ModifiedDate", updatedUser.ModifiedDate);
                    command.Parameters.AddWithValue("@Id", updatedUser.Id);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int id)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("DELETE FROM public.user WHERE id = @id", connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void PatchUser(UserModel patchedUser)
        {
            using (var connection = new NpgsqlConnection(CONNECTION_STRING))
            {
                connection.Open();

                using (var command = new NpgsqlCommand("UPDATE public.user SET email = @Email, passwordhash = @PasswordHash WHERE id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Email", patchedUser.Email);
                    command.Parameters.AddWithValue("@PasswordHash", patchedUser.PasswordHash);
                    command.Parameters.AddWithValue("@Id", patchedUser.Id);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
