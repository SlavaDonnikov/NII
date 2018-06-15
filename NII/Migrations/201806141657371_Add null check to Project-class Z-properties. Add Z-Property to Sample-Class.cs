namespace NII.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddnullchecktoProjectclassZpropertiesAddZPropertytoSampleClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tb_Samples", "ProjectsZCode", c => c.String(maxLength: 8000, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tb_Samples", "ProjectsZCode");
        }
    }
}
