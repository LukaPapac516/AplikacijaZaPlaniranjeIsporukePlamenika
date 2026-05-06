# Semantic DB Model

## Scope
This document describes the semantic database model for the application based on:
- EF model classes in Models/
- EF configuration in Data/AppDbContext.cs
- Initial migration in Migrations/20260420204430_Initial.cs

## Entities and Main Properties

### Project (table: Projekti)
- Id (PK)
- BrojProjekta (required, max 50)
- Naziv (required, max 200)
- KupacId (nullable FK)
- DatumUnosa
- StatusPripremeRada
- StatusLansiranja
- StatusProizvodnje
- OcekivaniRokIsporuke (nullable)
- RealniRokIsporuke (nullable)
- Napomena (max 500)
- NapomenaPrioritet
- Prioritet
- Kasnjenje (computed property in code, not persisted as DB column)

### Kupac (table: Kupci)
- Id (PK)
- Naziv (required, max 150)
- OIB (required, max 11)
- Adresa (max 200)
- Grad (max 100)
- KontaktOsoba (max 100)
- Email (email, max 150)
- Telefon (max 20)
- Tip
- DatumUnosa

### Djelatnik (table: Djelatnici)
- Id (PK)
- Ime (required, max 100)
- Prezime (required, max 100)
- RadnoMjesto (required, max 100)
- Email (required, email, max 150)
- Tip
- DatumZaposlenja

### RadniNalog (table: RadniNalozi)
- Id (PK)
- OznakaNaloga (required, max 50)
- DatumOtvaranja
- DatumZatvaranja (nullable)
- Status
- Opis (max 500)
- ProjektId (required FK)

### StavkaProizvodnje (table: StavkeProizvodnje)
- Id (PK)
- Tip
- DatumPlaniranja (nullable)
- DatumZavrsetka (nullable)
- Status
- Napomena (max 500)
- ProjektId (required FK)
- RadniNalogId (nullable FK)

### FazaProjekta (table: FazeProjekta)
- Id (PK)
- Naziv (required, max 150)
- Redosljed (required)
- PlaniraniPocetak (nullable)
- PlaniraniZavrsetak (nullable)
- StvarniPocetak (nullable)
- StvarniZavrsetak (nullable)
- Status
- Napomena (max 500)
- DatumPlaniranja
- ProjektId (required FK)

### Napomena (table: Napomene)
- Id (PK)
- Tekst (required, max 1000)
- DatumUnosa
- AutorId (required FK)
- Prioritet
- DatumOtvaranja
- ProjectId (nullable FK, generated in migration as shadow FK due Project.Napomene navigation)

### Junction table for many-to-many
#### DjelatnikProject
- DjelatniciId (FK -> Djelatnici.Id)
- ProjektiId (FK -> Projekti.Id)
- Composite PK: (DjelatniciId, ProjektiId)

## Relationships

1. Kupac (1) -> (N) Project
- FK: Projekti.KupacId
- On delete: SetNull
- Meaning: deleting a customer does not delete projects; project keeps existing with KupacId = null.

2. Project (1) -> (N) RadniNalog
- FK: RadniNalozi.ProjektId
- On delete: Cascade

3. Project (1) -> (N) StavkaProizvodnje
- FK: StavkeProizvodnje.ProjektId
- On delete: Cascade

4. RadniNalog (1) -> (N) StavkaProizvodnje
- FK: StavkeProizvodnje.RadniNalogId (nullable)
- On delete: SetNull

5. Project (1) -> (N) FazaProjekta
- FK: FazeProjekta.ProjektId
- On delete: Cascade

6. Djelatnik (1) -> (N) Napomena (as author)
- FK: Napomene.AutorId
- On delete: Restrict

7. Project (N) <-> (N) Djelatnik
- Junction: DjelatnikProject
- On delete: Cascade from both parent tables to junction rows

8. Project (1) -> (N) Napomena
- Implemented in DB migration as optional FK Napomene.ProjectId (shadow relation)
- On delete: default no action in migration (no explicit cascade)
- Note: this relation is not explicitly configured in OnModelCreating, but exists because of Project.Napomene navigation.

## Enums in Domain Model
- StatusTriStanja
- StatusProizvodnje
- TipKupca
- TipDjelatnika
- StatusNaloga
- TipOperacije
- StatusStavke
- StatusFaze
- PrioritetNapomene

## Notes
- Database provider: SQLite
- Connection string: Data Source=plamenici.db
- EF nullable warnings currently exist in models and can be cleaned separately.
