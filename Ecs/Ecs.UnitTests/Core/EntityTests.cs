using System;
using Ecs.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ecs.UnitTests.Core
{
    [TestClass]
    public class EntityTests
    {

        [TestInitialize]
        private void Setup()
        {
        }

        [TestMethod]
        public void AddChild_SetsParentToParent()
        {
            var world = new EntityWorld();
        
            var e = world.Create("Entity", 0);
            var child = world.Create("EntityChild", 0);
            e.AddChild(child);

            Assert.AreSame(e, child.Parent);
        }
    }
}
