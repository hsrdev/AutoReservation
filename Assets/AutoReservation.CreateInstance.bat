@ECHO OFF
ECHO Creating new Microsoft SQL Server LocalDB instance named 'MSSQLLocalDB'
sqllocaldb create "MSSQLLocalDB"
ECHO.
ECHO List of all Microsoft SQL Server LocalDB instances:
sqllocaldb info
ECHO.
ECHO Please ensure that an instance named 'MSSQLLocalDB' was created and listed above.
PAUSE