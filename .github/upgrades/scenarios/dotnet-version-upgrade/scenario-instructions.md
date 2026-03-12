# .NET Version Upgrade Scenario Instructions

## Scenario
**ID**: dotnet-version-upgrade  
**Description**: Upgrade solution from .NET 9 to .NET 10.0 (LTS)

## Target Framework
**.NET 10.0 (LTS)** - Long-Term Support release

## Preferences

### Flow Mode
**Guided** — Pause after each stage (assessment, plan, breakdowns) for user review

### Source Control
- **Source branch**: `master`
- **Working branch**: `upgrade-to-NET10`
- **Initial changes**: Committed before workflow initialization

## Strategy
**Selected**: All-At-Once
**Rationale**: Small solution (3 projects), all on .NET 9, clear dependency structure. API changes are documented .NET 10 breaking changes with established patterns.

### Execution Constraints
- Single atomic upgrade — all projects updated together
- Validate full solution build after upgrade
- One pass through build errors — fix all compilation issues in bounded effort
- Testing occurs after atomic upgrade completes successfully

## Preferences
- **Flow Mode**: Guided
- **Commit Strategy**: Single Commit at End
- **Source Branch**: master
- **Working Branch**: upgrade-to-NET10

## User Preferences

### Technical Preferences
- **CRFricke.Test.Support**: Update to 10.0.0 (preview version) instead of keeping at 9.0.0

### Execution Style
*(Execution preferences will be recorded here as they are expressed)*

### Custom Instructions
*(Task-specific instructions will be recorded here as they are expressed)*

## Key Decisions Log
- Strategy selection: All-At-Once chosen over Bottom-Up for efficiency (2024-01-15)
