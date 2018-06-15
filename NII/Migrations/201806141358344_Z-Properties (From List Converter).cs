namespace NII.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZPropertiesFromListConverter : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Tb_Equipment", newName: "Tb_Equipments");
            AddColumn("dbo.Tb_Projects", "ScientistsZ", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Tb_Projects", "TechniciansZ", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Tb_Projects", "SamplesZ", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Tb_Projects", "EquipmentsZ", c => c.String(maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tb_Projects", "EquipmentsZ");
            DropColumn("dbo.Tb_Projects", "SamplesZ");
            DropColumn("dbo.Tb_Projects", "TechniciansZ");
            DropColumn("dbo.Tb_Projects", "ScientistsZ");
            RenameTable(name: "dbo.Tb_Equipments", newName: "Tb_Equipment");
        }
    }
}
