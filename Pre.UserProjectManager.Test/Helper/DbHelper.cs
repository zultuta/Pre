using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pre.UserProjectManager.Core.Entity;
using Pre.UserProjectManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pre.UserProjectManager.Test.Helper
{
    public  class DbHelper
    {
        public static DbContextOptionsBuilder<ProjectManagerDbContext> GetContextOptionsBuilder(string dbName)
        {
            var builder = new DbContextOptionsBuilder<ProjectManagerDbContext>();

            builder.UseInMemoryDatabase(dbName) // doesn't suport transactions and sql statement , use InitContextWithTransactionAndSQLSupport
                                                // Don't raise the error warning us that the in memory db doesn't support transactions
                   .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            return builder;
        }
        public static ProjectManagerDbContext InitContext(string dbName)
        {
            var builder = GetContextOptionsBuilder(dbName);
            return new ProjectManagerDbContext(builder.Options);
        }
        public static void ClearDB(string dbName)
        {
            using var context = InitContext(dbName);

            var users = context.Users.ToList();
            var projects = context.Projects.ToList();
            var products = context.Products.ToList();
            var projectAssignments = context.ProjectAssignments.ToList();

            if (users.Count > 0)
                context.RemoveRange(users);

            if (projects.Count > 0)
                context.RemoveRange(projects);

            if (products.Count > 0)
                context.RemoveRange(products);

            if (projectAssignments.Count > 0)
                context.RemoveRange(projectAssignments);

            context.SaveChanges();
        }

        internal static object InitServiceScopeFactory(string dbName)
        {
            throw new NotImplementedException();
        }

        public static async Task<List<T>> GetEntityListAsync<T>(string dbName) where T : BaseEntity
        {
            using var context = InitContext(dbName);
            return await context.Set<T>().ToListAsync();
        }
        public static void SeedDB<T>(string dbName, T debt) where T : BaseEntity
        {
            using var context = InitContext(dbName);

            context.Set<T>().Add(debt);

            context.SaveChanges();
        }
    }
}
