using Microsoft.EntityFrameworkCore;

namespace GameBacklog.Data;

public class GameBacklogContext(DbContextOptions<GameBacklogContext> options) : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>();
}
