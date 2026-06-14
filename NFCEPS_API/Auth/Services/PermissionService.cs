using NFCEPS_API.Repository.Interfaces;

namespace NFCEPS_API.Services.Permission;

public class PermissionService(IServiceScopeFactory scopeFactory, ILogger<PermissionService> logger)
{
    private Dictionary<int, HashSet<string>> _cache = new();
    public async Task LoadAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<IGenericRepository>();

        var rows = await repo.QueryAsync<RolePermissionRow>("permission.sp_getallrolepermissions");

        var newCache = new Dictionary<int, HashSet<string>>();

        foreach (var row in rows)
        {
            if(!newCache.ContainsKey(row.RoleId))newCache[row.RoleId] = [];

            newCache[row.RoleId].Add(row.PermKey);
        }

        _cache = newCache;
        logger.LogInformation("Permission Cache Loaded - {Count} roles", newCache.Count);
    }

    public bool Has(int roleId, string permKey) => _cache.TryGetValue(roleId, out var perms) && perms.Contains(permKey);

    public HashSet<string> GetAll(int roleId) => _cache.TryGetValue(roleId, out var perms) ? perms : [];
}

public class RolePermissionRow
{
    public int RoleId { get; set; }
    public string PermKey { get; set; } = string.Empty;
}
