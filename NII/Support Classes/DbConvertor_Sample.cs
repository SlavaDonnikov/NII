using NII.Database_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NII.Support_Classes
{
    class DbConvertor_Sample
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public DateTime AddOrUpdateDate { get; set; }

        public ICollection<Project> Projects { get; set; }
        public string ProjectsZCode
        {
            get
            {
                string tmp = "";
                foreach (Project prj in Projects)
                {
                    tmp += prj.CodeName + "\n";
                }
                return tmp;
            }
            set { }
        }
    }
}
