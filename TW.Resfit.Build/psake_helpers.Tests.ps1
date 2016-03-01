
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