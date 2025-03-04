using Microsoft.AspNetCore.Mvc;
using TodoList.Data.Dto.Tarefa;
using TodoList.Models;
using TodoList.Repository;

namespace TodoList.Controllers
{
    [Route("api/[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly ITarefaRepository _tarefaRepository;
        private readonly IConfiguration _configuration;

        public TarefaController(ITarefaRepository tarefaRepository, IConfiguration configuration)
        {
            _tarefaRepository = tarefaRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("CriaTarefa")]
        public async Task<IActionResult> CriaTarefa([FromBody]CreateTarefa novaTarefa)
        {
            if (novaTarefa != null)
            {
                var result = await _tarefaRepository.Adiciona(novaTarefa);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("ObterTarefaPorId")]
        public async Task<IActionResult> GetTarefaPeloId( int id) 
        {
            var tarefa = await _tarefaRepository.ObterTarefaPeloId(id);
            if(tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        [HttpGet]
        [Route("ObterContagemDasTarefas")]
        public async Task<IActionResult> GetContagemDeTodasTarefas()
        {
            var tarefas = await _tarefaRepository.ObterContagemTarefasAsync();
            if (tarefas == null) return NotFound();
            return Ok(tarefas);
        }

        [HttpDelete]
        [Route("DeletarTarefa")]
        public async Task<IActionResult> DeletaTarefa(int id)
        {
            var tarefa = await _tarefaRepository.Delete(id);
            if (tarefa == null) return NotFound();
            return Ok(tarefa);
        }

        [HttpPatch]
        [Route("EditaTarefa")]
        public async Task<IActionResult> EditarTarefa(int id, [FromBody] UpdateTarefa tarefa)
        {
            if (id <= 0) return BadRequest("ID inválido");
            try
            {
                var resultado = await _tarefaRepository.Edit(id, tarefa);
                if (resultado > 0)
                    return Ok("Tarefa atualizada com sucesso.");

                return NotFound("Tarefa não encontrada");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        //[HttpGet("connection-string")]
        //public IActionResult GetConnectionString()
        //{
        //    // Obter a ConnectionString do appsettings ou variáveis de ambiente
        //    string connectionString = _configuration.GetConnectionString("TarefaConnection");

        //    // Retornar a ConnectionString (apenas para teste, remova ou proteja em produção)
        //    return Ok(new { ConnectionString = connectionString });
        //}

    }
}
