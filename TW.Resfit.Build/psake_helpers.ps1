
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

	Write-Debug "Looking for $packageName at $packagesPath"

	$pathWithWildcard = Join-Path $packagesPath $($packageName + "*")

	return (Get-ChildItem -Path $pathWithWildcard).FullName | Sort-Object $PSItem | Select-Object -Last 1
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

Function Remove-Directory {
	[CmdletBinding()]
	Param([string]$path)

	Write-Debug "Removing directory $path"

	Remove-Item -Path $path -Force -Recurse -ErrorAction SilentlyContinue | Out-Null
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