# Semantički Model Baze Podataka

## Popis Entiteta i Svojstava

### **1. Projekt (Project)**
Predstavlja proizvodnu narudžbu/projekt za kupca.

**Tablica:** `Projekti`

| Svojstvo | Tip | Opis | Ograničenja |
|----------|-----|------|-----------|
| **Id** | int | Primarni ključ | PRIMARY KEY, Auto-increment |
| **BrojProjekta** | string | Jedinstveni broj projekta | [Required], max 50 znakova |
| **Naziv** | string | Naziv projekta | [Required], max 200 znakova |
| **KupacId** | int? | FK - Kupac koji je naručio | [Foreign Key → Kupci] |
| **DatumUnosa** | DateTime | Datum unosa u sistem | format: DateTime |
| **StatusPripremeRada** | StatusTriStanja | Priprema (nije startano/startano/završeno) | enum |
| **StatusLansiranja** | StatusTriStanja | Lansiranje (nije startano/startano/završeno) | enum |
| **StatusProizvodnje** | StatusProizvodnje | Faza proizvodnje | enum: Nije/Bravari/Farbanje/Montaža/Elektrika/Gotovo |
| **OcekivaniRokIsporuke** | DateTime? | Očekivani rok | nullable |
| **RealniRokIsporuke** | DateTime? | Stvarni rok isporuke | nullable |
| **Napomena** | string | Tekstualna napomena | max 500 znakova |
| **NapomenaPrioritet** | PrioritetNapomene | Prioritet napomene | enum: Nizak/Srednji/Visok (default: Srednji) |
| **Prioritet** | bool | Je li projekt prioritetan | default: false |

**Navigacijska Svojstva (Relacije):**
- `Kupac` (1:1 obrnuto) → Kupac koji je naručio
- `Djelatnici` (N:M) → Lista djelatnika dodijeljena projektu
- `RadniNalozi` (1:M) → Lista radnih naloga za projekt
- `StavkeProizvodnje` (1:M) → Liste operacija/stavki proizvodnje
- `FazeProjekta` (1:M) → Faze projekta

**Kalkulirana Svojstva:**
- `Kasnjenje` (int?) = `RealniRokIsporuke - OcekivaniRokIsporuke` (dani)

---

### **2. Kupac (Kupac)**
Predstavlja kupca ili naručitelja koji naručuje projekte.

**Tablica:** `Kupci`

| Svojstvo | Tip | Opis | Ograničenja |
|----------|-----|------|-----------|
| **Id** | int | Primarni ključ | PRIMARY KEY, Auto-increment |
| **Naziv** | string | Naziv kupca/tvrtke | [Required], max 150 znakova |
| **OIB** | string | Osobni/Poslovni identifikacijski broj | [Required], max 11 znakova |
| **Adresa** | string | Fizička adresa | max 200 znakova |
| **Grad** | string | Grad | max 100 znakova |
| **KontaktOsoba** | string | Osoba za kontakt | max 100 znakova |
| **Email** | string | Email adresa | [EmailAddress] ✓, max 150 znakova |
| **Telefon** | string | Telefonski broj | max 20 znakova |
| **Tip** | TipKupca | Vrsta kupca | enum: PravnaOsoba / FizickaOsoba / InozemniPartner |
| **DatumUnosa** | DateTime | Datum unosa u sistem | format: DateTime |

**Navigacijska Svojstva (Relacije):**
- `Projekti` (1:M) → Lista projekata ovog kupca

**Enumeracije:**
```csharp
enum TipKupca {
    PravnaOsoba,
    FizickaOsoba,
    InozemniPartner
}
```

---

### **3. Djelatnik (Djelatnik)**
Predstavlja zaposlenika izvedbe projekta.

**Tablica:** `Djelatnici`

