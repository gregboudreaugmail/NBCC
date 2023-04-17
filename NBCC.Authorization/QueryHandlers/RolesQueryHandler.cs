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
