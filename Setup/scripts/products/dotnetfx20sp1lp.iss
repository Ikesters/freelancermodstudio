//http://www.microsoft.com/downloads/details.aspx?FamilyID=1cc39ffe-a2aa-4548-91b3-855a2de99304

[CustomMessages]
de.dotnetfx20sp1lp_title=.NET Framework 2.0 SP1 Sprachpaket: Deutsch

de.dotnetfx20sp1lp_size=3,4 MB

;http://www.microsoft.com/globaldev/reference/lcid-all.mspx
de.dotnetfx20sp1lp_lcid=1031

de.dotnetfx20sp1lp_url=http://download.microsoft.com/download/8/a/a/8aab7e6a-5e58-4e83-be99-f5fb49fe811e/NetFx20SP1_x86de.exe
de.dotnetfx20sp1lp_url_x64=http://download.microsoft.com/download/1/4/2/1425872f-c564-4ab2-8c9e-344afdaecd44/NetFx20SP1_x64de.exe
de.dotnetfx20sp1lp_url_ia64=http://download.microsoft.com/download/a/0/b/a0bef431-19d8-433c-9f42-6e2824a8cb90/NetFx20SP1_ia64de.exe


[Code]
procedure dotnetfx20sp1lp();
var
	version: cardinal;
	regPath: string;
begin
	if ActiveLanguage() <> 'en' then begin
		regPath := GetString('Software', 'Software\Wow6432Node', 'Software\Wow6432Node') + '\Microsoft\NET Framework Setup\NDP\v2.0.50727\' + CustomMessage('dotnetfx20lp_lcid');
		RegQueryDWordValue(HKLM, regPath, 'Install', version);
		
		if version < 1 then
			AddProduct(GetString('dotnetfx20sp1_' + ActiveLanguage() + '.exe', 'dotnetfx20sp1_x64_' + ActiveLanguage() + '.exe', 'dotnetfx20sp1_ia64_' + ActiveLanguage() + '.exe'),
				'/passive /norestart /lang:ENU',
				CustomMessage('dotnetfx20sp1lp_title'),
				CustomMessage('dotnetfx20sp1lp_size'),
				GetString(CustomMessage('dotnetfx20sp1lp_url'), CustomMessage('dotnetfx20sp1lp_url_x64'), CustomMessage('dotnetfx20sp1lp_url_ia64')));
	end;
end;