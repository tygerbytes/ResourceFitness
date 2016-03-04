# --------------------------------------------------------------------------------------------------------------------
# <copyright file="psake_helpers.Tests.ps1" company="Tygertec">
#   Copyright © 2016 Tyrone Walls.
#   All rights reserved.
# </copyright>
# <summary>
#   Defines Pester tests for the Psake helpers.
# </summary>
# --------------------------------------------------------------------------------------------------------------------

$here = Split-Path -Parent $MyInvocation.MyCommand.Path
$sut = (Split-Path -Leaf $MyInvocation.MyCommand.Path) -replace '\.Tests\.', '.'
. "$here\$sut"

Describe "Quote-String" {
	It "Surrounds a string with two double quotes" {
		Quote-String "Oy!" | Should Be "`"`"Oy!`"`""
	}
}

Describe "Find-PackagePath" {
	$packages = "TestDrive:\packages\"
	New-Item -ItemType Directory $(Join-Path $packages "\Banana.1.0.1\tools\bin\")
	New-Item -ItemType Directory $(Join-Path $packages "\Banana.1.0.2\tools\bin\")

	It "finds the most recent nuget package" {
		Find-PackagePath $packages "Banana" | Should Be $(Join-Path $TestDrive "packages\Banana.1.0.2")
	}
}

Describe "New-Directory" {
	It "creates a new directory" {
		New-Directory "TestDrive:\NewDir"
		"TestDrive:\NewDir" | Should Exist
	}
}

Describe "Remove-Contents" {
	$dir = Join-Path $TestDrive "Dir"

	# Create a bunch of content in $dir
	New-Item -ItemType Directory $dir
	Set-Content $(Join-Path $dir "fruit.txt") -Value "Yum!"
	New-Item -ItemType Directory $(Join-Path $dir "Banana")
	New-Item -ItemType Directory $(Join-Path $dir "Orange")
	Set-Content $(Join-Path $dir "Orange\MoreFruit.txt") -Value "More yummy!"

	Remove-Contents $dir
	
	It "recursively deletes all contents in a directory" {
		$fileCount = $(Get-ChildItem -Recurse $dir | Measure-Object).Count
		$fileCount | Should Be 0
	}

	It "but does not delete the directory itself" {
		$dir | Should Exist
	}
}

Describe "Remove-Directory" {
	$dir = Join-Path $TestDrive "Dir"
	New-Item -ItemType Directory $dir

	It "deletes a directory" {
		Remove-Directory $dir
		$dir | Should Not Exist
	}
}