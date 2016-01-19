Feature: Batch match and replace
	In order to rename resource keys and reword resource values in my source files
	As a developer or documenter
	I want to be able to match existing resources to new resources.

@ImportResourceKeys
Scenario: Load keys from an xml file
Given a file containing a list of resource keys
When I load the xml file
Then it is loaded as a list of resources