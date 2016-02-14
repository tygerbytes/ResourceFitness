
Remove-Module [p]sake

# Find path to psake
$psakeModule = (Get-ChildItem ("..\Packages\psake*\tools\psake.psm1")).FullName | Sort-Object $_ | Select-Object -Last 1

Import-Module $psakeModule

Invoke-psake -buildFile .\default.ps1 -taskList BakeAndShake -properties @{  }
