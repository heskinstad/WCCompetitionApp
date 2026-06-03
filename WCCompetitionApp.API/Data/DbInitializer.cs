using WCCompetitionApp.API.Models;

namespace WCCompetitionApp.API.Data
{
    public static class DbInitializer
    {
        public static void Initialize(WCCompetitionContext context)
        {
            context.Database.EnsureCreated();

            context.SaveChanges();
        }
    }
}
