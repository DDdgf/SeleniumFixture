﻿using System.Collections.Generic;
using OpenQA.Selenium;

namespace SeleniumFixture.Impl
{
    public interface IAutoFillAsAction<T>
    {
        IThenSubmitAction PerformFill(string requestName, object constraints);
    }

    public class AutoFillAsAction<T> : IAutoFillAsAction<T>
    {
        protected readonly IActionProvider _actionProvider;
        protected readonly IEnumerable<IWebElement> _elements;

        public AutoFillAsAction(IActionProvider actionProvider, IEnumerable<IWebElement> elements)
        {
            _actionProvider = actionProvider;
            _elements = elements;
        }

        public virtual IThenSubmitAction PerformFill(string requestName, object constraints)
        {
            var seedValue = _actionProvider.UsingFixture.Data.Generate<T>(requestName, constraints);

            return new AutoFillAction(_actionProvider, _elements, seedValue).PerformFill();
        }
    }
}
