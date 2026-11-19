using Microsoft.EntityFrameworkCore;

namespace GameBacklog.Data;

public static class DataExtensions
{
    public static void AddGameBacklogDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameBacklog");
        builder.Services.AddDbContext<GameBacklogContext>(options => options.UseNpgsql(connString));
    }
}
