Feature: Loading Resources
	In order to work with resources
	As a user of the the program
	I want to be able to load a resources from various file formats
	
@ImportResourceKeys
Scenario: Load keys from an xml file
Given a file containing a list of resource keys
When I load the xml file
Then it is loaded as a list of resources

