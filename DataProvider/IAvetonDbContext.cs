using DataCore.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataProvider
{
    public interface IAvetonDbContext
    {
        #region DbSets

        DbSet<AvetonUser> AvetonUsers { get; set; }

        DbSet<Employee> Employees { get; set; }

        DbSet<Position> Positions { get; set; }

        DbSet<Division> Divisions { get; set; }

        DbSet<Job> Jobs { get; set; }

        DbSet<AvetonRole> AvetonRoles { get; set; }

        DbSet<AvetonRoleAccess> AvetonRoleAccesses { get; set; }

        DbSet<AvetonUsersRoles> AvetonUsersRoles { get; set; }

        DbSet<Project> Projects { get; set; }

        DbSet<ProjectStage> ProjectStages { get; set; }

        DbSet<DivisionContractor> DivisionContractors { get; set; }

        DbSet<Person> Persons { get; set; }

        DbSet<Organization> Organizations { get; set; }

        DbSet<Client> Clients { get; set; }

        DbSet<StageManager> StageManagers { get; set; }

        DbSet<StageReport> StageReports { get; set; }

        DbSet<StageReportAttachedFile> StageReportAttachedFiles { get; set; }

        DbSet<Chat> Chats { get; set; }

        DbSet<ChatMember> ChatMembers { get; set; }

        DbSet<ChatMessage> ChatMessages { get; set; }

        DbSet<ChatMessageAttachedFile> ChatMessageAttachedFiles { get; set; }

        DbSet<ChatMessageViewedInfo> ChatMessageViewedInfos { get; set; }

        DbSet<SystemOwner> Owners { get; set; }

        #endregion


        #region BaseOperations

        Task<List<TValue>> GetUniqueValuesAsync<TEntity, TValue>(Expression<Func<TEntity, TValue>> selector) where TEntity : class where TValue : class;

        Task<TEntity?> GetFirstOrDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class;
        Task<TEntity?> GetFirstOrDefaultAsNoTrackingAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class;

        Task<List<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class;

        Task<List<TEntity>> GetAllAsNoTrackingAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class;

        Task<List<TEntity>> GetAllOrderedAscendingAsync<TEntity>(Expression<Func<TEntity, bool>> match, Expression<Func<TEntity, object>> orderBy) where TEntity : class;

        Task<List<TEntity>> GetAllOrderedDescendingAsync<TEntity>(Expression<Func<TEntity, bool>> match, Expression<Func<TEntity, object>> orderBy) where TEntity : class;

        Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class;

        Task<IList<TEntity>> UpdateRangeAsync<TEntity>(IList<TEntity> entities) where TEntity : class;

        Task<TEntity> UpdateAsync<TEntity>(TEntity entityToUpdate) where TEntity : class;

        Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class;

        Task<IList<TEntity>> InsertRangeAsync<TEntity>(IList<TEntity> entities, bool saveChanges = true) where TEntity : class;

        Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;

        Task DeleteRangeAsync<TEntity>(ICollection<TEntity> entities) where TEntity : class;

        Task<int> CountAsync<TEntity>() where TEntity : class;

        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> match) where TEntity : class;

        Task ReloadAsync<TEntity>(TEntity entity) where TEntity : class;

        #endregion
    }
}
