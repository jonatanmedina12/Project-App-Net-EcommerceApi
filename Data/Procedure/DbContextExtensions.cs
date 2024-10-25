using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Procedure
{
    public static class DbContextExtensions
    {
        public static async Task<List<T>> SpQueryAsync<T>(
        this ApplicationDbContext context,
        string storedProcedure,
        params SqlParameter[] parameters) where T : class, new()
        {
            var helper = new StoredProcedureHelper(context);
            return await helper.ExecuteStoredProcedureAsync<T>(storedProcedure, parameters);
        }
    }
}
