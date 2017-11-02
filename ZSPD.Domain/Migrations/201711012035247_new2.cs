namespace ZSPD.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Grades",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                        Psychologist_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Psychologists", t => t.Psychologist_Id)
                .Index(t => t.QuestionId)
                .Index(t => t.Psychologist_Id);
            
            DropColumn("dbo.Questions", "AverageGrade");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Questions", "AverageGrade", c => c.Double(nullable: false));
            DropForeignKey("dbo.Grades", "Psychologist_Id", "dbo.Psychologists");
            DropForeignKey("dbo.Grades", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Grades", new[] { "Psychologist_Id" });
            DropIndex("dbo.Grades", new[] { "QuestionId" });
            DropTable("dbo.Grades");
        }
    }
}
