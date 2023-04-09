using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using NBCC.Authorizaion.DataAccess;
using NBCC.Authorizaion.Query;
using NBCC.Courses.Commands;
using NBCC.Authorizaion;
using NBCC.Authorization;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NBCC.Authorizaion.QueryHandlers
{
    public sealed class RolesQueryHandler : IQueryHandler<RolesQuery, IEnumerable<Role>>
    {
        IRolesRepository RolesRepository { get; }
        public RolesQueryHandler(IRolesRepository rolesRepository) => RolesRepository = rolesRepository;
        public async Task<IEnumerable<Role>> Handle(RolesQuery rolesQuery) => await RolesRepository.GetRoles(rolesQuery.RoleID);
    }
}
[AttributeUsage(AttributeTargets.Class)]
public class DataBasedAuthorization : Attribute, IAuthorizationFilter
{
    IQueryHandler<RolesQuery, IEnumerable<Role>> Messages { get; }
    public DataBasedAuthorization(IQueryHandler<RolesQuery, IEnumerable<Role>> messages)
    {
        Messages = messages ?? throw new ArgumentNullException(nameof(messages));
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var roles = Messages.Handle(new RolesQuery(null)).Result;

        foreach (var role in roles)
        {
            if (!context.HttpContext.User.IsInRole(role.RoleName))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
public class HasPermissionAttribute : AuthorizeAttribute
{
    /// <summary>
    /// This creates an HasPermission attribute with a Permission enum member
    /// </summary>
    /// <param name="permission"></param>
    public HasPermissionAttribute(object permission) : base(permission?.ToString()!)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        permission.GetType().ThrowExceptionIfEnumIsNotCorrect();
    }
}

public static class ErrorReportingExtensions
{
    /// <summary>
    /// Checks the permission type is correct
    /// </summary>
    /// <param name="permissionType"></param>
    public static void ThrowExceptionIfEnumIsNotCorrect(this Type permissionType)
    {
        if (!permissionType.IsEnum)
            throw new AuthPermissionsException("The permissions must be an enum");
        if (Enum.GetUnderlyingType(permissionType) != typeof(ushort))
            throw new AuthPermissionsException(
                $"The enum permissions {permissionType.Name} should by 16 bits in size to work.\n" +
                $"Please add ': ushort' to your permissions declaration, i.e. public enum {permissionType.Name} : ushort " + "{...};");
    }
}

public class AuthPermissionsException : Exception
{
    /// <summary>
    /// Must contain a message
    /// </summary>
    /// <param name="message"></param>
    public AuthPermissionsException(string message)
        : base(message)
    { }
}

public enum Example2Permissions : ushort
{
    NotSet = 0, //error condition

    //Here is an example of very detailed control over something
    [Display(GroupName = "Test", Name = "Access page")]
    Permission1 = 1,

    [Display(GroupName = "Test", Name = "Display link")]
    Permission2 = 2,

    [Display(GroupName = "Test", Name = "Display link")]
    Permission3 = 3,
      

    //Setting the AutoGenerateFilter to true in the display allows we can exclude this permissions
    //to admin users who aren't allowed alter this permissions
    //Useful for multi-tenant applications where you can set up company-level admin users
    [Display(GroupName = "SuperAdmin", Name = "AccessAll",
        Description = "This allows the user to access every feature", AutoGenerateFilter = true)]
    AccessAll = ushort.MaxValue,
}