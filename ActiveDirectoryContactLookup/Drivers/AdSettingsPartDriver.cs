using System;
using System.DirectoryServices;
using ActiveDirectoryContactLookup.Models;
using ActiveDirectoryContactLookup.Services;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;
using Orchard.Localization;

namespace ActiveDirectoryContactLookup.Drivers
{
    public class AdSettingsPartDriver : ContentPartDriver<AdSettingsPart>
    {
        private const string TemplateName = "Parts/AdSettings";
        
        public AdSettingsPartDriver()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        protected override string Prefix { get { return "AdSettings"; } }

        protected override DriverResult Editor(AdSettingsPart part, dynamic shapeHelper)
        {
            return ContentShape("Parts_AdSettings_Edit",
                () => shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix))
                .OnGroup("ActiveDirectorySettings");
        }

        protected override DriverResult Editor(AdSettingsPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            return ContentShape("Parts_AdSettings_Edit", () =>
            {
                var previousPassword = part.Password;
                updater.TryUpdateModel(part, Prefix, null, null);

                //restore password if the input is empty, meaning it has not been reset
                if (string.IsNullOrEmpty(part.Password))
                    part.Password = previousPassword;

                part.Connectable = CanConnect(part);

                if (!part.Connectable)
                    updater.AddModelError("connectionError", T("Unable to connect to Active Directory."));

                return shapeHelper.EditorTemplate(TemplateName: TemplateName, Model: part, Prefix: Prefix);
            })
            .OnGroup("ActiveDirectorySettings");
        }

        /// <summary>
        /// Attempts to establish a connection with the active directory
        /// data store using the connection parameters provided.
        /// </summary>
        /// <param name="part">Contains the connection parameters.</param>
        /// <returns>True if a connection can be made, otherwise false.</returns>
        private bool CanConnect(AdSettingsPart part)
        {
            try {
                var entry = new DirectoryEntry(part.DomainAddress, part.Username, part.Password);
                var nativeObject = entry.NativeObject;
                entry.Dispose();
                   
                return true;
            } catch (Exception ex) {
                return false;
            }
        }
    }
}