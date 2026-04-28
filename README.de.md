# SmartKitchen

Sprachen: [English](README.md) | [Deutsch](README.de.md) | [Русский](README.ru.md) | [中文](README.zh-CN.md)

SmartKitchen ist eine Full-Stack-Anwendung auf Basis von .NET 8 für zentrale Küchenabläufe. Die Anwendung vereint Rezeptverwaltung, Zutaten- und Bestandsverwaltung, Wochenplanung, automatische Einkaufslisten und Bestellverwaltung in einer Blazor-Server-Oberfläche mit einer ASP.NET-Core-Web-API.

Das Repository ist als geschichtete Solution aufgebaut und trennt Frontend, API, Domainmodell und Infrastruktur sauber voneinander. Die aktuelle Sprache der Benutzeroberfläche ist Deutsch.

## Überblick

SmartKitchen bildet einen praktischen Küchenprozess in einer Anwendung ab:

- Rezepte anlegen und pflegen
- Zutaten und Lagerbestände verwalten
- Wochenpläne für Mahlzeiten erstellen
- Einkaufslisten aus Wochenplanung und aktuellem Bestand erzeugen
- Bestellungen erfassen und im Dashboard auswerten

Die API verwendet Entity Framework Core mit SQLite und wendet beim Start automatisch ausstehende Migrationen an. Zusätzlich enthält das Projekt Seed-Daten, damit die Anwendung in der lokalen Entwicklung direkt nutzbar ist.

## Kernfunktionen

- Dashboard mit Kennzahlen, letzten Bestellungen und heutigen Mahlzeiten
- Rezeptverwaltung mit Zeiten, Portionen, Schwierigkeit und Kostenschätzung
- Zutatenkatalog und Lagerbestandsverwaltung
- Erkennung von niedrigem Bestand und bald ablaufenden Artikeln
- Wochenplanung für Mahlzeiten
- Generierung von Einkaufslisten auf Basis der geplanten Mahlzeiten abzüglich vorhandener Bestände
- Bestellverwaltung
- Swagger-Dokumentation der API in der Entwicklungsumgebung

## Architektur

```text
Blazor Server UI (SmartKitchen)
        |
        v
ASP.NET Core Web API (SmartKitchen.API)
        |
        v
Entity Framework Core + SQLite
        |
        v
Domain- und Infrastrukturprojekte
```

Die Lösung ist bewusst geschichtet aufgebaut und nicht als einzelnes monolithisches Projekt organisiert. Dadurch lassen sich UI, API, Fachmodell und Persistenz unabhängig weiterentwickeln.

## Projektstruktur

```text
SmartKitchen/
|- SmartKitchen.csproj               # Blazor-Server-Frontend
|- Components/                      # Razor-Komponenten und Seiten
|- SmartKitchen.API/                # ASP.NET Core Web API
|- SmartKitchen.Domain/             # Domänenmodelle
|- SmartKitchen.Application/        # Anwendungsschicht
|- SmartKitchen.Infrastructure/     # EF-Core-DbContext und Migrationen
|- wwwroot/                         # Statische Assets
`- SmartKitchen.sln                 # Einstieg über die Solution
```

## Technologie-Stack

| Bereich | Technologie |
| --- | --- |
| Frontend | Blazor Server (.NET 8) |
| Backend | ASP.NET Core Web API |
| Persistenz | Entity Framework Core |
| Datenbank | SQLite |
| API-Dokumentation | Swagger / OpenAPI |
| Tooling | .NET SDK 8 |

## Lokale Entwicklung

### Voraussetzungen

- .NET 8 SDK

### Abhängigkeiten wiederherstellen

```powershell
dotnet restore .\SmartKitchen.sln
```

### API starten

```powershell
dotnet run --project .\SmartKitchen.API\SmartKitchen.API.csproj
```

### Frontend starten

```powershell
dotnet run --project .\SmartKitchen.csproj
```

### Standard-URLs lokal

- Frontend: `http://localhost:5037`
- API: `http://localhost:5011`
- Swagger: `http://localhost:5011/swagger`

Wenn du die Solution in Rider oder Visual Studio startest, sollten Frontend und API als gemeinsame Startprojekte konfiguriert werden.

## Konfigurationshinweise

Das Frontend verwendet derzeit in `Program.cs` eine fest konfigurierte API-Basisadresse:

- `http://localhost:5011`

Wenn du den API-Port änderst, musst du diese Adresse ebenfalls anpassen.

## Datenbank und Seed-Daten

- Für die lokale Persistenz wird SQLite verwendet
- Die Connection String verweist auf `SmartKitchen.db`
- Die EF-Core-Migrationen liegen unter `SmartKitchen.Infrastructure/Migrations`
- Ausstehende Migrationen werden beim Start der API automatisch angewendet
- Seed-Daten enthalten Beispielzutaten, Rezepte und Lagerbestände

Lokale Datenbankdateien sind über `.gitignore` vom Repository ausgeschlossen.

## API-Oberfläche

Die aktuelle API stellt Endpunkte für folgende Bereiche bereit:

- `api/dashboard`
- `api/recipes`
- `api/ingredients`
- `api/inventory`
- `api/mealplans`
- `api/orders`
- `api/shoppinglist`

Swagger steht in der Entwicklungsumgebung zur Verfügung, um Endpunkte zu prüfen und manuell zu testen.

## Build

```powershell
dotnet build .\SmartKitchen.sln
```

## Hinweise zum Repository

- IDE-Metadaten sind von der Versionskontrolle ausgeschlossen
- Build-Artefakte sind von der Versionskontrolle ausgeschlossen
- Lokale SQLite-Dateien sind von der Versionskontrolle ausgeschlossen

## Status

Das Repository enthält bereits die zentrale Anwendungsstruktur und einen nutzbaren Funktionsumfang für die lokale Entwicklung. Es ist eine belastbare Grundlage für weitere Fachlogik, stärkere API-Validierung, automatisierte Tests und spätere Deployment-Automatisierung.
