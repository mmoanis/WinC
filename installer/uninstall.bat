IF EXIST srm.exe (
	srm uninstall WinC.dll
) ELSE (
	ECHO "Missing installation files"
)