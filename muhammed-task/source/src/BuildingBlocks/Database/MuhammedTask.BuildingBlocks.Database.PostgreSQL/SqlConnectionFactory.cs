using System.Data;
using MuhammedTask.BuildingBlocks.Database.Base.Abstractions;
using Npgsql;

namespace MuhammedTask.BuildingBlocks.Database.PostgreSQL;
internal sealed class SqlConnectionFactory
    (string connectionString) : ISqlConnectionFactory
{
    public IDbConnection Create() => new NpgsqlConnection(connectionString);
}
