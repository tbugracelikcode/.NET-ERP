using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tsi.Core.Entities;
using Tsi.Core.Entities.Auditing;
using Tsi.Core.Utilities.Guids;
using TsiErp.DataAccess.Services.Login;
using TsiErp.Entities.Entities.Logging;

namespace TsiErp.DataAccess.EntityFrameworkCore.Repositories
{
    public class EfCoreRepository<TEntity> : IEfCoreRepository<TEntity> where TEntity : class, IEntity
    {

        private DbContext _dbContext;

        private DbSet<TEntity> _dbset;

        public IGuidGenerator GuidGenerator { get; set; } = new SequentialGuidGenerator();

        public EfCoreRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbset = _dbContext.Set<TEntity>();
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = await _dbset.SingleOrDefaultAsync(predicate);

            return entity;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = await WithDetailsAsync(includeProperties);

            if (predicate != null)
            {
                return await queryable.FirstOrDefaultAsync(predicate);
            }

            return await queryable.FirstOrDefaultAsync();

        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var entities = predicate == null ? await _dbset.ToListAsync() : await _dbset.Where(predicate).ToListAsync();

            return entities;
        }

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            var queryable = await WithDetailsAsync(includeProperties);

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            return await queryable.ToListAsync();
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity is IFullEntityObject)
            {
                entity.GetType().GetProperty("CreatorId").SetValue(entity, LoginedUserService.UserId);
                entity.GetType().GetProperty("CreationTime").SetValue(entity, DateTime.Now);
                entity.GetType().GetProperty("IsDeleted").SetValue(entity, false);
                entity.GetType().GetProperty("DeleterId").SetValue(entity, null);
                entity.GetType().GetProperty("DeletionTime").SetValue(entity, null);
                entity.GetType().GetProperty("LastModifierId").SetValue(entity, null);
                entity.GetType().GetProperty("LastModificationTime").SetValue(entity, null);
            }

            if (entity is IEntity)
            {
                entity.GetType().GetProperty("Id").SetValue(entity, GuidGenerator.CreateGuid());
            }

            await _dbset.AddAsync(entity);

            return entity;

        }

        public async Task InsertManyAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await InsertAsync(entity);
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var previousEntity = (IFullEntityObject)await GetAsync(t => t.Id == entity.Id);

            if (entity is IFullEntityObject && previousEntity != null)
            {
                entity.GetType().GetProperty("CreatorId").SetValue(entity, previousEntity.CreatorId);
                entity.GetType().GetProperty("CreationTime").SetValue(entity, previousEntity.CreationTime);
                entity.GetType().GetProperty("IsDeleted").SetValue(entity, previousEntity.IsDeleted);
                entity.GetType().GetProperty("DeleterId").SetValue(entity, previousEntity.DeleterId);
                entity.GetType().GetProperty("DeletionTime").SetValue(entity, previousEntity.DeletionTime);
                entity.GetType().GetProperty("LastModifierId").SetValue(entity, LoginedUserService.UserId);
                entity.GetType().GetProperty("LastModificationTime").SetValue(entity, DateTime.Now);
            }

            if (entity is IFullEntityObject && previousEntity == null)
            {
                entity.GetType().GetProperty("CreatorId").SetValue(entity, LoginedUserService.UserId);
                entity.GetType().GetProperty("CreationTime").SetValue(entity, DateTime.Now);
                entity.GetType().GetProperty("IsDeleted").SetValue(entity, false);
                entity.GetType().GetProperty("DeleterId").SetValue(entity, null);
                entity.GetType().GetProperty("DeletionTime").SetValue(entity, null);
                entity.GetType().GetProperty("LastModifierId").SetValue(entity, null);
                entity.GetType().GetProperty("LastModificationTime").SetValue(entity, null);
            }

            _dbContext.ChangeTracker.Clear();

            //_dbContext.Attach(entity);

            //var updatedEntity = _dbContext.Update(entity).Entity;

            _dbContext.Attach(entity);

            _dbContext.Entry(entity).State = EntityState.Modified;
            return _dbContext.Entry(entity).Entity;
        }

        public async Task<TEntity> LockRow(Guid id, bool lockRow, Guid userId)
        {
            var entity = (IFullEntityObject)await GetAsync(t => t.Id == id);

            if (entity != null)
            {
                entity.DataOpenStatus = lockRow;
                entity.DataOpenStatusUserId = userId;

                _dbContext.ChangeTracker.Clear(); _dbContext.Attach(entity);

                _dbContext.Entry(entity).State = EntityState.Modified;
                return (TEntity)entity;
            }

            return default;
        }

        public async Task UpdateManyAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = _dbset.Single(t => t.Id == id);

            _dbContext.Set<TEntity>().Remove(entity);
        }

        private static IQueryable<TEntity> IncludeDetails(IQueryable<TEntity> query, Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors != null)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        public async Task<IQueryable<TEntity>> WithDetailsAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            return IncludeDetails(_dbset.AsQueryable(), propertySelectors);
        }

        public async Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            await Task.CompletedTask;
            return _dbset.AsQueryable();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbset.AnyAsync(predicate);
        }

        public async Task<int> SaveChanges()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
