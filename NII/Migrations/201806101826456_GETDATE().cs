namespace NII.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GETDATE : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tb_Scientific_Research_Equipment", "AddOrUpdateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Tb_Projects", "AddOrUpdateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Tb_Scientific_Samples", "AddOrUpdateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Tb_Scientists", "AddOrUpdateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Tb_Technicians", "AddOrUpdateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tb_Technicians", "AddOrUpdateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Tb_Scientists", "AddOrUpdateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Tb_Scientific_Samples", "AddOrUpdateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Tb_Projects", "AddOrUpdateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
            AlterColumn("dbo.Tb_Scientific_Research_Equipment", "AddOrUpdateDate", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
    }
}
