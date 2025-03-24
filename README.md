# taxi_tilburg

De planner moet zorgen dat de reisbehoefte zo efficient mogelijk wordt uitgevoerd;
- zo weinig mogelijk taxi-kilometers
- zo weinig mogelijk totale reistijd
- zo klein mogelijke afwijking van de gewenste vertrek- of aankomsttijd
- zo veel mogelijk reizigers tegelijk per rit vervoeren
- zo weinig mogelijk tijd tussen twee ritten van een voertuig

### Database:
- locations
- locations_connections
- travelers
- vehicles

### API:
#### Database
- locations
- locatie/{id}
- travelers
- traveler/{id}
- vehicles
- vehicle/{id}
#### Excel
- upload
- locations
- locations-distances
- locations-traveltimes
- travelers
- vehicles
#### Logic
- shortest-route

## Issues:
#### Foutmeldig bij File Upload;
Authentication header moest aanwezig zijn, danwel client certificate, danwel cookie.

Oplossing: *DisableAntiforgery*

#### Excel import EPPlus ipv Microsoft.Interop
Microsoft.Interop kan niet overweg met stream, moest file locatie zijn.

Oplossing: EPPlus gebruiken

#### Duizendtallen in Excel

Oplossing: Convert via CultureInfo DE

#### Ja / Nee in Excel
Oplossing: Letterlijk als "Ja" dan true

#### Leesbaarheid in program.cs:
Wildgroei aan calls.

Oplossing: Gewrapped in handler methode, gebruikgemaakt van interfaces

#### Koppeltabel know-how
Andere entiteiten gingen vanzelf, EF had moeitje met de koppeltabellen. Ik wilde dit niet doen via annotaties.

Oplossing: Voorbeeld gezocht en overgenomen.

#### Route berekenen
Oplossing: Via recursive code alle routes berekenen, kortste aanhouden.
