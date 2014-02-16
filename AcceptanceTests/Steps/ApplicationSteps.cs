﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace AcceptanceTests
{
    [Binding]
    public class ApplicationSteps
    {
        [Given(@"the application is running")]
        public void GivenTheApplicationIsRunning()
        {
            Context.GetApplication();
        }
        
        [When(@"I close the application")]
        public void WhenICloseTheApplication()
        {
            Context.GetApplication().Exit();
        }
        
        [Then(@"application is no longer running")]
        public void ThenApplicationIsNoLongerRunning()
        {
            Assert.IsFalse(Context.IsApplicationRunning());
        }
    }
}