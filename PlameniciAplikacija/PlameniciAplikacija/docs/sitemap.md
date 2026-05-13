# Semantički Model Usmjeravanja (Sitemap)

Kompletan popis svih dostupnih URL-eva u aplikaciji sa mapiranjem na controller, akciju i view-ove.

---

## Pogled po Routama (Sveobuhvatan Pregled)

### **CUSTOM ROUTES - 10 ruta definisano u Program.cs**

| # | Ruta | HTTP | URL | Controller | Action | View | Opis |
|---|------|------|-----|-----------|--------|------|------|
| 1 | `projekti_lista` | GET | `/projekti` | Home | Index | `Views/Home/Index.cshtml` | Lista svih projekata |
| 2 | `projekt_detalji` | GET | `/projekti/{id:int}` | Home | Details | `Views/Home/Details.cshtml` | Detalji projekta |
| 3 | `projekt_novi` | GET/POST | `/projekti/novi` | Home | Create | `Views/Home/Create.cshtml` | Forma za novi projekt |
| 4 | `kupci_lista` | GET | `/kupci` | Kupci | Index | `Views/Kupci/Index.cshtml` | Lista svih kupaca |
| 5 | `kupac_novi` | GET/POST | `/kupci/novi` | Kupci | Create | `Views/Kupci/Create.cshtml` | Forma za novog kupca |
| 6 | `djelatnici_lista` | GET | `/djelatnici` | Djelatnici | Index | `Views/Djelatnici/Index.cshtml` | Lista svih djelatnika |
| 7 | `djelatnik_novi` | GET/POST | `/djelatnici/novi` | Djelatnici | Create | `Views/Djelatnici/Create.cshtml` | Forma za novog djelatnika |
| 8 | `djelatnik_uredi` | GET/POST | `/djelatnici/{id:int}/uredi` | Djelatnici | Edit | `Views/Djelatnici/Edit.cshtml` | Forma za uređivanje |
| 9 | `radni_nalozi_lista` | GET | `/radni-nalozi` | RadniNalozi | Index | `Views/RadniNalozi/Index.cshtml` | Lista radnih naloga |
| 10 | `radni_nalog_uredi` | GET/POST | `/radni-nalozi/{id:int}/uredi` | RadniNalozi | Edit | `Views/RadniNalozi/Edit.cshtml` | Uredi nalog |

---

## Pogled po Controllerima

### **=== HOME CONTROLLER ===**

Upravljač za projekte (osnovne operacije).

| URL | HTTP | Action | Returns | View | Napomena |
|-----|------|--------|---------|------|---------|
| `/projekti` | GET | Index | List<Project> | `Index.cshtml` | Lista svih projekata sa filterima |
| `/projekti/{id}` | GET | Details | Project | `Details.cshtml` | Detalji jednog projekta |
| `/projekti/novi` | GET | Create | ViewResult | `Create.cshtml` | Prazna forma za unos |
| `/projekti/novi` | POST | Create | RedirectToAction | - | Sprema novi projekt |
| `/Home` | GET | *default* | redirects | - | Redirect na `/projekti` |

**Korišteni Modeli:**
- `Project`
- `Kupac` (lookup za select)

**Korišteni View-ovi:**
```
Views/Home/
├── Index.cshtml          (lista projekata)
├── Details.cshtml        (detalji)
├── Create.cshtml         (forma - nova)
├── Edit.cshtml           (forma - edit)
├── Privacy.cshtml        (statična stranica)
├── Dashboard.cshtml      (info stranica)
└── Error.cshtml          (greške)
```

---

### **=== KUPCI CONTROLLER ===**

Upravljač za kupce (naručitelje).

| URL | HTTP | Action | Returns | View | Napomena |
|-----|------|--------|---------|------|---------|
| `/kupci` | GET | Index | List<Kupac> | `Index.cshtml` | Lista svih kupaca |
| `/kupci/novi` | GET | Create | ViewResult | `Create.cshtml` | Prazna forma za unos |
| `/kupci/novi` | POST | Create | RedirectToAction | - | Sprema novog kupca |

**Korišteni Modeli:**
- `Kupac`

**Korišteni View-ovi:**
```
Views/Kupci/
├── Index.cshtml    (lista kupaca)
├── Create.cshtml   (forma - nova)
└── Details.cshtml  (detalji - ako postoji)
```

---

### **=== DJELATNICI CONTROLLER ===**

