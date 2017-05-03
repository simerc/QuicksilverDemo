using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;
using EPiServer.Personalization.VisitorGroups;
using Mediachase.Commerce.Security;

namespace EPiServer.Reference.Commerce.Site.VisitorGroups
{
    public class UserGroupModel : CriterionModelBase
    {
        [DojoWidget(
            AdditionalOptions = "{ selectOnClick:true }"), Required]
        public string UserGroupName { get; set; }

        public override ICriterionModel Copy()
        {
            return ShallowCopy();
        }
    }

    [VisitorGroupCriterion(
    Category = "Exclude user group criteria",
    DisplayName = "User group",
    Description = "Criterion that matches a user that is not in the specified group")]
    public class ExcludeUserGroupCriterion : CriterionBase<UserGroupModel>
    {
        public override bool IsMatch(IPrincipal principal, HttpContextBase httpContext)
        {
            return !MatchCurrentUserGroup(principal);
        }

        public bool MatchCurrentUserGroup(IPrincipal principal)
        {
            return principal.IsInRole(this.Model.UserGroupName);
        }
    }
}