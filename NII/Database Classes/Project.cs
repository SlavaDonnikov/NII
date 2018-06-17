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

        [Column(TypeName = "VARCHAR", Order = 3)]        
        [Required(ErrorMessage = "'Description' field is required!")]
        public string Description { get; set; }

        [Column(TypeName = "INT", Order = 4)]   // Срок        
        [Required(ErrorMessage = "'Term' field is required! Enter the duration of the project in days!")]
        public int Term { get; set; }

		[Column(TypeName = "VARCHAR", Order = 5)]
		[Required(ErrorMessage = "'Location' field is required!")]  
		public string Location { get; set; }

		[Column(TypeName = "Date", Order = 6)]
        [Required(ErrorMessage = "'DateOfBeginning' field is required!")]		
        public DateTime DateOfBeginning { get; set; }
               
        [Column(TypeName = "Date", Order = 7)]
        [Required(ErrorMessage = "'DateOfEnding' field is required! DateOfEnding = DateOfBeginning + Term")]        
        public DateTime DateOfEnding
        {
            get { return this.DateOfBeginning.AddDays(this.Term); }
            private set { }            
        }        

        [Column(TypeName = "decimal", Order = 8)]       
        [Required(ErrorMessage = "'Cost' field is required! Cost : decimal(18,2)")]
        public decimal Cost { get; set; }

		// Many-to-many
		public virtual ICollection<Scientist> Scientists { get; set; }
        public virtual ICollection<Technician> Technicians { get; set; }
        public virtual ICollection<Sample> Samples { get; set; }
        public virtual ICollection<Equipment> Equipment { get; set; }
        
        public Project()
        {
            this.Scientists = new HashSet<Scientist>();
            this.Technicians = new HashSet<Technician>();
            this.Samples = new HashSet<Sample>();
            this.Equipment = new HashSet<Equipment>();			
        }

		[Column(TypeName = "DateTime2", Order = 9)]
		[Required(ErrorMessage = "'AddOrUpdateDate' field is required! This field must be set automatically!")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime AddOrUpdateDate { get; set; }          // defaultValueSql: "GETDATE()"

        [Column(TypeName = "VARCHAR", Order = 10)]
        public string ScientistsZ
        {
            get
            {
                string tmp = "";
                if (Scientists != null)
                {
                    foreach (Scientist s in this.Scientists)
                    {
                        tmp += s.Name + "\n";
                    }                   
                }
                return tmp.Trim();
            }
            set { }
        }

        [Column(TypeName = "VARCHAR", Order = 11)]
        public string TechniciansZ
        {
            get
            {
                string tmp = "";
                if(Technicians != null)
                {
                    foreach (Technician t in Technicians)
                    {
                        tmp += t.Name + "\n";
                    }                    
                }                
                return tmp.Trim();
            }
            set { }
        }

        [Column(TypeName = "VARCHAR", Order = 12)]
        public string SamplesZ
        {
            get
            {
                string tmp = "";
                if (Samples != null)
                {
                    foreach (Sample smp in Samples)
                    {
                        tmp += smp.Title + "\n";
                    }                    
                }
                return tmp.Trim();
            }
            set { }
        }

        [Column(TypeName = "VARCHAR", Order = 13)]
        public string EquipmentZ
        {
            get
            {
                string tmp = "";
                if(Equipment != null)
                {
                    foreach (Equipment eq in this.Equipment)
                    {
                        tmp += eq.Title + "\n";
                    }
                }
                return tmp.Trim();
            }
            set { }
        }    

        [Timestamp]
        [Column(Order = 14)]
        public byte[] RowVersion { get; set; }
    }
}
