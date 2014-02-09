Feature: Basic values
	As a player
    I want to see the names of troops listed
    So I can identify them

Scenario: Viewing one name
	Given that troops include "Barbed Wire"
	When troops are viewed
	Then "Barbed Wire" should be included in list of troops
