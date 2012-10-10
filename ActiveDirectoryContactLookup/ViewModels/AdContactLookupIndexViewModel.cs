using System.Collections.Generic;
using ActiveDirectoryContactLookup.Models;

namespace ActiveDirectoryContactLookup.ViewModels
{
    public class AdContactLookupIndexViewModel
    {
        public IList<ActiveDirectoryContact> Contacts { get; set; }
        public AdContactLookupIndexOptions Options { get; set; }
        public dynamic Pager { get; set; }

        public AdContactLookupIndexViewModel()
        {

        }

        public AdContactLookupIndexViewModel(IList<ActiveDirectoryContact> contacts, AdContactLookupIndexOptions options, dynamic pager)
        {
            Contacts = contacts;
            Options = options;
            Pager = pager;
        }
    }

    public class AdContactLookupIndexOptions
    {
        public string Search { get; set; }
    }
}