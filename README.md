# Auction Tracker Worker Dokumentation

Det nedenstående dokument beskriver de tilgængelige endpoints og deres funktionalitet i Auction Tracker Worker.

## CRUDController

`CRUDController` håndterer CRUD-operationer (Create, Read, Update, Delete) for auktionsdata i databasen.

### Endpoints

#### Get all active bids

- Metode: GET
- URL: `/bidworker/v1/getallactivebids`
- Beskrivelse: Henter alle aktive bud fra databasen.
- Parametre: Ingen
- Returneret respons: En liste af aktive bud.

#### Get active bid by catalog id

- Metode: GET
- URL: `/bidworker/v1/getactivebidbycatalogid/{id}`
- Beskrivelse: Henter et aktivt bud baseret på katalog-id.
- Parametre:
  - `id` (streng): Katalog-id'et for det ønskede bud.
- Returneret respons: Det aktive bud, der svarer til det angivne katalog-id.

#### Get active bids by email

- Metode: GET
- URL: `/bidworker/v1/getactivebidsbyemail/{email}`
- Beskrivelse: Henter alle aktive bud for en bestemt e-mail-adresse.
- Parametre:
  - `email` (streng): E-mail-adressen for budgiveren.
- Returneret respons: En liste af aktive bud, der tilhører den angivne e-mail-adresse.

#### Get all logs

- Metode: GET
- URL: `/bidworker/v1/getalllogs`
- Beskrivelse: Henter alle logposter fra databasen.
- Parametre: Ingen
- Returneret respons: En liste af logposter.

#### Get logs by catalog id

- Metode: GET
- URL: `/bidworker/v1/getlogsbycatalogid/{id}`
- Beskrivelse: Henter logposter baseret på katalog-id.
- Parametre:
  - `id` (streng): Katalog-id'et for de ønskede logposter.
- Returneret respons: En liste af logposter, der tilhører det angivne katalog-id.

#### Get logs by email

- Metode: GET
- URL: `/bidworker/v1/getlogsbycatalogid/{email}`
- Beskrivelse: Henter logposter baseret på e-mail-adresse.
- Parametre:
  - `email` (streng): E-mail-adressen for budgiveren.
- Returneret respons: En liste af logposter, der tilhører den angivne e-mail-adresse.

Bemærk: Alle endpoints returnerer enten en succesrespons (200 OK) med de relevante data eller en fejlrespons med en fejlbesked afhængigt af resultatet af operationen.

# Auction Tracker Worker - Klasser

Nedenstående dokument beskriver de tilhørende klasser, der bruges i Auction Tracker Worker.

## AuctionDTO

- Beskrivelse: Representerer auktionsdata.
- Egenskaber:
  - `Id` (streng): Unik identifikator for auktionen.
  - `SellerId` (streng): Identifikator for sælgeren.
  - `StartTime` (DateTime): Starttidspunkt for auktionen.
  - `EndTime` (DateTime): Sluttidspunkt for auktionen.
  - `StartingBid` (double): Startbud for auktionen.
  - `BuyoutPrice` (double): Købspris for auktionen.
  - `CurrentBid` (double): Nuværende bud på auktionen.

## Bid

- Beskrivelse: Representerer et bud på en auktion.
- Egenskaber:
  - `Id` (streng): Unik identifikator for budet.
  - `CatalogId` (streng): Katalog-id for auktionen, som budet er placeret på.
  - `BuyerEmail` (streng): E-mail-adressen for budgiveren.
  - `BidValue` (double): Værdien af budet.

## BidLogs

- Beskrivelse: Representerer en logpost for et bud på en auktion.
- Egenskaber:
  - `Id` (streng): Unik identifikator for logposten.
  - `CatalogId` (streng): Katalog-id for auktionen, som logposten tilhører.
  - `BuyerEmail` (streng): E-mail-adressen for budgiveren.
  - `BidValue` (double): Værdien af budet.
  - `LogTime` (DateTime): Tidspunktet for logposten.

## CustomException - ItemsNotFoundException

- Beskrivelse: En specialiseret exception-klasse, der bruges til at angive, at der ikke blev fundet nogen elementer.
- Arver fra: `Exception`

## IMongoService

- Beskrivelse: En interface, der definerer metoder til at håndtere CRUD-operationer på auktionsdata i databasen.
- Metoder:
  - `GetAllBids()`: Henter alle bud.
  - `GetById(string id)`: Henter et bud baseret på dets id.
  - `GetByEmail(string email)`: Henter alle bud for en given e-mail-adresse.
  - `UpdateBid(Bid newData)`: Opdaterer et bud med nye data.
  - `CreateNewBid(Bid data)`: Opretter et nyt bud.
  - `LogBid(Bid data)`: Opretter en logpost for et bud.
  - `GetAllLogs()`: Henter alle logposter.
  - `GetLogsById(string id)`: Henter logposter baseret på katalog-id.
  - `GetLogsByEmail(string email)`: Henter logposter baseret på e-mail-adresse.

## IMongoServiceFactory

- Beskrivelse: En interface, der definerer en fabrik til at oprette scoped instanser af `IMongoService`.
# AuctionTrackerWorker - Dokumentation

## IMongoServiceFactory

**Beskrivelse:**  
En interface, der definerer metoden til at oprette en scoped instans af `IMongoService`. Dette interface bruges til at definere kontrakten for MongoServiceFactory, der opretter `IMongoService`-objekter i en afhængighedsinjektionskontekst.

**Metoder:**

- `CreateScoped()`: Opretter en scoped instans af `IMongoService`.

## MongoServiceFactory

**Beskrivelse:**  
MongoServiceFactory` er en implementering af `IMongoServiceFactory`-interfacet. Denne klasse fungerer til at oprette scoped instanser af `IMongoService` ved hjælp af en `IServiceScopeFactory`.

**Constructor:**

- `MongoServiceFactory(IServiceScopeFactory scopeFactory)`: Opretter en ny instans af `MongoServiceFactory` med den nødvendige `IServiceScopeFactory`-afhængighed.

**Metoder:**

- `CreateScoped()`: Opretter en scoped instans af `IMongoService` ved at oprette en ny scope ved hjælp af `IServiceScopeFactory` og returnere `IMongoService`-objektet fra scope-providere.



