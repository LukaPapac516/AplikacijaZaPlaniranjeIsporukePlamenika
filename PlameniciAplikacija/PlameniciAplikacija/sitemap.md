# Sitemap

## Notes
- App uses conventional MVC routing plus custom routes.
- Default route pattern: `{controller=Home}/{action=Dashboard}/{id?}`.
- Custom routes are defined in `Program.cs` and listed below.
- For POST actions, view column points to the target view used after redirect/validation.

## Custom Routes (Program.cs)

| HTTP | URL pattern | Controller | Action | View |
|---|---|---|---|---|
| GET | /projekti | Home | Index | Views/Home/Index.cshtml |
| GET | /projekti/{id:int} | Home | Details | Views/Home/Details.cshtml |
| GET | /kupci | Kupci | Index | Views/Kupci/Index.cshtml |
| GET | /djelatnici | Djelatnici | Index | Views/Djelatnici/Index.cshtml |
| GET | /projekti/novi | Home | Create | Views/Home/Create.cshtml |
| GET | /kupci/novi | Kupci | Create | Views/Kupci/Create.cshtml |
| GET | /djelatnici/novi | Djelatnici | Create | Views/Djelatnici/Create.cshtml |
| GET | /djelatnici/{id:int}/uredi | Djelatnici | Edit | Views/Djelatnici/Edit.cshtml |
| GET | /radni-nalozi | RadniNalozi | Index | Views/RadniNalozi/Index.cshtml |
| GET | /radni-nalozi/{id:int}/uredi | RadniNalozi | Edit | Views/RadniNalozi/Edit.cshtml |

## Conventional Routes (Default MVC pattern)

### HomeController

| HTTP | URL pattern | Controller | Action | View |
|---|---|---|---|---|
| GET | / | Home | Dashboard | Views/Home/Dashboard.cshtml |
| GET | /Home/Dashboard | Home | Dashboard | Views/Home/Dashboard.cshtml |
| GET | /Home/Index | Home | Index | Views/Home/Index.cshtml |
| GET | /Home/Details/{id} | Home | Details | Views/Home/Details.cshtml |
| GET | /Home/Create | Home | Create | Views/Home/Create.cshtml |
| POST | /Home/Create | Home | Create | Views/Home/Create.cshtml (on invalid) / redirect Index (on valid) |
| POST | /Home/Update | Home | Update | Redirect to Home/Index |
| POST | /Home/Delete | Home | Delete | Redirect to Home/Index |
| POST | /Home/UpdateRadniNalog | Home | UpdateRadniNalog | Redirect to Home/Details/{id} |
| GET | /Home/Privacy | Home | Privacy | Views/Home/Privacy.cshtml |
| GET | /Home/Error | Home | Error | Views/Shared/Error.cshtml |

### KupciController

| HTTP | URL pattern | Controller | Action | View |
|---|---|---|---|---|
| GET | /Kupci/Index | Kupci | Index | Views/Kupci/Index.cshtml |
| GET | /Kupci/Details/{id} | Kupci | Details | Views/Kupci/Details.cshtml |
| GET | /Kupci/Create | Kupci | Create | Views/Kupci/Create.cshtml |
| POST | /Kupci/Create | Kupci | Create | Views/Kupci/Create.cshtml (on invalid) / redirect Index (on valid) |
| POST | /Kupci/Delete | Kupci | Delete | Redirect to Kupci/Index |

### DjelatniciController

| HTTP | URL pattern | Controller | Action | View |
|---|---|---|---|---|
| GET | /Djelatnici/Index | Djelatnici | Index | Views/Djelatnici/Index.cshtml |
| GET | /Djelatnici/Details/{id} | Djelatnici | Details | Views/Djelatnici/Details.cshtml |
| GET | /Djelatnici/Create | Djelatnici | Create | Views/Djelatnici/Create.cshtml |
| POST | /Djelatnici/Create | Djelatnici | Create | Views/Djelatnici/Create.cshtml (on invalid) / redirect Index (on valid) |
| GET | /Djelatnici/Edit/{id} | Djelatnici | Edit | Views/Djelatnici/Edit.cshtml |
| POST | /Djelatnici/Edit/{id} | Djelatnici | Edit | Views/Djelatnici/Edit.cshtml (on invalid) / redirect Index (on valid) |
| POST | /Djelatnici/Delete | Djelatnici | Delete | Redirect to Djelatnici/Index |

### RadniNaloziController

| HTTP | URL pattern | Controller | Action | View |
|---|---|---|---|---|
| GET | /RadniNalozi/Index | RadniNalozi | Index | Views/RadniNalozi/Index.cshtml |
| GET | /RadniNalozi/Edit/{id} | RadniNalozi | Edit | Views/RadniNalozi/Edit.cshtml |
| POST | /RadniNalozi/Edit/{id} | RadniNalozi | Edit | Views/RadniNalozi/Edit.cshtml (on invalid) / redirect Index (on valid) |

## Existing View Files (reference)
- Views/Home/Dashboard.cshtml
- Views/Home/Index.cshtml
- Views/Home/Details.cshtml
- Views/Home/Create.cshtml
- Views/Home/Privacy.cshtml
- Views/Kupci/Index.cshtml
- Views/Kupci/Details.cshtml
- Views/Kupci/Create.cshtml
- Views/Djelatnici/Index.cshtml
- Views/Djelatnici/Details.cshtml
- Views/Djelatnici/Create.cshtml
- Views/Djelatnici/Edit.cshtml
- Views/RadniNalozi/Index.cshtml
- Views/RadniNalozi/Edit.cshtml
- Views/Shared/Error.cshtml
