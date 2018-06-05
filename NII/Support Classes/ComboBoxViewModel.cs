using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NII.Database_Classes;

namespace NII.Support_Classes
{
	public class ComboBoxViewModel
	{
		public ObservableCollection<string> ProjectCollection { get; set; }
		public ObservableCollection<string> ScientistCollection { get; set; }
		public ObservableCollection<string> TechnicianCollection { get; set; }
		public ObservableCollection<string> SampleCollection { get; set; }
		public ObservableCollection<string> EquipmentCollection { get; set; }

		public ObservableCollection<string> ScientistPositionCollection { get; set; }
		public ObservableCollection<string> TechnicianPositionCollection { get; set; }
		
		public ObservableCollection<string> ScientistQualificationCollection { get; set; }
		public ObservableCollection<string> TechnicianQualificationCollection { get; set; }

		public void RefreshDbEntitiesCollections()
		{
			using (NIIDbContext db = new NIIDbContext())
			{
				var projects = db.Projects.ToList();
				ProjectCollection = new ObservableCollection<string>();
				foreach (Project p in projects)
				{
					if (!ProjectCollection.Contains(p.Name))
					{
						ProjectCollection.Add(p.Name.ToString());
					}
				}

				var scientists = db.Scientists.ToList();
				ScientistCollection = new ObservableCollection<string>();
				foreach (Scientist s in scientists)
				{
					if (!ScientistCollection.Contains(s.Name))
					{
						ScientistCollection.Add(s.Name.ToString());
					}
				}

				var technicians = db.Technicians.ToList();
				TechnicianCollection = new ObservableCollection<string>();
				foreach (Technician t in technicians)
				{
					if (!TechnicianCollection.Contains(t.Name))
					{
						TechnicianCollection.Add(t.Name.ToString());
					}
				}
				
				var samples = db.Samples.ToList();
				SampleCollection = new ObservableCollection<string>();
				foreach (Sample smp in samples)
				{
					if (!SampleCollection.Contains(smp.Title))
					{
						SampleCollection.Add(smp.Title.ToString());
					}
				}

				var equipment = db.Equipment.ToList();
				EquipmentCollection = new ObservableCollection<string>();
				foreach (Equipment eq in equipment)
				{
					if (!EquipmentCollection.Contains(eq.Title))
					{
						EquipmentCollection.Add(eq.Title.ToString());
					}
				}
			}
		}

		public ComboBoxViewModel()
		{
			ScientistPositionCollection = new ObservableCollection<string>()
			{
				"",
				""
			};

			TechnicianPositionCollection = new ObservableCollection<string>()
			{
				"",
				""
			};

			ScientistQualificationCollection = new ObservableCollection<string>()
			{
				"",
				""
			};

			TechnicianQualificationCollection = new ObservableCollection<string>()
			{
				"",
				""
			};

			RefreshDbEntitiesCollections();
		}
	}
}
