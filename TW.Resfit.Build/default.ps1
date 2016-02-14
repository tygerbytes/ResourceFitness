
$here = Split-Path -Parent $MyInvocation.MyCommand.Path

properties {
	$solutionDirectory = Split-Path -Parent $here
	$solutionFile = $(Get-ChildItem -Path $solutionDirectory -Filter *.sln).FullName
	$outputDirectory = Join-Path $solutionDirectory ".build"
	$buildConfiguration = "Release"
	$buildPlatform = "Any CPU"
}

FormatTaskName ">>>-- Executing {0} Task -->"

Task Default -depends BakeAndShake -description "Default task"

Task BakeAndShake `
	-description "Build solution and run all tests" `
	-depends Build, UnitTests, AcceptanceTests

Task Clean `
	-description "Clean up build cruft and initialize build folder structure" `
	-requiredVariables $outputDirectory `
{	
	New-Directory $outputDirectory
	Remove-Contents $outputDirectory
}

Task Build `
	-description "Build them... Build them all." `
	-depends Clean `
	-requiredVariables $solutionFile `
{
	Assert ("Debug", "Release" -contains $buildConfiguration) `
		"Invalid build configuration '$buildConfiguration'. Valid values are 'Debug' or 'Release'"
	Assert("x86", "x64", "Any CPU" -contains $buildPlatform) `
		"Invalid build platform '$buildPlatform'. Valid values are 'x86', 'x64', or 'Any CPU'"

	Exec {
		msbuild $solutionFile /verbosity:quiet /maxcpucount "/property:Configuration=$buildConfiguration;Platform=$buildPlatform;OutDir=$outputDirectory"
	}
}

Task UnitTests `
	-description "Run all unit tests" `
	-depends Build `
{
	
}

Task AcceptanceTests `
	-description "Run all acceptance tests" `
	-depends Build `
{

}

Function New-Directory {
	[CmdletBinding()]
	Param([string]$path)
	Write-Debug "Creating directory $path"
	
	New-Item -ItemType Directory $path -ErrorAction SilentlyContinue | Out-Null
}

Function Remove-Contents {
	[CmdletBinding()]
	Param([string]$path)
	Write-Debug "Removing contents of $path"

	Get-ChildItem -Recurse -Path $path | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue | Out-Null
}
