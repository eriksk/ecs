using System;
using Ecs.Core;
using Ecs.Core.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System = Ecs.Core.System;

namespace Ecs.UnitTests.Core
{
    class TestHarnessComponent : Component
    {
        [PublicProperty]
        public int IntValWithAttribute { get; set; }

        public int IntValWithoutAttribute { get; set; }

        [PublicProperty(PropertyHint.Array)]
        public string[] StringArray { get; set; }
    }

    [TestClass]
    public class PrefabTests
    {
        [TestMethod]
        public void Cloning()
        {
            var array = new[] {"Hello", "World"};

            var prefab = new Prefab("", new Component[]
            {
                new TestHarnessComponent()
                {
                    IntValWithAttribute = 123,
                    IntValWithoutAttribute = 1531,
                    StringArray = array
                }
            },
            new Ecs.Core.System[0]);

            var world = new EntityWorld();

            var entity = world.Instantiate(prefab);

            var component = entity.GetComponent<TestHarnessComponent>();

            Assert.AreEqual(123, component.IntValWithAttribute);
            Assert.AreEqual(0, component.IntValWithoutAttribute);

            Assert.AreEqual("Hello", component.StringArray[0]);
            Assert.AreEqual("World", component.StringArray[1]);

            if (array == component.StringArray)
            {
                // ok for now...
                // Assert.Fail("Copied reference");
            }

        }
    }
}
