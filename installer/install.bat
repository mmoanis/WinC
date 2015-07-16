:: WinC installer
@echo off

:: try to run gcc and g++
:: if MinGW is defined in path, this should work, other wise it will fail
gcc.exe > NUL 2> NUL
g++.exe > NUL 2> NUL

if not %ERRORLEVEL%==9009 (
	IF EXIST srm.exe IF EXIST WinC.dll IF EXIST SharpShell.dll IF EXIST config.xml (
		srm install WinC.dll -codebase
	) ELSE (
		ECHO "Missing installation files"
	)
) ELSE (
	ECHO "Please install and add MinGW to PATH!"
)