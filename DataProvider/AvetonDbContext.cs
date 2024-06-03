using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider.EntitiesTypeConfiguration;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace DataProvider
{
    public class AvetonDbContext : DbContext, IAvetonDbContext
    {
        public AvetonDbContext()
        {
        }

        public AvetonDbContext(DbContextOptions<AvetonDbContext> options)
            : base(options)
        {
        }

        #region configuringMethods

        //Точка входа
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyStructureConfiguration(modelBuilder);
        }

        private void ApplyStructureConfiguration(ModelBuilder modelBuilder)
        {
            //Конфигурации сущностей
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new AvetonUserConfiguration());
            modelBuilder.ApplyConfiguration(new AvetonRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AvetonUsersRolesConfiguration());
            modelBuilder.ApplyConfiguration(new AvetonRoleAccessConfiguration());
            modelBuilder.ApplyConfiguration(new JobConfiguration());
            modelBuilder.ApplyConfiguration(new PositionConfiguration());
            modelBuilder.ApplyConfiguration(new DivisionConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectStageConfiguration());
            modelBuilder.ApplyConfiguration(new DivisionContractorConfiguration());
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
            modelBuilder.ApplyConfiguration(new StageManagerConfiguration());
            modelBuilder.ApplyConfiguration(new StageReportConfiguration());
            modelBuilder.ApplyConfiguration(new StageReportAttachedFileConfiguration());
            modelBuilder.ApplyConfiguration(new ChatConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMemberConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMessageConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMessageAttachedFileConfiguration());
            modelBuilder.ApplyConfiguration(new ChatMessageViewedInfoConfiguration());
            modelBuilder.ApplyConfiguration(new SystemOwnerConfiguration());
        }

        #endregion

        #region DbSets

        public virtual DbSet<AvetonUser> AvetonUsers { get; set; }

        public virtual DbSet<Employee> Employees { get; set; }

        public virtual DbSet<AvetonRole> AvetonRoles { get; set; }

        public virtual DbSet<AvetonRoleAccess> AvetonRoleAccesses { get; set; }

        public virtual DbSet<AvetonUsersRoles> AvetonUsersRoles { get; set; }

        public virtual DbSet<Position> Positions { get; set; }

        public virtual DbSet<Division> Divisions { get; set; }

        public virtual DbSet<Job> Jobs { get; set; }

        public virtual DbSet<Project> Projects { get; set; }

        public virtual DbSet<ProjectStage> ProjectStages { get; set; }

        public virtual DbSet<DivisionContractor> DivisionContractors { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<Organization> Organizations { get; set; }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<StageManager> StageManagers { get; set; }

        public virtual DbSet<StageReport> StageReports { get; set; }

        public virtual DbSet<StageReportAttachedFile> StageReportAttachedFiles { get; set; }

        public virtual DbSet<SystemOwner> Owners { get; set; }

        public virtual DbSet<Chat> Chats { get; set; }

        public virtual DbSet<ChatMember> ChatMembers { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<ChatMessageAttachedFile> ChatMessageAttachedFiles { get; set; }

        public virtual DbSet<ChatMessageViewedInfo> ChatMessageViewedInfos { get; set; }


        #endregion

        #region BaseOperations


        public async Task<int> CountAsync<TEntity>() where TEntity : class
        {
            return await Set<TEntity>().CountAsync();
        }

        public async Task ReloadAsync<TEntity>(TEntity entity) where TEntity : class
        {            
            await Entry(entity).ReloadAsync();
        }
        #region Get   

        public virtual async Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class
        {
            return await Set<TEntity>().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class
        {
            return await Set<TEntity>().Where(match).ToListAsync();
        }
        public async Task<List<TEntity>> GetAllAsNoTrackingAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class
        {
            return await Set<TEntity>().Where(match).AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> GetAllOrderedAscendingAsync<TEntity>(
            Expression<Func<TEntity, bool>> match,
            Expression<Func<TEntity, object>> orderBy) where TEntity : class
        {
            return await Set<TEntity>().Where(match).OrderBy(orderBy).ToListAsync();
        }

        public async Task<List<TEntity>> GetAllOrderedDescendingAsync<TEntity>(
            Expression<Func<TEntity, bool>> match,
            Expression<Func<TEntity, object>> orderBy) where TEntity : class
        {
            return await Set<TEntity>().Where(match).OrderByDescending(orderBy).ToListAsync();
        }

        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class
        {
            return await Set<TEntity>().AnyAsync(match);
        }

        public virtual async Task<TEntity?> GetFirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class
        {
            return await Set<TEntity>().FirstOrDefaultAsync(match);
        }

        public virtual async Task<TEntity?> GetFirstOrDefaultAsNoTrackingAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class
        {
            return await Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(match);
        }

        #endregion

        #region Delete

        public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
        {
            Set<TEntity>().Remove(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteRangeAsync<TEntity>(ICollection<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().RemoveRange(entities);
            await SaveChangesAsync();
        }

        #endregion

        #region Update

        public virtual async Task<TEntity> UpdateAsync<TEntity>(TEntity entityToUpdate) where TEntity : class
        {
            try
            {
                if (Entry(entityToUpdate).State == EntityState.Detached)
                {                    
                    Set<TEntity>().Attach(entityToUpdate);
                }

                Entry(entityToUpdate).State = EntityState.Modified;
                ChangeTracker.AutoDetectChangesEnabled = false;
                await SaveChangesAsync();

                return entityToUpdate;
            }
            catch (Exception e)
            {
                throw new RepositoryException(e.Message, e.InnerException, typeof(TEntity).ToString());
            }
        }

        public virtual async Task<IList<TEntity>> UpdateRangeAsync<TEntity>(IList<TEntity> entities) where TEntity : class
        {
            var detachedEntities = new List<TEntity>();
            ChangeTracker.AutoDetectChangesEnabled = false;
            foreach (var entity in entities)
            {
                if (Entry(entity).State == EntityState.Detached)
                {
                    detachedEntities.Add(entity);
                }

                Entry(entity).State = EntityState.Modified;
            }

            Set<TEntity>().AttachRange(detachedEntities);
            await SaveChangesAsync();
            ChangeTracker.AutoDetectChangesEnabled = true;

            return entities;
        }

        #endregion

        #region Insert

        public virtual async Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            await Set<TEntity>().AddAsync(entity);
            await SaveChangesAsync();

            return entity;
        }

        public virtual async Task<IList<TEntity>> InsertRangeAsync<TEntity>(IList<TEntity> entities, bool saveChanges = true) where TEntity : class
        {
            await Set<TEntity>().AddRangeAsync(entities);
            if (saveChanges)
            {
                await SaveChangesAsync();
            }

            return entities;
        }

        public virtual async Task<List<TValue>> GetUniqueValuesAsync<TEntity, TValue>(Expression<Func<TEntity, TValue>> selector)
            where TEntity : class
            where TValue : class
        {
            return await Set<TEntity>().Select(selector).AsQueryable().Distinct().AsNoTracking().ToListAsync();
        }

        #endregion

        #endregion
    }
}
