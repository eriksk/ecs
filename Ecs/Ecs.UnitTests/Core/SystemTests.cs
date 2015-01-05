using System;
using Ecs.Core;
using Ecs.Core.Functions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ecs.UnitTests.Core
{
    internal class TestHarnessSystem : Ecs.Core.System, IStart, IUpdate
    {
        public TestHarnessComponent Component;

        public void Start()
        {
            Component = GetComponent<TestHarnessComponent>();
        }

        public void Update(float dt)
        {
            Component.IntValWithAttribute = 100;
        }
    }

    [TestClass]
    public class SystemTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var world = new EntityWorld();

            var entity = world.Create("", 0);

            entity.AddComponent(new TestHarnessComponent());
            entity.AddSystem(new TestHarnessSystem());

            entity.Start();
            entity.Update(16f);

            Assert.IsNotNull(entity.GetSystem<TestHarnessSystem>().Component);
            Assert.AreEqual(100, entity.GetComponent<TestHarnessComponent>().IntValWithAttribute);
        }
    }
}
