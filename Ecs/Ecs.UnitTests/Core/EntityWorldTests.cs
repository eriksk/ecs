using System;
using Ecs.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ecs.UnitTests.Core
{
    [TestClass]
    public class EntityWorldTests
    {
        [TestMethod]
        public void Create_WithNameAndLayer_CreatesEntityWithThoseValues()
        {
            var world = new EntityWorld();
            
            var entity = world.Create("Entity1", 12);

            Assert.AreEqual("Entity1", entity.Name);
            Assert.AreEqual(12, entity.Layer);
        }
    }
}
