using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HelpDesk.Models.Entities;
using MySql.Data.EntityFramework;

namespace HelpDesk.Models
{
    public class ConnectBD : DbContext
    {
        static ConnectBD()
        {
            Database.SetInitializer<ConnectBD>(null);
        }

        public ConnectBD() : base("name=ConnectBD") { }

        public virtual DbSet<CHAMADOS> CHAMADOS { get; set; }
        public virtual DbSet<USUARIOS> USUARIOS { get; set; }
        public virtual DbSet<USUARIOSENVOLVIDOS> USUARIOSENVOLVIDOS { get; set; }
        public virtual DbSet<SETORES> SETORES { get; set; }
        public virtual DbSet<CATEGORIAS> CATEGORIAS { get; set; }
        public virtual DbSet<HISTORICOCHAMADOS> HISTORICOCHAMADOS { get; set; }
        public virtual DbSet<PLANTOES> PLANTOES { get; set; }
        public virtual DbSet<CONFIGPLANTOES> CONFIGPLANTOES { get; set; }
        public virtual DbSet<LOGS> LOGS { get; set; }
    }
}


