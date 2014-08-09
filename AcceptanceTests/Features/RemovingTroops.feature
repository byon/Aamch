Feature: Removing troops from a group
    As a player
    I want to remove troops from a group
    So I can try different options for a group 

Scenario: Removing a troop from group
    Given that "Nebelwerfer 42" is in a troop group
    When troop named "Nebelwerfer 42" is removed from a group
    Then the group list does not contain "Nebelwerfer 42"

Scenario: Removing a troop leaves others intact
    Given that "Nebelwerfer 42" is in a troop group
    And that "Bazooka" is in a troop group
    When troop named "Nebelwerfer 42" is removed from a group
    Then the group list contains "Bazooka"