Upravljač za djelatnike (zaposlenike).

| URL | HTTP | Action | Returns | View | Napomena |
|-----|------|--------|---------|------|---------|
| `/djelatnici` | GET | Index | List<Djelatnik> | `Index.cshtml` | Lista svih djelatnika sortirano po prezimenu |
| `/djelatnici/{id}` | GET | Details | Djelatnik | `Details.cshtml` | Detalji djelatnika sa projektima |
| `/djelatnici/novi` | GET | Create | ViewResult | `Create.cshtml` | Prazna forma |
| `/djelatnici/novi` | POST | Create | RedirectToAction | - | Sprema novog djelatnika |
| `/djelatnici/{id}/uredi` | GET | Edit | Djelatnik | `Edit.cshtml` | Forma sa podacima za uređivanje |
| `/djelatnici/{id}/uredi` | POST | Edit | RedirectToAction | - | Sprema promjene |
| `/djelatnici/{id}/obriši` | POST | Delete | RedirectToAction | - | Obriši djelatnika |

**Korišteni Modeli:**
- `Djelatnik`
- `Project` (za multi-select projekata)

**Korišteni View-ovi:**
```
Views/Djelatnici/
├── Index.cshtml           (lista)
├── Details.cshtml         (detalji)
├── Create.cshtml          (forma - nova)
├── Edit.cshtml            (forma - edit)
└── _EmployeeForm.cshtml   (partial - koristi se u Create i Edit)
```

**Parcijalni Layout:**
- `_EmployeeForm.cshtml` - Ponovna korištena forma za Create i Edit

---

### **=== RADNI NALOZI CONTROLLER ===**

Upravljač za radne naloge.

| URL | HTTP | Action | Returns | View | Napomena |
|-----|------|--------|---------|------|---------|
| `/radni-nalozi` | GET | Index | List<RadniNalog> | `Index.cshtml` | Lista sa filtrima (status, pretraga) + KPI brojači |
| `/radni-nalozi/{id}/uredi` | GET | Edit | RadniNalog | `Edit.cshtml` | Forma sa podacima |
| `/radni-nalozi/{id}/uredi` | POST | Edit | RedirectToAction | - | Sprema promjene sa validacijom datuma |

**Korišteni Modeli:**
- `RadniNalog`
- `Project` (dropdown)

**Korišteni View-ovi:**
```
Views/RadniNalozi/
├── Index.cshtml   (lista sa filtrima i KPI)
└── Edit.cshtml    (forma za uređivanje)
```

**Validacije:**
- `DatumZatvaranja` mora biti >= `DatumOtvaranja`

---

## Pogled po View-ovima

### **Shared Layouts i Partiali**

```
Views/Shared/
├── _Layout.cshtml                  (master layout - header, footer, navbar)
├── _Layout.cshtml.css              (CSS za layout)
└── _ValidationScriptsPartial.cshtml (JS za client-side validaciju)
```

---

## Tok Podataka Po Akciji

### **1. Prikaz Liste**

```
URL: /projekti
  │
  ├─► HomeController.Index()
  │   ├─► _context.Projekti.Include(p => p.Kupac)
  │   ├─► .Where(...).ToList()
  │   └─► return View(projekti)
  │
  └─► Views/Home/Index.cshtml
      ├─► Prikaži <table> sa projekatima
      ├─► Za svaki red: [ Detalji ] [ Uredi ] [ Obriši ]
      └─► Linkovi na /projekti/{id}, /projekti/{id}/uredi, etc.
```

### **2. Prikaz Forme za Unos (GET)**

```
URL: /projekti/novi
  │
  ├─► HomeController.Create() [HttpGet]
  │   ├─► PopulateKupci() - PopuniSelectList sa kupcima
  │   └─► return View(new Project())
  │
  └─► Views/Home/Create.cshtml
      ├─► <form asp-action="Create" method="post">
      ├─► <input asp-for="BrojProjekta" />
      ├─► <select asp-for="KupacId" asp-items="...">
      ├─► <button type="submit">Spremi</button>
      └─► Data ide natrag kao querystring ili POST body
```

### **3. Obrada Forme (POST)**

