namespace NBCC.Authorizaion.Query
{
    public sealed record RolesQuery
    {
        public int? RoleID { get; }
        public RolesQuery(int? roleID) => RoleID = roleID;

    }
}
