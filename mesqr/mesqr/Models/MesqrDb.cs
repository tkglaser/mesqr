using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace mesqr.Models
{
    public class MesqrDb : DbContext
    {
        public DbSet<Msq> Msqs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<TableSubscription> TableSubscriptions { get; set; }
    }
}