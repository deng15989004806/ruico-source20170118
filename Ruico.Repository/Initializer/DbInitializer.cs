using System.Data.Entity;

namespace Ruico.Repository.Initializer
{
    public static class DbInitializer
    {
        /// <summary>
        /// 数据库初始化
        /// </summary>
        public static void Initialize()
        {
            // 初始化时使用
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<RuicoUnitOfWork, Configuration>());

            // 运行时使用
            Database.SetInitializer<RuicoUnitOfWork>(null);
        }
    }
}