| Svojstvo | Tip | Opis | Ograničenja |
|----------|-----|------|-----------|
| **Id** | int | Primarni ključ | PRIMARY KEY, Auto-increment |
| **Ime** | string | Ime | [Required], max 100 znakova |
| **Prezime** | string | Prezime | [Required], max 100 znakova |
| **RadnoMjesto** | string | Radno mjesto | [Required], max 100 znakova |
| **Email** | string | Email adresa | [EmailAddress] ✓, [Required], max 150 znakova |
| **Tip** | TipDjelatnika | Tip/kategorija djelatnika | enum |
| **DatumZaposlenja** | DateTime | Datum zaposlenja | format: DateTime |

**Navigacijska Svojstva (Relacije):**
- `Projekti` (N:M) → Lista projekata kojima je dodijeljen

**Enumeracije:**
```csharp
enum TipDjelatnika {
    Inzenjer,
    Tehnicar,
    Radnik,
    Voditelj,
    Administracija
}
```

---

### **4. Radni Nalog (RadniNalog)**
Predstavlja pojedini radni nalog vezan za projekt.

**Tablica:** `RadniNalozi`

| Svojstvo | Tip | Opis | Ograničenja |
|----------|-----|------|-----------|
| **Id** | int | Primarni ključ | PRIMARY KEY, Auto-increment |
| **OznakaNaloga** | string | Jedinstvena oznaka naloga | [Required], max 50 znakova |
| **DatumOtvaranja** | DateTime | Datum otvaranja naloga | format: DateTime |
| **DatumZatvaranja** | DateTime? | Datum zatvaranja naloga | nullable |
| **Status** | StatusNaloga | Status naloga | enum: Otvoren / U_Tijeku / Zatvoren |
| **Opis** | string | Opis posla | max 500 znakova |
| **ProjektId** | int | FK - Projekt kojem pripada | [Required], [Foreign Key → Projekti] |

**Navigacijska Svojstva (Relacije):**
- `Projekt` (1:1 obrnuto) → Projekt kojem pripada
- `Operacije` (1:M) → Lista stavki/operacija proizvodnje

**Enumeracije:**
```csharp
enum StatusNaloga {
    Otvoren,
    U_Tijeku,
    Zatvoren
}
```

---

### **5. Stavka Proizvodnje (StavkaProizvodnje)**
Predstavlja pojedinu stavku/operaciju proizvodnog procesa.

**Tablica:** `StavkeProizvodnje`

| Svojstvo | Tip | Opis | Ograničenja |
|----------|-----|------|-----------|
| **Id** | int | Primarni ključ | PRIMARY KEY, Auto-increment |
| **Tip** | TipOperacije | Tip operacije | enum: BravariZavarivaci / Planiranje / Elektricari / Farbanje / Montaza |
| **DatumPlaniranja** | DateTime? | Planiran datum | nullable |
| **DatumZavrsetka** | DateTime? | Datum završetka | nullable |
| **Status** | StatusStavke | Status stavke | enum: NijePoceto / U_Tijeku / Zavrseno |
| **Napomena** | string | Adicijska napomena | max 500 znakova |
| **ProjektId** | int | FK - Projekt kojem pripada | [Required], [Foreign Key → Projekti] |
| **RadniNalogId** | int? | FK - Radni nalog kojem pripada | [Foreign Key → RadniNalozi] |

**Navigacijska Svojstva (Relacije):**
- `Projekt` (1:M obrnuto) → Projekt kojem pripada (obavezna)
- `RadniNalog` (1:M obrnuto) → Radni nalog kojem pripada (opcionalna)

**Enumeracije:**
```csharp
enum TipOperacije {
    BravariZavarivaci,
    Planiranje,
    Elektricari,
    Farbanje,
    Montaza
}

enum StatusStavke {
    NijePoceto,
    U_Tijeku,
    Zavrseno
}
```

---

### **6. Faza Projekta (FazaProjekta)**
Predstavlja glavne faze izvršavanja projekta.

**Tablica:** `FazeProjekta`

