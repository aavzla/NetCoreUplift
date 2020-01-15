﻿using System;

namespace Uplift.DataAccess.Data.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Categories { get; }
        IFrequencyRepository Frequencies { get; }
        int Save();
    }
}
