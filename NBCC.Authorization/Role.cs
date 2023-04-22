namespace NBCC.Authorization
{
    public sealed record Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; } = string.Empty;
        public bool IsDefault { get; }
        public Role() { }
        public Role(int roleId, string roleName, bool isDefault)
        {
            RoleId = roleId;
            RoleName = roleName;
            IsDefault = isDefault;
        }
    }
}
