using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Uplift.DataAccess.Data.Repository.Interfaces;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly ApplicationDbContext _db;

        public FrequencyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetFrequencyListForDropDown()
        {
            return _db.Frequencies.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        public void Update(Frequency frequency)
        {
            Frequency objFromDB = GetFirstOrDefault(x => x.Id == frequency.Id);

            objFromDB.Name = frequency.Name;
            objFromDB.FrequencyCount = frequency.FrequencyCount;

            _db.SaveChanges();
        }
    }
}
