using Dapper;
using System.Data.SqlClient;
using TodoList.Models;
using TodoList.Data;
using TodoList.Models;

namespace TodoList.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private DbSession _db;
        public TarefaRepository(DbSession dbSession)
        {
            _db = dbSession;
        }

        public async Task<int> Adiciona(Tarefa tarefa)

        {
            using (var connection = _db.Connection)
            {
                string command = @"INSERT INTO [TODOLIST].[dbo].[TAREFAS] (Titulo, DataCriacao, Descricao)
                    VALUES (@Titulo, @DataCriacao, @Descricao)";
                var result = await connection.ExecuteAsync(sql: command, param: tarefa);
                return result;
            }
        }

        public async Task<Tarefa> ObterTarefaPeloId(int id)
        {
            using (var connection = _db.Connection)
            {
                string query = @"SELECT Titulo, DataCriacao, Descricao FROM [TODOLIST].[dbo].[TAREFAS] WHERE Id = @Id";
                Tarefa tarefa = await connection.QueryFirstOrDefaultAsync<Tarefa>(sql: query, param: new { id });
                return tarefa;
            }
        }

        public async Task<TarefaContainer> ObterContagemTarefasAsync()
        {
            using (var connection = _db.Connection)
            {
                string query = @" SELECT COUNT(*) FROM [TODOLIST].[dbo].[TAREFAS]
    	          SELECT * FROM [TODOLIST].[dbo].[TAREFAS]";

                var reader = await connection.QueryMultipleAsync(sql: query);

                return new TarefaContainer
                {
                    Item = (await reader.ReadAsync<int>()).FirstOrDefault(),
                    Tarefas = (await reader.ReadAsync<Tarefa>()).ToList()
                };
            };
        }
    }

}
