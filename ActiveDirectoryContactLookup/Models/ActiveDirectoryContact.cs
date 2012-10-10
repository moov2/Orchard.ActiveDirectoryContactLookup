using System;
using System.Collections.Generic;

namespace ActiveDirectoryContactLookup.Models
{
    public class ActiveDirectoryContact
    {
        public string Company { get; set; }
        public string DisplayName { get; set; }
        public string DistinguishedName { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string FirstName { get; set; }
        public string HomePhoneNumber { get; set; }
        public string IpPhoneNumber { get; set; }
        public string JobRole { get; set; }
        public string LastName { get; set; }
        public ActiveDirectoryContact Manager { get; set; }
        public IList<ActiveDirectoryContact> Manages { get; set; }
        public string Mobile { get; set; }
        public string Office { get; set; }
        public string Pager { get; set; }
        public string Telephone { get; set; }
        public string Username { get; set; }
    }
}