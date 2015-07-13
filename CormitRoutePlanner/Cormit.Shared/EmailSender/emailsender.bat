@echo off 
echo Sending Notification Email 
For /F "tokens=1* delims=:" %%A IN (%~dp0/emailnoticetemplate-%~1.txt) DO (	
	IF "%%A"=="mail to" set mailto=%%B
	IF "%%A"=="cc" set cc=%%B
	IF "%%A"=="bcc" set bcc=%%B 
	IF "%%A"=="subject" set subject=%~2 %%B 
	IF "%%A"=="content"  set content=%%B 	 
	IF "%%A"=="smtp" set smtpserver=%%B
	IF "%%A"=="user" set user=%%B
	IF "%%A"=="password" set pwd=%%B
	IF "%%A"=="from" set from=%%B
	IF "%%A"==" "  set content=%content% %%0D%%0A %%B 	
)	
if not "%smtpserver%"=="" set sss=-s %smtpserver%
if not "%user%"=="" set sss=%sss% -xu %user%
if not "%password%"=="" set sss=%sss% -xp %password%
if not "%~3"=="" set sff=-a %~3  
%~dp0/sendemail -f %from% -t %mailto% -cc %cc% -bcc %bcc% -u %subject% -m %content% %sss%  %sff%

exit