using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace EPiServer.Reference.Commerce.Site.Features.Start.Pages
{

    [AvailableContentTypes(Include = new[] { typeof(StartPage)})]
    [ContentType(DisplayName = "[Root] Root page", GUID = "452d1812-42ab-42c3-8073-c1b7481e7b20", Description = "", AvailableInEditMode = true)]
    public class SysRoot : PageData
    {

    }
}