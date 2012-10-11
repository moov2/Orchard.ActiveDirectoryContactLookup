# Active Directory Contact Lookup for Orchard

[Orchard](http://www.orchardproject.net/) is a community driven content management system that allows users to rapidly create websites for the .NET platform. This module allows a user to search by first name, surname & username the contacts inside active directory.

## Status

Not available on the Orchard Gallery just yet, available for manual install using the downloads section.

## Installation

1. Get the latest .nupkg from the downloads section of this Github repository.

2. Install the module into your instance of Orchard follow the instructions for [installing a module from your local computer](https://github.com/OrchardCMS/OrchardDoc/blob/master/Documentation/Installing-and-upgrading-modules.markdown#installing-a-module-from-your-local-computer).

3. Once installed and enabled, go to the **Active Directory** section (link is sub option in the **Settings** menu option) and enter your active directory connection settings (see [this ServerFault answer](http://serverfault.com/questions/130543/how-can-i-figure-out-my-ldap-connection-string#130556) for a good explanation of AD connection strings). Upon saving a check will be made to see if a connection can be established, with a warning being provided if not.

4. That's it! You can now search the contacts inside the active directory in the **Active Directory Contacts** section in the Admin dashboard, or go to **/ContactDirectory** on your website.

## Features

* Ability to define connection settings for Active Directory.
* Ability to filter contacts inside the active directory based on their first name, surname and username. This is available from inside the Admin dashboard and on the website.
* Once contacts have been filtered, on the website a user is able to drill down into a contact to view more details.
* Includes a widget that can initiate a search.

## Roadmap

Below are some of the things that we hope to include in the future.

* Configurable contact properties to allow an Admin to define the properties that are shown for a contact.
* Expanding the filter parameters.
