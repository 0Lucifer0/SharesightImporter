cd ..
dotnet restore
dotnet publish -c Release -o ./publish/bin/SharesightImporter
net stop SharesightImporter
taskkill /F /IM mmc.exe
sc.exe delete SharesightImporter
sc.exe create SharesightImporter binpath= %CD%\publish\bin\SharesightImporter\SharesightImporter.exe start=auto
net start SharesightImporter