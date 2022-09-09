namespace HelpDesk.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CRIAR_BANCO_HELPDESKTCC : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CATEGORIAS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TITULO = c.String(),
                        STATUS = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.CHAMADOS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TITULO = c.String(),
                        STATUS = c.String(),
                        CRIADOPOR = c.String(),
                        CRIADOEM = c.DateTime(nullable: false),
                        ALTERADOPOR = c.String(),
                        ALTERADOEM = c.DateTime(nullable: false),
                        CATEGORIA_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CATEGORIAS", t => t.CATEGORIA_ID)
                .Index(t => t.CATEGORIA_ID);
            
            CreateTable(
                "dbo.CONFIGPLANTOES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        COR = c.String(),
                        USUARIO_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USUARIOS", t => t.USUARIO_ID)
                .Index(t => t.USUARIO_ID);
            
            CreateTable(
                "dbo.USUARIOS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        NOME = c.String(),
                        EMAIL = c.String(),
                        LOGIN = c.String(),
                        SENHA = c.String(),
                        SETOR = c.String(),
                        TOKEN = c.String(),
                        STATUS = c.Boolean(nullable: false),
                        PERFIL = c.String(),
                        REDEFINIRSENHA = c.Boolean(nullable: false),
                        ULTIMOACESSO = c.DateTime(nullable: false),
                        CRIADOPOR = c.String(),
                        CRIADOEM = c.DateTime(nullable: false),
                        ALTERADOPOR = c.String(),
                        ALTERADOEM = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.HISTORICOCHAMADOS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        COMENTARIOS = c.String(),
                        ARQUIVOS = c.String(),
                        CRIADOEM = c.DateTime(nullable: false),
                        CRIADOPOR = c.String(),
                        CHAMADO_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CHAMADOS", t => t.CHAMADO_ID)
                .Index(t => t.CHAMADO_ID);
            
            CreateTable(
                "dbo.LOGS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MENSAGEM = c.String(),
                        METODO = c.String(),
                        CHAMADO = c.String(),
                        STATUS = c.String(),
                        CRIADOPOR = c.String(),
                        CRIADOEM = c.DateTime(nullable: false),
                        ALTERADOPOR = c.String(),
                        ALTERADOEM = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PLANTOES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TITULO = c.String(),
                        DATAINICIO = c.DateTime(nullable: false),
                        DATAFIM = c.DateTime(nullable: false),
                        CRIADOPOR = c.String(),
                        CRIADOEM = c.DateTime(nullable: false),
                        ALTERADOPOR = c.String(),
                        ALTERADOEM = c.DateTime(nullable: false),
                        USUARIO_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.USUARIOS", t => t.USUARIO_ID)
                .Index(t => t.USUARIO_ID);
            
            CreateTable(
                "dbo.SETORES",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TITULO = c.String(),
                        STATUS = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.USUARIOSENVOLVIDOS",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TIPO = c.String(),
                        CHAMADO_ID = c.Int(),
                        USUARIO_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.CHAMADOS", t => t.CHAMADO_ID)
                .ForeignKey("dbo.USUARIOS", t => t.USUARIO_ID)
                .Index(t => t.CHAMADO_ID)
                .Index(t => t.USUARIO_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.USUARIOSENVOLVIDOS", "USUARIO_ID", "dbo.USUARIOS");
            DropForeignKey("dbo.USUARIOSENVOLVIDOS", "CHAMADO_ID", "dbo.CHAMADOS");
            DropForeignKey("dbo.PLANTOES", "USUARIO_ID", "dbo.USUARIOS");
            DropForeignKey("dbo.HISTORICOCHAMADOS", "CHAMADO_ID", "dbo.CHAMADOS");
            DropForeignKey("dbo.CONFIGPLANTOES", "USUARIO_ID", "dbo.USUARIOS");
            DropForeignKey("dbo.CHAMADOS", "CATEGORIA_ID", "dbo.CATEGORIAS");
            DropIndex("dbo.USUARIOSENVOLVIDOS", new[] { "USUARIO_ID" });
            DropIndex("dbo.USUARIOSENVOLVIDOS", new[] { "CHAMADO_ID" });
            DropIndex("dbo.PLANTOES", new[] { "USUARIO_ID" });
            DropIndex("dbo.HISTORICOCHAMADOS", new[] { "CHAMADO_ID" });
            DropIndex("dbo.CONFIGPLANTOES", new[] { "USUARIO_ID" });
            DropIndex("dbo.CHAMADOS", new[] { "CATEGORIA_ID" });
            DropTable("dbo.USUARIOSENVOLVIDOS");
            DropTable("dbo.SETORES");
            DropTable("dbo.PLANTOES");
            DropTable("dbo.LOGS");
            DropTable("dbo.HISTORICOCHAMADOS");
            DropTable("dbo.USUARIOS");
            DropTable("dbo.CONFIGPLANTOES");
            DropTable("dbo.CHAMADOS");
            DropTable("dbo.CATEGORIAS");
        }
    }
}
