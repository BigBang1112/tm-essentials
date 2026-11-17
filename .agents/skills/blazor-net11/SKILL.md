---
name: blazor-net11
description: Implement, review, or migrate Blazor code using .NET 11 features and behavior changes, including virtualization, QuickGrid, forms, navigation, state, and hosting. Use for .NET 11-specific Blazor work; ordinary Blazor changes that use no version-specific behavior do not require this skill.
---

# Blazor on .NET 11

Use the Blazor sections of Microsoft's [ASP.NET Core 11 release notes](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-11?view=aspnetcore-11.0&tabs=minimal-apis) to choose framework capabilities for the requested change.

## Working approach

1. Inspect the affected projects' target frameworks, SDK selection, package versions, and rendering model: static SSR, Interactive Server, Interactive WebAssembly, Auto, standalone WebAssembly, or Hybrid. A Razor class library doesn't determine its consumer's render mode.
2. Read the relevant section of [feature guidance](references/features.md). Follow its Microsoft Learn links when exact signatures, configuration, or lifecycle behavior matter. Check that the selected documentation version is 11.0; a link can silently fall back to another version.
3. Adopt only the features needed for the user's task. A release-note entry isn't a request to upgrade packages, change render modes, or replace application infrastructure.
4. For libraries targeting older frameworks too, keep compatible behavior on those targets. Isolate .NET 11-only C# with target-framework conditions where appropriate; use compatible components or separate implementations for Razor markup. Don't reference a missing component and expect a runtime condition to make it compile. Don't remove supported targets to simplify the change.
5. Check API availability against the installed SDK and referenced packages. Release notes include evolving and experimental features; verify their status before adding dependencies or suppressing diagnostics.
6. Build the affected targets and run relevant existing tests. For scrolling, navigation, form feedback, caching, or circuit changes, verify the observable behavior in the applicable hosting mode when possible. Report unavailable SDKs or browser checks clearly.

## Choose the relevant guidance

- Scrolling, item identity, or grid paging: virtualization and QuickGrid.
- Labels, validation, or SSR form posts: forms.
- Links, fragments, and deployment beneath a path: navigation.
- Request-to-request data, cached markup, or circuit suspension: state and caching.
- Startup, WebAssembly, JS interop, or server connections: hosting and interoperability.
- AI interfaces: experimental AI components; verify the package separately.

The reference is a decision guide, not a replacement for current API documentation. It was checked against Microsoft documentation on 2026-11-12.
