select u.UserID, u.UserName,r.roleid, r.rolename, r.isdefault
from secure.roles r
inner join secure.UserRoles ur on r.RoleID = ur.RoleID
inner join secure.Users u on ur.UserID = u.UserID
where username=@userName