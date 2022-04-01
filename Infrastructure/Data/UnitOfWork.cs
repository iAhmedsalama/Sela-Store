using Core.Entities;
using Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        
        //To Store all repos in Hashtable
        private Hashtable _repositories;

        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            //to free resources
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            //check if we already have a Hashtable
            if (_repositories == null) _repositories = new Hashtable();

            //check the name of passed entity
            var type = typeof(TEntity).Name;

            //check if the Hashtable have a repo withthe name of passed type
            if (!_repositories.ContainsKey(type))
            {
                //if not create a repo of GenericRepository
                var repositoryType = typeof(GenericRepository<>);

                //Create instance of GenericRepository and passing _context which we will get from unit of work 
                var repositoryInstance = Activator
                    .CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                //Hashtable is going to store all of the repos in use inside our unit of work
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }
    }
}
