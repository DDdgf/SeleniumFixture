﻿using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace SeleniumFixture
{
    public static class LanguageExtensions
    {
        public static void Apply<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var t in enumerable)
            {
                action(t);
            }
        }

        public static IReadOnlyCollection<IWebElement> Click(this IReadOnlyCollection<IWebElement> elements)
        {
            elements.Apply(e => e.Click());

            return elements;
        }
        
        public static IReadOnlyCollection<IWebElement> Clear(this IReadOnlyCollection<IWebElement> elements)
        {
            elements.Apply(e => e.Clear());

            return elements;
        }

        public static IReadOnlyCollection<IWebElement> SendKeys(this IReadOnlyCollection<IWebElement> elements,
            string keys)
        {
            elements.Apply(e => e.SendKeys(keys));

            return elements;
        }

        public static IReadOnlyCollection<IWebElement> SendKeys(this IReadOnlyCollection<IWebElement> elements,
            Func<string> keysFunc)
        {
            elements.Apply(e => e.SendKeys(keysFunc()));

            return elements;
        }
    }
}
