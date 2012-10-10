using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using ActiveDirectoryContactLookup.Models;
using ActiveDirectoryContactLookup.ViewModels;
using Orchard;
using Orchard.ContentManagement;

namespace ActiveDirectoryContactLookup.Services
{
    public class ActiveDirectoryContactLookupService : IActiveDirectoryContactLookupService
    {
        private readonly AdSettingsPart _adSettings;

        public ActiveDirectoryContactLookupService(IOrchardServices orchardServices)
        {
            _adSettings = orchardServices.WorkContext.CurrentSite.As<AdSettingsPart>();
        }

        /// <summary>
        /// Adds the ActiveDirectoryContact that represents the Manager and a list of
        /// ActiveDirectoryContacts that represent the contacts that the contact
        /// inputted manages.
        /// </summary>
        /// <param name="contact">Contact that should have its Manages and Manager properties
        /// fulfilled.</param>
        /// <param name="result">Object returned from a query to the active directory store.</param>
        /// <returns>Contact with values set of Manages and Manager.</returns>
        private ActiveDirectoryContact AddManagerHeirachies(ActiveDirectoryContact contact, SearchResult result)
        {
            if (contact == null)
                return null;

            var managerDn = GetProperty(result, "manager");
            var managesDns = GetListProperty(result, "directReports");

            if (!string.IsNullOrWhiteSpace(managerDn))
                contact.Manager = GetContactByDistinguishedName(managerDn, false);

            if (managesDns != null && managesDns.Count > 0)
            {
                contact.Manages = new List<ActiveDirectoryContact>();

                foreach (var dn in managesDns)
                    contact.Manages.Add(GetContactByDistinguishedName(dn, false));
            }

            return contact;
        }

        /// <summary>
        /// Converts a SearchResult returned from a query to the active directory store into
        /// an ActiveDirectoryContact. This method won't include Manager & Manages, see overloaded
        /// method that takes boolean.
        /// </summary>
        /// <param name="result">Object returned from query to active directory store.</param>
        /// <returns>Contact with properties populated.</returns>
        private ActiveDirectoryContact ConvertSearchResultToContact(SearchResult result)
        {
            return ConvertSearchResultToContact(result, false);
        }

        /// <summary>
        /// Converts a SearchResult returned from a query to the active directory store into
        /// an ActiveDirectoryContact including the setting of the Manager & Manages properties
        /// when the withManagerAndManages parameter is set to true.
        /// </summary>
        /// <param name="result">Object returned from query to active directory store.</param>
        /// <param name="withManagerAndManages">Flag indicating whether to set Manager & Manages property.</param>
        /// <returns>Contact with properties populated.</returns>
        private ActiveDirectoryContact ConvertSearchResultToContact(SearchResult result, bool withManagerAndManages)
        {
            if (result == null)
                return null;

            var contact = new ActiveDirectoryContact();
            contact.DistinguishedName = GetProperty(result, "distinguishedName");
            contact.FirstName = GetProperty(result, "givenName");
            contact.LastName = GetProperty(result, "sn");
            contact.Username = GetProperty(result, "cn");
            contact.Telephone = GetProperty(result, "telephoneNumber");
            contact.Email = GetProperty(result, "mail");
            contact.Company = GetProperty(result, "company");
            contact.DisplayName = GetProperty(result, "displayName");
            contact.Office = GetProperty(result, "physicalDeliveryOfficeName");
            contact.Mobile = GetProperty(result, "mobile");
            contact.JobRole = GetProperty(result, "title");
            contact.Fax = GetProperty(result, "facsimileTelephoneNumber");
            contact.HomePhoneNumber = GetProperty(result, "ipPhone");
            contact.IpPhoneNumber = GetProperty(result, "facsimileTelephoneNumber");
            contact.Pager = GetProperty(result, "pager");

            if (withManagerAndManages)
                return AddManagerHeirachies(contact, result);

            return contact;
        }

        /// <summary>
        /// Queries the active directory store returning all the records a
        /// filter that filters the givenName, sn & cn properties based on
        /// the term provided.
        /// </summary>
        /// <param name="term">Term to filter active directory records by.</param>
        /// <returns>List of records that contain the term is specific fields.</returns>
        private SearchResultCollection FindAll(string term)
        {
            SearchResultCollection result;

            using (var entry = new DirectoryEntry(_adSettings.DomainAddress, _adSettings.Username, _adSettings.Password))
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                    term = ValidateFilterTerm(term);
                    searcher.Filter = "(&(objectClass=user)(|(givenName=*" + term + "*)(sn=*" + term + "*)(cn=*" + term + "*)))";
                    result = searcher.FindAll();
                }
            }

