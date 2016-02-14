
Include ".\psake_helpers.ps1"

$here = Split-Path -Parent $MyInvocation.MyCommand.Path

properties {
	$solutionDirectory = Split-Path -Parent $here
	$solutionFile = $(Get-ChildItem -Path $solutionDirectory -Filter *.sln | Select -First 1).FullName
	$outputDirectory = Join-Path $solutionDirectory ".build"
	$buildConfiguration = "Release"
	$buildPlatform = "Any CPU"

	$packagesDirectory = Join-Path $solutionDirectory "packages"

	$testResultsDirectory = Join-Path $outputDirectory "TestResults"
	$nunit = Join-Path $(Find-PackagePath $packagesDirectory "Nunit.Console") "Tools\nunit3-console.exe"
}

FormatTaskName ">>>-- Executing {0} Task -->"

Task Default -depends BakeAndShake -description "Default task"

Task BakeAndShake `
	-description "Build solution and run all tests" `
	-depends Build, UnitTests, AcceptanceTests

Task Check-Environment `
	-description "Verify parameters and build tools" `
	-requiredVariables $outputDirectory, $testResultsDirectory, $solutionFile `
{
	Assert ("Debug", "Release" -contains $buildConfiguration) `
		"Invalid build configuration '$buildConfiguration'. Valid values are 'Debug' or 'Release'"
	Assert ("x86", "x64", "Any CPU" -contains $buildPlatform) `
		"Invalid build platform '$buildPlatform'. Valid values are 'x86', 'x64', or 'Any CPU'"
	Assert (Test-Path $nunit) `
		"NUnit console test runner could not be found"
}

Task Clean `
	-description "Clean up build cruft and initialize build folder structure" `
	-depends Check-Environment `
{	
	New-Directory $outputDirectory
	Remove-Contents $outputDirectory

	Remove-Directory -Path $testResultsDirectory
	New-Directory $testResultsDirectory
}

Task Build `
	-description "Build them... Build them all." `
	-depends Clean `
{
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
