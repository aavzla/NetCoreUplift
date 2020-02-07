using System;
using System.Collections.Generic;
using Dapper;

namespace Uplift.DataAccess.Data.Repository.Interfaces
{
    public interface ISP_Call : IDisposable
    {
        IEnumerable<T> Execute<T>(string storedProcedure, DynamicParameters param = null);
        void Execute(string storedProcedure, DynamicParameters param = null);
        T ExecuteScalar<T>(string storedProcedure, DynamicParameters param = null);
    }
}
