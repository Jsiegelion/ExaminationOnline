using Mammothcode.Core.CoreTool.Html;
using NUnit.Framework;

namespace Tests.Core
{
    [TestFixture]
    public class TestRazorBind
    {
        private string _str     = string.Empty; //source string
        private string _prop    = string.Empty; //properties
        private string _target  = string.Empty; //target result
        private string _default = string.Empty; //if source is null or empty, we use defaultstring bind html

        [SetUp]
        public void TestSetUp()
        {
        }

        [Test]
        public void TestProperties()
        {
            _target = "src='str'";
            _str = "str";
            _prop = "src";
            var result = RazorBind.Properties(_str, _prop, _default);
            Assert.AreEqual(_target, result);
        }

        [Test]
        public void TestPropertiesByAnotherWord()
        {
            _target = "src='another word'";
            _str = "another word";
            _prop = "src";
            var result = RazorBind.Properties(_str, _prop, _default);
            Assert.AreEqual(_target, result);

        }

        [Test]
        public void TestPropertiesByAnotherProp()
        {
            _target = "value='str'";
            _str = "str";
            _prop = "value";
            var result = RazorBind.Properties(_str, _prop, _default);
            Assert.AreEqual(_target, result);
        }

        [Test]
        public void TestPropByNullStr()
        {
            _target = string.Empty;
            _str = null;
            _prop = "src";
            _default = null;
            var result = RazorBind.Properties(_str, _prop, _default);
            Assert.AreEqual(_target, result);
        }

        [Test]
        public void TestPropByNullProp()
        {
            _target = string.Empty;
            _str = "str";
            _prop = null;
            var result = RazorBind.Properties(_str, _prop, _default);
            Assert.AreEqual(_target, result);
        }

        [Test]
        public void TestPropByEmptyStr()
        {
            _target = string.Empty;
            _str = string.Empty;
            _prop = "src";
            _default = string.Empty;
            var result = RazorBind.Properties(_str, _prop, _default);
            Assert.AreEqual(_target, result);
        }

        [Test]
        public void TestPropByDefault()
        {
            _target = "src='default'";
            _str = string.Empty;
            _prop = "src";
            _default = "default";
            var result = RazorBind.Properties(_str, _prop, _default);
            Assert.AreEqual(_target, result);
        }
    }
}
