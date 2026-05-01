using System.Data;
using Dapper;
using NFCEPS_API.Repository.Interfaces;


namespace NFCEPS_API.Repository.Implementations;

public class GenericRepository : IGenericRepository
{
    private readonly DbConnectionFactory _factory;
    private readonly ILogger<GenericRepository> _logger;

    public GenericRepository(DbConnectionFactory factory, ILogger<GenericRepository> logger)
    {
        _factory = factory;
        _logger = logger;
    }
    
    //multiple rows, single or multiple tables
    public async Task<T> GetFromMultipleQueriesAsync<T>(string storedProcedure,
        Func<SqlMapper.GridReader, Task<T>> map, object? parameters = null)
    {
        try
        {
            using IDbConnection db = _factory.CreateConnection();
            using var multi = await db.QueryMultipleAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);

            return await map(multi);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetFromMultipleQueriesAsync - SP {SP}", storedProcedure);
            throw;
        }
    }
    
    //multiple rows, single table
    public async Task<IEnumerable<T>> QueryAsync<T>(string storedProcedure, object? parameters = null)
    {
        try
        {
            using IDbConnection db = _factory.CreateConnection();
            return await db.QueryAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in QueryAsync - SP {sp}", storedProcedure);
            throw;
        }
    }
    
    //single row, null if not found
    public async Task<T?> QueryFirstOrDefaultAsync<T>(string storedProcedure, object? parameters = null)
    {
        try
        {
            using IDbConnection db = _factory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in QueryFirstOrDefaultAsync - SP {SP}", storedProcedure);
            throw;
        }
    }
    
    //no return, for executions like insert, update and delete
    public async Task ExecuteAsync(string storedProcedure, object? parameters = null)
    {
        try
        {
            using IDbConnection db = _factory.CreateConnection();
            await db.ExecuteAsync(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ExeuteAsync - SP {SP}", storedProcedure);
            throw;
        }
    }
    
    //single scalar value
    public async Task<T?> ExecuteScalarAsync<T>(string storedProcedure, object? parameters = null)
    {
        try
        {
            using IDbConnection db = _factory.CreateConnection();
            return await db.ExecuteScalarAsync<T>(
                storedProcedure,
                parameters,
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ExecuteScalarAsync sp - {SP}", storedProcedure);
            throw;
        }
    }
}