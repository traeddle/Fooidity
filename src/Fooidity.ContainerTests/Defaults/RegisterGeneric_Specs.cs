﻿namespace Fooidity.ContainerTests.Defaults
{
    using Autofac;
    using NUnit.Framework;


    [TestFixture]
    public class A_default_disabled_foodid
    {
        [Test]
        public void Should_return_as_default()
        {
            var builder = new ContainerBuilder();

            builder.CodeSwitchesDisabledByDefault();

            IContainer container = builder.Build();

            var fooId = container.Resolve<CodeSwitch<Active>>();

            Assert.IsFalse(fooId.Enabled);
        }

        [Test]
        public void Should_return_as_specific()
        {
            var builder = new ContainerBuilder();

            builder.CodeSwitchesDisabledByDefault();

            builder.RegisterEnabled<Active>();

            IContainer container = builder.Build();

            var fooId = container.Resolve<CodeSwitch<Active>>();

            Assert.IsTrue(fooId.Enabled);
        }

        [Test]
        public void Should_convert_to_specific()
        {
            var builder = new ContainerBuilder();

            builder.CodeSwitchesDisabledByDefault();

            IContainer container = builder.Build();

            var fooId = container.Resolve<CodeSwitch<Active>>();

            Assert.IsFalse(fooId.Enabled);

            container.Enable<Active>();

            fooId = container.Resolve<CodeSwitch<Active>>();

            Assert.IsTrue(fooId.Enabled);
        }


        struct Active :
            CodeFeature
        {
        }
    }
}