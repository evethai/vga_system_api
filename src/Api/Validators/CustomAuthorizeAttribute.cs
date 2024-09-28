using Application.Common.Utils;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;

namespace Api.Validators
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute(params RoleEnum[] roleEnums)
        {
            var allowedRolesAsString = roleEnums.Select(x => x.GetDescriptionFromEnum());
            Roles = string.Join(",", allowedRolesAsString);
        }
    }
}
