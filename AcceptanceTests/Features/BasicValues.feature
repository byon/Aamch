﻿Feature: Basic values
    As a player
    I want to see the cost of the troops listed
    So I can pick the ones I can afford

    As a player
    I want to see the type and subtype of the troops listed
    So I can categorize troops based on their purpose

Scenario Outline: Viewing values of the troops
    Given a single troop with <field> set to <value>
    When troops are viewed
    Then the single troop listed has <field> of <value>

    Examples:
    | field                    | value         |
    | cost                     | 12            |
    | type                     | soldier       |
    | subtype                  | artillery     |
    | subtype                  |               |
    | front defense            | 48            |
    | rear defense             | 36            |
    | attack (soldier/short)   | 22            |
    | attack (soldier/medium)  | 18            |
    | attack (soldier/long)    | 2             |
    | attack (vehicle/short)   | 46            |
    | attack (vehicle/medium)  | 22            |
    | attack (vehicle/long)    | 12            |
    | special abilities        | an ability    |
    | commander ability        | Stalwart      |

Scenario: Viewing troops without defense
    Given a single troop without defense
    When troops are viewed
    Then the single troop listed has front defense of N/A
    Then the single troop listed has rear defense of N/A

Scenario: Viewing troop with several special abilities
    Given a single troop with special abilities set to first; and second
    When troops are viewed
    Then the single troop listed has special abilities of first, and second
