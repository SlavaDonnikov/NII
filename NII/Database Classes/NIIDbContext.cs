using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace NII.Database_Classes
{
	class NIIDbContext : DbContext
	{
		static NIIDbContext()
		{
			//Database.SetInitializer<NIIDbContext>(new CreateDatabaseIfNotExists<NIIDbContext>());
			//Database.SetInitializer<NIIDbContext>(new DropCreateDatabaseIfModelChanges<NIIDbContext>());
			//Database.SetInitializer<NIIDbContext>(new DropCreateDatabaseAlways<NIIDbContext>());
			//Database.SetInitializer<NIIDbContext>(new DbContextInitializer());

			Database.SetInitializer<NIIDbContext>(new DbContextInitializer());
		}
		
		public NIIDbContext() : base("Research_Institute_Db") { }

		public DbSet<Project> Projects { get; set; }
		public DbSet<Scientist> Scientists { get; set; }
		public DbSet<Technician> Technicians { get; set; }
		public DbSet<Sample> Samples { get; set; }
		public DbSet<Equipment> Equipment { get; set; }		
	}
}
