namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pessoa",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UltimoNome = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        DataMatricula = c.DateTime(),
                        DataContratacao = c.DateTime(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Matricula",
                c => new
                    {
                        MatriculaID = c.Int(nullable: false, identity: true),
                        CursoID = c.Int(nullable: false),
                        AlunoID = c.Int(nullable: false),
                        Grade = c.Int(),
                    })
                .PrimaryKey(t => t.MatriculaID)
                .ForeignKey("dbo.Pessoa", t => t.AlunoID, cascadeDelete: true)
                .ForeignKey("dbo.Curso", t => t.CursoID, cascadeDelete: true)
                .Index(t => t.CursoID)
                .Index(t => t.AlunoID);
            
            CreateTable(
                "dbo.Curso",
                c => new
                    {
                        CursoID = c.Int(nullable: false),
                        Titulo = c.String(maxLength: 50),
                        Creditos = c.Int(nullable: false),
                        DepartamentoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CursoID)
                .ForeignKey("dbo.Departamento", t => t.DepartamentoID, cascadeDelete: true)
                .Index(t => t.DepartamentoID);
            
            CreateTable(
                "dbo.Departamento",
                c => new
                    {
                        DepartamentoID = c.Int(nullable: false, identity: true),
                        Nome = c.String(maxLength: 50),
                        Despesas = c.Decimal(nullable: false, storeType: "money"),
                        DataInicio = c.DateTime(nullable: false),
                        InstrutorID = c.Int(),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.DepartamentoID)
                .ForeignKey("dbo.Pessoa", t => t.InstrutorID)
                .Index(t => t.InstrutorID);
            
            CreateTable(
                "dbo.Escritorio",
                c => new
                    {
                        InstrutorID = c.Int(nullable: false),
                        Localizacao = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.InstrutorID)
                .ForeignKey("dbo.Pessoa", t => t.InstrutorID)
                .Index(t => t.InstrutorID);
            
            CreateTable(
                "dbo.CursoInstrutor",
                c => new
                    {
                        CursoID = c.Int(nullable: false),
                        InstrutorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CursoID, t.InstrutorID })
                .ForeignKey("dbo.Curso", t => t.CursoID, cascadeDelete: true)
                .ForeignKey("dbo.Pessoa", t => t.InstrutorID, cascadeDelete: true)
                .Index(t => t.CursoID)
                .Index(t => t.InstrutorID);
            
            CreateStoredProcedure(
                "dbo.Departamento_Insert",
                p => new
                    {
                        Nome = p.String(maxLength: 50),
                        Despesas = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        DataInicio = p.DateTime(),
                        InstrutorID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Departamento]([Nome], [Despesas], [DataInicio], [InstrutorID])
                      VALUES (@Nome, @Despesas, @DataInicio, @InstrutorID)
                      
                      DECLARE @DepartamentoID int
                      SELECT @DepartamentoID = [DepartamentoID]
                      FROM [dbo].[Departamento]
                      WHERE @@ROWCOUNT > 0 AND [DepartamentoID] = scope_identity()
                      
                      SELECT t0.[DepartamentoID], t0.[RowVersion]
                      FROM [dbo].[Departamento] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartamentoID] = @DepartamentoID"
            );
            
            CreateStoredProcedure(
                "dbo.Departamento_Update",
                p => new
                    {
                        DepartamentoID = p.Int(),
                        Nome = p.String(maxLength: 50),
                        Despesas = p.Decimal(precision: 19, scale: 4, storeType: "money"),
                        DataInicio = p.DateTime(),
                        InstrutorID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"UPDATE [dbo].[Departamento]
                      SET [Nome] = @Nome, [Despesas] = @Despesas, [DataInicio] = @DataInicio, [InstrutorID] = @InstrutorID
                      WHERE (([DepartamentoID] = @DepartamentoID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))
                      
                      SELECT t0.[RowVersion]
                      FROM [dbo].[Departamento] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[DepartamentoID] = @DepartamentoID"
            );
            
            CreateStoredProcedure(
                "dbo.Departamento_Delete",
                p => new
                    {
                        DepartamentoID = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Departamento]
                      WHERE (([DepartamentoID] = @DepartamentoID) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Departamento_Delete");
            DropStoredProcedure("dbo.Departamento_Update");
            DropStoredProcedure("dbo.Departamento_Insert");
            DropForeignKey("dbo.Matricula", "CursoID", "dbo.Curso");
            DropForeignKey("dbo.CursoInstrutor", "InstrutorID", "dbo.Pessoa");
            DropForeignKey("dbo.CursoInstrutor", "CursoID", "dbo.Curso");
            DropForeignKey("dbo.Curso", "DepartamentoID", "dbo.Departamento");
            DropForeignKey("dbo.Departamento", "InstrutorID", "dbo.Pessoa");
            DropForeignKey("dbo.Escritorio", "InstrutorID", "dbo.Pessoa");
            DropForeignKey("dbo.Matricula", "AlunoID", "dbo.Pessoa");
            DropIndex("dbo.CursoInstrutor", new[] { "InstrutorID" });
            DropIndex("dbo.CursoInstrutor", new[] { "CursoID" });
            DropIndex("dbo.Escritorio", new[] { "InstrutorID" });
            DropIndex("dbo.Departamento", new[] { "InstrutorID" });
            DropIndex("dbo.Curso", new[] { "DepartamentoID" });
            DropIndex("dbo.Matricula", new[] { "AlunoID" });
            DropIndex("dbo.Matricula", new[] { "CursoID" });
            DropTable("dbo.CursoInstrutor");
            DropTable("dbo.Escritorio");
            DropTable("dbo.Departamento");
            DropTable("dbo.Curso");
            DropTable("dbo.Matricula");
            DropTable("dbo.Pessoa");
        }
    }
}
