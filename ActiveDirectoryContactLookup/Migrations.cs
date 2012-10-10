using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace ActiveDirectoryContactLookup
{
    public class AdContactsLookupDataMigration : DataMigrationImpl
    {
        public int Create()
        {
            SchemaBuilder.CreateTable("AdSettingsPartRecord",
                table => table
                    .ContentPartRecord()
                    .Column<string>("DomainAddress")
                    .Column<string>("Username")
                    .Column<string>("Password")
                    .Column<bool>("Connectable")
                );

            ContentDefinitionManager.AlterPartDefinition("AdContactLookupPart", cfg => cfg
                .Attachable());

            ContentDefinitionManager.AlterTypeDefinition("AdContactLookupWidget",
                cfg => cfg
                    .Named("AD Contact Lookup Widget")
                    .WithPart("AdContactLookupPart")
                    .WithPart("CommonPart")
                    .WithPart("WidgetPart")
                    .WithSetting("Stereotype", "Widget")
                );

            return 2;
        }

        public int UpdateFrom1()
        {
            SchemaBuilder.AlterTable("AdSettingsPartRecord", table => table
                .AddColumn<bool>("Connectable")
            );

            return 2;
        }
    }
}