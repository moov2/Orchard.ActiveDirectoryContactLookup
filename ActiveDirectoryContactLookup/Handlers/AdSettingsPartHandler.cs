using System;
using System.Text;
using ActiveDirectoryContactLookup.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Security;

namespace ActiveDirectoryContactLookup.Handlers
{
    public class AdSettingsPartHandler : ContentHandler
    {
        private readonly IEncryptionService _encryptionService;

        public Localizer T { get; set; }
        public new ILogger Logger { get; set; }

        public AdSettingsPartHandler(IRepository<AdSettingsPartRecord> repository, IEncryptionService encryptionService)
        {
            T = NullLocalizer.Instance;
            Logger = NullLogger.Instance;

            _encryptionService = encryptionService;

            Filters.Add(new ActivatingFilter<AdSettingsPart>("Site"));
            Filters.Add(StorageFilter.For(repository));

            OnLoaded<AdSettingsPart>(LazyLoadHandlers);
        }

        void LazyLoadHandlers(LoadContentContext context, AdSettingsPart part)
        {
            part.PasswordField.Getter(() =>
            {
                try
                {
                    return string.IsNullOrWhiteSpace(part.Record.Password) ? string.Empty : Encoding.UTF8.GetString(_encryptionService.Decode(Convert.FromBase64String(part.Record.Password)));
                }
                catch
                {
                    Logger.Error("The Ad Settings password could not be decrypted. It might be corrupted, try to reset it.");
                    return null;
                }
            });

            part.PasswordField.Setter(value => part.Record.Password = string.IsNullOrWhiteSpace(value) ? string.Empty : Convert.ToBase64String(_encryptionService.Encode(Encoding.UTF8.GetBytes(value))));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            if (context.ContentItem.ContentType != "Site")
                return;

            base.GetItemMetadata(context);

            var groupInfo = new GroupInfo(T("ActiveDirectorySettings"));
            groupInfo.Name = T("Active Directory");

            context.Metadata.EditorGroupInfo.Add(groupInfo);
        }
    }
}