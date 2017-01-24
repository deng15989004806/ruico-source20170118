using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Transactions;
using log4net;
using Ruico.Domain.BaseModule.Entities;
using Ruico.Domain.HrModule.Entities;
using Ruico.Domain.KaoQinModule.Entities;
using Ruico.Domain.SystemModule.Entities;
using Ruico.Domain.UserSystemModule.Entities;
using Ruico.Domain.WeixinModule.Entities;
using Ruico.Infrastructure.UnitOfWork;
using Ruico.Infrastructure.Utility.Helper;

namespace Ruico.Repository
{
    public class RuicoUnitOfWork : DbContext, IRuicoUnitOfWork
    {
        private static readonly ILog SqlLogger = LogManager.GetLogger("sql_trace_logger");

        protected static readonly ILog Logger = LogManager.GetLogger(typeof(RuicoUnitOfWork));

        protected static readonly string SetDatabaseLogConfig =
            ConfigurationManager.AppSettings["Ruico:SetDatabaseLog"];

        public RuicoUnitOfWork()
            : base("RuicoContext")
        {
            Initializer.DbInitializer.Initialize();

            if ("true".EqualsIgnoreCase(SetDatabaseLogConfig))
            {
                this.SetDatabaseLog(SqlLogger.Info);
            }
        }

        public virtual DbSet<RoleGroup> RoleGroups { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Module> Modules { get; set; }

        public virtual DbSet<OperateRecord> OperateRecords { get; set; }
        public virtual DbSet<OperateRecordArchive> OperateRecordArchives { get; set; }
        public virtual DbSet<OperateRecordExtend> OperateRecordExtends { get; set; }
        public virtual DbSet<SerialNumberRule> SerialNumberRules { get; set; }
        public virtual DbSet<SerialNumber> SerialNumbers { get; set; }

        public virtual DbSet<Cargo> Cargos { get; set; }
        public virtual DbSet<Company> Companys { get; set; }
        public virtual DbSet<Ship> Ships { get; set; }

        public virtual DbSet<AppMenu> AppMenus { get; set; }

        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Member> Members { get; set; }

        public virtual DbSet<ChuChai> ChuChais { get; set; }
        public virtual DbSet<WaiQin> WaiQins { get; set; }
        public virtual DbSet<WeiDaKa> WeiDaKas { get; set; }
        public virtual DbSet<XiuJia> XiuJias { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            base.OnModelCreating(mb);

            this.OnModelCreatingCommon(mb);

            this.OnModelCreatingUserSystem(mb);

            this.OnModelCreatingWeixin(mb);

            this.OnModelCreatingHr(mb);

        }

        private void OnModelCreatingCommon(DbModelBuilder mb)
        {
            mb.Entity<Menu>().HasRequired(x => x.Module);
            mb.Entity<Category>().HasOptional(x => x.Parent).WithMany(x => x.Children).Map(x => x.MapKey("Parent_Id"));
            mb.Entity<Cargo>().HasOptional(x => x.Category);
            mb.Entity<Company>().HasOptional(x => x.Category);
        }

        private void OnModelCreatingUserSystem(DbModelBuilder mb)
        {
            mb.Entity<Role>().HasRequired(x => x.RoleGroup).WithMany(x => x.Roles);

            mb.Entity<Menu>().HasMany(x => x.Permissions).WithRequired(x => x.Menu);

            mb.Entity<Menu>().HasOptional(x => x.Parent).WithMany(x => x.Children).Map(x => x.MapKey("Parent_Menu_Id"));

            mb.Entity<User>().HasMany(x => x.Groups).WithMany(x => x.Users).Map(x => x.MapLeftKey("User_Id").MapRightKey("RoleGroup_Id").ToTable("RoleGroup_User", "auth"));

            mb.Entity<User>().HasMany(x => x.Permissions).WithMany(x => x.Users).Map(x => x.MapLeftKey("User_Id").MapRightKey("Permission_Id").ToTable("User_Permission", "auth"));

            mb.Entity<Role>().HasMany(x => x.Permissions).WithMany(x => x.Roles).Map(x => x.MapLeftKey("Role_Id").MapRightKey("Permission_Id").ToTable("Role_Permission", "auth"));
        }

        private void OnModelCreatingWeixin(DbModelBuilder mb)
        {
            mb.Entity<AppMenu>().HasOptional(x => x.Parent).WithMany(x => x.Children).Map(x => x.MapKey("Parent_Menu_Id"));
        }

        private void OnModelCreatingHr(DbModelBuilder mb)
        {
            mb.Entity<Member>().HasMany(x => x.Departments).WithMany(x => x.Members).Map(x => x.MapLeftKey("Member_Id").MapRightKey("Department_Id").ToTable("Department_Member", "hr"));
        }

        #region Methods

        public DbContext DbContext
        {
            get
            {
                return this;
            }
        }
        public IDbSet<TEntity> CreateSet<TEntity>()
         where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Commit()
        {
            try
            {
                base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                if (ex.EntityValidationErrors != null)
                {
                    Logger.Debug(
                        ex.EntityValidationErrors.SelectMany(
                            x => x.ValidationErrors.Select(
                                e => string.Format("{0}-{1}", e.PropertyName, e.ErrorMessage))));
                }
                throw;
            }
        }

        public void CommitAndRefreshChanges()
        {
            bool saveFailed = false;

            do
            {
                try
                {
                    base.SaveChanges();

                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    ex.Entries.ToList()
                        .ForEach(entry => entry.OriginalValues.SetValues(entry.GetDatabaseValues()));

                }
            } while (saveFailed);
        }

        public void RollbackChanges()
        {
            // set all entities in change tracker 
            // as 'unchanged state'
            base.ChangeTracker.Entries()
                .ToList()
                .ForEach(entry => entry.State = EntityState.Unchanged);
        }

        public void SetDatabaseLog(Action<string> logAction)
        {
            this.Database.Log = logAction;
        }

        #endregion
    }
}
