using NFCEPS_API.Repository.Interfaces;

namespace NFCEPS_API.Services.Permission;

public class PermissionService
{
    private Dictionary<int, HashSet<string>> _cache = new();
    private readonly IGenericRepository _repo;
    private readonly ILogger<PermissionService> _logger;

    public PermissionService(IGenericRepository repo,
        ILogger<PermissionService> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task LoadAsync()
    {
        var rows = await _repo.QueryAsync<RolePermissionRow>(
            "Permission.sp_GetAllRolePermissions");

        var newCache = new Dictionary<int, HashSet<string>>();

        foreach (var row in rows)
        {
            if (!newCache.ContainsKey(row.RoleId))
                newCache[row.RoleId] = [];

            newCache[row.RoleId].Add(row.PermKey);
        }

        _cache = newCache;
        _logger.LogInformation(
            "Permission cache loaded — {Count} roles", newCache.Count);
    }

    public bool Has(int roleId, string permKey)
        => _cache.TryGetValue(roleId, out var perms) 
           && perms.Contains(permKey);

    public HashSet<string> GetAll(int roleId)
        => _cache.TryGetValue(roleId, out var perms) 
            ? perms 
            : [];
}

public class RolePermissionRow
{
    public int RoleId { get; set; }
    public string PermKey { get; set; } = string.Empty;
}