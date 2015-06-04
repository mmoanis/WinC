IF EXIST C:\MinGW\bin\gcc.exe IF EXIST C:\MinGW\bin\g++.exe (
	IF EXIST srm.exe IF EXIST WinC.dll IF EXIST SharpShell.dll (
		srm install WinC.dll -codebase
	) ELSE (
		ECHO "Missing installation files"
	)
) ELSE (
	ECHO "Please install MinGW compiler!"
)