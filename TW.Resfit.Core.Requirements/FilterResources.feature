Feature: Filter Resources
	In order to load only pertinent resources
	As a developer or documenter
	I want to be able to filter resources based on a criteria I provide

@FilterOnLoadFromXml
Scenario: Filter by resource key
	Given an xml file containing valid resources
	And I have a filter criterion matching one or more of those resource keys
	When I load the resources from the file into a new resource list
	Then the list contains only the resources matching the filter criterion

Scenario: Filter by resource value
	Given an xml file containing valid resources
	And I have a filter criterion matching one or more of those resource values
	When I load the resources from the file into a new resource list
	Then the list contains only the resources matching the filter criterion
