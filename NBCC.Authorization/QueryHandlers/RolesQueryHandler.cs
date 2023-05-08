using NBCC.Authorization.DataAccess;
using NBCC.Authorization.Query;
using NBCC.CQRS.Commands;

namespace NBCC.Authorization.QueryHandlers;

public sealed class RolesQueryHandler : IQueryHandler<RolesQuery, IEnumerable<Role>>
    {
        IRolesRepository RolesRepository { get; }
        public RolesQueryHandler(IRolesRepository rolesRepository) => RolesRepository = rolesRepository ?? throw new ArgumentNullException(nameof(rolesRepository));
        public async Task<IEnumerable<Role>> Handle(RolesQuery rolesQuery) => await RolesRepository.GetRoles(rolesQuery.RoleId);
    }
