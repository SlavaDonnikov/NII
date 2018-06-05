using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NII.Database_Classes
{
	class DbContextInitializer : DropCreateDatabaseAlways<NIIDbContext>
	{
		protected override void Seed(NIIDbContext db)
		{

		}
	}
}
