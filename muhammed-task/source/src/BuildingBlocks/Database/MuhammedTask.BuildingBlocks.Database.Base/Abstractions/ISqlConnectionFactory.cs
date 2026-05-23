using System.Data;

namespace MuhammedTask.BuildingBlocks.Database.Base.Abstractions;
public interface ISqlConnectionFactory
{
    IDbConnection Create();
}
