Feature: Listing troops
	As a player
    I want to list the available troops
    So I can compare them

Scenario: Viewing name of one troop
	Given that troops include "Barbed Wire"
	When troops are viewed
	Then "Barbed Wire" should be included in list of troops

Scenario: Viewing several troops
	Given that troops include "Barbed Wire"
    And that troops include "6-Pounder ATG"
    And that troops include "Vickers MG Team"
	When troops are viewed
	Then "Barbed Wire" should be included in list of troops
    And "6-Pounder ATG" should be included in list of troops
    And "Vickers MG Team" should be included in list of troops

Scenario: Empty troop list is handled
	Given that there are no troops
	When troops are viewed
	Then no troops are included in list of troops
