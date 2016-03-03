
# Where am I? :)
$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path) -replace '\.Tests\.', '.'
. "$here\$sut"

Describe "Quote-String" {
	It "Surrounds a string with two double quotes" {
		# $str = "Oy!" | Quote-String
		$str = Quote-String "Oy!"

		$str | Should Be "`"`"Oy!`"`""
	}
}

Describe "Find-PackagePath" {
	$packages = "TestDrive:\packages\"
	$version1 = Join-Path $packages "\Banana.1.0.1\tools\bin\"
	$version2 = Join-Path $packages "\Banana.1.0.2\tools\bin\"
	New-Item -ItemType Directory $version2
	New-Item -ItemType Directory $version1

	$foundPath = Find-PackagePath $packages "Banana"

	It "finds the most recent nuget package" {
		(-join $foundPath) | Should Be $(Join-Path $TestDrive "packages\Banana.1.0.2")
	}
}

Describe "New-Directory" {
	$dir = "TestDrive:\NewDir"
	New-Directory $dir 

	It "creates a new directory" {
		Test-Path $dir | Should Be $true
	}
}

Describe "Remove-Contents" {
	$dir = Join-Path $TestDrive "Dir"
	New-Item -ItemType Directory $dir
	Set-Content $(Join-Path $dir "fruit.txt") -Value "Yum!"
	New-Item -ItemType Directory $(Join-Path $dir "Banana")
	New-Item -ItemType Directory $(Join-Path $dir "Orange")
	Set-Content $(Join-Path $dir "Orange\MoreFruit.txt") -Value "More yummy!"

	Remove-Contents $dir

	It "recursively deletes all contents in a directory, but not the directory itself" {
		$fileCount = $(Get-ChildItem -Recurse $dir | Measure-Object).Count
		(-join $fileCount) | Should Be 0
	}
}

Describe "Remove-Directory" {
	$dir = Join-Path $TestDrive "Dir"
	New-Item -ItemType Directory $dir

	Remove-Directory $dir

	It "deletes a directory" {
		Test-Path $dir | Should Be $false
	}
}