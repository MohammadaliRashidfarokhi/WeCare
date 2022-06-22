# WeCare

# Run Instructions:

1- Delete .vs folder before opening the project with Microsoft Visual Studio.

2- Open the project.

3- Delete Migration Folder.

4- Open the terminal in Microsoft Visual Studio and run ' dotnet restore ' to make sure that all the required packages is installed.

5- Check appsettings.json to make sure that Server= is right for you, you can even cahnge Database='....' if you want to change the database name.

6- Go to tools from the menu (Nuget package manager ---> package manager console).

For the database:

Option 1:

7- Run ' Add-Migration InitialCreate -Context ApplicationDBContext ' in console to create the database, when it is done, run ' Update-Database -Context ApplicationDBContext' to update the database and make sure that everything is created correctly.

8- Run ' Add-Migration InitialCreate -Context AppIdentityDbContext ' in console to create the database, when it is done, run ' Update-Database -Context AppIdentityDbContext' to update the database and make sure that everything is created correctly..

9- Run ' Add-Migration InitialCreate -Context ApplicationDBContext2 ' in console when it is done, run ' Update-Database -Context ApplicationDBContext2 ' to update the database and make sure that everything is created correctly.

Option 2:
To save time create all the databases at once:

- Add-Migration InitialCreate -Context ApplicationDBContext; Add-Migration InitialCreate -Context ApplicationDBContext2; Add-Migration InitialCreate -Context AppIdentityDbContext;
- Update-Database -Context ApplicationDBContext; Update-Database -Context ApplicationDBContext2; Update-Database -Context AppIdentityDbContext;

10 - Run the program to initialize the database.
