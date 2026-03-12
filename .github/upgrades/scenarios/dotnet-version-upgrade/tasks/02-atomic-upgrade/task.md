# 02-atomic-upgrade: Update All Projects to .NET 10.0

Update all 3 projects simultaneously:
- Change TargetFramework from net9.0 to net10.0 in all .csproj files
- Update all package references (Entity Framework Core, ASP.NET Core Identity, diagnostics packages from 9.0.12 → 10.0.4)
- Update CRFricke.Test.Support from 9.0.0 → 10.0.0 (preview)
- Restore dependencies
- Build solution and fix all compilation errors from API breaking changes

**Key API changes in Razor Pages project** (IdentityHostingStartup.cs, Program.cs):
- `AddDefaultIdentity<T>` signature changes
- `AddEntityFrameworkStores<T>` signature changes  
- `UseMigrationsEndPoint()` changes
- `AddDatabaseDeveloperPageExceptionFilter()` changes
- `UseExceptionHandler()` behavioral changes

**Affected projects:**
- EF.Core.Utilities (ClassLibrary) - foundation library
- EF.Core.Utilities.Test (TestProject) - test library
- EF.Core.Utilities.Test.Web (Razor Pages) - web application with Identity

**Done when**: Solution builds with zero errors, all projects targeting net10.0, all packages updated to compatible versions

---
