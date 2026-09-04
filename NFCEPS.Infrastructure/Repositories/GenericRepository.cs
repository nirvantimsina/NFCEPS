using Dapper;
using Microsoft.Extensions.Logging;
using NFCEPS.Application.Interfaces;
using System.Data;

namespace NFCEPS.Infrastructure.Repositories;

public class GenericRepository(DbConnectionFactory factory, ILogger<GenericRepository> logger) : IGenericRepository
{
    // Multiple rows, single or multiple tables
    public async Task<T?> GetFromMultipleQueriesAsync<T>(string sql,
        Func<SqlMapper.GridReader, Task<T>> map, object? parameters = null, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using IDbConnection db = factory.CreateConnection();
            using var multi = await db.QueryMultipleAsync(
                sql,
                parameters,
                commandType: commandType);

            return await map(multi);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetFromMultipleQueriesAsync - SQL {SQL}", sql);
            throw;
        }
    }

    // Multiple rows, single table
    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using IDbConnection db = factory.CreateConnection();
            return await db.QueryAsync<T>(
                sql,
                parameters,
                commandType: commandType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in QueryAsync - SQL {sql}", sql);
            throw;
        }
    }

    // Single row, null if not found
    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using IDbConnection db = factory.CreateConnection();
            return await db.QueryFirstOrDefaultAsync<T>(
                sql,
                parameters,
                commandType: commandType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in QueryFirstOrDefaultAsync - SQL {SQL}", sql);
            throw;
        }
    }

    // No return, for executions like insert, update and delete
    public async Task ExecuteAsync(string sql, object? parameters = null, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using IDbConnection db = factory.CreateConnection();
            await db.ExecuteAsync(
                sql,
                parameters,
                commandType: commandType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in ExecuteAsync - SQL {SQL}", sql);
            throw;
        }
    }

    // Single scalar value
    public async Task<T?> ExecuteScalarAsync<T>(string sql, object? parameters = null, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using IDbConnection db = factory.CreateConnection();
            return await db.ExecuteScalarAsync<T>(
                sql,
                parameters,
                commandType: commandType);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in ExecuteScalarAsync SQL - {SQL}", sql);
            throw;
        }
    }
}
