using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace ShoppingCart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddCart([FromQuery] string userName)
        {
            using (var connection = new SqliteConnection("Data Source=./DataBase/Banco.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"insert into Cart(UserName) values (@UserName)";
                command.Parameters.AddWithValue("@UserName", userName);

                await command.ExecuteNonQueryAsync(); 
                
            }

            return Ok($"Usuario {userName} Adicionado com Sucesso!");

        }


        [HttpPost]
        [Route("AddItem")]
        public async Task<IActionResult> Additem([FromQuery] int CartId, int ProdutoId)
        {

            using (var connection = new SqliteConnection("Data Source=./DataBase/Banco.db"))
            {
                connection.Open();
                var selectcommand = connection.CreateCommand();

                selectcommand.CommandText = $"select Quantidade from carrinho_has_produto where CartId = @CartId and ProdutoId = @ProdutoId";
                selectcommand.Parameters.AddWithValue("@CartId", CartId);
                selectcommand.Parameters.AddWithValue("@ProdutoId", ProdutoId);

                var Quantidade = 1;

                using (var reader = await selectcommand.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        await reader.ReadAsync();
                        Quantidade = reader.GetInt32(0);

                        var UpdateCommand = connection.CreateCommand();
                        UpdateCommand.CommandText = $"update carrinho_has_produto set Quantidade = @Quantidade where CartId = @CartId and ProdutoId = @ProdutoId";
                        UpdateCommand.Parameters.AddWithValue("@Quantidade", Quantidade + 1);
                        UpdateCommand.Parameters.AddWithValue("@CartId", CartId);
                        UpdateCommand.Parameters.AddWithValue("@ProdutoId", ProdutoId);

                        await UpdateCommand.ExecuteNonQueryAsync();

                        return Ok("Quantidade atualizada com sucesso!");

                    }
                }

                var command = connection.CreateCommand();
                command.CommandText = $"insert into carrinho_has_produto(CartId, ProdutoId, Quantidade) values (@CartId,@ProdutoId, @Quantidade)";
                command.Parameters.AddWithValue("@Quantidade", Quantidade);
                command.Parameters.AddWithValue("@CartId", CartId);
                command.Parameters.AddWithValue("@ProdutoId", ProdutoId);


                await command.ExecuteNonQueryAsync();

            }

            return Ok($"Produto adicionado com sucesso");

        }
    }
}
