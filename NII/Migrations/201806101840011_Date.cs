namespace NII.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Date : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tb_Projects", "DateOfBeginning", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Tb_Projects", "DateOfEnding", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Tb_Scientists", "DateOfEmployment", c => c.DateTime(nullable: false, storeType: "date"));
            AlterColumn("dbo.Tb_Technicians", "DateOfEmployment", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tb_Technicians", "DateOfEmployment", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Tb_Scientists", "DateOfEmployment", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Tb_Projects", "DateOfEnding", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Tb_Projects", "DateOfBeginning", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
