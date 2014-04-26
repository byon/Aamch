Feature: Basic values
    As a player
    I want to see the cost of the troops listed
    So I can pick the ones I can afford

Scenario Outline: Viewing values of the troops
    Given a single troop with <field> <value>
    When troops are viewed
    Then the single troop listed has <field> of <value>

    Examples:
    | field | value |
    | cost  | 12    |
