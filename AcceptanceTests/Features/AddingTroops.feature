﻿Feature: Adding troops into a group
    As a player
    I want to add troops into a group
    So I can create a group of troops for a game

Scenario: At start-up there are no troops in a group
    Given the application is just started
    When I view the troop group
    Then the troop group is empty

Scenario: Adding a troop to group
    Given that viewed troops include "Nebelwerfer 42"
    When troop named "Nebelwerfer 42" is added to a group
    Then the group list contains "Nebelwerfer 42"

Scenario: Adding several troops to group
    Given that viewed troops include "Nebelwerfer 42"
    And that viewed troops include "Blackshirts"
    When troop named "Nebelwerfer 42" is added to a group
    And troop named "Blackshirts" is added to a group
    Then the group list contains "Nebelwerfer 42"
    And the group list contains "Blackshirts"

Scenario: Adding same group twice to group
    Given that viewed troops include "Nebelwerfer 42"
    When troop named "Nebelwerfer 42" is added to a group
    And troop named "Nebelwerfer 42" is added to a group
    Then the group list contains "Nebelwerfer 42" 2 times
