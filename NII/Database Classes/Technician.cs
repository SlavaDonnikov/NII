using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NII.Database_Classes
{
    [Table("Tb_Technicians")]
    public class Technician
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

		[Column(TypeName = "VARCHAR", Order = 1)]
		[MaxLength(150, ErrorMessage = "Name must be 150 characters or less")]
		[Required(ErrorMessage = "'Name' field is required!")]
		public string Name { get; set; }

		[Column(TypeName = "INT", Order = 2)]
		[Required(ErrorMessage = "'Age' field is required!")]
		public int Age { get; set; }

		[Column(TypeName = "VARCHAR", Order = 3)]
		[MaxLength(100, ErrorMessage = "Position must be 100 characters or less")]
		[Required(ErrorMessage = "'Position' field is required!")]
		public string Position { get; set; }    

		[Column(TypeName = "VARCHAR", Order = 4)]
		[MaxLength(100, ErrorMessage = "Personal Identification Number must be 100 characters or less")]
		[Required(ErrorMessage = "'Personal_Identification'_Number field is required!")]
		public string Personal_Identification_Number { get; set; }

		[Column(TypeName = "VARCHAR", Order = 5)]
		[MaxLength(100, ErrorMessage = "Qualification must be 100 characters or less")]
		[Required(ErrorMessage = "'Qualification' field is required!")]
		public string Qualification { get; set; }

        [Column(TypeName = "VARCHAR", Order = 6)]
        [Required(ErrorMessage = "'EducationalBackground' field is required!")]
        public string EducationalBackground { get; set; }

        [Column(TypeName = "Date", Order = 7)]
		[Required(ErrorMessage = "'DateOfEmployment' field is required!")]
		public DateTime DateOfEmployment { get; set; }

		// Many-to-many <Project>
		public virtual ICollection<Project> Projects { get; set; }
		public Technician()
		{
            this.Projects = new HashSet<Project>();
		}

		[Column(TypeName = "DateTime2", Order = 8)]
		[Required(ErrorMessage = "'AddOrUpdateDate' field is required! This field must be set automatically!")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime AddOrUpdateDate { get; set; }          // defaultValueSql: "GETDATE()"

        [Column(TypeName = "VARCHAR", Order = 9)]
        public string ProjectsZCode
        {
            get
            {
                string tmp = "";
                if (Projects != null)
                {
                    foreach (Project prj in Projects)
                    {
                        tmp += prj.CodeName + "\n";
                    }                    
                }
                return tmp.Trim();
            }
            set { }
        }

        [Timestamp]
		[Column(Order = 10)]
		public byte[] RowVersion { get; set; }
	}
}
