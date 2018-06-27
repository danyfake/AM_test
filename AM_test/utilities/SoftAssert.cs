using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
namespace AM_test.utilities
{
    public class SoftAssert
    {
        private readonly List<SingleAssert>
        _verifications = new List<SingleAssert>();

        public void AddTrue(string message)
        {
            _verifications
                .Add(new SingleAssert(message, true.ToString(), false.ToString()));
        }

        public void AssertAll()
        {
            var failed = _verifications.Where(v => v.Failed).ToList();
            failed.Should().BeEmpty();
        }


        private class SingleAssert
        {
            private readonly string _message;
            private readonly string _expected;
            private readonly string _actual;

            public bool Failed => _expected != _actual;

            public SingleAssert(string message, string expected, string actual)
            {
                _message = message;
                _expected = expected;
                _actual = actual;
            }

            public override string ToString()
            {
                return $"'{_message}'";
            }
        }
    }
}