| Svojstvo | Tip | Opis | Ograničenja |
|----------|-----|------|-----------|
| **Id** | int | Primarni ključ | PRIMARY KEY, Auto-increment |
| **Naziv** | string | Naziv faze | [Required], max 150 znakova |
| **Redosljed** | int | Redosljed izvršavanja | [Required], int > 0 |
| **PlaniraniPocetak** | DateTime? | Planiran početak | nullable |
| **PlaniraniZavrsetak** | DateTime? | Planiran završetak | nullable |
| **StvarniPocetak** | DateTime? | Stvarni početak | nullable |
| **StvarniZavrsetak** | DateTime? | Stvarni završetak | nullable |
| **Status** | StatusFaze | Status faze | enum: NijePocelo / U_Tijeku / Zavrseno |
| **Napomena** | string | Napomena | max 500 znakova |
| **DatumPlaniranja** | DateTime | Datum planiranja faze | format: DateTime |
| **ProjektId** | int | FK - Projekt kojem pripada | [Required], [Foreign Key → Projekti] |

**Navigacijska Svojstva (Relacije):**
- `Projekt` (1:M obrnuto) → Projekt kojem pripada

**Enumeracije:**
```csharp
enum StatusFaze {
    NijePocelo,
    U_Tijeku,
    Zavrseno
}
```

---

### **7. Napomena (Napomena)**
Predstavlja bilješku/komentar vezan za projekt.

**Tablica:** `Napomene`

| Svojstvo | Tip | Opis | Ograničenja |
|----------|-----|------|-----------|
| **Id** | int | Primarni ključ | PRIMARY KEY, Auto-increment |
| **Tekst** | string | Tekstualna napomena | [Required], max 1000 znakova |
| **DatumUnosa** | DateTime | Datum unosa napomene | format: DateTime |
| **AutorId** | int | FK - Autor napomene | [Required], [Foreign Key → Djelatnici] |
| **Prioritet** | PrioritetNapomene | Prioritet napomene | enum: Nizak / Srednji / Visok |
| **DatumOtvaranja** | DateTime | Datum otvaranja/kreiranja | format: DateTime |

**Navigacijska Svojstva (Relacije):**
- `Autor` (1:M obrnuto) → Djelatnik koji je upisao napomenu

**Enumeracije:**
```csharp
enum PrioritetNapomene {
    Nizak,
    Srednji,
    Visok
}
```

---

## Relacijski Dijagram

### **Glavne Relacije:**

```
┌──────────────────┐
│     KUPCI        │ (1)
│  (7 instances)   │
└────────┬─────────┘
         │ 1:M (FK: KupacId)
         │ [OnDelete: SetNull]
         │
┌────────▼─────────────────────────┐
│      PROJEKTI                    │ (M)
│  (primarni entitet - 50+)        │
└────┬──────────┬─────────┬────────┘
     │          │         │
   1:M        N:M       1:M
     │          │         │
     │   ┌──────▼──────┐   │
     │   │  DJELATNICI │   │
     │   │  (junction:  │   │
     │   │ DjelatnikPro│   │
     │   │  ject)       │   │
     │   └─────────────┘   │
     │                     │
     ├─► RADNI_NALOZI (1:M)
     │   └─► STAVKE_PROIZVODNJE (1:M)
     │       [FK: RadniNalogId nullable]
     │
     ├─► FAZE_PROJEKTA (1:M)
     │   [OnDelete: Cascade]
     │
     ├─► STAVKE_PROIZVODNJE (1:M)
     │   [OnDelete: Cascade]
     │
     └─► NAPOMENE (1:M obrnuto)


DJELATNICI (1) ──┐
                 │ 1:M (kao Autor)
                 │ [OnDelete: Restrict]
                 │
           ┌─────▼──────────┐
           │   NAPOMENE     │ (M)
           └────────────────┘
```

---

## Enumeracije i Tipovi

### **StatusTriStanja**
- `NijeStartano` = 0
- `Startano` = 1
- `Zavrseno` = 2

