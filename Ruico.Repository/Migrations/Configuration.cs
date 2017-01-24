using System.Data.Entity.Migrations;

namespace Ruico.Repository.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<RuicoUnitOfWork>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
            //AutomaticMigrationsEnabled = false;
            //ContextKey = "Ruico.Repository.UnitOfWork.RuicoUnitOfWork";
        }

        protected override void Seed(RuicoUnitOfWork context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