```
URL: /projekti/novi [POST]
  │
  ├─► HomeController.Create(Project projekt) [HttpPost]
  │   ├─► ValidateProject(projekt)
  │   ├─► if (!ModelState.IsValid)
  │   │   └─► return View(projekt)  // Vrati formu sa greškama
  │   │
  │   ├─► _context.Projekti.Add(projekt)
  │   ├─► _context.SaveChanges()
  │   ├─► TempData["SuccessMessage"] = "Projekt dodan"
  │   └─► return RedirectToAction("Index")  // Preusmjeri na listu
  │
  └─► Browser: GET /projekti  (redirect)
      └─► Prikaži UpdatedList sa novim projektom
```

### **4. Uređivanje (GET)**

```
URL: /djelatnici/5/uredi
  │
  ├─► DjelatniciController.Edit(int id) [HttpGet]
  │   ├─► var djelatnik = _context.Djelatnici
  │   │        .Include(d => d.Projekti)
  │   │        .FirstOrDefault(d => d.Id == id)
  │   │
  │   ├─► PopulateProjects(djelatnik.Projekti.Select(p => p.Id))
  │   └─► return View(djelatnik)
  │
  └─► Views/Djelatnici/Edit.cshtml
      ├─► <form asp-action="Edit" method="post">
      ├─► <input asp-for="Ime" value="Marko" />
      ├─► <input asp-for="Email" value="marko@..." />
      ├─► <select name="projektiIds" multiple>
      │   └─► <option value="1" selected>Projekt A</option>
      │       <option value="2">Projekt B</option>
      │
      └─► [ Spremi ] [ Odustani ]
```

### **5. Uređivanje (POST)**

```
URL: /djelatnici/5/uredi [POST]
  │
  ├─► DjelatniciController.Edit(int id, Djelatnik updated, List<int> projektiIds) [HttpPost]
  │   ├─► var djelatnik = _context.Djelatnici.Include(...).FirstOrDefault(id)
  │   ├─► ValidateEmployee(updated)
  │   ├─► if (!ModelState.IsValid)
  │   │   └─► return View(updated)  // Vrati formu sa greškama
  │   │
  │   ├─► djelatnik.Ime = updated.Ime
  │   ├─► djelatnik.Email = updated.Email
  │   │
  │   ├─► djelatnik.Projekti.Clear()
  │   ├─► var selectedProjects = _context.Projekti.Where(...).ToList()
  │   ├─► foreach(projekt in selectedProjects)
  │   │   └─► djelatnik.Projekti.Add(projekt)
  │   │
  │   ├─► _context.SaveChanges()
  │   ├─► TempData["SuccessMessage"] = "Djelatnik ažuriran"
  │   └─► return RedirectToAction("Index")
  │
  └─► Browser: GET /djelatnici  (redirect)
      └─► Prikaži UpdatedList sa promijenjenim podatkom
```

---

## Validacije Po Akciji

### **Projekt - Create/Edit**
```
✓ BrojProjekta [Required], max 50 znakova
✓ Naziv [Required], max 200 znakova
✓ StatusPripremeRada - enum (mora biti jedan od mogućih)
✓ StatusLansiranja - enum (mora biti jedan od mogućih)
✓ StatusProizvodnje - enum (mora biti jedan od mogućih)
✓ Datumi: OcekivaniRokIsporuke, RealniRokIsporuke (nullable)
```

### **Kupac - Create**
```
✓ Naziv [Required], max 150 znakova
✓ OIB [Required], max 11 znakova
✓ Email [EmailAddress] validacija
✓ Tip [Required] - enum
```

### **Djelatnik - Create/Edit**
```
✓ Ime [Required], max 100 znakova
✓ Prezime [Required], max 100 znakova
✓ RadnoMjesto [Required], max 100 znakova
✓ Email [Required], [EmailAddress] validacija
✓ Tip [Required] - enum
✓ DatumZaposlenja - datum
✓ Projekti - može biti prazan ili više odabira (CTRL+Click)
```

### **RadniNalog - Edit**
```
✓ OznakaNaloga [Required], max 50 znakova
✓ Status - enum (Otvoren/U_Tijeku/Zatvoren)
✓ DatumOtvaranja - datum [Required]
✓ DatumZatvaranja - datum koji mora biti >= DatumOtvaranja
✓ ProjektId [Required] - mora postojati projekt
```

---

## HTTP Metode Po Akciji

