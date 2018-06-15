namespace NII.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddZPropertiestoScientistTechnicianEquipmentclasses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tb_Equipments", "ProjectsZCode", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Tb_Scientists", "ProjectsZCode", c => c.String(maxLength: 8000, unicode: false));
            AddColumn("dbo.Tb_Technicians", "ProjectsZCode", c => c.String(maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tb_Technicians", "ProjectsZCode");
            DropColumn("dbo.Tb_Scientists", "ProjectsZCode");
            DropColumn("dbo.Tb_Equipments", "ProjectsZCode");
        }
    }
}
