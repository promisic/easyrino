; EasyRino tool installer for Windows/x86
;
; Installer script version: 1.0
;
; Copyright: Dusan Misic 2016 <promisic@outlook.com>
;

[Setup]
; Application information section
AppId=EasyRino
AppName=EasyRino
AppVersion=1.0 Beta 2
AppCopyright=Dusan Misic
AppPublisher=Dusan Misic
AppPublisherURL=https://sourceforge.net/projects/easyrino/
AppSupportURL=http://www.facebook.com/easyrino
LicenseFile=..\gpl_license.txt
; Enable welcome sceen
DisableWelcomePage = no
; Setup version information section
VersionInfoCompany=Dusan Misic
VersionInfoCopyright=Dusan Misic
VersionInfoDescription=
VersionInfoVersion=1.0.0.0
; Install location section
DefaultDirName={pf}\EasyRino
DefaultGroupName=EasyRino
; Compression
Compression=lzma2/ultra64
; Output directory (. means current directory)
OutputDir=.
; Output setup filename 
OutputBaseFilename= easyrino_1.0_beta_2_setup
; Specifying that minimal OS version is Windows 7
MinVersion=0, 6.1
; Uninstall section
Uninstallable=yes
UninstallDisplayIcon={uninstallexe}
CreateUninstallRegKey=yes
UninstallDisplayName=EasyRino
; Privileges section

PrivilegesRequired=admin
; Windows architecture (blank for all architectures)
ArchitecturesAllowed=

[Types]
; Defines installation types
;Name: "full"; Description: "Puna instalacija"
;Name: "minimal"; Description: "Minimalna instalacija"
;Name: "custom"; Description: "Napredna instalacija"; Flags: iscustom

[Components]
; Defines installation components
;Name: "main"; Description: "Glavne aplikacije"; Types: full minimal custom; Flags: fixed
;Name: "runtime"; Description: "Instaliraj i runtime"; Types: full custom

[Tasks]
; Defines tasks routines


[Dirs]
; Defines additional directories
Name: "{app}\bin"
Name: "{app}\doc"

[Files]
; Files to include in setup file
Source: "..\EasyRino\bin\Release\EasyRino.exe"; DestDir: "{app}\bin"; 
Source: "..\EasyRino\bin\Release\GriffinSoft.EasyRino.RinoCore.dll"; DestDir: "{app}\bin"; 
Source: "..\EasyRino\bin\Release\GriffinSoft.EasyRino.RinoXmlFilter.dll"; DestDir: "{app}\bin";
Source: "..\gpl_license.txt"; DestDir: "{app}\bin";
Source: "..\doc\easyrino_manual.pdf"; DestDir: "{app}\doc"; 

[Icons]
; Icons to create
Name: "{group}\EasyRino Beta 2"; Filename: "{app}\bin\EasyRino.exe"; IconFilename: "{app}\bin\EasyRino.exe"; WorkingDir: "{app}\bin";
Name: "{userdesktop}\EasyRino Beta 2"; Filename: "{app}\bin\EasyRino.exe"; IconFilename: "{app}\bin\EasyRino.exe"; WorkingDir: "{app}\bin";
Name: "{group}\EasyRino Uputstvo"; Filename: "{app}\doc\easyrino_manual.pdf"; IconFilename: "{app}\doc\easyrino_manual.pdf"; WorkingDir: "{app}\doc"; 
Name: "{group}\GPL licenca v3"; Filename: "{app}\bin\gpl_license.txt"; IconFilename: "{app}\bin\gpl_license.txt"; WorkingDir: "{app}\bin"; 
Name: "{group}\Uninstall EasyRino"; Filename: "{uninstallexe}"; IconFilename: "{uninstallexe}"; WorkingDir: "{app}"
