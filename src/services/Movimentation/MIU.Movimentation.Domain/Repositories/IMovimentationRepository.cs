using MIU.Core.Data;
using MIU.Movimentations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MIU.Movimentations.Domain.Repositories
{
    public interface IMovimentationRepository : IRepository<Movimentation>
    {
        void AddMovimentation(Movimentation movimentation);
        void RemoveMovimentation(Movimentation movimentation);
        Task<IList<Movimentation>> GetMovimentations();
        Movimentation GetMovimentationById(Guid id);
        void UpdateMovimentation(Movimentation movimentation);
    }
}
