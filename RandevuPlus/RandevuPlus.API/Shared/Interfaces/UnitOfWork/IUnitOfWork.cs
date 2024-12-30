using RandevuPlus.API.Shared.Interfaces.Repository;

namespace RandevuPlus.API.Shared.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IInstructorRepository Instructors { get; }
        IUserRepository Users { get; }
        Task<int> Commit();
    }
}
