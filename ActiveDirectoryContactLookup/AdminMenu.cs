using Orchard.Localization;
using Orchard.UI.Navigation;

namespace ActiveDirectoryContactLookup
{
    public class AdminMenu:INavigationProvider
    {
        public Localizer T { get; set; }

        public string MenuName { get { return "admin"; } }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.AddImageSet("adcontactlookup")
                .Add(T("Active Directory Contacts"), "10",
                    menu => menu.Add(T("List"), "0", item => item.Action("Index", "Admin", new { area = "ActiveDirectoryContactLookup" })
                        .Permission(Permissions.SearchContacts)));
        }
    }
}