# NFCEPS — Near Field Communication Enabled Payment System

A full-stack NFC-based bus payment system built with ASP.NET Core, Blazor, and ESP32. Passengers tap a MIFARE Classic card (or any other NFC card) on a bus-mounted NFC reader to pay fares. The system handles offline operation, end-of-day sync, route management, and admin settlement.

---

## Overview

```
Passenger taps card on bus → ESP32 reads RFID → fare deducted from card
Bus reaches depot → ESP32 syncs transactions to API → database updated
Admin settles collected fares → entity owners paid via branch
```

---

## Architecture

```text
NFCEPS/
├── NFCEPS.Domain/          Core entities, enums, and base response models
├── NFCEPS.Application/     CQRS (MediatR), Handlers, DTOs, and Interfaces
├── NFCEPS.Infrastructure/  Dapper implementation (GenericRepository)
├── NFCEPS.Presentation/    ASP.NET Core 9 Web API Controllers
├── NFCEPS.UI/              Blazor Web App 9 + MudBlazor (Vertical Slice Architecture)
└── NFCEPS.TEST/            xUnit Tests (Mirroring solution structure)
```

**Hardware**
- ESP32 microcontroller
- PN532 NFC module
- MIFARE Classic 1K cards
- LittleFS for offline transaction storage

**Database**
- PostgreSQL (migrated from MSSQL)
- 19 tables across 8 schemas
- All business logic in stored procedures and functions
- Views are implemented for scalable data retrival
- Dapper for data access
- CQRS implementation in database level


---

## Features

### Payment
- NFC card tap-in / tap-out on bus
- Route-based fixed fare lookup
- Offline operation — balance stored on MIFARE card sectors
- End-of-day sync to database when bus returns to depot
- Duplicate transaction prevention via `LastTransactionId`
- Force-close open transactions on reset at the last bus stop

### Admin (Blazor Web App)
- User and card management
- Entity owner and entity (bus company) management
- Route and stop management
- Fare rule configuration per route
- Card recharge
- Owner settlement tracking
- Role-based permission management (dot-notation: `USR.R`, `CRD.C`, etc.)
- Bus session monitoring

### Driver (Blazor Web App)
- Login and session creation
- Route selection for the day
- Machine assignment
- Session management

### User (Mobile App — maybe?)
- View card balance
- View transaction history
- View recharge history

---

## Database Schema

| Schema | Tables |
|---|---|
| `Permission` | `tblRoles`, `tblPermission`, `tblRolePermission` |
| `User` | `tblUsers` |
| `Entity` | `tblEntity`, `tblEntityOwner` |
| `Route` | `tblRoute`, `tblStop`, `tblRouteStop`, `tblFareRule` |
| `Machine` | `tblMachine`, `tblBusSession` |
| `Card` | `tblCard`, `tblCardHistory` |
| `Transaction` | `tblUserPaymentHistory`, `tblPendingSync` |
| `Branch` | `tblBranch`, `tblOwnerSettlement`, `tblCardRecharge` |

---

## Permission System

Permissions use dot-notation CRUD format per module:

```
USR.C  USR.R  USR.U  USR.D   — User
CRD.C  CRD.R  CRD.U  CRD.D   — Card
ENT.C  ENT.R  ENT.U  ENT.D   — Entity
MCH.C  MCH.R  MCH.U  MCH.D   — Machine
TXN.R                        — Transactions
STL.C  STL.R                 — Settlement
RCH.C  RCH.R                 — Recharge
PIM.C  PIM.R  PIM.U  PIM.D   — Permission Management
SES.C  SES.R  SES.U          — Session
```

Permissions are loaded into a singleton `Dictionary<int, HashSet<string>>` cache at startup — no database hit per request.

---

## Tech Stack

| Layer | Technology |
|---|---|
| API | ASP.NET Core 9 |
| UI | Blazor Server 9 + MudBlazor |
| Database | PostgreSQL |
| ORM | Dapper |
| Auth | JWT Bearer + BCrypt |
| Mapping | AutoMapper |
| API Docs | Scalar |
| Firmware | Arduino / PlatformIO (ESP32) |
| NFC Library | Adafruit PN532 |
| Offline Storage | LittleFS (ESP32 internal flash) |

---

## Getting Started

### Prerequisites

- .NET 9 SDK
- Visual Studio Code with C# Dev Kit or Visual Studio
- PostgreSQL
- pgAdmin
- Arduino IDE or PlatformIO

### 1 — Database Setup

Run the scripts in order:

```
database/usefulscripts
├── create_table.sql    — creates all schemas and tables
└── foreign_keys.sql      — add after testing is complete
(more folders such as 'Stored Procedure' and 'Functions' will be added as the project grows)
(soon implementing DbUp for this)
```

