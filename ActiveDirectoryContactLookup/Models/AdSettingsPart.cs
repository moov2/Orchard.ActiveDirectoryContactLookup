using System;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Utilities;

namespace ActiveDirectoryContactLookup.Models
{
    public class AdSettingsPart : ContentPart<AdSettingsPartRecord>
    {
        private readonly ComputedField<string> _password = new ComputedField<string>();

        public ComputedField<string> PasswordField
        {
            get { return _password; }
        }

        public string DomainAddress
        {
            get {
                return (Record.DomainAddress.StartsWith("LDAP://")) ? Record.DomainAddress : "LDAP://" + Record.DomainAddress;
            }
            set { Record.DomainAddress = value; }
        }

        public string Username
        {
            get { return Record.Username; }
            set { Record.Username = value; }
        }

        public string Password
        {
            get { return _password.Value; }
            set { _password.Value = value; }
        }

        public bool Connectable
        {
            get { return Record.Connectable; }
            set { Record.Connectable = value; }
        }

        public bool IsValid()
        {
            return !String.IsNullOrWhiteSpace(Record.DomainAddress)
                && !String.IsNullOrWhiteSpace(Record.Username)
                && !String.IsNullOrWhiteSpace(Record.Password)
                && Connectable;
        }
    }
}