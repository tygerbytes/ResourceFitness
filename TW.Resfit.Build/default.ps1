
properties {

}

Task Default -depends BakeAndShake -description "Default task"

Task BakeAndShake -depends Build, UnitTests, AcceptanceTests -description "Build solution and run all tests"

Task Clean -description "Clean up build cruft" {

}

Task Build -depends Clean -description "Build them... Build them all." {

}

Task UnitTests -depends Build -description "Run all unit tests" {
	
}

Task AcceptanceTests -depends Build -description "Run all acceptance tests" {

}