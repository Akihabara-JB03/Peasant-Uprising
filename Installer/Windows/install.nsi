!include nsDialogs.nsh
Unicode True
ShowInstDetails show
ChangeUI all "${NSISDIR}\Contrib\UIs\modern.exe"
Name "Peasant Uprising"
Page custom MyPage
Page custom LicensePage
Page instfiles
Function MyPage
    nsDialogs::Create 1018
    Pop $0

    ${NSD_CreateLabel} 0 0 100% 12u "Peasant Uprising Install Media."
    ${NSD_CreateLabel} 0 24u 100% 12u "ボードゲーム　「Peasant Uprising」（百姓一揆）へようこそ。"
    ${NSD_CreateLabel} 0 36u 100% 12u "本作品をご自由にお楽しみください。"
    Pop $1

    nsDialogs::Show
FunctionEnd
Function OpenLicense
    ExecShell "open" "https://www.gnu.org/licenses/gpl-3.0.html"
FunctionEnd
Function LicensePage
    nsDialogs::Create 1018
    Pop $0
    ${NSD_CreateLabel} 0 0 100% 24u "LICENSE:"
    ${NSD_CreateLink} 10u 40u 100% 12u "GPL-v3.0"
     
    Pop $1
    ${NSD_OnClick} $1 OpenLicense
    nsDialogs::Show	
FunctionEnd
OutFile "install.exe"
InstallDir "$LOCALAPPDATA\Programs\PeasantUprising"
RequestExecutionLevel user
Section "PeasantUprising"
	SetOutPath "$INSTDIR\Data"
	File "..\..\Data\*"
	SetOutPath "$INSTDIR\assets\"
	File /r "..\..\assets\**"
	SetOutPath "$INSTDIR\bin\"
	File /r "..\..\bin\**"
	WriteUninstaller "$INSTDIR\uninstall.exe"
SectionEnd
Section "Uninstall"

    Delete "$INSTDIR\uninstall.exe"
    RMDir /r "$INSTDIR\assets"
    RMDir /r "$INSTDIR\bin"
SectionEnd