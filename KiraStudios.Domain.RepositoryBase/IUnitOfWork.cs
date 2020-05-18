using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace KiraStudios.Domain.RepositoryBase
{
    public partial interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        //  Stanndard Behavior 
        int Commit();
        Task<int> CommitAsync();

        //    void BeginTransaction();
        //    void RollBackTransaction();
        //    void CommitTransaction();
    }
}
