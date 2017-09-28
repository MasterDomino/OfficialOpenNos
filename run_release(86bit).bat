start "Master.Server" /D "%~dp0OpenNos.Master.Server\bin\x86\Release\" "OpenNos.Master.Server.exe"
timeout 10
start "Login" /D "%~dp0OpenNos.Login\bin\x86\Release\" "OpenNos.Login.exe"
timeout 5
:: edit to have wanted amount of world servers,
:: dont forget to wait about 5 seconds before starting next world server
start "World" /D "%~dp0OpenNos.World\bin\x86\Release\" "OpenNos.World.exe"
exit