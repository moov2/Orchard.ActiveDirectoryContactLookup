using Orchard.ContentManagement.Records;

namespace ActiveDirectoryContactLookup.Models
{
    public class AdSettingsPartRecord : ContentPartRecord
    {
        /// <summary>
        /// Location of the LDAP AD server
        /// </summary>
        public virtual string DomainAddress { get; set; }

        /// <summary>
        /// Username for LDAP access
        /// </summary>
        public virtual string Username { get; set; }

        /// <summary>
        /// Password for LDAP access
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Flag that indicates whether the connection settings
        /// entered by the user are valid.
        /// </summary>
        public virtual bool Connectable { get; set; }
    }
}