namespace ZSPD.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Grades", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Grades", new[] { "QuestionId" });
            RenameColumn(table: "dbo.Grades", name: "QuestionId", newName: "Question_Id");
            AlterColumn("dbo.Grades", "Question_Id", c => c.Int());
            CreateIndex("dbo.Grades", "Question_Id");
            AddForeignKey("dbo.Grades", "Question_Id", "dbo.Questions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Grades", "Question_Id", "dbo.Questions");
            DropIndex("dbo.Grades", new[] { "Question_Id" });
            AlterColumn("dbo.Grades", "Question_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Grades", name: "Question_Id", newName: "QuestionId");
            CreateIndex("dbo.Grades", "QuestionId");
            AddForeignKey("dbo.Grades", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
        }
    }
}
