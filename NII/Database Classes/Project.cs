using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NII.Database_Classes
{
    [Table("Tb_Projects")]
    public class Project
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR", Order = 1)]
        [MaxLength(300, ErrorMessage = "Name must be 300 characters or less!")]
        [Required(ErrorMessage = "'Name' field is required!")]
        public string Name { get; set; }

        [Column(TypeName = "VARCHAR", Order = 2)]
        [MaxLength(50, ErrorMessage = "CodeName must be 50 characters or less!")]
        [Required(ErrorMessage = "'CodeName' field is required!")]
        public string CodeName { get; set; }

        [Column(TypeName = "INT", Order = 3)]   // Срок        
        [Required(ErrorMessage = "'Term' field is required! Enter the duration of the project in days!")]
        public int Term { get; set; }

		[Column(TypeName = "VARCHAR", Order = 4)]
		[Required(ErrorMessage = "'Location' field is required!")]  // The location of the scientific project == Address
		public string Location { get; set; }

		[Column(TypeName = "DateTime2(7)", Order = 5)]
        [Required(ErrorMessage = "'DateOfBeginning' field is required!")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateOfBeginning { get; set; }
               
        [Column(TypeName = "DateTime2(7)", Order = 6)]
        [Required(ErrorMessage = "'DateOfEnding' field is required! DateOfEnding = DateOfBeginning + Term")]        
        public DateTime DateOfEnding
        {
            get { return this.DateOfBeginning.AddDays(this.Term); }
            private set { }            
        }
        private DateTime dateOfEnding;

        [Column(TypeName = "decimal(18,2)", Order = 7)]       
        [Required(ErrorMessage = "'Cost' field is required! Cost : decimal(18,2)")]
        public decimal Cost { get; set; }

		// Many-to-many <Scientist> <Technician>
		public virtual ICollection<Scientist> Scientists { get; set; }
        public virtual ICollection<Technician> Technicians { get; set; }
        public virtual ICollection<Sample> Samples { get; set; }
        public virtual ICollection<Equipment> Equipments { get; set; }
        
        public Project()
        {
            Scientists = new HashSet<Scientist>();
            Technicians = new HashSet<Technician>();
            Samples = new HashSet<Sample>();
            Equipments = new HashSet<Equipment>();
        }

		[Column(TypeName = "DateTime2(7)", Order = 8)]
		[Required(ErrorMessage = "'AddOrUpdateDate' field is required! This field must be set automatically!")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime AddOrUpdateDate { get; set; }          // defaultValueSql: "GETDATE()"

		[Timestamp]
        [Column(Order = 9)]
        public byte[] RowVersion { get; set; }
    }
}
