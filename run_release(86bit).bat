start %~dp0OpenNos.Master.Server\bin\x86\Release\OpenNos.Master.Server.exe
timeout 10
:: edit to have wanted amount of world servers,
:: dont forget to wait about 5 seconds before starting next world server
start %~dp0OpenNos.World\bin\x86\Release\OpenNos.World.exe
timeout 20
start %~dp0OpenNos.Login\bin\x86\Release\OpenNos.Login.exe
exit