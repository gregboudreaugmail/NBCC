namespace NBCC.Authorizaion.Query
{
    public sealed class RolesQuery
    {
        public int? RoleID { get; }
        public RolesQuery(int? roleID) => RoleID = roleID;

    }
}
