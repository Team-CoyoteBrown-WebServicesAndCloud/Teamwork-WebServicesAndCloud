namespace SocialNetwork.Data.Migrations
{
    using System.Data.Entity.Migrations;
    using Data;

    internal sealed class Configuration : DbMigrationsConfiguration<SocialNetworkContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(SocialNetworkContext context)
        {
            
        }
    }
}
