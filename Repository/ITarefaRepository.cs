using TodoList.Data.Dto.Tarefa;
using TodoList.Models;

namespace TodoList.Repository
{
    public interface ITarefaRepository
    {
        Task <int> Adiciona(CreateTarefa tarefa);
        //List<Tarefa> GetTarefas(); 
        Task <Tarefa> ObterTarefaPeloId(int id);
        Task<TarefaContainer> ObterContagemTarefasAsync();

        Task<int> Edit(int id, UpdateTarefa tarefa);
        Task<int> Delete(int id);
    }
}
