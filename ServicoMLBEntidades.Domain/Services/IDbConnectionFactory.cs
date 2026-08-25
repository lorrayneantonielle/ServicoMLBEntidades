using System.Data.Common;

namespace ServicoMLBEntidades.Domain.Services;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}
