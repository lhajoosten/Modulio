# Modulio

**Modulio** is een modulair, uitbreidbaar platform voor persoonlijke administratie, ontworpen voor zelfstandige professionals, hobbyisten en kleine teams. Het combineert functies voor planning, urenregistratie, boekhouding, notities met versiebeheer en toegangsbeheer in één centrale applicatie.

---

## 🚀 Features

- 📆 **Weekplanner**: plan en beheer je werk in een intuïtieve dag- of weekweergave
- ⏱️ **Urenregistratie**: log gewerkte uren per taak, project of klant
- 💸 **Boekhouding**: beheer inkomsten, uitgaven en btw met maandrapporten
- 📝 **Notitiesysteem**: schrijf, structureer en versieer notities per project
- 🔐 **Gebruikersbeheer**: rollen, rechten en policies voor elke module
- ⚙️ **Schaalbaar & herbruikbaar**: gebouwd met Clean Architecture, CQRS en DDD

---

## 🧱 Projectstructuur

```plaintext
AdminMate/
├── src/
│   ├── Modulio.Api                 # API layer (ASP.NET Core)
│   ├── Modulio.Application         # CQRS, DTOs, policies, behaviors
│   ├── Modulio.Domain              # Domeinmodellen en kernlogica
│   ├── Modulio.Infrastructure      # Logging, SMTP, FileStorage, etc.
│   ├── Modulio.Persistence         # DbContext, repositories, migrations
│   ├── Modulio.IdentityAccess      # Auth, rollen, permissies
│   ├── Modules/                    # Modules zoals Planning, TimeTracking, etc.
│   └── Shared/                     # Utilities, helpers, abstracties
├── tests/                          # Unit/integration tests per laag
├── docker/                         # Docker compose files voor lokale omgeving
└── README.md
````

---

## ⚙️ Technologieën

* Backend: **.NET 8**, **EF Core**, **CQRS**, **Clean Architecture**
* Database: **SQL Server**
* Frontend (apart): **Angular 17** (optioneel via Nx mono-repo)
* CI/CD: **GitHub Actions**
* Containerisatie: **Docker**

---

## 🧪 In ontwikkeling

| Module          | Status                   |
| --------------- | ------------------------ |
| 🧍 Identity     | ✅ Basisstructuur opgezet |
| 📆 Planning     | ⏳ MVP in opbouw          |
| ⏱️ TimeTracking | ⏳ Ontwerp                |
| 💸 Finance      | 🔜 Gepland               |
| 📝 Notes        | 🔜 Gepland               |

---

## 🛠️ Installatie

```bash
git clone https://github.com/<jouw-gebruikersnaam>/modulio.git
cd modulio
dotnet restore
dotnet build
dotnet run --project src/Modulio.Api
```

Optioneel (Docker):

```bash
docker compose -f docker/dev.yml up --build
```

---

## 📚 Documentatie

* [Architectuurprincipes](docs/architecture.md)
* [C4-model (Context, Container, Component)](docs/c4/)
* [Gebruikshandleiding (in aanbouw)](docs/manual.md)

---

## 📄 Licentie

MIT – Vrij te gebruiken, bewerken en distribueren.

