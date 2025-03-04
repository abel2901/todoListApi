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
            const string adicionaAtividade = @"INSERT INTO public.""tarefas"" (titulo, datacriacao, descricao, concluido)
                    VALUES (@titulo, NOW(), @descricao, false)";
            try
            {
                using var connection = _db.Connection;
                return await connection.ExecuteAsync(sql: adicionaAtividade, param: tarefa);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                throw new Exception("Erro ao salvar a tarefa no banco de dados");
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

        public async Task<int> Edit(int id, UpdateTarefa tarefa)
        {
            var camposParaAtualizar = new List<string>();
            var parametros = new DynamicParameters();
            parametros.Add("Id", id);

            if(!string.IsNullOrWhiteSpace(tarefa.Titulo))
            {
                camposParaAtualizar.Add("titulo = @titulo");
                parametros.Add("titulo", tarefa.Titulo);
            }

            if(!string.IsNullOrWhiteSpace(tarefa.Descricao))
            {
                camposParaAtualizar.Add("descricao = @descricao");
                parametros.Add("descricao", tarefa.Descricao);
            }

            if(tarefa.Concluido.HasValue)
            {
                camposParaAtualizar.Add("concluido = @concluido");
                parametros.Add("concluido", tarefa.DataConclusao);
            }

            if (tarefa.DataConclusao.HasValue)
            {
                camposParaAtualizar.Add("data_concluao = @data_conclusao");
                parametros.Add("data_conclusao", tarefa.DataConclusao.Value);
            }

            if (!camposParaAtualizar.Any())
                throw new ArgumentException("Nenhum campo para atualizar foi fornecido.");

            string sql = $@"
            UPDATE public.""tarefas""
            SET {string.Join(", ", camposParaAtualizar)}
            WHERE id = @Id";

            try
            {
                return await _db.Connection.ExecuteAsync(sql, parametros);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Erro ao atualizar tarefa: {ex.Message}");
                throw new Exception("Erro ao atualizar a tarefa no banco de dados.");
            }
        }
    }

}
