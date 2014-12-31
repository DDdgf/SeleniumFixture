﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    /// <summary>
    /// Provides wait fluent syntax
    /// </summary>
    public interface IWaitActionProvider
    {
        /// <summary>
        /// Wait for an element to be visible
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitActionProvider ForElement(string selector, double? timeout = null);

        /// <summary>
        /// Wait for an element to be visible
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitActionProvider ForElement(By selector, double? timeout = null);

        /// <summary>
        /// Wait for no ajax calls to be active
        /// </summary>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitActionProvider ForAjax(double? timeout = null);

        /// <summary>
        /// Wait until the provided delegate is true
        /// </summary>
        /// <param name="testFunc">func to test</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        IWaitActionProvider Until(Func<IActionProvider, bool> testFunc, double? timeout = null);

        /// <summary>
        /// Provides a way back to action syntax
        /// </summary>
        IActionProvider Then { get; }
    }

    /// <summary>
    /// Provides wait fluent syntax
    /// </summary>
    public class WaitActionProvider : IWaitActionProvider
    {
        private readonly IActionProvider _actionProvider;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="actionProvider"></param>
        public WaitActionProvider(IActionProvider actionProvider)
        {
            _actionProvider = actionProvider;
            Then = _actionProvider;
        }

        /// <summary>
        /// Wait for an element to be present
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        public IWaitActionProvider ForElement(string selector, double? timeout = null)
        {
            return Until(i => i.CheckForElement(selector), timeout);
        }

        /// <summary>
        /// Wait for an element to be present
        /// </summary>
        /// <param name="selector">element selector</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        public IWaitActionProvider ForElement(By selector, double? timeout = null)
        {
            return Until(i => i.CheckForElement(selector), timeout);
        }

        /// <summary>
        /// Wait for no ajax calls to be active
        /// </summary>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        public IWaitActionProvider ForAjax(double? timeout = null)
        {
            return Until(
                i =>
                {
                    IJavaScriptExecutor executor = (IJavaScriptExecutor)_actionProvider.UsingFixture.Driver;

                    return (bool)executor.ExecuteScript(_actionProvider.UsingFixture.Configuration.AjaxActiveTest);
                },timeout);
        }

        /// <summary>
        /// Wait until the provided delegate is true
        /// </summary>
        /// <param name="testFunc">func to test</param>
        /// <param name="timeout">timeout in seconds</param>
        /// <returns>fluent syntax object</returns>
        public IWaitActionProvider Until(Func<IActionProvider, bool> testFunc, double? timeout = null)
        {
            if (!timeout.HasValue)
            {
                timeout = _actionProvider.UsingFixture.Configuration.DefaultWaitTimeout;
            }

            DateTime expire = DateTime.Now.AddSeconds(timeout.Value);
            bool untilResult = false;

            while (!untilResult)
            {
                Thread.Sleep((int)(_actionProvider.UsingFixture.Configuration.DefaultWaitInterval * 1000));

                untilResult = testFunc(_actionProvider);

                if (!untilResult && DateTime.Now > expire)
                {
                    break;
                }
            }

            if (!untilResult)
            {
                throw new Exception("Timedout waiting: " + timeout);    
            }

            return this;
        }

        /// <summary>
        /// Provides a way back to action syntax
        /// </summary>
        public IActionProvider Then { get; private set; }
    }
}
