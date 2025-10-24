using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeBook.Infrastructure.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_USER, "Criacao da tabela do usuario")]
    public class Version001 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Users")
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("Email").AsString(100).NotNullable()
                .WithColumn("Password").AsString(255).NotNullable()
                .WithColumn("UserIdentifier").AsGuid().NotNullable();
        }
    }
}
