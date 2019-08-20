@setupContainer
Feature: PropertyBagSpecs
	In order to provide strongly typeed, extensible properties for a class
	As a developer
	I want to be able to use a property bag

Scenario: Get and set a property
	Given I set a property called "hello" to the value "world"
	When I get the property called "hello"
	Then the result should be "world"

Scenario: Get and set a missing property
	Given I set a property called "hello" to the value "world"
	When I get the property called "goodbye"
	Then the result should be null

Scenario: Get and set a null property
	Given I set a property called "hello" to null
	When I get the property called "hello"
	Then the result should be null

Scenario: Get and set a badly serialized property
	Given I set a property called "hello" to the value "jiggerypokery"
	When I get the property called "hello" as a custom object
	Then the result should be null

Scenario: Convert to a JObject
	Given I set a property called "hello" to the value "world"
	And I set a property called "number" to the value 3
	When I cast to a JObject
	Then the result should be the JObject
	| Property | Value | Type    |
	| hello    | world | string  |
	| number   | 3     | integer |

Scenario: Construct from a JObject
	Given I create a JObject
	| Property | Value | Type    |
	| hello    | world | string  |
	| number   | 3     | integer |
	When I construct a PropertyBag from the JObject
	Then the result should have the properties
	| Property | Value | Type    |
	| hello    | world | string  |
	| number   | 3     | integer |

Scenario: Construct from a Dictionary
	Given I create a Dictionary
	| Property | Value | Type    |
	| hello    | world | string  |
	| number   | 3     | integer |
	When I construct a PropertyBag from the Dictionary
	Then the result should have the properties
	| Property | Value | Type    |
	| hello    | world | string  |
	| number   | 3     | integer |
