cd %~dp0
cd ..
net stop SharesightImporter
taskkill /F /IM mmc.exe
sc.exe delete SharesightImporter
dotnet restore
dotnet publish -c Release -o ./publish/bin/SharesightImporter --runtime win10-x64
sc.exe create SharesightImporter binpath= %CD%\publish\bin\SharesightImporter\SharesightImporter.exe start=auto
net start SharesightImporter
PAUSE