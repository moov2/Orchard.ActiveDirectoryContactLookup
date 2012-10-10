using System.Web.Mvc;
using ActiveDirectoryContactLookup.Services;
using Orchard.DisplayManagement;
using Orchard.Settings;
using Orchard.Themes;

namespace ActiveDirectoryContactLookup.Controllers
{
    [ValidateInput(false), Themed]
    public class ContactLookupController : ActiveDirectoryContactLookupController
    {
        public ContactLookupController(IActiveDirectoryContactLookupService activeDirectoryContactLookupService, ISiteService siteService, IShapeFactory shapeFactory)
            : base(activeDirectoryContactLookupService, siteService, shapeFactory)
        {

        }

        public ActionResult GetUser(string activeDirectoryDistinguishedName)
        {
            var user = ActiveDirectoryContactLookupService.GetContactByDistinguishedName(activeDirectoryDistinguishedName);
            
            if (user == null) 
                return HttpNotFound("Active Directory User could not be found.");

            return View(user);
        }
    }
}