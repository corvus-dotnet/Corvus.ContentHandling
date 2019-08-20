@setupContainer
Feature: Serializing content
	In order to support polymorphic content type serialization
	As a developer
	I want to be able to serialize polymorphic types to a JSON format

Scenario: Serialize a polymorphic content object implementing a common interface
	Given I have an instance of a content object called 'item' with content type 'application/vnd.corvus.somecontentwithinterface'
	| SomeValue |
	| Hello     |
	When I serialize the content object called 'item' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithinterface", "someValue": "Hello" }'

@useChildObjects
Scenario: Serialize a polymorphic content object implementing a common interface with a null polymorphic child object
	Given I have an instance of a content object called 'item' with content type 'application/vnd.corvus.somecontentwithinterfaceandchild'
	| SomeValue | Child |
	| Hello     | null  |
	When I serialize the content object called 'item' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithinterfaceandchild", "someValue": "Hello", "child": null }'

@useChildObjects
Scenario: Serialize a polymorphic content object implementing a common interface with a polymorphic child object
	Given I have an instance of a content object called 'child' with content type 'application/vnd.corvus.somecontentwithinterface'
	| SomeValue |
	| Dolly     |
	And I have an instance of a content object called 'parent' with content type 'application/vnd.corvus.somecontentwithinterfaceandchild'
	| SomeValue | Child   |
	| Hello     | {child} |
	When I serialize the content object called 'parent' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithinterfaceandchild", "someValue": "Hello", "child": { "contentType": "application/vnd.corvus.somecontentwithinterface", "someValue": "Dolly" } }'

@useChildObjects
Scenario: Serialize a polymorphic content object implementing a common interface with a POC child object
	Given I have an instance of a poc object called 'child'
	| SomeValue |
	| Dolly     |
	And I have an instance of a content object called 'parent' with content type 'application/vnd.corvus.somecontentwithinterfaceandpocchild'
	| SomeValue | Child   |
	| Hello     | {child} |
	When I serialize the content object called 'parent' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithinterfaceandpocchild", "someValue": "Hello", "child": { "someValue": "Dolly" } }'

##############

Scenario: Serialize a polymorphic content object implementing a common abstract base
	Given I have an instance of a content object called 'item' with content type 'application/vnd.corvus.somecontentwithabstractbase'
	| SomeValue |
	| Hello     |
	When I serialize the content object called 'item' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithabstractbase", "someValue": "Hello" }'

@useChildObjects
Scenario: Serialize a polymorphic content object implementing a common abstract base with a null polymorphic child object
	Given I have an instance of a content object called 'item' with content type 'application/vnd.corvus.somecontentwithabstractbaseandchild'
	| SomeValue | Child |
	| Hello     | null  |
	When I serialize the content object called 'item' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithabstractbaseandchild", "someValue": "Hello", "child": null }'

@useChildObjects
Scenario: Serialize a polymorphic content object implementing a common abstract base with a polymorphic child object
	Given I have an instance of a content object called 'child' with content type 'application/vnd.corvus.somecontentwithabstractbase'
	| SomeValue |
	| Dolly     |
	And I have an instance of a content object called 'parent' with content type 'application/vnd.corvus.somecontentwithabstractbaseandchild'
	| SomeValue | Child   |
	| Hello     | {child} |
	When I serialize the content object called 'parent' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithabstractbaseandchild", "someValue": "Hello", "child": { "contentType": "application/vnd.corvus.somecontentwithabstractbase", "someValue": "Dolly" } }'

@useChildObjects
Scenario: Serialize a polymorphic content object implementing a common abstract base with a POC child object
	Given I have an instance of a poc object called 'child'
	| SomeValue |
	| Dolly     |
	And I have an instance of a content object called 'parent' with content type 'application/vnd.corvus.somecontentwithabstractbaseandpocchild'
	| SomeValue | Child   |
	| Hello     | {child} |
	When I serialize the content object called 'parent' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithabstractbaseandpocchild", "someValue": "Hello", "child": { "someValue": "Dolly" } }'

	##############

Scenario: Serialize a polymorphic content object implementing a common base
	Given I have an instance of a content object called 'item' with content type 'application/vnd.corvus.somecontentwithbase'
	| SomeValue |
	| Hello     |
	When I serialize the content object called 'item' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithbase", "someValue": "Hello" }'

@useChildObjects
Scenario: Serialize a polymorphic content object implementing a common base with a null polymorphic child object
	Given I have an instance of a content object called 'item' with content type 'application/vnd.corvus.somecontentwithbaseandchild'
	| SomeValue | Child |
	| Hello     | null  |
	When I serialize the content object called 'item' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithbaseandchild", "someValue": "Hello", "child": null }'

@useChildObjects
Scenario: Serialize a polymorphic content object implementing a common base with a polymorphic child object
	Given I have an instance of a content object called 'child' with content type 'application/vnd.corvus.somecontentwithbase'
	| SomeValue |
	| Dolly     |
	And I have an instance of a content object called 'parent' with content type 'application/vnd.corvus.somecontentwithbaseandchild'
	| SomeValue | Child   |
	| Hello     | {child} |
	When I serialize the content object called 'parent' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithbaseandchild", "someValue": "Hello", "child": { "contentType": "application/vnd.corvus.somecontentwithbase", "someValue": "Dolly" } }'

@useChildObjects
Scenario: Serialize a polymorphic content object implementing a common base with a POC child object
	Given I have an instance of a poc object called 'child'
	| SomeValue |
	| Dolly     |
	And I have an instance of a content object called 'parent' with content type 'application/vnd.corvus.somecontentwithbaseandpocchild'
	| SomeValue | Child   |
	| Hello     | {child} |
	When I serialize the content object called 'parent' as 'result'
	Then the value called 'result' should be a json object '{ "contentType": "application/vnd.corvus.somecontentwithbaseandpocchild", "someValue": "Hello", "child": { "someValue": "Dolly" } }'

@useChildObjects
Scenario: Serialize a poc object with an enum
	Given I have an instance of a poc object with enum called 'subject'
	| SomeValue | SomeEnum            |
	| Hello     | ThirdTimeIsTheCharm |
	When I serialize the content object called 'subject' as 'result'
	Then the value called 'result' should be a json object '{ "someValue": "Hello", "someEnum": "thirdTimeIsTheCharm" }'
