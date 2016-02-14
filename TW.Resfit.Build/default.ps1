
$here = Split-Path -Parent $MyInvocation.MyCommand.Path

properties {
	$solutionDirectory = Split-Path -Parent $here
	$outputDirectory = Join-Path $solutionDirectory ".build"
}

Task Default -depends BakeAndShake -description "Default task"

Task BakeAndShake `
	-description "Build solution and run all tests" `
	-depends Build, UnitTests, AcceptanceTests 

Task Clean `
	-description "Clean up build cruft and initialize build folder structure" `
	-requiredVariables $outputDirectory 
{	
	New-Directory $outputDirectory
	Remove-Contents $outputDirectory
}

Task Build `
	-description "Build them... Build them all." `
	-depends Clean 
{

}

Task UnitTests `
	-description "Run all unit tests" `
	-depends Build 
{
	
}

Task AcceptanceTests `
	-description "Run all acceptance tests" `
	-depends Build 
{

}

Function New-Directory {
	[CmdletBinding()]
	Param([string]$path)
	Write-Output "Creating directory $path"
	
	New-Item -ItemType Directory $path -ErrorAction SilentlyContinue | Out-Null
}

Function Remove-Contents {
	[CmdletBinding()]
	Param([string]$path)
	Write-Output "Removing contents of $path"

	Get-ChildItem -Recurse -Path $path | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue | Out-Null
}