
Remove-Module [p]sake

# Find path to psake
$psakeModule = (Get-ChildItem (".\Packages\psake*\tools\psake.psm1")).FullName | Sort-Object $_ | Select-Object -Last 1

Import-Module $psakeModule

Invoke-psake `
	-buildFile .\TW.Resfit.Build\default.ps1 `
	-taskList BakeAndShake `
	-framework 4.5.2 `
	-properties @{
		"buildConfiguration" = "Release"
		"buildPlatform" = "Any CPU"} `
	-parameters @{ 
		"solutionFile" = "..\ResourceFitnesss.sln"}

Write-Output ("`r`nBuild finished with code: {0}" -f $LASTEXITCODE)
exit $LASTEXITCODE
