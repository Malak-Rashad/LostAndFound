# La2eet (لقيت) — Egyptian Lost & Found Platform

La2eet is a web platform that helps people in Egypt find their lost belongings and return found items to their rightful owners in a safer, faster, and more organized way.

When someone loses something, they report it with details about the item. When someone finds an item, they post it too. La2eet connects both sides through search, filtering, ownership verification, and a claim-review flow.

## The Problem

In Egypt, lost items are usually reported through informal channels — Facebook groups, WhatsApp messages, posters, word of mouth. These are scattered, hard to search, and often unsafe:

- Lost item posts get buried quickly.
- Found items may never reach the right owner.
- There's no one trusted place to search.
- Communication between finder and owner is unorganized.
- There's no clear way to verify ownership before handing an item back — a real risk for sensitive items like phones, wallets, IDs, and keys.

## The Solution

La2eet provides a simple, trust-first flow:

1. A user reports a lost item or posts a found item, with category, city, location, date, description, and an optional photo.
2. Other users browse and filter reports by keyword, status, city, and date.
3. If someone believes an item belongs to them, they submit a claim.
4. For found items, the finder attaches verification questions — details only the real owner would know (e.g. "What color is the phone case?").
5. The claimant answers those questions as part of their claim; the finder reviews the answers and approves or rejects the claim.
6. Once a claim is approved, the two users can safely arrange the item's return.

## Why Egypt?

Egypt has high daily movement through public transportation, universities, malls, workplaces, cafes, and crowded streets — losing personal belongings is common, but recovery is usually random and unstructured. La2eet is designed around this local need. It uses Arabic RTL UI, Egyptian cities, familiar item categories, and a simple user experience suitable for everyday users.

## Who It's For

- People who've lost personal belongings, and people who've found items and want to return them.
- Students, commuters, and anyone moving through universities, public transport, malls, cafes, gyms, or offices.
- Employees and customers to recover lost items through a centralized platform.
- Schools, companies, and institutions that need a centralized lost-and-found system.
- On the metro, buses, and train stations — don’t leave things behind for good.

## Features

- **Accounts & authentication** — registration and login via ASP.NET Core Identity, email confirmation before first login, a Google sign-in option, and a forgot-password flow, with password rules, account lockout after repeated failures, and role-based access (regular users vs. admins).
- **Lost/Found reporting** — create a report with title, description, city (all 27 Egyptian governorates), specific location, date, and an optional photo.
- **Verification questions** — finders attach ownership-verification questions to Found reports; at least one is required.
- **Browse & search** — filter public reports by keyword, status (Lost/Found/Returned), city, and date range, with newest/oldest sorting and a similar-items suggestion on each report's detail page.
- **Claims** — claim an item you believe is yours, answer its verification questions (once per item), and track your claim's status; report owners review each claim in detail and approve or reject it from a dedicated Claims page.
- **Verified Chat (real-time)** — once a claim is approved, finder and claimant get a live chat thread (via SignalR) scoped to that item, and either side can mark the item as returned to close it out.
- **User profiles** — a profile page showing a user's reports and claims, with an edit-profile flow for updating name and phone; admins viewing another user's profile see an admin-specific view.
- **Admin dashboard** — total/lost/found/returned report counts, active claims, recent reports, and a per-city breakdown at a glance.
- **Report moderation** — every new report starts as Pending; only admin-approved reports are publicly visible on Browse.
- **User moderation** — admins can ban/unban accounts; banned users' content is hidden and they're blocked from signing back in.
- **Excel export** — admins can export all reports to a formatted `.xlsx` workbook for offline record-keeping.

**Live Demo:**
🔗 https://malakrashad-001-site1.jtempurl.com

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 9) |
| ORM | Entity Framework Core 9 |
| Database | Microsoft SQL Server |
| Auth | ASP.NET Core Identity (cookie-based, integer user keys), + Google OAuth |
| Real-time chat | SignalR (`/chatHub`) |
| Email | MailKit / MimeKit (email confirmation, password reset) |
| Excel export | ClosedXML |
| Front end | Razor views, Bootstrap, Font Awesome, jQuery Validation, SignalR JS client |

## System Requirements

**Software**
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB, SQL Server Express, or a full instance)
- Visual Studio 2022 (17.13+) or VS Code with the C# extension
- Git

**Hardware / OS**
- Any modern dev machine (4GB+ RAM) running Windows, macOS, or Linux is sufficient. A WSL launch profile is included for developing inside WSL against a Windows-hosted SQL Server.

## Installation

```bash
git clone https://github.com/Malak-Rashad/LostAndFound.git
cd LostAndFound
dotnet restore
```

This restores the project's NuGet packages: `Microsoft.EntityFrameworkCore.SqlServer`, `Microsoft.EntityFrameworkCore.Tools`, `Microsoft.AspNetCore.Identity.EntityFrameworkCore`, `Microsoft.AspNetCore.Authentication.Google`, `MailKit`, and `ClosedXML`.

## Configuration

