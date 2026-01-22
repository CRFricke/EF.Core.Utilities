# EF.Core.Utilities .NET 9.0 Upgrade Tasks

## Overview

This document tracks the execution of the EF.Core.Utilities solution upgrade from .NET 8.0 to .NET 9.0. All three projects will be upgraded simultaneously in a single atomic operation, followed by comprehensive testing and validation.

**Progress**: 4/4 tasks complete (100%) ![0%](https://progress-bar.xyz/100)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-01-22 21:10)*
**References**: Plan §Phase 0

- [✓] (1) Verify .NET 9.0 SDK installed on local machine
- [✓] (2) SDK version meets minimum requirements for .NET 9.0 (**Verify**)

---

### [✓] TASK-002: Atomic framework and dependency upgrade *(Completed: 2026-01-22 21:11)*
**References**: Plan §Phase 1, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [✓] (1) Update TargetFramework to net9.0 in all 3 project files per Plan §Phase 1 (EF.Core.Utilities, EF.Core.Utilities.Test, EF.Core.Utilities.Test.Web)
- [✓] (2) All project files updated to net9.0 (**Verify**)
- [✓] (3) Update package references per Plan §Package Update Reference (10 packages total: Microsoft ASP.NET Core packages 8.0.14→9.0.12, Microsoft Entity Framework Core packages 8.0.14→9.0.12, Microsoft Extensions packages 8.0.x→9.0.12, CRFricke.Test.Support 8.0.1→9.0.0)
- [✓] (4) All package references updated to target versions (**Verify**)
- [✓] (5) Restore dependencies for entire solution
- [✓] (6) All dependencies restored successfully (**Verify**)
- [✓] (7) Build solution and fix all compilation errors per Plan §Breaking Changes Catalog (focus: 8 source incompatible APIs in Test.Web - add required using directives for Microsoft.AspNetCore.Identity, Microsoft.AspNetCore.Identity.EntityFrameworkCore, Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore)
- [✓] (8) Solution builds with 0 errors and 0 warnings (**Verify**)

---

### [✓] TASK-003: Run full test suite and validate upgrade *(Completed: 2026-01-22 21:12)*
**References**: Plan §Phase 2 Testing, Plan §Testing & Validation Strategy

- [✓] (1) Run all tests in EF.Core.Utilities.Test project
- [✓] (2) Fix any test failures (reference Plan §Breaking Changes Catalog for guidance, check CRFricke.Test.Support 9.0.0 compatibility if test utility failures occur)
- [✓] (3) Re-run tests after fixes
- [✓] (4) All tests pass with 0 failures (**Verify**)

---

### [✓] TASK-004: Final commit *(Completed: 2026-01-22 21:13)*
**References**: Plan §Source Control Strategy

- [✓] (1) Commit all changes with message: "Upgrade solution from .NET 8.0 to .NET 9.0"

---







