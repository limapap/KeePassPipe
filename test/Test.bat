start C:\dev\KeePassPipe\assets\KeePass.exe "C:\dev\KeePassPipe\assets\database.kdbx" -pw:test
pause
KeePassPipe.exe -u "Sample Entry #2" 

KeePassPipe.exe -p "Sample Entry #2" 

KeePassPipe.exe -u noentry 
echo errorlevel: %ERRORLEVEL%
KeePassPipe.exe -p noentry 

KeePassPipe.exe -t 

KeePassPipe.exe -x noentry 
echo errorlevel: %ERRORLEVEL%

pause


set "PTITLE=Sample Entry #2"

for /F "tokens=*" %%l in ('KeePassPipe.exe -u "%PTITLE%"') do set "PUSER=%%~l"
for /F "tokens=*" %%l in ('KeePassPipe.exe -p "%PTITLE%"') do set "PPASS=%%~l"

echo SomeApp.exe "%PUSER%" "%PPASS%" 

pause

set "PTITLE=notexist"

for /F "tokens=*" %%l in ('KeePassPipe.exe -U "%PTITLE%"') do set "PUSER=%%~l"
for /F "tokens=*" %%l in ('KeePassPipe.exe -P "%PTITLE%"') do set "PPASS=%%~l"

echo SomeApp.exe "%PUSER%" "%PPASS%" 

pause

set "PTITLE=notexist"

for /F "tokens=*" %%l in ('KeePassPipe.exe -u "%PTITLE%"') do set "PUSER=%%~l"
for /F "tokens=*" %%l in ('KeePassPipe.exe -p "%PTITLE%"') do set "PPASS=%%~l"

echo SomeApp.exe "%PUSER%" "%PPASS%" 

pause