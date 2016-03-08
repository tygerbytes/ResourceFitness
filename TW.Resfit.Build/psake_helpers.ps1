# --------------------------------------------------------------------------------------------------------------------
# <copyright file="psake_helpers.ps1" company="Tygertec">
#   Copyright © 2016 Ty Walls.
#   All rights reserved.
# </copyright>
# <summary>
#   A set of functions to simplify things in the Psake build script.
# </summary>
# --------------------------------------------------------------------------------------------------------------------

Function Quote-String {
	[CmdletBinding()]
	param([string]$str)

	Write-Debug "Quoting $str"

	return "`"`"$str`"`""
}

Function Find-PackagePath {
	[CmdletBinding()]
	param(
		[Parameter(Position=0,Mandatory=1)]$packagesPath,
		[Parameter(Position=1,Mandatory=1)]$packageName)

	Write-Verbose "Looking for $packageName at $packagesPath"

	$pathWithWildcard = Join-Path $packagesPath $($packageName + "*")

	return (Get-ChildItem -Path $pathWithWildcard).FullName | Sort-Object $PSItem | Select-Object -Last 1
}

Function Get-CommitHash {
	[CmdletBinding()]

	$hash = $(& $git rev-parse HEAD)
	return $hash
}

Function New-Directory {
	[CmdletBinding(SupportsShouldProcess=$true)]
	Param([string]$path)

	Write-Verbose "Creating directory $path"
	
	if ($PSCmdlet.ShouldProcess($path)) {
		New-Item -ItemType Directory $path -ErrorAction SilentlyContinue | Out-Null
	}
}

Function Remove-Contents {
	[CmdletBinding(SupportsShouldProcess=$true)]
	Param([string]$path)

	Write-Verbose "Removing contents of $path"

	Get-ChildItem -Recurse -Path $path `
		| %{ 
				if ($PSCmdlet.ShouldProcess($path)) {
					Remove-Item $PSItem.FullName -Force -Recurse -ErrorAction SilentlyContinue | Out-Null 
				} 
			} 
}

Function Remove-Directory {
	[CmdletBinding(SupportsShouldProcess=$true)]
	Param([string]$path)

	Write-Verbose "Removing directory $path"

	if ($PSCmdlet.ShouldProcess($path)) {
		Remove-Item -Path $path -Force -Recurse -ErrorAction SilentlyContinue | Out-Null
	}
}

Function Run-Tests {
	[CmdletBinding()]
	Param(
		[Parameter(Position=0,Mandatory=1)]$openCoverExe,
		[Parameter(Position=1,Mandatory=1)]$testRunner,
		[Parameter(Position=2,Mandatory=1)]$testRunnerArgs,
		[Parameter(Position=3,Mandatory=1)]$coverageReportPath,
		[Parameter(Position=4,Mandatory=1)]$filter,
		[Parameter(Position=5,Mandatory=0)]$excludeByAttribute,
		[Parameter(Position=6,Mandatory=0)]$excludeByFile
	)

	Write-Debug "OpenCover ($openCoverExe) is running tests with:"
	Write-Debug " ($testRunner, $testRunnerArgs)"
	Write-Debug " and reporting overage metrics to ($coverageReportPath)"

	Exec {
		& $openCoverExe -target:$testRunner `
						-targetargs:$testRunnerArgs `
						-output:$coverageReportPath `
						-register:user `
						-filter:$filter `
						-excludebyattribute:$excludeByAttribute `
						-excludebyfile:$excludeByFile `
						-skipautoprops `
						-mergebyhash `
						-mergeoutput `
						-hideskipped:All `
						-returntargetcode 
	}
}

Function Write-CommonAssemblyInfo {
	[CmdletBinding()]
	Param(
		[Parameter(Position=0,Mandatory=1)]$assemblyInfoDirectory,
		[Parameter(Position=1,Mandatory=1)]$version,
		[Parameter(Position=2,Mandatory=0)]$commitHash
	)

	$templateFilePath = Join-Path $assemblyInfoDirectory "CommonAssemblyInfo.template"
	$commonAssemblyInfoPath = Join-Path $assemblyInfoDirectory "CommonAssemblyInfo.cs"

	Write-Verbose "CommonAssemblyInfo.template: $templateFilePath"
	Write-Verbose "CommonAssemblyInfo.cs: $commonAssemblyInfoPath"

	if (!(Test-Path $templateFilePath)) {
		throw "CommonAssemblyInfo.template not found at `"$templateFilePath`""
	}

	$templateContents = Get-Content $templateFilePath

	$commonAssemblyInfo = $templateContents -replace "#__YEAR__#","$((Get-Date).Year)"
	$commonAssemblyInfo = $commonAssemblyInfo -replace "#__VERSION__#","$version"
	$commonAssemblyInfo = $commonAssemblyInfo -replace "#__HASH__#",$commitHash

	Set-Content -Path $commonAssemblyInfoPath -Value $commonAssemblyInfo
}