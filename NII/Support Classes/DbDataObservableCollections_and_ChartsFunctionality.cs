using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NII.Database_Classes;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;
using System.Windows.Media;

namespace NII.Support_Classes
{
	public class DbDataObservableCollections_and_ChartsFunctionality
	{
		public ObservableCollection<string> ProjectCollection { get; set; }
        public ObservableCollection<string> ProjectCodeCollection { get; set; }

        public ObservableCollection<string> ScientistCollection { get; set; }
		public ObservableCollection<string> TechnicianCollection { get; set; }
		public ObservableCollection<string> SampleCollection { get; set; }
		public ObservableCollection<string> EquipmentCollection { get; set; }

		public ObservableCollection<string> ScientistPositionCollection { get; set; }
		public ObservableCollection<string> TechnicianPositionCollection { get; set; }
		
		public ObservableCollection<string> ScientistQualificationCollection { get; set; }
		public ObservableCollection<string> TechnicianQualificationCollection { get; set; }

		private void RefreshDbEntitiesCollections()
		{
			using (NIIDbContext db = new NIIDbContext())
			{
				var projects = db.Projects.ToList();
				ProjectCollection = new ObservableCollection<string>();
                ProjectCodeCollection = new ObservableCollection<string>();
                foreach (Project p in projects)
				{
					if (!ProjectCollection.Contains(p.Name))
					{
						ProjectCollection.Add(p.Name.ToString());
					}
                    if (!ProjectCodeCollection.Contains(p.CodeName))
                    {
                        ProjectCodeCollection.Add(p.CodeName.ToString());
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

		private void Create_Scientists_Technicians_Position_Qualification_Collections()
		{
			ScientistPositionCollection = new ObservableCollection<string>()
			{
				"Analytical Services Chemist",
				"Bioanalytical Scientist",
				"Biochemist",
				"Bioinformatics Research Scientist",
				"Biology Professor",
				"Cell Biology Scientist",
				"Chemical Engineer",
				"Clinical Pharmacology Professor",
				"Environmental Data Analyst",
				"Environmental Health Scientist",
				"Environmental Scientist",
				"Gene Editing Manager",
				"Health Research Assistant",
				"Immunology Scientist",
				"Medical Physics Researcher",
				"Molecular Biologist",
				"Molecular Scientist",
				"Oncology Researcher",
				"Research Chemist",
				"Research and Development Chemist",
				"Research Scientist",
				"Safety Data Specialist",
				"Satellite Data Analyst",
				"Scientific Programmer",
				"Stem Cell Researcher",
				"Structural Biologist",
				"Toxicologist"
			};

			TechnicianPositionCollection = new ObservableCollection<string>()
			{
				"Analytical Lab Technician",
				"Assistant Technician",
				"Chemical Technician",
				"Clinical Pharmacy Assistant",
				"Compliance Technician",
				"Computational Chemistry Manager",
				"Computer Programmer",
				"Conservation Technician",
				"Development Technologist",
				"Environmental Emergencies Assistant",
				"Environmental Research Assistant",
				"Exploration Director",
				"Genetic Counselor",
				"Hardware Designer",
				"Health Technology Assistant",
				"Industrial Designer",
				"Laboratory Assistant",
				"Laboratory Manager",
				"Laboratory Technician",
				"Medical Research Assistant",
				"Medical Research Technician",
				"Organic Lab Research Assistant",
				"Pharmaceutical Research Assistant",
				"Pharmaceutical Research Technician",
				"Power Regulator",
				"Process Engineer",
				"Product Engineer",
				"Product Test Specialist",
				"Quality Assurance Technologist",
				"Research Assistant",
				"Research Technician",
				"Research and Development Technician",
				"Research and Development Tester",
				"Science Technician",
				"Software Developer",
				"Software Engineering Assistant",
				"Structural Engineer",
				"Technical Support Technician"
			};

			ScientistQualificationCollection = new ObservableCollection<string>()
			{
				"Advanced Laboratory Operation",
				"Metallurgy",
				"Scientific Networking",
				"Amarr Encryption Methods",
				"Caldari Encryption Methods",
				"Defensive Subsystem Technology",
				"Electromagnetic Physics",
				"Gallente Encryption Methods",
				"Graviton Physics",
				"High Energy Physics",
				"Hydromagnetic Physics",
				"Laser Physics",
				"Nuclear Physics",
				"Plasma Physics",
				"Quantum Physics",
				"Rocket Science",
				"Sleeper Encryption Methods",
				"Sleeper Technology",
				"Takmahl Technology",
				"Talocan Technology",
				"Yan Jung Technology"
			};

			TechnicianQualificationCollection = new ObservableCollection<string>()
			{
				"Engineering analysis",
				"Chemistry",
				"Linear networks",
				"Fluid mechanics",
				"Circuits",
				"Thermodynamics",
				"Dynamics",
				"Machine design",
				"Welding",
				"Robotics",
				"Manufacturing Skills",
				"Amarr Starship Engineering",
				"Astronautic Engineering",
				"Caldari Starship Engineering",
				"Electronic Engineering",
				"Electronic Subsystem Technology",
				"Engineering Subsystem Technology",
				"Gallente Starship Engineering",
				"Mechanical Engineering",
				"Minmatar Starship Engineering",
				"Molecular Engineering",
				"Nanite Engineering",
				"Offensive Subsystem Technology",
				"Propulsion Subsystem Technology"
			};
		}

		#region Charts
		public SeriesCollection SamplesSeriesCollection { get; set; }		
		public ObservableCollection<string> SamplesLabels { get; set; }
		public Func<double, string> SamplesFormatter { get; set; }

		private void FillInSamplesChart()
		{
			using (NIIDbContext db = new NIIDbContext())
			{
				List<Sample> samples = db.Samples.Include("Projects").ToList();
				ChartValues<ObservableValue> sampleQuantityValues = new ChartValues<ObservableValue>();
				SamplesLabels = new ObservableCollection<string>();
				foreach (Sample sample in samples)
				{
					sampleQuantityValues.Add(new ObservableValue(sample.Quantity));
					SamplesLabels.Add(sample.Title);					
				}

				var converter = new System.Windows.Media.BrushConverter();
				var brush = (Brush)converter.ConvertFromString("#D2B48C");
				SamplesSeriesCollection = new SeriesCollection
				{
					new ColumnSeries
					{
						Title = "Quantity",
						Values = sampleQuantityValues,
						DataLabels = true,
						Fill = brush
					}
				};
				//SamplesFormatter = value => value.ToString("N");
				//SamplesFormatter = value => value + " m3";
			}
		}

		public SeriesCollection EquipmentSeriesCollection { get; set; }
		public ObservableCollection<string> EquipmentLabels { get; set; }
		public Func<double, string> EquipmentFormatter { get; set; }

		private void FillInEquipmentChart()
		{
			using(NIIDbContext db = new NIIDbContext())
			{
				List<Equipment> equipment = db.Equipment.Include("Projects").ToList();
				ChartValues<ObservableValue> pieceOfEquipmentQuantityValues = new ChartValues<ObservableValue>();
				EquipmentLabels = new ObservableCollection<string>();
				foreach(Equipment pieceOfEquipment in equipment)
				{
					pieceOfEquipmentQuantityValues.Add(new ObservableValue(pieceOfEquipment.Quantity));
					EquipmentLabels.Add(pieceOfEquipment.Title);
				}

				var converter = new System.Windows.Media.BrushConverter();
				var brush = (Brush)converter.ConvertFromString("#D2B48C");
				EquipmentSeriesCollection = new SeriesCollection
				{
					new ColumnSeries
					{
						Title = "Quantity",
						Values = pieceOfEquipmentQuantityValues,
						DataLabels = true,
						Fill = brush
					}
				};
			}
		}
		#endregion


		public DbDataObservableCollections_and_ChartsFunctionality()
		{						
			RefreshDbEntitiesCollections();
			Create_Scientists_Technicians_Position_Qualification_Collections();
			FillInSamplesChart();
			FillInEquipmentChart();
		}
	}
}
