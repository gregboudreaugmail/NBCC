Select [hash],[password] from secure.Credentials c
inner join secure.users u 
on u.UserID = c.UserID
where username=@userName and IsActive=1