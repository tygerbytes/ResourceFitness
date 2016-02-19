
Include ".\psake_helpers.ps1"

$here = Split-Path -Parent $MyInvocation.MyCommand.Path

properties {
	$solutionDirectory = Split-Path -Parent $here
	$solutionFile = $(Get-ChildItem -Path $solutionDirectory -Filter *.sln | Select -First 1).FullName
	
	$outputDirectory = Join-Path $solutionDirectory ".build"
	
	$buildConfiguration = "Release"
	$buildPlatform = "Any CPU"

	$packageDirectory = Join-Path $solutionDirectory "packages"

	$testResultsDirectory = Join-Path $outputDirectory "TestResults"
	$nunit = Join-Path $(Find-PackagePath $packageDirectory "Nunit.Console") "Tools\nunit3-console.exe"

	$openCover = Join-Path $(Find-PackagePath $packageDirectory "OpenCover") "tools\OpenCover.Console.exe"
	$testCoverageDirectory = Join-Path $outputDirectory "TestCoverage"
	$testCoverageReportPath = Join-Path $testCoverageDirectory "OpenCoverReport.xml"
	$testCoverageFilter = "+[*]* -[*.Tests]*"
	$testCoverageExclusionAttribute = "System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute"
	$testCoverageExcludeFiles = "*\*Designer.cs;*\*.g.cs;*.g.i.cs"
}

Framework "4.5.2"

FormatTaskName ">>>-- Executing {0} Task -->"

Task Default -depends BakeAndShake -description "Default task"

Task BakeAndShake `
	-description "Build solution and run all tests" `
	-depends Build, UnitTests, AcceptanceTests

Task Check-Environment `
	-description "Verify parameters and build tools" `
	-requiredVariables $outputDirectory, $testResultsDirectory, $solutionFile, $testCoverageDirectory `
{
	Assert ("Debug", "Release" -contains $buildConfiguration) `
		"Invalid build configuration '$buildConfiguration'. Valid values are 'Debug' or 'Release'"
	Assert ("x86", "x64", "Any CPU" -contains $buildPlatform) `
		"Invalid build platform '$buildPlatform'. Valid values are 'x86', 'x64', or 'Any CPU'"
	Assert (Test-Path $nunit) `
		"NUnit console test runner could not be found"
	Assert (Test-Path $openCover) `
		"OpenCover console could not be found"
}

Task Clean `
	-description "Clean up build cruft and initialize build folder structure" `
	-depends Check-Environment `
{	
	New-Directory $outputDirectory
	Remove-Contents $outputDirectory

	Remove-Directory -Path $testResultsDirectory
	New-Directory $testResultsDirectory

	Remove-Directory -Path $testCoverageDirectory
	New-Directory $testCoverageDirectory	
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
	-precondition { $(Get-ChildItem -Path $outputDirectory *.Tests.dll).Count -gt 0 } `
{
	$assemblies = Get-ChildItem -Path $outputDirectory *.Tests.dll `
		| ForEach-Object { Quote-String($PSItem.FullName) }
	
	$testResultsXml = Quote-String("$testResultsDirectory\{0}Results.xml" -f $Task.Name)
	$testOutput = $testResultsXml -replace 'xml','txt'

	$nunitArgs = "$assemblies /result:$testResultsXml /out=$testOutput /noheader"

	Run-Tests -openCoverExe $openCover `
			  -testRunner $nunit `
			  -testRunnerArgs $nunitArgs `
			  -coverageReportPath $testCoverageReportPath `
			  -filter $testCoverageFilter `
			  -excludeByAttribute $testCoverageExclusionAttribute `
			  -excludeByFile $testCoverageExcludeFiles
}

Task AcceptanceTests `
	-description "Run all acceptance tests" `
	-depends Build `
	-precondition { $(Get-ChildItem -Path $outputDirectory *.Requirements.dll).Count -gt 1 } `
{
	# Get the acceptance testing assemblies,
	#  except for the framework one, which doesn't currently have any tests in it.
	$assemblies = Get-ChildItem -Path $outputDirectory *.Requirements.dll `
		| Where-Object { $PSItem.Name -NotMatch "Framework" } `
		| ForEach-Object { Quote-String($PSItem.FullName) }
	
	$testResultsXml = Quote-String("$testResultsDirectory\{0}Results.xml" -f $Task.Name)
	$testOutput = $testResultsXml -replace 'xml','txt'

	$nunitArgs = "$assemblies /result:$testResultsXml /out=$testOutput /noheader"

	Run-Tests -openCoverExe $openCover `
			  -testRunner $nunit `
			  -testRunnerArgs $nunitArgs `
			  -coverageReportPath $testCoverageReportPath `
			  -filter $testCoverageFilter `
			  -excludeByAttribute $testCoverageExclusionAttribute `
			  -excludeByFile $testCoverageExcludeFiles
}
