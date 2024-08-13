
using Management_System.Models.Entities;
using Microsoft.EntityFrameworkCore;


namespace Management_System.Contexts
{

    //communication between database and the classes
    internal class DataContext : DbContext
    {
        private readonly string? _connectionString;

        #region ConnectionString
        public DataContext()
        {
            _connectionString = Environment.GetEnvironmentVariable("Connection_String");
        }
        
    #endregion
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
        }

        #region connection
        private readonly string _connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\filip\OneDrive\Dokument\Datalagring\course-submission\Management-System\Contexts\db_context.mdf;Integrated Security=True;Connect Timeout=30";
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
