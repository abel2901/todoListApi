using Dapper;
using TodoList.Models;
using TodoList.Data;
using TodoList.Data.Dto.Tarefa;

namespace TodoList.Repository
{
    public class TarefaRepository : ITarefaRepository
    {
        private DbSession _db;
        public TarefaRepository(DbSession dbSession)
        {
            _db = dbSession;
        }

        public async Task<int> Adiciona(CreateTarefa tarefa)
        {
            using (var connection = _db.Connection)
            {
                string command = @"INSERT INTO public.""tarefas"" (titulo, datacriacao, descricao, concluido)
                    values (@titulo, NOW(), @descricao, false)";
                var result = await connection.ExecuteAsync(sql: command, param: tarefa);
                return result;
            }
        }

        public async Task<Tarefa> ObterTarefaPeloId(int id)
        {
            using (var connection = _db.Connection)
            {
                string query = @"SELECT titulo, datacriacao, descricao FROM public.""tarefas"" WHERE Id = @Id";
                Tarefa tarefa = await connection.QueryFirstOrDefaultAsync<Tarefa>(sql: query, param: new { id });
                return tarefa;
            }
        }

        public async Task<TarefaContainer> ObterContagemTarefasAsync()
        {
            using var connection = _db.Connection;
            
              string query = @"
                   SELECT COUNT(*) FROM public.""tarefas""; 
                   SELECT * FROM public.""tarefas"";
              ";

            var reader = await connection.QueryMultipleAsync(sql: query);

              return new TarefaContainer
              {
                  Item = (await reader.ReadAsync<int>()).FirstOrDefault(),
                  Tarefas = (await reader.ReadAsync<Tarefa>()).ToList()
              };
            
        }

        public async Task<int> Delete(int id)
        {
            using (var connection = _db.Connection)
            {
                string query = @"DELETE FROM public.""tarefas"" WHERE Id = @Id";
                var tarefa = await connection.ExecuteAsync(sql: query, param: new { id });
                return tarefa;
            }
        }

    }

}
