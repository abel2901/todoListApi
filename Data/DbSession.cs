using System.Data;
using System.Data.SqlClient;

namespace TodoList.Data
{
    public class DbSession : IDisposable
    {
        public IDbConnection Connection { get; }

        public DbSession(IConfiguration configuration)
        {
            Connection = new SqlConnection(configuration.GetConnectionString("TarefaConnection"));
            Connection.Open();
        }
        public void Dispose() => Connection?.Close();
        
    }
}
