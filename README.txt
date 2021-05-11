
Start med at kontrollere din SQL connection string i appsettings.json. 

Anvend herefter din package manager til "update-database" for at udføre migrationen til din lokale SQL database. 

Du kan nu herefter udføre nedenstående use case. 

=============================================================================================================================================================================
For at køre løsningen skal både WEB API'et og SignedUpClient projektet afvikles samtidigt: 

1) Højre klik på solutionen i solution exploreren og vælg "Set startup projects"

2) Vælg herefter de to projekter som starup projects og kør løsningen med F5. 

3) Du kan nu åbne et ekstra browser eller fane i din localhost browser og tilgå Postman hvori du kan udføre bruger registrering/login/Get weatherlogs/Post weatherlogs. 

4) Ved POST af weatherlogs vil du kunne se at de i client fanen bliver udskrevet. 

Seeded bruger login:
Email = "test1@testesen.dk",
Password = "Sommer25!"

=============================================================================================================================================================================
Test suites: 

For at afvikle tests for WeatherLogControlleren og AccountControlleren kan disse blot afvikles direkte i test exploren. 