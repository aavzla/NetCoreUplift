using System;
using System.Collections.Generic;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Uplift.DataAccess.Data.Repository.Interfaces;

namespace Uplift.DataAccess.Data.Repository
{
    public class SP_Call : ISP_Call
    {
        private readonly ApplicationDbContext _db;
        private static string ConnectionString = string.Empty;

        public SP_Call(ApplicationDbContext db)
        {
            _db = db;
            ConnectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public void Execute(string storedProcedure, DynamicParameters param = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(storedProcedure, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public T ExecuteScalar<T>(string storedProcedure, DynamicParameters param = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return (T)Convert.ChangeType(connection.ExecuteScalar<T>(storedProcedure, param, commandType: System.Data.CommandType.StoredProcedure), typeof(T));
            }
        }

        public IEnumerable<T> Execute<T>(string storedProcedure, DynamicParameters param = null)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                return connection.Query<T>(storedProcedure, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }
        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
