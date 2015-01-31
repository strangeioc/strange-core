using NUnit.Framework;
using strange.extensions.context.impl;
using strange.extensions.listBind.api;
using strange.extensions.listBind.impl;
using System.Collections.Generic;

namespace strange.unittests
{
    [TestFixture]
    public class TestListBinder
    {
        private MockContext context;
        IListBinder binder;
        private object contextView;

        [SetUp]
        public void SetUp()
        {
            Context.firstContext = null;
            contextView = new object();
            context = new MockContext(contextView, true);
            context.Start();
            binder = context.listBinder;

        }

        [Test]
        public void TestInjectListWithValues()
        {
            binder.Bind<string>().ToValue("a");
            binder.Bind<string>().ToValue("b");
            binder.Bind<string>().ToValue("c");

            var strings = context.injectionBinder.GetInstance<List<string>>();

            Assert.AreEqual(3, strings.Count);
            Assert.AreEqual("a", strings[0]);
            Assert.AreEqual("b", strings[1]);
            Assert.AreEqual("c", strings[2]);
        }

        [Test]
        public void TestInjectTypesToList()
        {
            binder.Bind<IListItem>().To<ListItemA>();
            binder.Bind<IListItem>().To<ListItemB>();

            var list = context.injectionBinder.GetInstance<List<IListItem>>();

            Assert.AreEqual(2, list.Count);
            Assert.IsAssignableFrom(typeof(ListItemA), list[0]);
            Assert.IsAssignableFrom(typeof(ListItemB), list[1]);
        }


        [Test]
        public void TestInjectListWithSingletonItems()
        {
            binder.Bind<IListItem>().To<ListItemA>().ToSingleton();
            binder.Bind<IListItem>().To<ListItemB>();

            var list1 = context.injectionBinder.GetInstance<List<IListItem>>();
            var list2 = context.injectionBinder.GetInstance<List<IListItem>>();

            Assert.AreNotEqual(list1, list2);

            // Singleton
            Assert.AreEqual(list1[0], list2[0]);

            // New instance
            Assert.AreNotEqual(list1[1], list2[1]);

        }

        [Test]
        public void TestInjectListWithMixedValuesAndTypes()
        {
            binder.Bind<IListItem>().ToValue(new ListItemA());
            binder.Bind<IListItem>().To<ListItemB>();

            var list1 = context.injectionBinder.GetInstance<List<IListItem>>();
            var list2 = context.injectionBinder.GetInstance<List<IListItem>>();

            Assert.AreNotEqual(list1, list2);

            // Bound by value
            Assert.AreEqual(list1[0], list2[0]);

            // New instance
            Assert.AreNotEqual(list1[1], list2[1]);

        }
    }

    public interface IListItem
    {

    }

    public class ListItemA : IListItem
    {

    }
    public class ListItemB: IListItem
    {

    }
}
