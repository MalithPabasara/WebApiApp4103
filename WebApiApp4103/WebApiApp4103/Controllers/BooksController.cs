using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApiApp4103.Models;
using WebApiApp4103.Util;

namespace WebApiApp4103.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly DBConnection db = new DBConnection();

        [HttpGet("get-all")]
        public IActionResult GetAll()
        {
            var list = new List<Books>();
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Books";
            using var cmd = new SqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Books
                {
                    BookID = Convert.ToInt32(reader["BookID"]),
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    ISBN = reader["ISBN"].ToString(),
                    YearPublished = (int)reader["YearPublished"],
                    Publisher = reader["Publisher"].ToString(),
                    Category = reader["Category"].ToString()
                });
            }


            return Ok(list);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetById(int id)
        {
            Books emp = null;
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "SELECT * FROM Books WHERE BookID = @id"; // Fixed here
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                emp = new Books
                {
                    BookID = Convert.ToInt32(reader["BookID"]),
                    Title = reader["Title"].ToString(),
                    Author = reader["Author"].ToString(),
                    ISBN = reader["ISBN"].ToString(),
                    YearPublished = (int)reader["YearPublished"],
                    Publisher = reader["Publisher"].ToString(),
                    Category = reader["Category"].ToString()
                };
            }

            db.ConClose();
            return emp == null ? NotFound("Books not found") : Ok(emp);
        }

        [HttpPost("add")]
        public IActionResult AddBooks([FromBody] Books emp)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "INSERT INTO Books (Title, Author, ISBN, YearPublished, Publisher, Category) " +
                         "VALUES (@Title, @Author, @ISBN, @YearPublished, @Publisher, @Category)";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Title", emp.Title);
            cmd.Parameters.AddWithValue("@Author", emp.Author);
            cmd.Parameters.AddWithValue("@ISBN", emp.ISBN);
            cmd.Parameters.AddWithValue("@YearPublished", emp.YearPublished);
            cmd.Parameters.AddWithValue("@Publisher", emp.Publisher);
            cmd.Parameters.AddWithValue("@Category", emp.Category);

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0 ? Ok("Books added successfully") : StatusCode(500, "Error adding Books");
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBooks(int id)
        {
            var conn = db.GetConn();
            db.ConOpen();

            string sql = "DELETE FROM Books WHERE BookID = @BookID"; // Fixed column name
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@BookID", id); // Updated parameter name

            int rows = cmd.ExecuteNonQuery();
            db.ConClose();

            return rows > 0 ? Ok("Books deleted successfully") : NotFound("Books not found");
        }

    }
}