### **StatusProizvodnje**
- `NijeUProizvodnji` = 0
- `BravariZavarivaci` = 1
- `Farbanje` = 2
- `Montaza` = 3
- `Elektricari` = 4
- `Gotovo` = 5

### **TipKupca**
- `PravnaOsoba` = 0
- `FizickaOsoba` = 1
- `InozemniPartner` = 2

### **TipDjelatnika**
- `Inzenjer` = 0
- `Tehnicar` = 1
- `Radnik` = 2
- `Voditelj` = 3
- `Administracija` = 4

### **TipOperacije**
- `BravariZavarivaci` = 0
- `Planiranje` = 1
- `Elektricari` = 2
- `Farbanje` = 3
- `Montaza` = 4

### **StatusNaloga**
- `Otvoren` = 0
- `U_Tijeku` = 1
- `Zatvoren` = 2

### **StatusStavke**
- `NijePoceto` = 0
- `U_Tijeku` = 1
- `Zavrseno` = 2

### **StatusFaze**
- `NijePocelo` = 0
- `U_Tijeku` = 1
- `Zavrseno` = 2

### **PrioritetNapomene**
- `Nizak` = 0
- `Srednji` = 1 (default)
- `Visok` = 2

---

## Foreign Key Ponašanja

| FK Relacija | OnDelete Ponašanje | Razlog |
|-------------|-------------------|--------|
| Projekt → Kupac | **SetNull** | Ako se kupac obriše, projekti ostaju ali FK postaje null |
| Projekt → RadniNalozi | **Cascade** | Obriši sve radne naloge ako se projekt obriše |
| Projekt → FazeProjekta | **Cascade** | Obriši sve faze ako se projekt obriše |
| Projekt → StavkeProizvodnje | **Cascade** | Obriši sve stavke ako se projekt obriše |
| RadniNalog → StavkeProizvodnje | **SetNull** | Ako se nalog obriše, stavke ostaju ali FK postaje null |
| Napomena → Djelatnik (Autor) | **Restrict** | Ne dozvoli brisanje djelatnika ako ima napomena (zaštita) |
| DjelatnikProject (N:M) | **Cascade** | Ako se projekt obriše, obriši sve veze; ako se djelatnik obriše, obriši sve veze |

---

## Indeksi

Automatski generirani indeksi za performanse:

```sql
CREATE INDEX IX_Projekti_KupacId ON Projekti(KupacId);
CREATE INDEX IX_RadniNalozi_ProjektId ON RadniNalozi(ProjektId);
CREATE INDEX IX_StavkeProizvodnje_ProjektId ON StavkeProizvodnje(ProjektId);
CREATE INDEX IX_StavkeProizvodnje_RadniNalogId ON StavkeProizvodnje(RadniNalogId);
CREATE INDEX IX_FazeProjekta_ProjektId ON FazeProjekta(ProjektId);
CREATE INDEX IX_Napomene_AutorId ON Napomene(AutorId);
CREATE INDEX IX_Napomene_ProjectId ON Napomene(ProjectId);
CREATE INDEX IX_DjelatnikProject_ProjektiId ON DjelatnikProject(ProjektiId);
```

---

## Sažetak

| Entitet | Tablica | Zapisa | PK | FK | Tip |
|---------|---------|--------|----|----|-----|
| **Project** | Projekti | ~50+ | Id | KupacId | Master |
| **Kupac** | Kupci | 7+ | Id | - | Lookup |
| **Djelatnik** | Djelatnici | 10+ | Id | - | Lookup |
| **RadniNalog** | RadniNalozi | M | Id | ProjektId | Detail |
| **StavkaProizvodnje** | StavkeProizvodnje | M | Id | ProjektId, RadniNalogId | Detail |
| **FazaProjekta** | FazeProjekta | M | Id | ProjektId | Detail |
| **Napomena** | Napomene | M | Id | AutorId | Detail |
| **Junction** | DjelatnikProject | M | (DjelatniciId, ProjektiId) | - | N:M |

