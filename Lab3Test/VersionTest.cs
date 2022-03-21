using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3Test
{
	[TestFixture]
	class VersioningTest
	{

		[Test]
		public void MoreTest()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				_ = new Version("1.0,0-beta.alpha") > new Version("1.0.0-beta.1");
			});

			Assert.Throws<ArgumentException>(() =>
			{
				_ = new Version("QWERTY") > new Version("1.0.0-beta.1");
			});

			Assert.IsTrue(new Version("1.0.0-alpha.beta") > new Version("1.0.0-alpha.1"));
			Assert.IsTrue(new Version("1.0.1.1") > new Version("1.0.1"));
			Assert.IsTrue(new Version("1.0.1") > new Version("1.0.0"));
			Assert.IsTrue(new Version("1.1.0.1") > new Version("1.0.0.1"));
			Assert.IsTrue(new Version("1.1.0.1.7") > new Version("1.0.0.1"));

			Assert.IsFalse(new Version("0.0.9-alpha.5") > new Version("0.10.0-beta.10.5"));
			Assert.IsFalse(new Version("4.1.9-1") > new Version("5.9.0-5"));
			Assert.IsTrue(new Version("7.1.4.3-1") > new Version("7.1.4-1"));
			Assert.IsFalse(new Version("0.1.9-1") > new Version("0.2.0-5"));
		}

		[Test]
		public void LessTest()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				_ = new Version("?.!,0-beta.alpha") > new Version("1.0.0-alpha.1");
			});

			Assert.IsTrue(new Version("1.0.0-beta.1") < new Version("1.0.0-beta.alpha"));
			Assert.IsTrue(new Version("5.5.4-beta.1") < new Version("5.5.5-beta.alpha"));
			Assert.IsTrue(new Version("0.1.1-beta.1") < new Version("0.1.2-beta.alpha"));

			Assert.IsFalse(new Version("54.48.89") < new Version("54.48.80-alpha"));
			Assert.IsFalse(new Version("8.14.16") < new Version("8.14.15"));
			Assert.IsFalse(new Version("1.0.8-beta.1") < new Version("1.0.8-beta.0"));
		}

		[Test]
		public void MoreOrEqualTest()
		{
			Assert.IsTrue(new Version("1.0.0.1") >= new Version("1.0.0.1"));
			Assert.IsTrue(new Version("4.6.7") >= new Version("4.6.0"));
			Assert.IsTrue(new Version("5.5.5") >= new Version("5.5.5-alpha"));

			Assert.IsFalse(new Version("5.10.45") >= new Version("5.10.46"));
			Assert.IsFalse(new Version("40.50.40") >= new Version("40.50.45"));
			Assert.IsFalse(new Version("10.1.5-alpha.54.1") >= new Version("10.1.5-alpha.54.beta"));
		}

		[Test]
		public void LessOrEqualTest()
		{
			Assert.IsTrue(new Version("1.0.0") <= new Version("1.1.0"));
			Assert.IsTrue(new Version("4.8.0") <= new Version("4.9.1"));
			Assert.IsTrue(new Version("40.40.40-alpha") <= new Version("40.40.40"));
			Assert.IsTrue(new Version("0.1.0-alpha") <= new Version("0.1.0-alpha"));

			Assert.IsFalse(new Version("5.8.1-alpha") <= new Version("4.8.7-alpha"));
			Assert.IsFalse(new Version("4.1.7-alpha.5") <= new Version("4.1.7-alpha.1"));
			Assert.IsFalse(new Version("50.80.1") <= new Version("50.80.1-alpha")); // here
		}

		[Test]
		public void EqualTest()
		{
			Assert.IsTrue(new Version("0.0.0") == new Version("0.0.0"));
			Assert.IsTrue(new Version("5.1.1") == new Version("5.1.1"));
			Assert.IsTrue(new Version("1.1.1-beta") == new Version("1.1.1-beta"));

			Assert.IsFalse(new Version("50.10.8") == new Version("50.10.9"));
			Assert.IsFalse(new Version("40.88.99-alpha") == new Version("40.88.99-alpha.5"));
			Assert.IsFalse(new Version("80.10.7-alpha") == new Version("80.10.7"));
		}

		[Test]
		public void NoEqualTest()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				bool metka = new Version("1.0,0-beta.alpha") > new Version("qwerty");
			});

			Assert.IsTrue(new Version("1.0.0") != new Version("1.0.1"));
			Assert.IsTrue(new Version("5.1.5-alpha") != new Version("5.1.5"));
			Assert.IsTrue(new Version("10.44.1-alpha.beta") != new Version("10.44.1-alpha.beta.1"));

			Assert.IsFalse(new Version("1.0.0") != new Version("1.0.0"));
			Assert.IsFalse(new Version("10.5.1-aplha.beta") != new Version("10.5.1-aplha.beta"));
			Assert.IsFalse(new Version("8.7.4-1") != new Version("8.7.4-1"));
		}

		[Test]
		public void ToStringTest()
		{
			Assert.AreEqual("1.0.0", new Version("1.0.0").ToString());
			Assert.AreEqual("1.1.1-alpha", new Version("1.1.1-alpha").ToString());
			Assert.AreEqual("10.50.1-alpha.beta.1", new Version("10.50.1-alpha.beta.1").ToString());
		}

		[Test]
		public void FindVertionInIntervalTest()
		{
			VersionInterval interval = new VersionInterval("2.x.3");

			Assert.IsTrue(interval.FindInterval(new Version("2.1.3")));
			Assert.IsTrue(interval.FindInterval(new Version("2.500.3")));
			Assert.IsTrue(interval.FindInterval(new Version("2.0.3")));
			Assert.IsFalse(interval.FindInterval(new Version("1.1.3")));
			Assert.IsFalse(interval.FindInterval(new Version("2.1.0")));
			Assert.IsFalse(interval.FindInterval(new Version("1.1.2")));

			interval = new VersionInterval("2.x.x");

			Assert.IsTrue(interval.FindInterval(new Version("2.1.3")));
			Assert.IsTrue(interval.FindInterval(new Version("2.1.0")));
			Assert.IsTrue(interval.FindInterval(new Version("2.1.500")));
			Assert.IsTrue(interval.FindInterval(new Version("2.500.0")));
			Assert.IsTrue(interval.FindInterval(new Version("2.500.1")));
			Assert.IsTrue(interval.FindInterval(new Version("2.500.3")));
			Assert.IsTrue(interval.FindInterval(new Version("2.0.3")));
			Assert.IsTrue(interval.FindInterval(new Version("2.0.1")));
			Assert.IsTrue(interval.FindInterval(new Version("2.0.500")));
			Assert.IsTrue(interval.FindInterval(new Version("2.0.0")));
			Assert.IsFalse(interval.FindInterval(new Version("1.1.0")));
			Assert.IsFalse(interval.FindInterval(new Version("1.3.2")));
		}

		[Test]
		public void FindIntervalInIntervalTest()
		{
			VersionInterval version = new VersionInterval("2.x.3");

			Assert.IsTrue(version.FindIntervalInInterval(new VersionInterval("2.x.3")));
			Assert.IsFalse(version.FindIntervalInInterval(new VersionInterval("2.x.4")));
			Assert.IsFalse(version.FindIntervalInInterval(new VersionInterval("2.x.x")));
			Assert.IsFalse(version.FindIntervalInInterval(new VersionInterval("x.x.4")));

			version = new VersionInterval("2.x.x");

			Assert.IsTrue(version.FindIntervalInInterval(new VersionInterval("2.x.3")));
			Assert.IsTrue(version.FindIntervalInInterval(new VersionInterval("2.1.x")));
			Assert.IsTrue(version.FindIntervalInInterval(new VersionInterval("2.x.x")));
			Assert.IsFalse(version.FindIntervalInInterval(new VersionInterval("5.x.x")));
			Assert.IsFalse(version.FindIntervalInInterval(new VersionInterval("x.2.4")));

			version = new VersionInterval("x.x.3");

			Assert.IsTrue(version.FindIntervalInInterval(new VersionInterval("2.x.3")));
			Assert.IsTrue(version.FindIntervalInInterval(new VersionInterval("3.x.3")));
			Assert.IsFalse(version.FindIntervalInInterval(new VersionInterval("2.x.5")));
			Assert.IsFalse(version.FindIntervalInInterval(new VersionInterval("x.x.4")));
		}

		[Test]
		public void ToStringIntervalTest()
		{
			VersionInterval version = new VersionInterval("2.x.3");

			Assert.IsTrue(new VersionInterval("2.x.3").GetStringInterval() == " => 2.0.3 < 2.1000.3");
			Assert.IsTrue(new VersionInterval("*").GetStringInterval() == " => 0.0.0 < 1000.1000.1000");
			Assert.IsTrue(new VersionInterval("2.x").GetStringInterval() == " => 2.0.0 < 2.1000.1000");
			Assert.IsTrue(new VersionInterval("x.x.3").GetStringInterval() == " => 0.0.3 < 1000.1000.3");
			Assert.IsTrue(new VersionInterval("x.x.2").GetStringInterval() == " => 0.0.2 < 1000.1000.2");
			Assert.IsTrue(new VersionInterval("2").GetStringInterval() == " => 2.0.0 < 2.1000.1000");
			Assert.IsTrue(new VersionInterval("x.x.x").GetStringInterval() == " => 0.0.0 < 1000.1000.1000");

			Assert.IsFalse(new VersionInterval("2").GetStringInterval() == " => 2.2.0 < 2.4.1000");
			Assert.IsFalse(new VersionInterval("x.1.x").GetStringInterval() == " => 0.0.0 < 1000.1000.1000");
		}
	}
}
