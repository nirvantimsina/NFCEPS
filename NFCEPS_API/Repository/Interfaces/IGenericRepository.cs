using Dapper;

namespace NFCEPS_API.Repository.Interfaces;

public interface IGenericRepository
{
    //for returning mltiple rows, single or multiple tables
    Task<T?> GetFromMultipleQueriesAsync<T>(string storedProcedure, Func<SqlMapper.GridReader, Task<T>> map,
        object? parameters = null);
    
    //multiple rows, single table
    Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object? parameters = null);
    
    //single row and null if not found
    Task<T?> QueryFirstOrDefaultAsync<T>(string storedProcedure, object? parameters = null);
    
    // no return, just used for inserts, updates and deletes
    Task ExecuteAsync(string storedProcedure, object? parameters = null);
    
    //for returning single scalar values
    Task<T?> ExecuteScalarAsync<T>(string storedProcedure, object? parameters = null);
}