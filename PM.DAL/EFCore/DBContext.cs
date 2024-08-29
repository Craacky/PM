using Microsoft.EntityFrameworkCore;
using System;

namespace PM.DAL.EFCore
{
    public class DBContext : DbContext
    {
        private ConnectionState _connectionState;


        public string ConnectionString { get; private set; }
        public bool IsConnected => _connectionState == ConnectionState.Connected || _connectionState == ConnectionState.Created;


        public delegate void ConnectionHandler(DBContext db, DateTime datetime, ConnectionState connectionState);
        public static event ConnectionHandler ConnectionChanged;


        public DbSet<Entities.Nomenclature> Nomenclatures { get; set; }
        public DbSet<Entities.Attribute> Attributes { get; set; }
        public DbSet<Entities.Product> Products { get; set; }
        public DbSet<Entities.Box> Boxes { get; set; }
        public DbSet<Entities.Pallet> Pallets { get; set; }
        public DbSet<Entities.ReportTask> ReportTasks { get; set; }
        public DbSet<Entities.Line> Lines { get; set; }
        public DbSet<Entities.Settings> Settings { get; set; }


        public DBContext(string connectionString)
        {
            _connectionState = ConnectionState.Disconnected;
            ConnectionString = connectionString;
            Connect();
        }

        public void Connect()
        {
            try
            {
                bool isDBExist = Database.EnsureCreated();
                if (isDBExist)
                {
                    _connectionState = ConnectionState.Created;
                }
                else
                {
                    _connectionState = ConnectionState.Connected;
                }
            }
            catch (ArgumentException ex)
            {
                _connectionState = ConnectionState.NotCorrectConnectionString;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                _connectionState = ConnectionState.NotFoundDB;
            }
            catch (Exception ex) when (ex.Message.Contains("server was not found"))
            {
                _connectionState = ConnectionState.NotFoundDB;
            }
            catch (Exception ex)
            {
                _connectionState = ConnectionState.NotFoundDB;
            }

            ConnectionChanged?.Invoke(this, DateTime.Now, _connectionState);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Nomenclature>()
                        .HasMany(n => n.Attributes)
                        .WithOne(a => a.Nomenclature)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.ReportTask>()
                        .HasMany(rt => rt.Pallets)
                        .WithOne(p => p.ReportTask)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.Pallet>()
                        .HasMany(p => p.Boxes)
                        .WithOne(b => b.Pallet)
                        .OnDelete(DeleteBehavior.Cascade) ;

            modelBuilder.Entity<Entities.Box>()
                        .HasMany(b => b.Products)
                        .WithOne(p => p.Box)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.Settings>()
                        .OwnsOne(s => s.Line);
            modelBuilder.Entity<Entities.Settings>()
                        .OwnsOne(s => s.ProductCameraMaster);
            modelBuilder.Entity<Entities.Settings>()
                        .OwnsOne(s => s.ProductCameraSlave);
            modelBuilder.Entity<Entities.Settings>()
                        .OwnsOne(s => s.BoxCamera);
            modelBuilder.Entity<Entities.Settings>()
                        .OwnsOne(s => s.BoxPrinter);
            modelBuilder.Entity<Entities.Settings>()
                        .OwnsOne(s => s.PalletPrinter);
            modelBuilder.Entity<Entities.Settings>()
                        .OwnsOne(s => s.ServerDB);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }
    }
}
