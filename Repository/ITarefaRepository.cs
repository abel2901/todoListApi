using TodoList.Models;

namespace TodoList.Repository
{
    public interface ITarefaRepository
    {
        Task <int> Adiciona(Tarefa tarefa);
        //List<Tarefa> GetTarefas(); 
        Task <Tarefa> ObterTarefaPeloId(int id);
        Task<TarefaContainer> ObterContagemTarefasAsync();

        //int Edit(Tarefa tarefa);
        //int Delete(int id);
    }
}