            return result;
        }

        /// <summary>
        /// Queries the active directory store to find a single active directory user
        /// with a specific distinguishedName.
        /// </summary>
        /// <param name="distinguishedName">Value that should match a distinguishedName of active directory user.</param>
        /// <returns>Result from the search containing records for a user if found.</returns>
        private SearchResult FindByDistinguishedName(string distinguishedName)
        {
            SearchResult result;

            using (var entry = new DirectoryEntry(_adSettings.DomainAddress, _adSettings.Username, _adSettings.Password))
            {
                using (var searcher = new DirectorySearcher(entry))
                {
                    searcher.Filter = "(&(objectClass=user)(distinguishedName=" + distinguishedName + "))";
                    result = searcher.FindOne();
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves a list of all the active directory users from the data store that
        /// matches the options provided.
        /// </summary>
        /// <param name="filterTerm">Term used to filter the contacts.</param>
        /// <returns>List of contacts that match the filter options.</returns>
        public IList<ActiveDirectoryContact> GetContacts(string filterTerm)
        {
            if (!_adSettings.IsValid()) 
                throw new ConfigurationErrorsException("Active Directory settings not configured");

            var output = new List<ActiveDirectoryContact>();

            using (var results = FindAll(filterTerm))
            {
                foreach (SearchResult result in results)
                    output.Add(ConvertSearchResultToContact(result, true));
            }

            return output;
        }

        /// <summary>
        /// Returns a single contact who distinguishedName property matches the parameter
        /// provided.
        /// </summary>
        /// <param name="distinguishedName">Contacts distinguishName should match this value.</param>
        /// <returns>Single contact filtered by distinguishedName.</returns>
        public ActiveDirectoryContact GetContactByDistinguishedName(string distinguishedName)
        {
            return GetContactByDistinguishedName(distinguishedName, true);
        }

        /// <summary>
        /// Returns a single contact who distinguishedName property matches the parameter
        /// provided.
        /// </summary>
        /// <param name="distinguishedName">Contacts distinguishName should match this value.</param>
        /// <param name="withManagerAndManages">Flag to determine whether the Manager & Manages property
        /// should be set.</param>
        /// <returns>Single contact filtered by distinguishedName.</returns>
        private ActiveDirectoryContact GetContactByDistinguishedName(string distinguishedName, bool withManagerAndManages)
        {
            var result = FindByDistinguishedName(distinguishedName);
            return ConvertSearchResultToContact(result, withManagerAndManages);
        }

        /// <summary>
        /// Accesses a property from the SearchResult, if the propertyName doesn't
        /// match a property in SearchResult, then an empty string is returned.
        /// </summary>
        /// <param name="searchResult">Object returned from query to active directory data store.</param>
        /// <param name="propertyName">Name of property that a value should be retrieved from.</param>
        /// <returns>Value of the specified property.</returns>
        private static string GetProperty(SearchResult searchResult, string propertyName)
        {
            return searchResult.Properties.Contains(propertyName) ? searchResult.Properties[propertyName][0].ToString() : string.Empty;
        }


        /// <summary>
        /// Accesses a property from the SearchResult, if the propertyName doesn't
        /// match a property in SearchResult, then null is returned.
        /// </summary>
        /// <param name="searchResult">Object returned from query to active directory data store.</param>
        /// <param name="propertyName">Name of property that a value should be retrieved from.</param>
        /// <returns>Value of the specified property.</returns>
        private static IList<string> GetListProperty(SearchResult searchResult, string propertyName)
        {
            List<string> output = null;

            if (searchResult.Properties.Contains(propertyName))
            {
                output = new List<string>();

                foreach (string propertyEntry in searchResult.Properties[propertyName])
                    output.Add(propertyEntry.ToString());
            }

            return output;
        }


        /// <summary>
        /// Validates the term that the user is wishing to filter the
        /// active directory ensuring that the string doesn't contains 
        /// any characters that would break the search.
        /// </summary>
        /// <param name="value">Term entered by the user.</param>
        /// <returns>Term that can be used to filter the active directory</returns>
        private static string ValidateFilterTerm(string value)
        {
            var toReturn = value;

            if (toReturn == null)
                toReturn = "";

            toReturn = toReturn.Replace("\\", "\\5c");
            toReturn = toReturn.Replace("*", "\\2a");
            toReturn = toReturn.Replace("(", "\\28");
            toReturn = toReturn.Replace(")", "\\29");
            toReturn = toReturn.Replace("NUL", "\\00");
            toReturn = toReturn.Replace("/", "\\2f");

            return toReturn;
        }

    }

    public interface IActiveDirectoryContactLookupService : IDependency
    {
        IList<ActiveDirectoryContact> GetContacts(string filterTerm);
        ActiveDirectoryContact GetContactByDistinguishedName(string distinguishedName);
    }
}