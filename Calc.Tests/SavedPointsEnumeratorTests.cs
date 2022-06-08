using Calc.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc.Tests
{
    [TestClass]
    public class SavedPointsEnumeratorTests
    {
        [TestMethod]
        public void ThreeSavedPoints()
        { 
            var e = new SavedPointsEnumerator<int>(((IEnumerable<int>)new[] { 1, 2, 3, 4, 5, 6, 7 }).GetEnumerator());

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(1, e.Current);

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(2, e.Current);

            e.CreateSavePoint();

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(3, e.Current);

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(4, e.Current);

            e.CreateSavePoint();

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(5, e.Current);

            e.CreateSavePoint();

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(6, e.Current);

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(7, e.Current);

            Assert.IsFalse(e.MoveNext());

            e.RestoreLastSavedPoint();

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(6, e.Current);

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(7, e.Current);

            Assert.IsFalse(e.MoveNext());

            e.RestoreLastSavedPoint();

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(5, e.Current);

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(6, e.Current);

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(7, e.Current);

            Assert.IsFalse(e.MoveNext());

            e.RestoreLastSavedPoint();

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(3, e.Current);

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(4, e.Current);

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(5, e.Current);

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(6, e.Current);

            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual(7, e.Current);
        }
    }
}
