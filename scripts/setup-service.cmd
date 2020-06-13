cd ..
dotnet restore
dotnet publish -c Release -o ./publish/bin/SharesiesToSharesight
net stop SharesiesToSharesight
taskkill /F /IM mmc.exe
sc.exe delete SharesiesToSharesight
sc.exe create SharesiesToSharesight binpath= %CD%\publish\bin\SharesiesToSharesight\SharesiesToSharesight.exe start=auto
net start SharesiesToSharesight