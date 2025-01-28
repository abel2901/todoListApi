using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> CriaTarefa([FromBody] Tarefa novaTarefa)
        {
            var result = await _tarefaRepository.Adiciona(novaTarefa);
            return Ok(result);
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

        [HttpGet("connection-string")]
        public IActionResult GetConnectionString()
        {
            // Obter a ConnectionString do appsettings ou variáveis de ambiente
            string connectionString = _configuration.GetConnectionString("TarefaConnection");

            // Retornar a ConnectionString (apenas para teste, remova ou proteja em produção)
            return Ok(new { ConnectionString = connectionString });
        }

    }
}
