using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using ActiveDirectoryContactLookup.Models;
using ActiveDirectoryContactLookup.Services;
using ActiveDirectoryContactLookup.ViewModels;
using Orchard.DisplayManagement;
using Orchard.Settings;
using Orchard.UI.Navigation;

namespace ActiveDirectoryContactLookup.Controllers
{
    public class ActiveDirectoryContactLookupController : Controller
    {
        protected readonly IActiveDirectoryContactLookupService ActiveDirectoryContactLookupService;
        protected readonly ISiteService SiteService;

        protected dynamic Shape { get; set; }

        public ActiveDirectoryContactLookupController(IActiveDirectoryContactLookupService activeDirectoryContactLookupService, ISiteService siteService, IShapeFactory shapeFactory)
        {
            ActiveDirectoryContactLookupService = activeDirectoryContactLookupService;
            SiteService = siteService;
            Shape = shapeFactory;
        }

        public ActionResult Index(AdContactLookupIndexOptions options, PagerParameters pagerParameters)
        {
            var pager = new Pager(SiteService.GetSiteSettings(), pagerParameters);

            // if no options are provided use default.
            if (options == null)
                options = new AdContactLookupIndexOptions();

            var contacts = new List<ActiveDirectoryContact>();

            // if a search has been made, retrieve the contacts from the service.
            if (!string.IsNullOrWhiteSpace(options.Search))
                contacts = ActiveDirectoryContactLookupService.GetContacts(options.Search).ToList();

            // filters the contacts so only the contacts that fit the paging parameters
            // are included in the result.
            var pagerShape = Shape.Pager(pager).TotalItemCount(contacts.Count());
            var results = new List<ActiveDirectoryContact>();
            var index = pager.GetStartIndex();

            if (index < contacts.Count)
            {
                var count = pager.PageSize;

                if (index + count > contacts.Count)
                    count = index + (contacts.Count) - index;

                results = contacts.GetRange(index, count).ToList();
            }

            // maintain previous route data when generating page links
            var routeData = new RouteData();
            routeData.Values.Add("Options.Search", options.Search);

            pagerShape.RouteData(routeData);

            return View(new AdContactLookupIndexViewModel(results, options, pagerShape));
        }
    }
}