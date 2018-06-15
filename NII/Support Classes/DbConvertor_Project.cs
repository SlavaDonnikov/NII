using NII.Database_Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NII.Support_Classes
{
    class DbConvertor_Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }
        public string Description { get; set; }
        public int Term { get; set; }
        public string Location { get; set; }
        public DateTime DateOfBeginning { get; set; }
        public DateTime DateOfEnding
        {
            get { return this.DateOfBeginning.AddDays(this.Term); }
            set { }
        }
        public decimal Cost { get; set; }
        public DateTime AddOrUpdateDate { get; set; }

        public ICollection<Scientist> Scientists { get; set; }
        public ICollection<Technician> Technicians { get; set; }
        public ICollection<Sample> Samples { get; set; }
        public ICollection<Equipment> Equipments { get; set; }

        public string ScientistsZ
        {
            get
            {
                string tmp = "";
                foreach (Scientist s in this.Scientists)
                {
                    tmp += s.Name + "\n";
                }
                return tmp;
            }
            set { }
        }

        public string TechniciansZ
        {
            get
            {
                string tmp = "";
                foreach (Technician t in Technicians)
                {
                    tmp += t.Name + "\n";
                }
                return tmp;
            }
            set { }
        }

        public string SamplesZ
        {
            get
            {
                string tmp = "";
                foreach (Sample smp in Samples)
                {
                    tmp += smp.Title + "\n";
                }
                return tmp;
            }
            set { }
        }

        public string EquipmentsZ
        {
            get
            {
                string tmp = "";
                foreach (Equipment eq in this.Equipments)
                {
                    tmp += eq.Title + "\n";
                }
                return tmp;
            }
            set { }
        }       
    }
}
