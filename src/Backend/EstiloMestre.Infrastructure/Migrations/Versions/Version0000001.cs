using FluentMigrator;

namespace EstiloMestre.Infrastructure.Migrations.Versions;

[Migration(1, "create table to save the users information")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
        CreateTable("Users")
            .WithColumn("UserIdentifier").AsGuid().NotNullable()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable()
            .WithColumn("Phone").AsString(255).NotNullable();
        
        CreateTable("Owners")
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_Owner_UserId", "Users", "Id");
        
        CreateTable("Barbershops")
            .WithColumn("BarbershopName").AsString(255).NotNullable()
            .WithColumn("Address").AsString(255).NotNullable()
            .WithColumn("Phone").AsString(255).Nullable()
            .WithColumn("OwnerId").AsInt64().NotNullable().ForeignKey("FK_Barbershop_OwnerId", "Owners", "Id");
    }
}