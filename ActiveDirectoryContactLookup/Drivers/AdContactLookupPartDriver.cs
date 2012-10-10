using ActiveDirectoryContactLookup.Models;
using ActiveDirectoryContactLookup.ViewModels;
using Orchard.ContentManagement.Drivers;

namespace ActiveDirectoryContactLookup.Drivers
{
    public class AdContactLookupPartDriver : ContentPartDriver<AdContactLookupPart>
    {
        protected override DriverResult Display(AdContactLookupPart part, string displayType, dynamic shapeHelper)
        {
            var model = new AdContactLookupIndexOptions();
            return ContentShape("Parts_ActiveDirectoryContactLookup",
                                () =>
                                {
                                    var shape = shapeHelper.Parts_ActiveDirectoryContactLookup();
                                    shape.ContentPart = part;
                                    shape.ViewModel = model;
                                    return shape;
                                });
        }
    }
}