| Action | HTTP | Opis |
|--------|------|------|
| **Index** | GET | Učitaj stranicu sa listom |
| **Details** | GET | Učitaj stranicu sa detaljima |
| **Create** (forma) | GET | Učitaj praznu formu za unos |
| **Create** (obrada) | POST | Spremi novi entitet |
| **Edit** (forma) | GET | Učitaj formu sa postojećim podacima |
| **Edit** (obrada) | POST | Spremi promjene |
| **Delete** | POST | Obriši entitet |

---

## Status Kodovi i Redirecti

| Akcija | Success | Redirect Na | Error | Action |
|--------|---------|------------|-------|--------|
| Kreiraj Projekt | 201 | /projekti | 400/422 | Vrati formu sa greškama |
| Uredi Djelatnika | 200 | /djelatnici | 400/422 | Vrati formu sa greškama |
| Obriši Kupca | 204 | /kupci | 404 | Prikaži grešku |
| ListaRadnih | 200 | Prikaži | - | - |

---

## TempData Poruke

**Success Messages:**
- "Projekt dodan" 
- "Projekt ažuriran"
- "Projekt obrisan"
- "Kupac dodan"
- "Kupac obrisan"
- "Djelatnik dodan"
- "Djelatnik ažuriran"
- "Djelatnik obrisan"

**Error Messages:**
- "Projekt nije pronađen"
- "Kupac nije pronađen"
- "Djelatnik nije pronađen"
- "[Field] je obavezno"
- "Email mora biti validan"
- Model validacijske greške

---

## ViewBag Svojstva

| Controller | ViewBag | Sadržaj | Korištenje |
|-----------|---------|--------|-----------|
| Home | Kupci | List<Kupac> | Select dropdown u Create/Edit |
| Kupci | - | - | - |
| Djelatnici | Projekti | List<Project> | Multi-select u Create/Edit |
| Djelatnici | SelektiraniProjekti | HashSet<int> | Selected vrednosti |
| RadniNalozi | TotalCount | int | KPI chip - Ukupno |
| RadniNalozi | OpenCount | int | KPI chip - Otvoreni |
| RadniNalozi | ActiveCount | int | KPI chip - Aktivni |
| RadniNalozi | ClosedCount | int | KPI chip - Zatvoreni |

---

## Povezani CSS i JavaScript

**CSS:**
- `wwwroot/css/site.css` - Globalni CSS

**JavaScript (Validacija):**
- `_ValidationScriptsPartial.cshtml` (učitava se na svim forma-stranicama)
  - jQuery validation
  - jquery-validation-unobtrusive (server-strana atributi → client-side validacija)

---

## Breadcrumb Navigacija

| Stranica | Breadcrumb |
|----------|-----------|
| Projekti - Početna | Home › Projekti |
| Projekti - Detalji | Home › Projekti › Detalji |
| Projekti - Novi | Home › Projekti › Novi |
| Kupci - Lista | Home › Kupci |
| Kupci - Novi | Home › Kupci › Novi |
| Djelatnici - Lista | Home › Djelatnici |
| Djelatnici - Novi | Home › Djelatnici › Novi |
| Djelatnici - Uredi | Home › Djelatnici › Uredi |
| Radni Nalozi - Lista | Home › Radni Nalozi |
| Radni Nalozi - Uredi | Home › Radni Nalozi › Uredi |

---

## Zaključak - Routing Matrica

```
┌─────────────────────────────────────────────────────────────┐
│ APLIKACIJA PLAMENICI - URL ROUTING MAPA                    │
├─────────────────────────────────────────────────────────────┤
│                                                              │
│ /projekti                    ──► Home.Index()               │
│ /projekti/{id}               ──► Home.Details()             │
│ /projekti/novi               ──► Home.Create()              │
│                                                              │
│ /kupci                       ──► Kupci.Index()              │
│ /kupci/novi                  ──► Kupci.Create()             │
│                                                              │
│ /djelatnici                  ──► Djelatnici.Index()         │
│ /djelatnici/novi             ──► Djelatnici.Create()        │
│ /djelatnici/{id}             ──► Djelatnici.Details()       │
│ /djelatnici/{id}/uredi       ──► Djelatnici.Edit()          │
│                                                              │
│ /radni-nalozi                ──► RadniNalozi.Index()        │
│ /radni-nalozi/{id}/uredi     ──► RadniNalozi.Edit()         │
│                                                              │
│ DEFAULT: /{controller}/{action}/{id?}                       │
│                                                              │
└─────────────────────────────────────────────────────────────┘
```

