using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Reference.Commerce.Site.Features.Start.Pages;

namespace EPiServer.Reference.Commerce.Site.Features.Standard.Pages
{
    [ContentType(DisplayName = "Standard page", GUID = "6e0c84de-bd17-3211-9019-04f08c7fcf8d", Description = "", AvailableInEditMode = true)]
    [AvailableContentTypes(IncludeOn = new [] { typeof(StartPage)})]

    public class StandardPage : PageData
    {
        [Display(Name="Main content area")]
        public virtual ContentArea MainContentArea { get; set; }
    }
}