namespace TodoList.Data.Dto.Tarefa
{
    public class UpdateTarefa
    {
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public bool? Concluido { get; set; }
        public DateTime? DataConclusao { get; set; }
    }
}
