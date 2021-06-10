using Microsoft.EntityFrameworkCore;
using MIU.Core.Data;
using MIU.Movimentations.Domain.Entities;
using MIU.Movimentations.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIU.Movimentations.Infra.Repositories
{
    public class MovimentationRepository : IMovimentationRepository
    {
        private readonly MovimentationContext _context;

        public MovimentationRepository(MovimentationContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IList<Movimentation>> GetMovimentations()
        {
            return await _context.Movimentations.AsNoTracking().ToListAsync();
        }

        public void AddMovimentation(Movimentation movimentation)
        {
            _context.Movimentations.Add(movimentation);
        }

        public void RemoveMovimentation(Movimentation movimentation)
        {
            _context.Remove(movimentation);
        }

        public Movimentation GetMovimentationById(Guid id)
        {
            var movimentation = _context.Movimentations.Where(a => a.Id == id).Include(x => x.Cpf).AsNoTracking().FirstOrDefault();
            return movimentation;
        }

        public void UpdateMovimentation(Movimentation movimentation)
        {
            _context.Update(movimentation);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
