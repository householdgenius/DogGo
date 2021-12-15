using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walk> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT Id, Date, Duration, WalkerId, DogId
                        FROM Walks
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();
                        while (reader.Read())
                        {
                            Walk walk = new Walk
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration= reader.GetInt32(reader.GetOrdinal("Duration")),
                                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            };

                            walks.Add(walk);
                        }

                        return walks;
                    }
                }
            }
        }

        public Walk GetWalksById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                          SELECT Id, Date, Duration, WalkerId, DogId
                        FROM Walks
                        WHERE Id = @id
                    ";

                    cmd.Parameters.AddWithValue("@id", id);
                    Walk walk = null;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (walk == null)
                            {
                                walk = new Walk
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                    Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                    WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                    DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                                };

                            }
                        }


                        return walk;
                    }
                }
            }
        }
        public List <Walk> GetWalksByWalkerId(int WalkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT Id, Date, Duration, WalkerId, DogId
                        FROM Walks
                        WHERE WalkerId = @walkerid";

                    cmd.Parameters.AddWithValue("@walkerid", WalkerId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Walk> walks = new List<Walk>();
                        while (reader.Read())
                        {
                           Walk walk = new Walk()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                                WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                                DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            };
                            walks.Add(walk);
                           
                        }
                        return walks;
                        
                    }
                }
            }
        }

        public void AddWalks(Walk walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Walks (Date, Duration, WalkerId, DogId)
                    OUTPUT INSERTED.ID
                    VALUES (@date, @duration, @walkerId, @dogId);
                ";

                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration);
                    cmd.Parameters.AddWithValue("@walkerId", walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);

                    int id = (int)cmd.ExecuteScalar();

                    walk.Id = id;
                }
            }
        }

        public void UpdateWalks(Walk walk)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Walks
                            SET 
                                Date = @date, 
                                Duration = @duration, 
                                WalkerId = @walkerId, 
                                DogId = @dogId, 
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@date", walk.Date);
                    cmd.Parameters.AddWithValue("@duration", walk.Duration);
                    cmd.Parameters.AddWithValue("@awalkerId",walk.WalkerId);
                    cmd.Parameters.AddWithValue("@dogId", walk.DogId);
                    cmd.Parameters.AddWithValue("@id", walk.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteWalks(int walkId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            DELETE FROM Walks
                            WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", walkId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
