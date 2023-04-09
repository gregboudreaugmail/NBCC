namespace NBCC.Authorizaion
{
    public sealed class Role
    {
        public int RoleID { get; private set; }
        public string RoleName { get; private set; }
        public bool IsDefault { get; private set; }
        public Role() { }
        public Role(int roleID, string roleName, bool isDefault)
        {
            RoleID = roleID;
            RoleName = roleName;
            IsDefault = isDefault;
        }
    }
}
