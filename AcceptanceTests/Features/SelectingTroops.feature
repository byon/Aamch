Feature: Selecting troops into a group
    As a player
    I want to select troops into a group
    So I can create a group of troops for a game

Scenario: At start-up there are no troops in a group
    Given the application is just started
    When I view the troop group
    Then the troop group is empty

Scenario: Selecting a troop to group
    Given that viewed troops include "Nebelwerfer 42"
    When troop named "Nebelwerfer 42" is selected for a group
    Then the group list contains "Nebelwerfer 42"
