using PlatformService.Models;

namespace PlatformService.Data;
public class PlatformRepo : IPlatformRepo
{
    private readonly AppDataContext _context;

    public PlatformRepo(AppDataContext context)
    {
        _context = context;
    }
    public void CreatePlatform(Platform platform)
    {
        if (platform is null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        _context.Platforms.Add(platform);
    }

    public IEnumerable<Platform> GetAllPlatform()
    {
        return _context.Platforms.ToList();
    }

    public Platform GetPlatformById(int id)
    {
        return _context.Platforms.FirstOrDefault(i => i.Id == id);
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() > 0;
    }
}
