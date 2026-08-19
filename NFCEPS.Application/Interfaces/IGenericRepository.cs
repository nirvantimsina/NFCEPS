using System.Data;
using Dapper;

namespace NFCEPS.Application.Interfaces;

public interface IGenericRepository
{
    // For returning multiple rows, single or multiple tables
    Task<T?> GetFromMultipleQueriesAsync<T>(
        string storedProcedure,
        Func<SqlMapper.GridReader, Task<T>> map,
        object? parameters = null,
        CommandType commandType = CommandType.StoredProcedure);
    
    // Multiple rows, single table
    Task<IEnumerable<T>> QueryAsync<T>(
        string storedProcedure,
        object? parameters = null,
        CommandType commandType = CommandType.StoredProcedure);
    
    // Single row and null if not found
    Task<T?> QueryFirstOrDefaultAsync<T>(
        string storedProcedure,
        object? parameters = null,
        CommandType commandType = CommandType.StoredProcedure);
    
    // No return, just used for inserts, updates and deletes
    Task ExecuteAsync(
        string storedProcedure,
        object? parameters = null,
        CommandType commandType = CommandType.StoredProcedure);
    
    // For returning single scalar values
    Task<T?> ExecuteScalarAsync<T>(
        string storedProcedure,
        object? parameters = null,
        CommandType commandType = CommandType.StoredProcedure);
}


