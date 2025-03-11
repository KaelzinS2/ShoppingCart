using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace ShoppingCart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddProduto([FromQuery] string Name, double Preco)
        {
            using (var connection = new SqliteConnection("Data Source=./DataBase/Banco.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"insert into produto(Name, Preco) values (@Name, @Preco)";
                command.Parameters.AddWithValue("@Name", Name);
                command.Parameters.AddWithValue("@Preco", Preco);

                await command.ExecuteNonQueryAsync();

            }

            return Ok($"Produto {Name} de preco {Preco} Adicionado com sucesso!");

        }
    }
}
