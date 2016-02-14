
Function Find-PackagePath {
	[CmdletBinding()]
	param(
		[Parameter(Position=0,Mandatory=1)]$packagesPath,
		[Parameter(Position=1,Mandatory=1)]$packageName
	)

	$pathWithWildcard = Join-Path $packagesPath $($packageName + "*")
	
	return (Get-ChildItem -Path $pathWithWildcard).FullName | Sort-Object $_ | Select-Object -Last 1
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

	Remove-Item -Path $testResultsDirectory -Force -Recurse -ErrorAction SilentlyContinue | Out-Null
}

