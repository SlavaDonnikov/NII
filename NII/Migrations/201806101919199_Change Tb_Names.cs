namespace NII.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTb_Names : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Tb_Scientific_Research_Equipment", newName: "Tb_Equipment");
            RenameTable(name: "dbo.Tb_Scientific_Samples", newName: "Tb_Samples");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Tb_Samples", newName: "Tb_Scientific_Samples");
            RenameTable(name: "dbo.Tb_Equipment", newName: "Tb_Scientific_Research_Equipment");
        }
    }
}
