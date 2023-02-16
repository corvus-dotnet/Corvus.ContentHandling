@perFeatureContainer

Feature: Deserializing content
	In order to support polymorphic content type serialization
	As a developer
	I want to be able to deserialize polymorphic types from a JSON format

Scenario: Deserialize a polymorphic content object implementing a common interface
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithinterface", "someValue": "Hello" }' to the common interface as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithinterface'
	| SomeValue |
	| Hello     |

@useChildObjects
Scenario: Deserialize a polymorphic content object implementing a common interface with a null polymorphic child object
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithinterfaceandchild", "someValue": "Hello", "child": null }' to the common interface as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithinterfaceandchild'
	| SomeValue | Child |
	| Hello     | null  |

@useChildObjects
Scenario: Deserialize a polymorphic content object implementing a common interface with a polymorphic child object
	Given I have an instance of a content object called 'child' with content type 'application/vnd.corvus.somecontentwithinterface' available as a child object
	| SomeValue |
	| Dolly     |
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithinterfaceandchild", "someValue": "Hello", "child": { "contentType": "application/vnd.corvus.somecontentwithinterface", "someValue": "Dolly" } }' to the common interface as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithinterfaceandchild'
	| SomeValue | Child   |
	| Hello     | {child} |

@useChildObjects
Scenario: Deserialize a polymorphic content object implementing a common interface with a POC child object
	Given I have an instance of a poc object called 'child' available as a child object
	| SomeValue |
	| Dolly     |
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithinterfaceandpocchild", "someValue": "Hello", "child": { "someValue": "Dolly" } }' to the common interface as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithinterfaceandpocchild'
	| SomeValue | Child   |
	| Hello     | {child} |

##############

Scenario: Deserialize a polymorphic content object implementing a common base
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithbase", "someValue": "Hello" }' to the common base as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithbase'
	| SomeValue |
	| Hello     |

@useChildObjects
Scenario: Deserialize a polymorphic content object implementing a common base with a null polymorphic child object
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithbaseandchild", "someValue": "Hello", "child": null }' to the common base as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithbaseandchild'
	| SomeValue | Child |
	| Hello     | null  |

@useChildObjects
Scenario: Deserialize a polymorphic content object implementing a common base with a polymorphic child object
	Given I have an instance of a content object called 'child' with content type 'application/vnd.corvus.somecontentwithbase' available as a child object
	| SomeValue |
	| Dolly     |
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithbaseandchild", "someValue": "Hello", "child": { "contentType": "application/vnd.corvus.somecontentwithbase", "someValue": "Dolly" } }' to the common base as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithbaseandchild'
	| SomeValue | Child   |
	| Hello     | {child} |

@useChildObjects
Scenario: Deserialize a polymorphic content object implementing a common base with a POC child object
	Given I have an instance of a poc object called 'child' available as a child object
	| SomeValue |
	| Dolly     |
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithbaseandpocchild", "someValue": "Hello", "child": { "someValue": "Dolly" } }' to the common base as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithbaseandpocchild'
	| SomeValue | Child   |
	| Hello     | {child} |

##############

Scenario: Deserialize a polymorphic content object implementing a common abstract base
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithabstractbase", "someValue": "Hello" }' to the common abstract base as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithabstractbase'
	| SomeValue |
	| Hello     |

@useChildObjects
Scenario: Deserialize a polymorphic content object implementing a common abstract base with a null polymorphic child object
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithabstractbaseandchild", "someValue": "Hello", "child": null }' to the common abstract base as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithabstractbaseandchild'
	| SomeValue | Child |
	| Hello     | null  |

@useChildObjects
Scenario: Deserialize a polymorphic content object implementing a common abstract base with a polymorphic child object
	Given I have an instance of a content object called 'child' with content type 'application/vnd.corvus.somecontentwithabstractbase' available as a child object
	| SomeValue |
	| Dolly     |
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithabstractbaseandchild", "someValue": "Hello", "child": { "contentType": "application/vnd.corvus.somecontentwithabstractbase", "someValue": "Dolly" } }' to the common abstract base as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithabstractbaseandchild'
	| SomeValue | Child   |
	| Hello     | {child} |

@useChildObjects
Scenario: Deserialize a polymorphic content object implementing a common abstract base with a POC child object
	Given I have an instance of a poc object called 'child' available as a child object
	| SomeValue |
	| Dolly     |
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithabstractbaseandpocchild", "someValue": "Hello", "child": { "someValue": "Dolly" } }' to the common abstract base as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentwithabstractbaseandpocchild'
	| SomeValue | Child   |
	| Hello     | {child} |

@useChildObjects
Scenario: Deserialize a polymorphic content object implementing a common abstract base with a POC child object via constructor initialization
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentwithabstractbaseandpocchildctorinit", "someValue": "Hello", "child": { "someValue": "Dolly" } }' to the common abstract base as 'result'
	Then the value called 'result' should match the content object implementing a common abstract base with a POC child object via constructor with value 'Hello', and child some value 'Dolly'

@useChildObjects
Scenario: Deserialize an object with a dictionary
	Given I have a dictionary called 'dictionary' with values available as a child object
	| Key  | Value   |
	| Key1 | Value 1 |
	| KEY2 | Value 2 |
	| key3 | Value 3 |
	When I deserialize the json object '{ "someValue": "Hello", "dictionary": { "Key1": "Value 1", "KEY2": "Value 2", "key3": "Value 3" } }' as a poc object with dictionary as 'result'
	Then the value called 'result' should match the poc object with dictionary
	| SomeValue | Dictionary   |
	| Hello     | {dictionary} |


Scenario: Deserialize a polymorphic content object requiring DI
	When I deserialize the json object '{ "contentType": "application/vnd.corvus.somecontentrequiringdiinitialization", "someValue": "Hello" }' to the common interface as 'result'
	Then the value called 'result' should match the content object with content type 'application/vnd.corvus.somecontentrequiringdiinitialization'
	| SomeValue |
	| Hello     |
