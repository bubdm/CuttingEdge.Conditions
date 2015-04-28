#region Copyright (c) 2009 S. van Deursen
/* The CuttingEdge.Conditions library enables developers to validate pre- and postconditions in a fluent 
 * manner.
 * 
 * To contact me, please visit my blog at http://www.cuttingedge.it/blogs/steven/ 
 *
 * Copyright (c) 2009 S. van Deursen
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

using System;
using System.Globalization;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CuttingEdge.Conditions.UnitTests.StringTests
{
    /// <summary>
    /// Tests the ValidatorExtensions.StartsWith method.
    /// </summary>
    [TestClass]
    public class StringStartsWithTests
    {
        [TestMethod]
        [Description("Calling StartsWith on string x with 'x StartsWith x' should pass.")]
        public void StartsWithTest01()
        {
            string a = "test";
            Condition.Requires(a).StartsWith(a);
        }

        [TestMethod]
        [Description("Calling StartsWith on string x (\"test\") with 'x StartsWith \"tes\"' should pass.")]
        public void StartsWithTest02()
        {
            string a = "test";
            Condition.Requires(a).StartsWith("tes");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [Description("Calling StartsWith on string x (\"test\") with 'x StartsWith null' should fail.")]
        public void StartsWithTest03()
        {
            string a = "test";
            // A null value will never be found
            Condition.Requires(a).StartsWith(null);
        }

        [TestMethod]
        [Description("Calling StartsWith on string x (\"test\") with 'x StartsWith \"\"' should pass.")]
        public void StartsWithTest04()
        {
            string a = "test";
            // An empty string will always be found
            Condition.Requires(a).StartsWith(String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [Description("Calling StartsWith on string x (null) with 'x StartsWith \"\"' should fail.")]
        public void StartsWithTest05()
        {
            string a = null;
            // A null string only contains other null strings.
            Condition.Requires(a).StartsWith(String.Empty);
        }

        [TestMethod]
        [Description("Calling StartsWith on string x (\"test\") with 'x StartsWith null' should pass.")]
        public void StartsWithTest06()
        {
            string a = null;
            Condition.Requires(a).StartsWith(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [Description("Calling StartsWith on string x (\"test\") with 'x StartsWith \"test me\"' should fail.")]
        public void StartsWithTest07()
        {
            string a = "test";
            Condition.Requires(a).StartsWith("test me");
        }

        [TestMethod]
        [Description("Calling StartsWith on string x (\"test\") with 'x StartsWith \"test me\"' should fail with a correct exception message.")]
        public void StartsWithTest08()
        {
            string expectedMessage =
                "a should start with 'test me'." + Environment.NewLine +
                TestHelper.CultureSensitiveArgumentExceptionParameterText + ": a";

            try
            {
                string a = "test";
                Condition.Requires(a, "a").StartsWith("test me");
            }
            catch (Exception ex)
            {
                Assert.AreEqual(expectedMessage, ex.Message);
            }
        }

        [TestMethod]
        [Description("Calling StartsWith with conditionDescription parameter should pass.")]
        public void StartsWithTest09()
        {
            string a = null;
            Condition.Requires(a).StartsWith(null, string.Empty);
        }

        [TestMethod]
        [Description("Calling a failing StartsWith should throw an Exception with an exception message that contains the given parameterized condition description argument.")]
        public void StartsWithTest10()
        {
            string a = null;
            try
            {
                // A null string is considered to have a length of 0.
                Condition.Requires(a, "a").StartsWith("test", "qwe {0} xyz");
                Assert.Fail();
            }
            catch (ArgumentException ex)
            {
                Assert.IsTrue(ex.Message.Contains("qwe a xyz"));
            }
        }

        [TestMethod]
        [Description("Calling StartsWith should be language dependent.")]
        public void StartsWithTest11()
        {
            CultureInfo originalCulture = Thread.CurrentThread.CurrentCulture;

            string a = "hi ya'all";

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

                // We check this using the Turkish-I problem.
                // see: http://msdn.microsoft.com/en-us/library/ms973919.aspx#stringsinnet20_topic5
                string turkishUpperCase = "Hİ";

                Condition.Requires(a).StartsWith(turkishUpperCase, StringComparison.CurrentCultureIgnoreCase);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }   
        }

        [TestMethod]
        [Description("Calling StartsWith on string x (\"test\") with 'x StartsWith null' should succeed when exceptions are suppressed.")]
        public void StartsWithTest12()
        {
            string a = "test";
            // A null value will never be found
            Condition.Requires(a).SuppressExceptionsForTest().StartsWith(null);
        }
    }
}