using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using System.Linq.Expressions;
using System.Net.NetworkInformation;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly AplicationDbContext _db;

        public VillaRepository(AplicationDbContext db)
        {
            _db = db;
        }
        public async Task Create(Villa entity)
        {
            await _db.Villas.AddAsync(entity);
        }

        public Task Delete(Villa entity)
        {
            throw new NotImplementedException();
        }

        public Task<Villa> Get(Expression<Func<Villa>> filter = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task<List<Villa>> GetAll(Expression<Func<Villa>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
