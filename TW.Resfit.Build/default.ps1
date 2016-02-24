
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
	$testCoverageFilter = "+[*]* -[*.Tests]* -[*.Requirements]*"
	$testCoverageExclusionAttribute = "System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute"
	$testCoverageExcludeFiles = "*\*Designer.cs;*\*.g.cs;*.g.i.cs"

	$reportGenerator = Join-Path $(Find-PackagePath $packageDirectory "ReportGenerator") "tools\ReportGenerator.exe"

	$7zip = Join-Path $(Find-PackagePath $packageDirectory "7-Zip.CommandLine") "tools\7za.exe"
	$releaseDirectory = Join-Path $outputDirectory "Release"

	$nuget = Join-Path $(Find-PackagePath $packageDirectory "NuGet.CommandLine") "tools\NuGet.exe"
}

Framework "4.5.2"

FormatTaskName ">>>-- Executing {0} Task -->"

Task Default -depends Package -description "Default task"

Task BakeAndShake `
	-description "Build solution and run all tests" `
	-depends Build #, Tests

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
	Assert (Test-Path $reportGenerator) `
		"ReportGenerator console could not be found"
	Assert (Test-Path $7zip) `
		"7-Zip console could not be found"
	Assert (Test-Path $nuget) `
		"NuGet CommandLine could not be found"
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

	Remove-Directory $releaseDirectory
	New-Directory $releaseDirectory
}

Task Build `
	-description "Build them... Build them all." `
	-depends Clean `
{
	Exec {
		msbuild $solutionFile /verbosity:quiet /maxcpucount "/property:Configuration=$buildConfiguration;Platform=$buildPlatform;OutDir=$outputDirectory"
	}
}

Task Tests `
	-description "Run all tests and generate code coverage report" `
	-depends UnitTests, AcceptanceTests `
{
	Write-Output "Generating test coverage report"

	if (Test-Path $testCoverageReportPath) {
		Exec { & $reportGenerator $testCoverageReportPath $testCoverageDirectory }
	}
	else {
		Write-Output "OpenCover results not found at ($testCoverageReportPath)"
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

Task Package `
	-description "Create all packages" `
	-depends PackageZip, PackageNuget

Task PackageZip `
	-description "Package the application as a zip file" `
	-depends BakeAndShake `
{
	$assemblies = @(
		"TW.Resfit.Core.dll",
		"TW.Resfit.FileUtils.dll"
		# ...
	)

	$filesToPackage = Get-ChildItem -Path $outputDirectory -Recurse -Include $assemblies

	ForEach($file in $filesToPackage) {
		Copy-Item $file $releaseDirectory
	}

	$archivePath = Join-Path $releaseDirectory "TW.Resfit.7z"

	Exec { & $7zip a -r -mx3 $archivePath "$releaseDirectory\*"	}
}

Task PackageNuget `
	-description "Package the application as a nuget package" `
	-depends BakeAndShake `
{
	# Coming soon...
}