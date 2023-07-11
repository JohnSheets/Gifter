using Gifter.Models;
using Gifter.Utils;
using Gifter.Models;
using Gifter.Utils;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;

namespace Gifter.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public List<UserProfile> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT up.Id as UserProfileId, up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
                       up.ImageUrl AS UserProfileImageUrl
                    FROM UserProfile up
                    ORDER BY up.DateCreated";

                    var reader = cmd.ExecuteReader();

                    var userProfiles = new List<UserProfile>();
                    while (reader.Read())
                    {
                        userProfiles.Add(new UserProfile()
                        {
                            Id = DbUtils.GetInt(reader, "UserProfileId"),
                            Name = DbUtils.GetString(reader, "Name"),
                            Email = DbUtils.GetString(reader, "Email"),
                            DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                            Bio = DbUtils.GetString(reader, "Bio"),
                            ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                        });
                    }

                    reader.Close();

                    return userProfiles;
                }

            }
        }
        public UserProfile GetById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT up.Name, up.Bio, up.Email, up.DateCreated AS UserProfileDateCreated, 
                       up.ImageUrl AS UserProfileImageUrl
                        FROM UserProfile up
                        WHERE up.Id = @id";


                    DbUtils.AddParameter(cmd, "@id", id);

                    var reader = cmd.ExecuteReader();

                    UserProfile userProfile = null;
                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = id,
                            Name = DbUtils.GetString(reader, "Name"),
                            Bio = DbUtils.GetString(reader, "Bio"),
                            Email = DbUtils.GetString(reader, "Email"),
                            DateCreated = DbUtils.GetDateTime(reader, "UserProfileDateCreated"),
                            ImageUrl = DbUtils.GetString(reader, "UserProfileImageUrl"),
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
        public void Add(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO UserProfile (Name, Bio, Email, DateCreated, ImageUrl)
                        OUTPUT INSERTED.ID
                        VALUES (@Name, @Bio, @Email, @DateCreated, @ImageUrl)";

                    DbUtils.AddParameter(cmd, "@Name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@Bio", userProfile.Bio);
                    DbUtils.AddParameter(cmd, "@Email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@DateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", userProfile.ImageUrl);

                    userProfile.Id = (int)cmd.ExecuteScalar();
                }
            }
        }
        public void Update(UserProfile userProfile)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE UserProfile
                        SET Name = @Name,
                            Bio = @Bio,
                            Email = @Email,
                            DateCreated = @DateCreated,
                            ImageUrl = @ImageUrl
                        WHERE Id = @id";

                    DbUtils.AddParameter(cmd, "@Name", userProfile.Name);
                    DbUtils.AddParameter(cmd, "@Bio", userProfile.Bio);
                    DbUtils.AddParameter(cmd, "@Email", userProfile.Email);
                    DbUtils.AddParameter(cmd, "@DateCreated", userProfile.DateCreated);
                    DbUtils.AddParameter(cmd, "@ImageUrl", userProfile.ImageUrl);
                    DbUtils.AddParameter(cmd, "@id", userProfile.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void Delete(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM UserProfile WHERE Id = @id";
                    DbUtils.AddParameter(cmd, "@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public UserProfile GetUserProfileByIdWithPosts(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT u.Id AS UId, u.[Name], u.Email, u.ImageUrl AS UImage, u.DateCreated AS UDate, p.Id AS PId, p.Title, p.ImageUrl AS PImage, p.Caption, p.UserProfileId, p.DateCreated AS PDAte From UserProfile u 
                        LEFT JOIN Post p ON p.UserProfileId = u.Id               
                        WHERE u.Id = @id";

                    DbUtils.AddParameter(cmd, "@id", id);

                    var reader = cmd.ExecuteReader();

                    UserProfile userProfile = null;
                    while (reader.Read())
                    {
                        if (userProfile == null)
                        {
                            userProfile = new UserProfile()
                            {
                                Id = DbUtils.GetInt(reader, "UId"),
                                Name = DbUtils.GetString(reader, "Name"),
                                Email = DbUtils.GetString(reader, "Email"),
                                ImageUrl = DbUtils.GetString(reader, "UImage"),
                                DateCreated = DbUtils.GetDateTime(reader, "UDate"),
                                Posts = new List<Post>()
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("PId")))
                        {
                            Post post = new Post
                            {
                                Id = DbUtils.GetInt(reader, "PId"),
                                Title = DbUtils.GetString(reader, "Title"),
                                ImageUrl = DbUtils.GetString(reader, "PImage"),
                                Caption = DbUtils.GetString(reader, "Caption"),
                                UserProfileId = DbUtils.GetInt(reader, "UserProfileId"),
                                DateCreated = DbUtils.GetDateTime(reader, "PDate"),

                            };
                            userProfile.Posts.Add(post);
                        }

                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }

    }
}