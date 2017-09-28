start "Master.Server" /D "%~dp0OpenNos.Master.Server\bin\Release\" "OpenNos.Master.Server.exe"
timeout 10
start "Login" /D "%~dp0OpenNos.Login\bin\Release\" "OpenNos.Login.exe"
timeout 5
:: edit to have wanted amount of world servers,
:: dont forget to wait about 5 seconds before starting next world server
start "World" /D "%~dp0OpenNos.World\bin\Release\" "OpenNos.World.exe"
exit