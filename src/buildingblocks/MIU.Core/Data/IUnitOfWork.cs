using System.Threading.Tasks;

namespace MIU.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
