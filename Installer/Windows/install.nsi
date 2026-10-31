Unicode True
Name "Peasant Uprising"
OutFile "install.exe"
InstallDir "$LOCALAPPDATA\Programs\PeasantUprising"
RequestExecutionLevel user
Section "PeasantUprising"
	SetOutPath "$INSTDIR\Data"
	File "..\..\Data\*"
	SetOutPath "$INSTDIR\assets\"
	File /r "..\..\assets\**"
	WriteUninstaller "$INSTDIR\uninstall.exe"
SectionEnd
Section "Uninstall"

    Delete "$INSTDIR\uninstall.exe"
    Delete "$INSTDIR\assets\fonts\*"
SectionEnd