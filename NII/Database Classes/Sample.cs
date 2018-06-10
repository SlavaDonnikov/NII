using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NII.Database_Classes
{
    [Table("Tb_Samples")]
    public class Sample
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

		[Column(TypeName = "VARCHAR", Order = 1)]
		[MaxLength(100, ErrorMessage = "Title must be 100 characters or less")]
		[Required(ErrorMessage = "'Title' field is required!")]
		public string Title { get; set; }

        [Column(TypeName = "VARCHAR", Order = 2)]
        [Required(ErrorMessage = "'Description' field is required!")]
        public string Description { get; set; }

        [Column(TypeName = "VARCHAR", Order = 3)]
		[Required(ErrorMessage = "'Quantity' field is required!")]
		public string Quantity { get; set; }
        		
		// Many-to-many <Project>
		public virtual ICollection<Project> Projects { get; set; }
		public Sample()
		{
            this.Projects = new HashSet<Project>();
		}

		[Column(TypeName = "DateTime2", Order = 4)]
		[Required(ErrorMessage = "'AddOrUpdateDate' field is required! This field must be set automatically!")]
		[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime AddOrUpdateDate { get; set; }          // defaultValueSql: "GETDATE()"

		[Timestamp]
		[Column(Order = 5)]
		public byte[] RowVersion { get; set; }
	}
}
