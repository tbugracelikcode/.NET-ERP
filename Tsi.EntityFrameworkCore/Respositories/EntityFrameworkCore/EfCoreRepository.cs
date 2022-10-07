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
using Tsi.EntityFrameworkCore.Repositories;
using Tsi.EntityFrameworkCore.Repositories.EntityFrameworkCore;
using Tsi.EntityFrameworkCore.Respositories.Extensions;
using TsiErp.Entities.Entities.Logging;

namespace Tsi.EntityFrameworkCore.Respositories.EntityFrameworkCore
{
    public class EfCoreRepository<TEntity> : IEfCoreRepository<TEntity> where TEntity : class, IEntity, new()
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

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate,  params Expression<Func<TEntity, object>>[] includeProperties)
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

        public async Task<IList<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null,  params Expression<Func<TEntity, object>>[] includeProperties)
        {

            var queryable = await WithDetailsAsync(includeProperties);

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            return await queryable.ToListAsync();
        }

        public async Task<TEntity> InsertAsync(TEntity entity, bool autoSave = true)
        {
            //var addedEntity = _dbContext.Entry(entity);
            //addedEntity.State = EntityState.Added;
            //await _dbContext.SaveChangesAsync();
            //return addedEntity.Entity;

            if (entity is IFullEntityObject)
            {
                entity.GetType().GetProperty("CreatorId").SetValue(entity, Guid.NewGuid());
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

            _dbset.Add(entity);


            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(true);
            }

            return entity;

        }

        public async Task InsertManyAsync(IEnumerable<TEntity> entities, bool autoSave = true)
        {
            foreach (var entity in entities)
            {
                await InsertAsync(entity);
            }

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(true);
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity, bool autoSave = true)
        {

            ////var updatedEntity = _dbContext.Set<TEntity>().AsNoTracking().Single(t => t.Id == entity.Id);

            //var updatedEntity = _dbContext.Entry(entity);
            //updatedEntity.State = EntityState.Modified;
            //await _dbContext.SaveChangesAsync();
            //return updatedEntity.Entity;

            var previousEntity = (IFullEntityObject)await GetAsync(t => t.Id == entity.Id);

            if (entity is IFullEntityObject)
            {
                entity.GetType().GetProperty("CreatorId").SetValue(entity, previousEntity.CreatorId);
                entity.GetType().GetProperty("CreationTime").SetValue(entity, previousEntity.CreationTime);
                entity.GetType().GetProperty("IsDeleted").SetValue(entity, previousEntity.IsDeleted);
                entity.GetType().GetProperty("DeleterId").SetValue(entity, previousEntity.DeleterId);
                entity.GetType().GetProperty("DeletionTime").SetValue(entity, previousEntity.DeletionTime);
                entity.GetType().GetProperty("LastModifierId").SetValue(entity, Guid.NewGuid());
                entity.GetType().GetProperty("LastModificationTime").SetValue(entity, DateTime.Now);
            }

            _dbContext.ChangeTracker.Clear();

            _dbContext.Attach(entity);

            var updatedEntity = _dbContext.Update(entity).Entity;



            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(true);
            }

            return updatedEntity;
        }

        public async Task UpdateManyAsync(IEnumerable<TEntity> entities, bool autoSave = true)
        {
            foreach (var entity in entities)
            {
                await UpdateAsync(entity);
            }

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(Guid id, bool autoSave = true)
        {
            var entity = _dbset.Single(t => t.Id == id);

            _dbContext.Set<TEntity>().Remove(entity);

            if (autoSave)
            {
                await _dbContext.SaveChangesAsync(true);
            }
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
    }
}
