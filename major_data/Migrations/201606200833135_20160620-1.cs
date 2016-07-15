namespace major_data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606201 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "Department_Id", newName: "DepartmentId");
            RenameColumn(table: "dbo.AspNetUsers", name: "Secshondeportament_Id", newName: "SecshondeportamentId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Department_Id", newName: "IX_DepartmentId");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Secshondeportament_Id", newName: "IX_SecshondeportamentId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_SecshondeportamentId", newName: "IX_Secshondeportament_Id");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_DepartmentId", newName: "IX_Department_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "SecshondeportamentId", newName: "Secshondeportament_Id");
            RenameColumn(table: "dbo.AspNetUsers", name: "DepartmentId", newName: "Department_Id");
        }
    }
}
