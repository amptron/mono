//
// WarningHeaderValueTest.cs
//
// Authors:
//	Marek Safar  <marek.safar@gmail.com>
//
// Copyright (C) 2011 Xamarin Inc (http://www.xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Net.Http.Headers;

namespace MonoTests.System.Net.Http.Headers
{
	[TestFixture]
	public class WarningHeaderValueTest
	{
		[Test]
		public void Ctor_InvalidArguments ()
		{
			try {
				new WarningHeaderValue (1000, "rb", "\"\"");
				Assert.Fail ("#1");
			} catch (ArgumentOutOfRangeException) {
			}

			try {
				new WarningHeaderValue (1, null, null);
				Assert.Fail ("#2");
			} catch (ArgumentException) {
			}

			try {
				new WarningHeaderValue (1, "a", null);
				Assert.Fail ("#3");
			} catch (ArgumentException) {
			}
		}

		[Test]
		public void Equals ()
		{
			var value = new WarningHeaderValue (13, "x", "\"v\"");
			Assert.AreEqual (value, new WarningHeaderValue (13, "X", "\"v\""), "#1");
			Assert.AreNotEqual (value, new WarningHeaderValue (13, "x", "\"V\""), "#2");
			Assert.AreNotEqual (value, new WarningHeaderValue (13, "y", "\"V\""), "#3");
			Assert.AreNotEqual (value, new WarningHeaderValue (1, "x", "\"V\""), "#4");

			value = new WarningHeaderValue (6, "DD", "\"c\"", DateTimeOffset.MaxValue);
			Assert.AreEqual (value, new WarningHeaderValue (6, "DD", "\"c\"", DateTimeOffset.MaxValue), "#5");
			Assert.AreNotEqual (value, new WarningHeaderValue (6, "y", "\"V\""), "#6");
		}

		[Test]
		public void Parse ()
		{
			var res = WarningHeaderValue.Parse ("1 n \"\"");
			Assert.IsNull (res.Date, "#1");
			Assert.AreEqual (1, res.Code, "#2");
			Assert.AreEqual ("n", res.Agent, "#3");
			Assert.AreEqual ("\"\"", res.Text, "#4");
		}

		[Test]
		public void Parse_Invalid ()
		{
			try {
				WarningHeaderValue.Parse (null);
				Assert.Fail ("#1");
			} catch (FormatException) {
			}

			try {
				WarningHeaderValue.Parse ("  ");
				Assert.Fail ("#2");
			} catch (FormatException) {
			}

			try {
				WarningHeaderValue.Parse ("a");
				Assert.Fail ("#3");
			} catch (FormatException) {
			}
		}


		[Test]
		public void Properties ()
		{
			var value = new WarningHeaderValue (5, "ag", "\"tt\"");
			Assert.IsNull (value.Date, "#1");
			Assert.AreEqual (5, value.Code, "#2");
			Assert.AreEqual ("ag", value.Agent, "#3");
			Assert.AreEqual ("\"tt\"", value.Text, "#4");

			value = new WarningHeaderValue (5, "ag", "\"tt\"", DateTimeOffset.MinValue);
			Assert.AreEqual (DateTimeOffset.MinValue, value.Date, "#5");
			Assert.AreEqual (5, value.Code, "#6");
			Assert.AreEqual ("ag", value.Agent, "#7");
			Assert.AreEqual ("\"tt\"", value.Text, "#8");
		}

		[Test]
		public void TryParse ()
		{
			WarningHeaderValue res;
			Assert.IsTrue (WarningHeaderValue.TryParse ("22 a \"b\"", out res), "#1");
			Assert.IsNull (res.Date, "#2");
			Assert.AreEqual (22, res.Code, "#3");
			Assert.AreEqual ("a", res.Agent, "#4");
			Assert.AreEqual ("\"b\"", res.Text, "#5");
		}

		[Test]
		public void TryParse_Invalid ()
		{
			WarningHeaderValue res;
			Assert.IsFalse (WarningHeaderValue.TryParse ("", out res), "#1");
			Assert.IsNull (res, "#2");
		}
	}
}
