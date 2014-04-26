@ApplicationIsNotRunningBeforeTest
Feature: Stopping the application
    As a user
    I want to stop the application
    So that I can have a life

Scenario: Application is stopped
    Given the application is running
    When I close the application
    Then application is no longer running

Scenario: Application can start without troop file
    Given there is no troop file
    When I start the application
    Then application is running
    And status message tells that troop file could not be read

Scenario: Application can start with invalid troop file
    Given the troop file is invalid
    When I start the application
    Then application is running
    And status message tells that troop file could not be read
