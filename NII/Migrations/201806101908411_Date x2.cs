namespace NII.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Datex2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tb_Projects", "DateOfBeginning", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tb_Projects", "DateOfBeginning", c => c.DateTime(nullable: false, storeType: "date"));
        }
    }
}
