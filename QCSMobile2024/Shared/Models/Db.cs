using Microsoft.EntityFrameworkCore;
using QCSMobile2024.Shared.Models.EntityModels;
namespace QCSMobile2024.Shared.Models
{
    public class Db : DbContext
    {
        public Db(DbContextOptions<Db> option) : base(option)
        {

        }

        // Add entity models here in order to access them in Entity Framework.
       
        public DbSet<CarrierProgramCode> CarrierProgramCode { get; set; }
        public DbSet<EAS_Inbound> EAS_Inbound { get; set; }
        public DbSet<Fnol> Fnol { get; set; }
        public DbSet<Fnol_Attachments> Fnol_Attachments { get; set; }
        public DbSet<Fnol_Insured> Fnol_Insured { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<PhotosExpress> PhotosExpress { get; set; }
        public DbSet<PhotosExpress_Attachment> PhotosExpress_Attachment { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<VehicleMake> VehicleMake { get; set; }




        // On Model Creating helps configure tables that are not standard.
        // For example, if the primary key is not named "Id" or "TableNameId", as shown below.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fnol_Insured>().HasNoKey();
        }

        // On Configuring helps configure the connection string if it is not already configured for some reason.
        // This will not currently work, as we do not pull the connection string from the appsettings.json file.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    IConfigurationRoot configuration = new ConfigurationBuilder()
            //       .SetBasePath(Directory.GetCurrentDirectory())
            //       .AddJsonFile("appsettings.json")
            //       .Build();
            //    var connectionString = configuration.GetConnectionString("DefaultConnection");
            //    optionsBuilder.UseSqlServer(connectionString);
            //}
        }
    }
}
