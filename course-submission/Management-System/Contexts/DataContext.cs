
using Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Management_System.Contexts
{

    //communication between database and the classes
    internal class DataContext : DbContext
    {

        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        }

        #region ConnectionString
            public DataContext()
        {
        _connectionString = Environment.GetEnvironmentVariable("Connection_String");
        }
    
        #endregion

        #region constructors
       

        #endregion

        #region OnConfiguring
        //Connectar till databasen
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
	            if (_connectionString == null)
	        {
		        throw new InvalidOperationException("Anslutningssträngen är inte satt.");
	        }
        optionsBuilder.UseSqlServer(_connectionString);
        }

        #endregion

        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<CustomerTypeEntity> CustomerTypes { get; set; }
        public DbSet<StatusTypeEntity> StatusTypes { get; set; }
        public DbSet<CustomerEntity> Customers { get; set; }
        public DbSet<CaseEntity> Cases { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }


    }

}
