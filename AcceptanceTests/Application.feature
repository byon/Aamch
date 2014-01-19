Feature: Stopping the application
As a user
I want to stop the application
So that I can have a life

Scenario: Application is stopped
Given the application is running
When I close the application
Then application is no longer running