```bash
# OUT OF SUPPORT
# connect to your SQL Server instance and run:
sqlcmd -S localhost -i database/001_initial_schema.sql
sqlcmd -S localhost -i database/002_seed_data.sql
```

### 2 — API Setup

```bash
cd NFCEPS.Presentation
```

Set your user secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "JwtSettings:SecretKey" "your-secret-key-min-32-chars"
dotnet user-secrets set "ConnectionStrings:NFCEPS_DB" "Server=localhost;Database=NFCEPS;Trusted_Connection=True;TrustServerCertificate=True;"
```

Run:

```bash
dotnet run
```

Scalar available at `https://localhost:7279/scalar`

### 3 — UI Setup

```bash
cd NFCEPS.UI
dotnet run
```

UI available at `https://localhost:7183`

### 4 — Via the signup page create your credentials for login

```
# Example Login Credentials
Username: admin
Password: admin123
```

---

## Backend Structure (Clean Architecture & CQRS)

```text
NFCEPS.Application/
├── Features/            — CQRS Commands and Queries
│   ├── Auth/
│   │   ├── Commands/
│   │   └── Queries/
│   └── Reports/
├── Interfaces/          — IGenericRepository
└── Models/              — DTOs for Request/Response
```

### Generic Repository Methods

```csharp
// single table result
Task<IEnumerable<T>> QueryAsync<T>(string sp, object? params)

// single row
Task<T?> QueryFirstOrDefaultAsync<T>(string sp, object? params)

// multiple tables
Task<T> GetFromMultipleQueriesAsync<T>(string sp, Func<GridReader, Task<T>> map, object? params)

// no return
Task ExecuteAsync(string sp, object? params)

// scalar value
Task<T?> ExecuteScalarAsync<T>(string sp, object? params)
```

---

## UI Structure (Vertical Slice Architecture)

The Blazor UI is structured using Vertical Slices, organizing code by feature rather than by type:

```text
NFCEPS.UI/
├── Features/
│   ├── Auth/             — Login.razor, AuthManager.cs, LoginRequest.cs
│   ├── Dashboard/        — Dashboard.razor, DashboardManager.cs
│   ├── Reports/          — UserReport.razor, ReportManager.cs
│   └── Card/             — AssignCard.razor, CardManager.cs
├── Shared/
│   ├── Layouts/          — MainLayout.razor, NavMenu.razor
│   ├── Security/         — PermissionService.cs, AuthStateProvider.cs
│   └── Infrastructure/   — BaseManager.cs, ApiResponse.cs
└── wwwroot/              — CSS, JS, Bootstrap
```

---

## ESP32 Flow

```
Boot
└── Connect to WiFi
└── Poll GET /api/machine/{machineId}/session
└── Download route + stop list + fare table
└── Store in LittleFS

Journey (offline)
└── Driver button press → advance current stop
└── Passenger tap → read RFID from card
    ├── No open transaction → TAP IN
    │   └── Check balance >= minimum fare
    │   └── Set open flag on card
    │   └── Store tap-in in LittleFS
    └── Open transaction exists → TAP OUT
        └── Calculate stops traveled
        └── Lookup fare from local table
        └── Deduct from card balance
        └── Write new balance to card
        └── Store completed transaction in LittleFS

Depot (online)
└── POST /api/machine/{machineId}/sync
└── Send all LittleFS transactions
└── Clear LittleFS on success confirmation
```

---

## Security Notes

- JWT secret stored in environment variables / user secrets — never in `appsettings.json`
- Passwords hashed with BCrypt
- Machine API keys stored as `varbinary` with expiry tracking
- MIFARE Classic UIDs stored as `E2:B7:B5:02` format — colon-separated, no transformation on ESP32
- RFID has a unique index for fast payment lookups
- Rate limiting on payment endpoint recommended before production

> **Note:** MIFARE Classic encryption is known to be vulnerable. Card UIDs can be cloned with hardware like a Proxmark. For a production deployment, validate transaction history on sync and flag balance anomalies.

---

## Roadmap

- [x] Database schema design
- [x] Architectural Decisions
- [x] API foundation — auth, repository, middleware
- [x] JWT + BCrypt authentication
- [x] Role-based permission system with cache
- [x] Blazor login page
- [ ] User and card management UI
- [ ] Entity and machine management UI
- [ ] Route and fare management UI
- [ ] Bus session management (driver flow)
- [ ] Payment processing endpoint
- [ ] ESP32 firmware — breadboard prototype
- [ ] ESP32 firmware — payment flow
- [ ] Offline sync system
- [ ] PCB design (EasyEDA)
- [ ] 3D enclosure design (OpenSCAD)
- [ ] Reporting and fraud detection
- [ ] Production hardening

---

## License

MIT
