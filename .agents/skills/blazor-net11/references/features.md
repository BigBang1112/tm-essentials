# Blazor .NET 11 feature guidance

Read only the sections relevant to the task. The [release notes](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-11?view=aspnetcore-11.0&tabs=minimal-apis#blazor) are the feature index; linked topic pages supply implementation details.

## Virtualization

Source: [Razor component virtualization](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/virtualization?view=aspnetcore-11.0).

- Measured item sizes and scroll anchoring accommodate changing heights. Keep an accurate initial item-size estimate and a supported container layout.
- The default overscan is 15 in .NET 11, versus 3 previously. Explicit values override it. Tune overscan against rendering cost and fast-scroll gaps; increasing it creates more DOM.
- Use stable item keys. For class instances returned afresh by an items provider, consider an identity-based equality comparer through ItemComparer for prepend/append detection. This serves a different purpose from Razor keys. Records and value types can already have suitable equality.
- AnchorMode chooses edge pinning: Start for top-following lists, End for bottom-following logs, None for neither. Verify experimental status and diagnostics for the referenced package.
- InitialItemIndex sets the first interactive position. ScrollToItemAsync works after interactive rendering, clamps indexes, and aligns the target at the top. The latest request wins; user scrolling takes precedence.
- Validate height changes above the viewport and both scroll directions, not just a static initial render.

## QuickGrid

Source: [QuickGrid](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/quickgrid?view=aspnetcore-11.0).

- Sorting and pagination use URL state, supporting static SSR, shared links, and browser history. Defaults are page, sort, and direction; page numbers in URLs are one-based.
- Assign distinct QueryParameterNameOptions prefixes and separate PaginationState instances when rendering multiple grids.
- Column Title identifies URL sorting. Keep titles stable when preserving bookmarked sort links.
- Use OnRowClick for item callbacks.
- Virtualized grids expose InitialItemIndex, ScrollToItemAsync, AnchorMode, and ItemComparer. Scrolling requires virtualization and a rendered grid; check experimental diagnostics for anchoring/comparison APIs.

Migration details: [ASP.NET Core 10 to 11](https://learn.microsoft.com/en-us/aspnet/core/migration/100-to-110?view=aspnetcore-11.0).

- Update custom CSS and tests for link-based headers/paginators, including aria-disabled links.
- The Microsoft.AspNetCore.Components.QuickGrid.EnableUrlBasedQuickGridNavigationAndSorting AppContext switch restores button markup when false, requiring interactivity. It does not eliminate internal URL reads and writes.

## Forms

Sources: [SSR client validation](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/validation-client-side?view=aspnetcore-11.0) and [advanced form validation](https://learn.microsoft.com/en-us/aspnet/core/blazor/forms/validation-advanced?view=aspnetcore-11.0).

- Static SSR forms using DataAnnotationsValidator gain browser validation for supported data annotations. Keep server validation authoritative. Custom rules need their own client implementation; arbitrary server validators don't execute live in the browser.
- DisableClientValidation on the validator opts out per form; the Razor components service option disables it application-wide. A formnovalidate submit button skips only the browser check.
- Register form-level asynchronous work with ValidationRequestedEventArgs.AddAsyncValidator; register field work with EditContext.RegisterAsyncFieldValidator. EditForm awaits submit validation before choosing its submission callback.
- Forward the framework cancellation token to I/O, avoid stale results, update ValidationMessageStore, and notify validation state changes. Remove event subscriptions when their owner is disposed.
- AsyncValidationAttribute and IAsyncValidatableObject support model-level async rules. Check pending/faulted validation state when designing feedback; don't use async void validation handlers.

The release notes also introduce DisplayName for metadata-based labels: prefer DisplayAttribute, which outranks DisplayNameAttribute and supports localization; an unannotated property falls back to its name.

The [migration guide](https://learn.microsoft.com/en-us/aspnet/core/migration/100-to-110?view=aspnetcore-11.0) explains removal of obsolete ASP0029 suppressions for ValidatableTypeAttribute and SkipValidationAttribute on .NET 11. Preserve any suppression still needed by an older target.

## Navigation

From the [release notes](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-11?view=aspnetcore-11.0#blazor):

- BasePath can generate the base element for Blazor Web Apps.
- RelativeToCurrentUri on NavigationOptions and NavLink selects current-page-relative navigation.
- NavigationManager.GetUriWithFragment constructs fragment links.

For changes here, verify nested routes, trailing slashes, and non-root deployment paths against the current API documentation.

## State and caching

Source: [server-side state management](https://learn.microsoft.com/en-us/aspnet/core/blazor/state-management/server?view=aspnetcore-11.0).

- Use TempData or SupplyParameterFromTempData for one-time static SSR messages; use SupplyParameterFromSession for values surviving reads across requests. These parameter supplies aren't interactive component state.
- Session requires a cache/store, AddSession, and UseSession. Check supported stored-value types rather than assuming any JSON-serializable object works.
- During streaming SSR, establish the session before headers flush; cookie-backed TempData can't save after response start.
- RequestCircuitPauseAsync requests a client pause; acceptance doesn't mean the pause has completed.
- AutoPause is opt-in through Microsoft.AspNetCore.Components.Server.AutoPause and WithBrowserOptions/AddAutoPause. Preserve serializable state and test resume; mobile background suspension can prevent the graceful timer from firing.

Source: [CacheView](https://learn.microsoft.com/en-us/aspnet/core/blazor/state-management/cacheview-component?view=aspnetcore-11.0).

Use CacheView for expensive static SSR subtrees. Cache hits replay markup without executing child lifecycles. Choose expiration and vary keys for the content's inputs. User identity alone doesn't vary by every permission or claim; keep per-request authorization outside cached children or vary on the relevant authorization state. Vary cached form output by query parameters when its action depends on them.

## Hosting and interoperability

Feature lookup: [Blazor release notes](https://learn.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-11?view=aspnetcore-11.0#blazor).

- WithBrowserOptions/ConfigureBrowser configure browser startup from C#.
- ConfigureConnection exposes Interactive Server transport options.
- WebAssembly adds IHostedService, environment configuration, and server-culture transfer; UseCultureFromServer opts out.
- BL0010 directs void JS calls to InvokeVoidAsync.
- EnvironmentView renders by environment; IComponentPropertyActivator customizes injection.
- Interactive MathML, WebAssembly trimming/metrics, Gateway hosting, and blazorwebworker are additional task-specific lookup topics.

For startup JavaScript, use the applicable [startup documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/startup?view=aspnetcore-11.0): group options under ssr, circuit, and webAssembly as supported. With manual startup, set autostart="false" and start Blazor once. Retain supported legacy options unless changing them serves the task.

For migration, follow the [migration guide](https://learn.microsoft.com/en-us/aspnet/core/migration/100-to-110?view=aspnetcore-11.0): replace template inline navigation-menu handlers with a collocated module where relevant. Automatic CSRF protection can make explicit antiforgery middleware unnecessary, but retain token protection when required by the app's clients or configuration. Don't remove protection merely to silence a failing form post.

Browser-delivered configuration is visible to users. Background services in WebAssembly don't establish that CPU-heavy work runs off the browser thread; consider workers for that requirement.

## Experimental AI components

Source: [Blazor AI components](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/ai-components?view=aspnetcore-11.0).

For an explicitly requested agent interface, investigate Microsoft.AspNetCore.Components.AI, UIAgent, ChatPage, and BlockRenderer. Confirm package availability, target framework, and experimental status; documentation can redirect to newer versions. Keep tool approval and authorization in the application workflow. Don't add this dependency to ordinary Blazor components.
