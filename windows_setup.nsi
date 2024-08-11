!include "MUI2.nsh"

Name "WallPal"
OutFile "wallpal_installer.exe"
InstallDir "$PROGRAMFILES\WallPal"

; Request administrative privileges
RequestExecutionLevel admin

Page directory
Page instfiles

Section "Main Section"
  SetOutPath "$INSTDIR"
  File /r "C:\Users\hello\Documents\Git\WallPal\release\app\*"
  CreateShortCut "$DESKTOP\WallPal.lnk" "$INSTDIR\WallPal.exe"
SectionEnd

Section "Uninstall"
  Delete "$INSTDIR\*.*"
  RMDir "$INSTDIR"
  Delete "$DESKTOP\WallPal.lnk"
SectionEnd
