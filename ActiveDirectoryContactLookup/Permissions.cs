using System.Collections.Generic;
using Orchard.Environment.Extensions.Models;
using Orchard.Security.Permissions;

namespace ActiveDirectoryContactLookup
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission SearchContacts = new Permission { Description = "Search Contacts", Name = "SearchContacts" };

        public virtual Feature Feature { get; set; }

        public IEnumerable<Permission> GetPermissions()
        {
            return new[] {
                SearchContacts
            };
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[] {
                new PermissionStereotype {
                    Name = "Administrator",
                    Permissions = new[] {SearchContacts}
                },
                new PermissionStereotype {
                    Name = "Authenticated",
                    Permissions = new[] {SearchContacts}
                },
                new PermissionStereotype {
                    Name = "Editor",
                    Permissions = new[] {SearchContacts}
                },
                new PermissionStereotype {
                    Name = "Moderator",
                    Permissions = new[] {SearchContacts}
                },
                new PermissionStereotype {
                    Name = "Author",
                    Permissions = new[] {SearchContacts}
                },
                new PermissionStereotype {
                    Name = "Contributor",
                    Permissions = new[] {SearchContacts}
                }
            };
        }
    }
}