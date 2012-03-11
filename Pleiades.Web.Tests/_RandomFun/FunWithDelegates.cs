using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Gallio.Framework;
using MbUnit.Framework;


namespace Pleiades.Web.Tests.RandomFun
{
    delegate void SomeAction(int x);
    delegate Encoding DelegateType1(string parameter);


    [TestFixture]
    public class FunWithDelegates
    {
        [Test]
        public void Test1()
        {
            var action = new SomeAction(HappyDelegate);
            action(3);
        }

        public void HappyDelegate(int x)
        {
            // Do stuff
        }


        [Test]
        public void Test2()
        {
            // classic delegate instancing
            var action = new DelegateType1(SpecialDelegate);

            // delegate inference
            DelegateType1 action2 = SpecialDelegate;
        }

        // Covariant, because the UTF8Encoding return type is more derived than the signature
        // Contravariant, because object parameter type is less derived than the signature
        public UTF8Encoding SpecialDelegate(object p)
        {
            // Do stuff with p
            return new UTF8Encoding();
        }
    }
}
