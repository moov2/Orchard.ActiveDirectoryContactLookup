using Orchard.UI.Resources;

namespace ActiveDirectoryContactLookup
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            builder.Add().DefineStyle("ContactLookup").SetUrl("contact-lookup");
        }
    }
}