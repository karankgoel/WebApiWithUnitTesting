# WebApiWithUnitTesting - eBroker

1. Clone/download the solution.
2. Build the solution.
3. In Package Manager Console run the command: update-database
4. Local instance of database will be created. Insert data into Equities and Wallet table.
5. Run the solution keeping API as startup project.
6. Use Postman to make calls to the API.


How to run test cases

1. Open the solution in Visual studio.
2. Go to test explorer and run all tests.

How to create Code Coverage report

Run the following commands in the folder containing test project

1. dotnet add package coverlet.msbuild 
2. dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:CoverletOutput=TestResults\Coverage\ 
3. dotnet tool install -g dotnet-reportgenerator-globaltool 
4. reportgenerator -reports:"[Path where you saved the solution]\NAGP_eBroker\eBroker.Tests\TestResults\Coverage\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html
5. You will see index.html file in \NAGP_eBroker\eBroker.Tests\coveragereport location. Open index.html for code coverage report.
