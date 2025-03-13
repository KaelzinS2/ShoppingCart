using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace ShoppingCart.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CupomController : ControllerBase
    {
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddCupom([FromQuery] int QuantidadeDisponivel, int DescontoPorcentagem)
        {
            using (var connection = new SqliteConnection("Data Source=./DataBase/Banco.db"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"insert into Cupom(QuantidadeDisponivel,DescontoPorcentagem) values (@QuantidadeDisponivel,@DescontoPorcentagem)";
                command.Parameters.AddWithValue("@QuantidadeDisponivel", QuantidadeDisponivel);
                command.Parameters.AddWithValue("@DescontoPorcentagem", DescontoPorcentagem);

                await command.ExecuteNonQueryAsync();

            }

            return Ok($"Cupom adicionado {QuantidadeDisponivel} com a porcentagem {DescontoPorcentagem}.");

        }

    }
}
