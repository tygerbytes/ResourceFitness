# --------------------------------------------------------------------------------------------------------------------
# <copyright file="build.ps1" company="Tygertec">
#   Copyright © 2016 Tyrone Walls.
#   All rights reserved.
# </copyright>
# <summary>
#   Script for kicking off the command line build.
# </summary>
# --------------------------------------------------------------------------------------------------------------------

param([Parameter(Position=0, Mandatory=$false)] [string[]]$taskList=@())

Remove-Module [p]sake

# Find path to psake
$psakeModule = (Get-ChildItem (".\Packages\psake*\tools\psake.psm1")).FullName | Sort-Object $PSItem | Select-Object -Last 1

Import-Module $psakeModule

Invoke-psake `
	-buildFile .\TW.Resfit.Build\default.ps1 `
	-taskList $taskList `
	-framework 4.5.2 `
	-properties @{
		"buildConfiguration" = "Release"
		"buildPlatform" = "Any CPU"} `
	-parameters @{ 
		"solutionFile" = "..\ResourceFitnesss.sln"}

Write-Output ("`r`nBuild finished with code: {0}" -f $LASTEXITCODE)
exit $LASTEXITCODE
