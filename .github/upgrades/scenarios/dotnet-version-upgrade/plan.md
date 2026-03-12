# .NET 10.0 Upgrade Plan

## Overview

**Target**: Upgrade solution from .NET 9 to .NET 10.0 (LTS)
**Scope**: 3 projects (1 class library, 1 test project, 1 Razor Pages web app), ~15k-20k LOC

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: Small solution (3 projects), all on .NET 9, clear dependency structure, straightforward package upgrades with documented API breaking changes.

## Tasks

### 01-prerequisites: Validate Prerequisites

Verify .NET 10.0 SDK is installed and global.json files (if any) are compatible with .NET 10 upgrade. Ensures the build environment supports the target framework before attempting project modifications.

**Done when**: .NET 10 SDK availability confirmed, no global.json conflicts that would block the upgrade

---

### 02-atomic-upgrade: Update All Projects to .NET 10.0

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

### 03-validation: Verify Tests and Build

Run full test suite and verify solution builds correctly in the upgraded state. Confirms functional correctness after the upgrade.

**Done when**: All unit tests pass, solution builds successfully, no runtime errors in test execution

---

### 04-cleanup: Address Deprecated Packages (Optional)

Review and optionally address deprecated xunit package (2.9.3) in test project. This is advisory only and does not block upgrade success.

**Done when**: User decision made on whether to replace xunit or keep as-is
