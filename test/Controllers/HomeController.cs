using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace api
{
    [Route("api/users")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private static UserModel UM = new UserModel(); 
        private static List<UserModel> LUM = new List<UserModel>();
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        [HttpGet("{id}")]
        public List<UserModel> Get(int id)
        {
            UserModel user = new UserModel();
            List<UserModel> LUM = new List<UserModel>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _configuration.GetConnectionString("myDb1");
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT TOP 10 Username, Email, PhoneNumber, Skillsets,Hobby FROM Users ORDER BY id DESC", conn);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException er)
                {
                    _logger.LogError(er.Message);
                    return LUM;
                }

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user.Username = reader[0].ToString() ?? reader[0].ToString();
                        user.Email = reader[1].ToString() ?? reader[0].ToString();
                        user.PhoneNumber = reader[2].ToString() ?? reader[0].ToString();
                        user.Skillsets = reader[3].ToString() ?? reader[0].ToString();
                        user.Hobby = reader[4].ToString() ?? reader[0].ToString();
                        LUM.Add(user);
                    }
                }

                return LUM;
            }
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool Post([FromBody] UserModel user)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("invalid inputs");
                return false;
            }

            // insert details to ms sql
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _configuration.GetConnectionString("myDb1"); 
                conn.Open();

                SqlCommand insertCommand = new SqlCommand("INSERT INTO Users (Username, Email, PhoneNumber, Skillsets,Hobby) VALUES(@0,@1,@2,@3,@4)", conn);
                insertCommand.Parameters.AddWithValue("@0", UM.Username);
                insertCommand.Parameters.AddWithValue("@1", UM.Email);
                insertCommand.Parameters.AddWithValue("@2", UM.PhoneNumber);
                insertCommand.Parameters.AddWithValue("@3", UM.Skillsets);
                insertCommand.Parameters.AddWithValue("@4", UM.Hobby);

                try
                {
                    insertCommand.ExecuteNonQuery();
                }
                catch (SqlException er)
                {
                    _logger.LogError(er.Message);
                    return false;
                }
            }

            return true;
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public bool Put(int id, [FromBody] UserModel updatedUser)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("invalid inputs");
                return false;
            }            

            // Update details to ms sql
            using (SqlConnection conn = new SqlConnection())
            {               
                conn.ConnectionString = _configuration.GetConnectionString("myDb1");
                conn.Open();
                
                SqlCommand insertCommand = new SqlCommand("UPDATE Users SET Username= @0, Email=@1, PhoneNumber=@2, Skillsets=@3,Hobby=@4 WHERE id=@5", conn);
                insertCommand.Parameters.AddWithValue("@0", UM.Username);
                insertCommand.Parameters.AddWithValue("@1", UM.Email);
                insertCommand.Parameters.AddWithValue("@2", UM.PhoneNumber);
                insertCommand.Parameters.AddWithValue("@3", UM.Skillsets);
                insertCommand.Parameters.AddWithValue("@4", UM.Hobby);
                insertCommand.Parameters.AddWithValue("@5", id);

                try
                {
                    insertCommand.ExecuteNonQuery();
                }
                catch (SqlException er)
                {
                    _logger.LogError(er.Message);
                    return false;
                }
            }

            return true;
        }

        [HttpDelete("{Username}")]
        public bool Delete(string Username)
        {
            // Update details to ms sql
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = _configuration.GetConnectionString("myDb1");
                conn.Open();

                SqlCommand insertCommand = new SqlCommand("DELETE Users WHERE Username=@0", conn);
                insertCommand.Parameters.AddWithValue("@0", Username);

                try
                {
                    insertCommand.ExecuteNonQuery();
                }
                catch (SqlException er)
                {
                    _logger.LogError(er.Message);
                    return false;
                }
            }
            return true;
        }
    }
}
