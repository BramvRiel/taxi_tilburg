# taxi_tilburg

De planner moet zorgen dat de reisbehoefte zo efficient mogelijk wordt uitgevoerd;
- zo weinig mogelijk taxi-kilometers
- zo weinig mogelijk totale reistijd
- zo klein mogelijke afwijking van de gewenste vertrek- of aankomsttijd
- zo veel mogelijk reizigers tegelijk per rit vervoeren
- zo weinig mogelijk afstand tussen twee ritten van een voertuig

### Database:
- locaties
- locaties_afstanden
- locaties_reistijden
- reizigers
- voertuigen

<!-- ### UI:
#### Excel import pagina
+ formulier
+ resultaat

#### route maker pagina
+ vertrektijd
+ route bouwer
+ aankomsttijd
+ totaal km
+ totaal h m s

## Fase 2 Optimalisatie:
slimme SQL query
default route voor route maker pagina

## Fase 3 Routeplanner:
Meerdere routes klaarzetten
routes optimaliseren -->

## Issues:
#### Foutmeldig bij File Upload;
- UseAntiforgery(); 
- UseCors();
*disabled*
#### Excel import EPPlus ipv Microsoft.Interop
- Microsoft.Interop kan niet overweg met stream, moest file locatie zijn.
#### Leesbaarheid in program.cs:
- Wildgroei aan calls.
- Veel calls die op elkaar leken.
*gegroepeerd* *handler methode*
#### Koppeltabel know-how
- Moest opzoeken hoe je koppeltabel configureerd in DbContext