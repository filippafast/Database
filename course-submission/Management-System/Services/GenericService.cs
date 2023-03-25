using System.Linq.Expressions;
using Management_System.Contexts;
using Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Management_System.Services;


internal abstract class GenericService<TEntity> where TEntity : class
{
    private readonly DataContext _context = new DataContext();

    #region GetAllAsync
    //Get All depending on which entity I want to get
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    #endregion

    #region GetAsync
    //predicate filters search results. Bool shows whether the entity meets the condition or not
    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
        

        var item = await _context.Set<TEntity>().FirstOrDefaultAsync(predicate, CancellationToken.None);
        if (item != null)
            return item;

        return null!;
    }

    #endregion

    #region SaveAsync
    public virtual async Task<TEntity> SaveAsync(TEntity entity)
    {
        _context.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    #endregion

    #region SaveAsyncTwoArguments
    //A method that takes two arguments, entity & checkIfExist. Checks if the entity already exists in the database
    //If a entity already exist, it returns the existing one without any changes being made to the database
    public virtual async Task<TEntity> SaveAsync(TEntity entity, Expression<Func<TEntity, bool>> checkIfExist)
    {
           var item = await GetAsync(checkIfExist);
            if (item == null)
        {
            _context.Add(entity);
            //The changes are saved using SaveChangesAsync
            await _context.SaveChangesAsync();
            return entity;
        }
        return item;

        }
    #endregion
}




