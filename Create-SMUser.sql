/* Change to the SocialMedia database */
USE Journal;
GO

/* Create user */
IF NOT EXISTS(SELECT *
FROM sys.server_principals
WHERE name = 'SMUserTrx')
BEGIN
	CREATE LOGIN SMUserTrx WITH PASSWORD=N'SmPA$$06500', DEFAULT_DATABASE=Journal
END


IF NOT EXISTS(SELECT *
FROM sys.database_principals
WHERE name = 'SMUser')
BEGIN
	EXEC sp_adduser 'SMUser', 'SMUser', 'db_owner';
END
