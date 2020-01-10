﻿using Uplift.DataAccess.Data.Repository.Interfaces;

namespace Uplift.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Categories { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Categories = new CategoryRepository(_db);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public int Save()
        {
            return _db.SaveChanges();
        }
    }
}