using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using VulnerableApp.Models;
using Microsoft.Extensions.Configuration;

namespace VulnerableApp.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // SQL Injection vulnerable action
        public IActionResult GetUserByName(string name)
        {
            var connectionStr = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                var command = new SqlCommand($"SELECT * FROM Users WHERE Name = '{name}'", connection);
                var reader = command.ExecuteReader();
                string userInfo = "";
                while (reader.Read())
                {
                    userInfo += $"ID: {reader["Id"]}, Name: {reader["Name"]}, Password: {reader["Password"]}<br>";
                }
                return Content(userInfo);
            }
        }

        // IDOR vulnerable action
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                return Content($"ID: {user.Id}, Name: {user.Name}, Password: {user.Password}");
            }
            return NotFound();
        }
    }
}
