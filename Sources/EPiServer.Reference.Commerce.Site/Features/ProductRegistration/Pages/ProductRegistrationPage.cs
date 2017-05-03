using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EPiServer.Reference.Commerce.Site.Features.ProductRegistration.Pages
{
    [ContentType(DisplayName = "Product Registration", 
                    GUID = "55971B2D-FB9F-4611-920E-9FDE280F150C", 
                    Description = "", 
                    AvailableInEditMode = false)]
    public class ProductRegistrationPage : PageData
    {
        [CultureSpecific]
        [Display(
            Name = "Product Registration",
            Description = "The main body will be shown in the main content area of the page, using the XHTML-editor you can insert for example text, images and tables.",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual XhtmlString MainBody { get; set; }
    }
}