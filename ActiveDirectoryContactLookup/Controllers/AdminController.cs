using System.Web.Mvc;
using Orchard.DisplayManagement;
using Orchard.Settings;
using ActiveDirectoryContactLookup.Services;

namespace ActiveDirectoryContactLookup.Controllers
{
    [ValidateInput(false)]
    public class AdminController : ActiveDirectoryContactLookupController
    {
        public AdminController(IActiveDirectoryContactLookupService activeDirectoryContactLookupService, ISiteService siteService, IShapeFactory shapeFactory)
            : base(activeDirectoryContactLookupService, siteService, shapeFactory)
        {

        }
    }
}