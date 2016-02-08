Feature: Batch match and replace
	In order to rename resource keys and reword resource values in my source files
	As a developer or documenter
	I want to be able to match existing resources to new resources.

@ImportResourceKeys
Scenario: Load keys from an XML file
Given a file containing a list of resource keys
When I load the XML file
Then it is loaded as a list of resources

@ManuallyMatchOneResourceToAnother
Scenario: Manually match a resource with a replacement resource
Given a list of resources
When I match one of the resources with a replacement resource
Then the replacement resource is stored alongside the original resource
 And the original resource is tagged for eventual replacement 

@AutomaticallyMatchOneResourceListToAnother
#Scenario: Auto match one list of resources to another
#Given a list of resources
#When I attempt to match the list of resources to another list of resources 
# And a valid matching criteria is supplied 
#Then successful matches are stored alongside their matches in the original resource list

@BatchReplaceResourcesInFiles
Scenario: Replace resources in files
Given a list of resources with matches
When I initiate a batch resource replacement command
 And I supply a directory of files to search and replace
Then all of the existing resources from the resource list will be replaced with their matches