#include "scripts\products.iss"

#include "scripts\products\winversion.iss"
#include "scripts\products\fileversion.iss"

#include "scripts\products\ngen.iss"
#include "scripts\products\msi31.iss"
#include "scripts\products\dotnetfx35sp1.iss"

#define MyAppSetupName 'Freelancer Mod Studio'
#define MyAppVersion '0.9.7'

[Setup]
AppName={#MyAppSetupName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppSetupName} {#MyAppVersion}
AppCopyright=Copyright � stfx 2009-2011
VersionInfoVersion={#MyAppVersion}
VersionInfoCompany=stfx
AppPublisher=stfx
AppPublisherURL=http://code.google.com/p/freelancermodstudio
AppUpdatesURL=http://code.google.com/p/freelancermodstudio/downloads/list
OutputBaseFilename=FreelancerModStudio-{#MyAppVersion}
DefaultGroupName={#MyAppSetupName}
DefaultDirName={pf}\{#MyAppSetupName}
UninstallDisplayIcon={app}\FreelancerModStudio.exe
UninstallDisplayName={#MyAppSetupName}
Uninstallable=yes
CreateUninstallRegKey=yes
UpdateUninstallLogAppName=yes
CreateAppDir=yes
OutputDir=bin
SourceDir=.
AllowNoIcons=yes
WizardImageFile=src\FreelancerModManager.bmp
WizardSmallImageFile=src\FreelancerModManager_small.bmp
LanguageDetectionMethod=uilanguage
SolidCompression=yes
Compression=lzma/max

MinVersion=0,5.01
PrivilegesRequired=admin
ArchitecturesAllowed=x86 x64
ArchitecturesInstallIn64BitMode=x64 ia64

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "quicklaunchicon"; Description: "{cm:CreateQuickLaunchIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "..\FreelancerModStudio\bin\Release\FreelancerModStudio.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\FreelancerModStudio\bin\Release\FreelancerModStudio.XmlSerializers.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\FreelancerModStudio\bin\Release\HelixEngine.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\FreelancerModStudio\bin\Release\ObjectListView.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\FreelancerModStudio\bin\Release\WeifenLuo.WinFormsUI.Docking.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "..\FreelancerModStudio\bin\Release\de\FreelancerModStudio.resources.dll"; DestDir: "{app}\de"; Flags: ignoreversion
Source: "..\FreelancerModStudio\bin\Release\Template.xml"; DestDir: "{app}"; Flags: ignoreversion
Source: "src\Settings_en.xml"; DestName: "FreelancerModStudio.xml"; DestDir: "{userappdata}\Freelancer Mod Studio"; Languages: en; Flags: onlyifdoesntexist
Source: "src\Settings_de.xml"; DestName: "FreelancerModStudio.xml"; DestDir: "{userappdata}\Freelancer Mod Studio"; Languages: de; Flags: onlyifdoesntexist

[UninstallDelete]
Name: "{userappdata}\Freelancer Mod Studio\FreelancerModStudio.Layout.xml"; Type: files
Name: "{userappdata}\Freelancer Mod Studio\FreelancerModStudio.xml"; Type: files
Name: "{userappdata}\Freelancer Mod Studio"; Type: dirifempty

[Icons]
Name: "{group}\Freelancer Mod Studio"; Filename: "{app}\FreelancerModStudio"
Name: "{group}\{cm:UninstallProgram,Freelancer Mod Studio}"; Filename: "{uninstallexe}"
Name: "{commondesktop}\Freelancer Mod Studio"; Filename: "{app}\FreelancerModStudio.exe"; Tasks: desktopicon
Name: "{userappdata}\Microsoft\Internet Explorer\Quick Launch\FreelancerModStudio"; Filename: "{app}\FreelancerModStudio.exe"; Tasks: quicklaunchicon

[Run]
Filename: "{app}\FreelancerModStudio.exe"; Description: "{cm:LaunchProgram,FreelancerModStudio}"; Flags: nowait postinstall skipifsilent

[Code]
function InitializeSetup(): boolean;
begin
	//init windows version
	initwinversion();

	msi31('3.1');

	//install .netfx 3.5 sp1
	dotnetfx35sp1();

	Result := true;
end;