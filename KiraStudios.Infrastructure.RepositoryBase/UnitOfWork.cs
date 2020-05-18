using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KiraStudios.Infrastructure.RepositoryBase
{
    public partial class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(ILoggerFactory loggerFactory)
        {
            if(Context is null) Context = new ApplicationContext(loggerFactory);
            //InitRepositories();
        }

        public UnitOfWork(DbContext context) { Context = context; }

        public DbContext Context { get; private set; }

        public int Commit()
        {
            try { return Context.SaveChanges(); }
            catch (Exception e)
            {
                if (e.InnerException != null) throw e.InnerException;
                else throw e;
            }
        }

        public Task<int> CommitAsync()
        {
            try { return Context.SaveChangesAsync(); }
            catch (Exception e)
            {
                if (e.InnerException != null) throw e.InnerException;
                else throw e;
            }
        }

        public void Dispose()
        {
            if (Context != null) Context.Dispose();
        }
    }
}
