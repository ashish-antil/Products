REM Batch file for generating business service config files.

set recipe=c:\TeamImarda\Imarda360\Imarda360.Tools\recipe\recipe\bin\Debug\recipe.exe BusServiceConfig.txt
set fixedarg=service=JobDispatch endpoint=I360SVC-BUSJobDispatch:7019

%recipe% database=I360DB-JOB server=alpha %fixedarg%
%recipe% database=I360DB-JOB server=AU %fixedarg%
%recipe% database=I360DB-JOB server=beta %fixedarg%
%recipe% database=I360DB-JOB server=dev %fixedarg%
%recipe% "database=i360-sql-green-vip.i360.local\GREEN" server=NY %fixedarg%
%recipe% database=I360DB-JOB server=NZ %fixedarg%
%recipe% database=I360DB-JOB server=test %fixedarg%
%recipe% database=I360DB-JOB server=US %fixedarg%