`appsettings.json` is gitignored (it can hold real credentials), so the repo ships an `appsettings.example.json` template instead. Copy it and fill in your own values:

```bash
cp LostAndFound/appsettings.example.json LostAndFound/appsettings.json
```

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "AllowedHosts": "*",
  "Authentication": {
    "Google": {
      "ClientId": "",
      "ClientSecret": ""
    }
  }
}
```

- **Connection string** — for local SQL Server: `server=.;database=LostAndFoundDB;Trusted_connection=True;TrustServerCertificate=True` (swap `server=.` for `(localdb)\MSSQLLocalDB` if you're using LocalDB). The `LostAndFoundDB` database itself is created for you by the migration in the next step.
- **Google sign-in** — optional. Leave `ClientId`/`ClientSecret` blank to skip it; the app only registers the Google auth handler when both are present. Create credentials in the [Google Cloud Console](https://console.cloud.google.com/apis/credentials) if you want to enable it.
- **Email (account confirmation & password reset)** — required for registration to fully work, since new accounts must confirm their email before logging in. Add an `EmailSettings` section (used by `EmailService` via MailKit):

  ```json
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Email": "your-address@gmail.com",
    "Password": "your-app-password",
    "DisplayName": "لقيت"
  }
  ```

  This section isn't in `appsettings.example.json` yet — add it yourself, using an SMTP provider's credentials (e.g. a Gmail App Password).

## Running the Project Locally

1. Apply the EF Core migrations to create and seed the database:

   ```bash
   cd LostAndFound
   dotnet ef database update
   ```

   (Install the tool once if needed: `dotnet tool install --global dotnet-ef`.)

2. Run the app:

   ```bash
   dotnet run
   ```

3. Open it in your browser:
   - `https://localhost:60332`
   - `http://localhost:60334`

   (These ports come from `Properties/launchSettings.json`; running/debugging from Visual Studio uses the same profile.)

On first run, `SeedData.Initialize` seeds initial roles/accounts automatically. Note that any *new* account you register yourself must confirm its email (via the link sent by `EmailService`) before it can sign in — so make sure your `EmailSettings` are configured, or sign in with Google instead.

## API Documentation

Laqeet doesn't expose a separate REST/JSON API — it's a server-rendered MVC app where each route returns an HTML view. Here's the full route surface, grouped by controller:

| Controller | Route | Method | Description |
|---|---|---|---|
| Auth | `/Login` | GET/POST | Sign in (blocks unconfirmed or banned accounts) |
| Auth | `/Register` | GET/POST | Create an account; sends an email confirmation link |
| Auth | `/Logout` | POST | Sign out |
| Auth | `/ForgotPassword` | GET/POST | Request a password reset link |
| Auth | `/Auth/ConfirmEmail` | GET | Confirms a new account via the emailed token |
| Auth | `/Auth/GoogleLogin`, `/Auth/GoogleResponse` | GET | Google OAuth sign-in flow |
| Auth | `/AccessDenied` | GET | Shown when a signed-in user lacks the required role |
| Home | `/` | GET | Recently approved reports |
| Items | `/Items/Browse` | GET | Search/filter/sort reports (keyword, status, city, date range) |
| Items | `/Items/Details/{id}` | GET | Report detail page, with similar-item suggestions |
| Items | `/Items/Create` | GET/POST | Submit a Lost/Found report (auth required to POST) |
| Claims | `/Claims` | GET | Your claims + incoming claims (auth required) |
| Claims | `/Claims/Create?itemId=` | GET/POST | Submit a claim with verification answers |
| Claims | `/Claims/Review/{id}` | GET | Full detail view of a claim, for the report owner |
| Claims | `/Claims/Approve/{id}` | POST | Approve a claim on your own report |
| Claims | `/Claims/Reject/{id}` | POST | Reject a claim on your own report |
| Messages | `/Messages?itemId=&receiverId=` | GET | Open (or start) a real-time chat thread for an item |
| Messages | `/Messages/MarkAsReturned` | POST | Mark an item as returned, closing its chat |
| Users | `/Users/Profile/{id?}` | GET | View your own profile, or another user's (auth required) |
| Users | `/Users/Edit` | GET/POST | Edit your name and phone number |
| Admin | `/Admin` | GET | Dashboard: report/claim/return counts, recent items, city breakdown |
| Admin | `/Admin/ApproveItem/{id}` | POST | Approve a pending report |
| Admin | `/Admin/RejectItem/{id}` | POST | Reject a pending report |
| Admin | `/Admin/BanUser/{userId}` | POST | Toggle a user's ban status |
| Admin | `/Admin/ExportReportsToExcel` | GET | Download all reports as `.xlsx` |

Real-time chat additionally uses a SignalR hub at `/chatHub` (see `Hubs/ChatHub.cs`) alongside the `Messages` routes above.

## Executable Files & Deployment

La2eet is deployed and available online.

**Live Demo:**
🔗 https://malakrashad-001-site1.jtempurl.com

📍 La2eet — Helping Lost Items Find Their Way Home.
