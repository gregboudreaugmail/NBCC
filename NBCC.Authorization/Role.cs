namespace NBCC.Authorizaion
{
    public sealed record Role
    {
        public int RoleID { get; set; }
        public string RoleName { get; } = string.Empty;
        public bool IsDefault { get; }
        public Role() { }
        public Role(int roleID, string roleName, bool isDefault)
        {
            RoleID = roleID;
            RoleName = roleName;
            IsDefault = isDefault;
        }
    }
}
