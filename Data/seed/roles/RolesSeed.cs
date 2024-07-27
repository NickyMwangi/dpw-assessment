using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Data.seed.Roles;

public class RolesSeed
{
    private readonly IdContext _dbContext;
    private const string LIST_FILENAME = "roles-data.json";

    public RolesSeed(IdContext dbContext)
    {
        this._dbContext = dbContext;    
    }

    public void BeginSeeding()
    {
        // remove existing Roles
        List<IdentityRole> exisitingRoles = _dbContext.Roles.ToList();
        if (exisitingRoles.Any())
            return;

        List<IdentityRole> roles = GetSeedData();
        for(int i = 0; i < roles.Count; i++)
        {
            roles[i].Id = Guid.NewGuid().ToString();
            roles[i].ConcurrencyStamp = Guid.NewGuid().ToString();
            _dbContext.Roles.AddAsync(roles[i]).GetAwaiter().GetResult();
            _dbContext.SaveChanges();
        }
    }

    public List<IdentityRole> GetSeedData()
    {
        var strJson = JsonInterpreter.GetResourceAsString($"{LIST_FILENAME}");
        List<IdentityRole> seedData = JsonSerializer.Deserialize<List<IdentityRole>>(strJson) ?? new List<IdentityRole>();
        return seedData;
    }
}
