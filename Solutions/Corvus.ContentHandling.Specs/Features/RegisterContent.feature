﻿Feature: RegisterContent
	In order to create content dynamically by a media type 
	As a developer
	I want to be able to register and retrieve various kinds of content

Scenario: Register and retrieve content
	Given I have registered the following types
	| Type                  | Explicit content type                         | Registration kind |
	| ExplicitType1         | application/vnd.endjin.explicittype1          | Singleton         |
	| ExplicitType2         | application/vnd.endjin.explicittype2          | Singleton         |
	| ExplicitType1Subtype1 | application/vnd.endjin.explicittype1.subtype1 | Singleton         |
	| ExplicitType3         | application/vnd.endjin.explicittype3          | Transient         |
	| ExplicitType4         | application/vnd.endjin.explicittype4          | Transient         |
	| ExplicitType3Subtype1 | application/vnd.endjin.explicittype3.subtype1 | Transient         |
	| ExplicitType5         | application/vnd.endjin.explicittype5          | Scoped            |
	| ExplicitType6         | application/vnd.endjin.explicittype6          | Scoped            |
	| ExplicitType5Subtype1 | application/vnd.endjin.explicittype5.subtype1 | Scoped            |
	| ContentType1          |                                               | Singleton         |
	| ContentType2          |                                               | Singleton         |
	| ContentType1Subtype1  |                                               | Singleton         |
	| ContentType3          |                                               | Transient         |
	| ContentType4          |                                               | Transient         |
	| ContentType3Subtype1  |                                               | Transient         |
	| ContentType5          |                                               | Scoped            |
	| ContentType6          |                                               | Scoped            |
	| ContentType5Subtype1  |                                               | Scoped            |
	When I get the following content
	| Content type                                  |
	| application/vnd.endjin.explicittype1          |
	| application/vnd.endjin.explicittype2          |
	| application/vnd.endjin.explicittype1.subtype1 |
	| application/vnd.endjin.explicittype2.subtype1 |
	| application/vnd.endjin.explicittype3          |
	| application/vnd.endjin.explicittype4          |
	| application/vnd.endjin.explicittype3.subtype1 |
	| application/vnd.endjin.explicittype4.subtype1 |
	| application/vnd.endjin.explicittype5          |
	| application/vnd.endjin.explicittype6          |
	| application/vnd.endjin.explicittype5.subtype1 |
	| application/vnd.endjin.explicittype6.subtype1 |
	| application/vnd.endjin.contenttype1           |
	| application/vnd.endjin.contenttype2           |
	| application/vnd.endjin.contenttype1.subtype1  |
	| application/vnd.endjin.contenttype2.subtype1  |
	| application/vnd.endjin.contenttype3           |
	| application/vnd.endjin.contenttype4           |
	| application/vnd.endjin.contenttype3.subtype1  |
	| application/vnd.endjin.contenttype4.subtype1  |
	| application/vnd.endjin.contenttype5           |
	| application/vnd.endjin.contenttype6           |
	| application/vnd.endjin.contenttype5.subtype1  |
	| application/vnd.endjin.contenttype6.subtype1  |
	Then the results should be of types
	| Type                  |
	| ExplicitType1         |
	| ExplicitType2         |
	| ExplicitType1Subtype1 |
	| ExplicitType2         |
	| ExplicitType3         |
	| ExplicitType4         |
	| ExplicitType3Subtype1 |
	| ExplicitType4         |
	| ExplicitType5         |
	| ExplicitType6         |
	| ExplicitType5Subtype1 |
	| ExplicitType6         |
	| ContentType1          |
	| ContentType2          |
	| ContentType1Subtype1  |
	| ContentType2          |
	| ContentType3          |
	| ContentType4          |
	| ContentType3Subtype1  |
	| ContentType4          |
	| ContentType5          |
	| ContentType6          |
	| ContentType5Subtype1  |
	| ContentType6          |