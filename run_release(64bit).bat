start %~dp0OpenNos.Master.Server\bin\x64\Release\OpenNos.Master.Server.exe
timeout 10
start %~dp0OpenNos.Login\bin\x64\Release\OpenNos.Login.exe
timeout 5
:: edit to have wanted amount of world servers,
:: dont forget to wait about 5 seconds before starting next world server
start "%~dp0OpenNos.World\bin\x64\Release\OpenNos.World.exe
exit