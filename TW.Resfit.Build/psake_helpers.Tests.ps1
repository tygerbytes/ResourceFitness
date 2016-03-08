# --------------------------------------------------------------------------------------------------------------------
# <copyright file="psake_helpers.Tests.ps1" company="Tygertec">
#   Copyright © 2016 Ty Walls.
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

Describe "Write-CommonAssemblyInfo" {
	It "requires an existing template file" {
		{ Write-CommonAssemblyInfo "InvalidDir" "1.2.3.4" } | Should Throw "template not found"
	}

	Context "When there is an existing CommonAssemblyInfo.template" {
		Set-Content `
			-Path "$TestDrive\CommonAssemblyInfo.template" `
			-Value "Line1`r`nLine2 Copyright #__YEAR__# Ty`r`nLine3 Vesion #__VERSION__#`r`nLine4 What what!"
		
		It "sets the current year" {
			Mock Get-Date { return [datetime]"12/31/1999" }
			Write-CommonAssemblyInfo $TestDrive "1.2.3.4"
			"$TestDrive\CommonAssemblyInfo.cs" | Should Contain "1999"
		}

		It "sets the version number" {
			Write-CommonAssemblyInfo $TestDrive "1.2.3.4"
			"$TestDrive\CommonAssemblyInfo.cs" | Should Contain "1.2.3.4"
		}
	}

	Context "When a commit hash is provided" {
		Set-Content `
			-Path "$TestDrive\CommonAssemblyInfo.template" `
			-Value "Hello #__HASH__#!`r`n"

		It "sets the commit hash" {
			Write-CommonAssemblyInfo $TestDrive "1.2.3.4" "0123456789"
			"$TestDrive\CommonAssemblyInfo.cs" | Should Contain "0123456789"
		}
	}
}
