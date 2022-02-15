using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using P2PLending.Web.Entities.DTO.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2PLending.Web.DAL.DataContext
{
    public class P2PDBContextFactory
    {
        private string connectionString;
        public P2PDBContextFactory(IOptions<ConnectionStrings> connectionStrings)
        {
            connectionString = connectionStrings.Value.DefaultConnection;
        }
        public P2PLendingDbContext Create()
        {
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            var options = new DbContextOptionsBuilder<P2PLendingDbContext>()
                .UseMySql(connectionString, serverVersion)
                .Options;

            return new P2PLendingDbContext(options);
        }
    }
}
