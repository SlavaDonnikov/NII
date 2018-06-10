namespace NII.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tb_Scientific_Research_Equipment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100, unicode: false),
                        Description = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Quantity = c.Int(nullable: false),
                        AddOrUpdateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tb_Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 300, unicode: false),
                        CodeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Term = c.Int(nullable: false),
                        Location = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DateOfBeginning = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        DateOfEnding = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AddOrUpdateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tb_Scientific_Samples",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100, unicode: false),
                        Description = c.String(nullable: false, maxLength: 8000, unicode: false),
                        Quantity = c.String(nullable: false, maxLength: 8000, unicode: false),
                        AddOrUpdateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tb_Scientists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150, unicode: false),
                        Age = c.Int(nullable: false),
                        Position = c.String(nullable: false, maxLength: 100, unicode: false),
                        Personal_Identification_Number = c.String(nullable: false, maxLength: 100, unicode: false),
                        Qualification = c.String(nullable: false, maxLength: 100, unicode: false),
                        EducationalBackground = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DateOfEmployment = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AddOrUpdateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tb_Technicians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150, unicode: false),
                        Age = c.Int(nullable: false),
                        Position = c.String(nullable: false, maxLength: 100, unicode: false),
                        Personal_Identification_Number = c.String(nullable: false, maxLength: 100, unicode: false),
                        Qualification = c.String(nullable: false, maxLength: 100, unicode: false),
                        EducationalBackground = c.String(nullable: false, maxLength: 8000, unicode: false),
                        DateOfEmployment = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        AddOrUpdateDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectEquipments",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Equipment_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Equipment_Id })
                .ForeignKey("dbo.Tb_Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tb_Scientific_Research_Equipment", t => t.Equipment_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.Equipment_Id);
            
            CreateTable(
                "dbo.SampleProjects",
                c => new
                    {
                        Sample_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Sample_Id, t.Project_Id })
                .ForeignKey("dbo.Tb_Scientific_Samples", t => t.Sample_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tb_Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Sample_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.ScientistProjects",
                c => new
                    {
                        Scientist_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Scientist_Id, t.Project_Id })
                .ForeignKey("dbo.Tb_Scientists", t => t.Scientist_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tb_Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Scientist_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.TechnicianProjects",
                c => new
                    {
                        Technician_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Technician_Id, t.Project_Id })
                .ForeignKey("dbo.Tb_Technicians", t => t.Technician_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tb_Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Technician_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TechnicianProjects", "Project_Id", "dbo.Tb_Projects");
            DropForeignKey("dbo.TechnicianProjects", "Technician_Id", "dbo.Tb_Technicians");
            DropForeignKey("dbo.ScientistProjects", "Project_Id", "dbo.Tb_Projects");
            DropForeignKey("dbo.ScientistProjects", "Scientist_Id", "dbo.Tb_Scientists");
            DropForeignKey("dbo.SampleProjects", "Project_Id", "dbo.Tb_Projects");
            DropForeignKey("dbo.SampleProjects", "Sample_Id", "dbo.Tb_Scientific_Samples");
            DropForeignKey("dbo.ProjectEquipments", "Equipment_Id", "dbo.Tb_Scientific_Research_Equipment");
            DropForeignKey("dbo.ProjectEquipments", "Project_Id", "dbo.Tb_Projects");
            DropIndex("dbo.TechnicianProjects", new[] { "Project_Id" });
            DropIndex("dbo.TechnicianProjects", new[] { "Technician_Id" });
            DropIndex("dbo.ScientistProjects", new[] { "Project_Id" });
            DropIndex("dbo.ScientistProjects", new[] { "Scientist_Id" });
            DropIndex("dbo.SampleProjects", new[] { "Project_Id" });
            DropIndex("dbo.SampleProjects", new[] { "Sample_Id" });
            DropIndex("dbo.ProjectEquipments", new[] { "Equipment_Id" });
            DropIndex("dbo.ProjectEquipments", new[] { "Project_Id" });
            DropTable("dbo.TechnicianProjects");
            DropTable("dbo.ScientistProjects");
            DropTable("dbo.SampleProjects");
            DropTable("dbo.ProjectEquipments");
            DropTable("dbo.Tb_Technicians");
            DropTable("dbo.Tb_Scientists");
            DropTable("dbo.Tb_Scientific_Samples");
            DropTable("dbo.Tb_Projects");
            DropTable("dbo.Tb_Scientific_Research_Equipment");
        }
    }
}
