# 04-cleanup: Address Deprecated Packages (Optional)

Review and optionally address deprecated xunit package (2.9.3) in test project. This is advisory only and does not block upgrade success.

**Done when**: User decision made on whether to replace xunit or keep as-is

---

## Research Findings

### Current State
- **Package**: xunit 2.9.3
- **Status**: Deprecated by NuGet
- **Project**: EF.Core.Utilities.Test
- **Impact**: Tests currently run successfully - no functional issues

### Context
The xunit package is a meta-package that references both:
- `xunit.core` - the core framework
- `xunit.assert` - assertion library

This meta-package has been deprecated in favor of referencing the individual packages directly or upgrading to xunit v3 (when stable).

### Options

**Option 1: Keep as-is** ✅ Recommended for now
- Tests work perfectly on .NET 10
- Minimal risk
- Can defer until xunit v3 is stable/LTS

**Option 2: Replace with individual packages**
- Remove `xunit` 2.9.3
- Add `xunit.core` 2.9.3 and `xunit.assert` 2.9.3 explicitly
- Same functionality, just explicit references

**Option 3: Upgrade to xunit v3 (preview)**
- xunit v3 is currently in preview/beta
- Would require testing to ensure compatibility
- Higher risk for a stable project

## Recommendation
**Keep as-is** - The deprecation is advisory, tests pass, and xunit v3 is not yet stable. This can be addressed in a future update when v3 reaches LTS status.

