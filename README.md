
dotnet ef dbcontext scaffold "Server=.;Database=REMS;User Id=sa;Password=sasa@123;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o AppDbContextModels -c AppDbContext -f

Scaffold-DbContext "Server=.;Database=AdminPortal;User ID=sa; Password=sa@123;Integrated Security=True;Trusted_Connection=true;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir AppDbContext -Tables Tbl_AdminUserLogin -f
