using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace ActiveDirectoryContactLookup
{
    public class Routes : IRouteProvider
    {

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                             new RouteDescriptor {
                                                     Name = "AdContactLookupSearchUsers",
                                                     Priority = 5,
                                                     Route = new Route( 
                                                         "ContactDirectory",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ActiveDirectoryContactLookup"},
                                                                                      {"controller", "contactlookup"},
                                                                                      {"action", "index"}
                                                                                  },
                                                         null,
                                                         new RouteValueDictionary {
                                                                                      {"area", "ActiveDirectoryContactLookup"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 },
                                                 new RouteDescriptor { Name="AdContactLookupGetUser",
                                                     Priority = 5,
                                                     Route = new Route(
                                                         "ContactDirectory/Contact/{activeDirectoryDistinguishedName}",
                                                         new RouteValueDictionary {
                                                                                      {"area", "ActiveDirectoryContactLookup"},
                                                                                      {"controller", "contactlookup"},
                                                                                      {"action", "getuser"}
                                                                                  },
                                                         null,
                                                         new RouteValueDictionary {
                                                                                      {"area", "ActiveDirectoryContactLookup"}
                                                                                  },
                                                         new MvcRouteHandler())
                                                 }
                         };
        }

        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);
        }
    }
}