Feature: Selecting troops
    As a player
    I want to select a troop into a group
    So I can create a group of troops for a game

Scenario: At start-up there are no troops in a group
    Given the application is just started
    When I view the troop group
    Then the troop group is empty

Scenario: Selecting a troop to group
    Given that viewed troops include "Nebelwerfer 42"
    When troop named "Nebelwerfer 42" is selected for a group
    Then the group list contains "Nebelwerfer 42"

Scenario: Removing a troop from group
    Given that "Nebelwerfer 42" is selected into a troop group
    When troop named "Nebelwerfer 42" is removed from a group
    Then the group list does not contain "Nebelwerfer 42"
