namespace NII.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EquipmentstoEquipmentSampleQuantitytypeVARCHARtotypeINT : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Tb_Equipments", newName: "Tb_Equipment");
            AddColumn("dbo.Tb_Projects", "EquipmentZ", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Tb_Samples", "Quantity", c => c.Int(nullable: false));
            DropColumn("dbo.Tb_Projects", "EquipmentsZ");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tb_Projects", "EquipmentsZ", c => c.String(maxLength: 8000, unicode: false));
            AlterColumn("dbo.Tb_Samples", "Quantity", c => c.String(nullable: false, maxLength: 8000, unicode: false));
            DropColumn("dbo.Tb_Projects", "EquipmentZ");
            RenameTable(name: "dbo.Tb_Equipment", newName: "Tb_Equipments");
        }
    }
}
