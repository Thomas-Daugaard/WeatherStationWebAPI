
Start med at kontrollere din SQL connection string i appsettings.json. 

Anvend herefter din package manager til "update-database" for at udf�re migrationen til din lokale SQL database. 

Du kan nu herefter udf�re nedenst�ende use case. 

=============================================================================================================================================================================
For at k�re l�sningen skal b�de WEB API'et og SignedUpClient projektet afvikles samtidigt: 

1) H�jre klik p� solutionen i solution exploreren og v�lg "Set startup projects"

2) V�lg herefter de to projekter som starup projects og k�r l�sningen med F5. 

3) Du kan nu �bne et ekstra browser eller fane i din localhost browser og tilg� Postman hvori du kan udf�re bruger registrering/login/Get weatherlogs/Post weatherlogs. 

4) Ved POST af weatherlogs vil du kunne se at de i client fanen bliver udskrevet. 

Seeded bruger login:
Email = "test1@testesen.dk",
Password = "Sommer25!"

=============================================================================================================================================================================
Test suites: 

For at afvikle tests for WeatherLogControlleren og AccountControlleren kan disse blot afvikles direkte i test exploren. 