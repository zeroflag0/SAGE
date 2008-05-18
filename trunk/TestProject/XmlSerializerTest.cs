#region LGPL License
//********************************************************************
//	author:         Thomas "zeroflag" Kraemer
//	author email:   zeroflag@zeroflag.de
//	
//	Copyright (C) 2006-2008  Thomas "zeroflag" Kraemer
//	
//	license:	(LGPL)
//	
//		This library is free software; you can redistribute it and/or
//		modify it under the terms of the GNU Lesser General Public
//		License as published by the Free Software Foundation; either
//		version 2.1 of the License, or (at your option) any later version.
//
//		This library is distributed in the hope that it will be useful,
//		but WITHOUT ANY WARRANTY; without even the implied warranty of
//		MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//		Lesser General Public License for more details.
//
//		You should have received a copy of the GNU Lesser General Public
//		License along with this library; if not, write to the Free Software
//		Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
//
//		http://www.gnu.org/licenses/lgpl.html#TOC1
//
//*********************************************************************
#endregion LGPL License

// The following code was generated by Microsoft Visual Studio 2005.
// The test owner should check each test for validity.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Sage.Data;
using System.Xml.Serialization;
namespace TestProject
{
	/// <summary>
	///This is a test class for Sage.Data.XmlSerializer&lt;T&gt; and is intended
	///to contain all Sage.Data.XmlSerializer&lt;T&gt; Unit Tests
	///</summary>
	[TestClass()]
	public class XmlSerializerTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}
		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for XmlSerializer ()
		///</summary>
		[TestMethod()]
		public void ConstructorTest()
		{
			try
			{
				XmlSerializer<Generic> target = (XmlSerializer<Generic>)new XmlSerializer<Generic>.XmlSerializerFactory().Create();
			}
			catch (System.Reflection.ReflectionTypeLoadException exc)
			{
				string message = exc.ToString() + "\r\nTypes loaded:\r\n{\r\n";
				foreach (Type type in exc.Types)
				{
					message += type + ", ";
				}
				message += "\r\n}\r\n";
				foreach (Exception sub in exc.LoaderExceptions)
				{
					if (sub is System.IO.FileNotFoundException)
					{
						message += sub.ToString() + "\r\n[\r\n" + ((System.IO.FileNotFoundException)sub).InnerException + " " + ((System.IO.FileNotFoundException)sub).FileName + "\r\n]\r\n";
					}
					else
					{
						message += sub.ToString() + "\r\n";
					}
				}
				Assert.Fail(message);
			}
		}


		/// <summary>
		///A test for Serializer
		///</summary>
		[TestMethod()]
		public void SerializerTest()
		{
			XmlSerializer<Generic> target = (XmlSerializer<Generic>)new XmlSerializer<Generic>.XmlSerializerFactory().Create();

			XmlSerializer val = target.Serializer; // TODO: Assign to an appropriate value for the property

			target.Serializer = null;

			Assert.AreEqual(null, target.Serializer, "Sage.Data.XmlSerializer<T>.Serializer was not set correctly to null.");

			target.Serializer = val;

			Assert.AreEqual(val, target.Serializer, "Sage.Data.XmlSerializer<T>.Serializer was not set correctly to value.");
		}

		/// <summary>
		///A test for Serialize (T)
		///</summary>
		[TestMethod()]
		public void SerializeTest()
		{
			Generic value = new Generic("TestKey", "TestValue");
			XmlSerializer<Generic> target = (XmlSerializer<Generic>)new XmlSerializer<Generic>.XmlSerializerFactory().Create();
			target.FileName = "XmlSerializerTest.xml";
			target.Serialize(value);
		}

		/// <summary>
		///A test for Deserialize ()
		///</summary>
		[TestMethod()]
		public void DeserializeTest()
		{
			Generic value = new Generic("TestKey", "TestValue");
			XmlSerializer<Generic> target = (XmlSerializer<Generic>)new XmlSerializer<Generic>.XmlSerializerFactory().Create();
			target.FileName = "XmlSerializerTest.xml";
			target.Serialize(value);

			Generic ret = target.Deserialize();

			Assert.AreEqual(value.Name, ret.Name);
			Assert.AreEqual(value.Value, ret.Value);
		}


	}


}
