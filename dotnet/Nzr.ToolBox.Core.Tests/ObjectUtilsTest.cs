using NSubstitute;
using System;
using Xunit;
using static Nzr.ToolBox.Core.ToolBox;

namespace Nzr.ToolBox.Core.Tests
{
    public class ObjectUtilsTest
    {
        [Fact]
        public void IfNotNull_WithNonNullValue_ShouldExecuteNotNullAction()
        {
            // Arrange

            string s = "CRF";
            Action<string> actionNotNull = Substitute.For<Action<string>>();
            Action actionNull = Substitute.For<Action>();

            // Act

            s.IfNotNull(i => actionNotNull(i)).Else(() => actionNull());

            // Assert

            actionNotNull.Received(1).Invoke(s);
            actionNull.DidNotReceive().Invoke();
        }

        [Fact]
        public void IfNotNull_WithNullValue_ShouldExecuteNullAction()
        {
            // Arrange

            string s = null;
            Action<string> actionNotNull = Substitute.For<Action<string>>();
            Action actionNull = Substitute.For<Action>();

            // Act
            s.IfNotNull(i => actionNotNull(i), () => actionNull());
            s.IfNotNull(i => actionNotNull(i)).Else(() => actionNull());

            // Assert

            actionNotNull.DidNotReceive().Invoke(s);
            actionNull.Received(2).Invoke();
        }

        [Fact]
        public void IfNotNull_WithNullValue_ShouldThrowException()
        {
            // Arrange

            string s = null;
            Action<string> actionNotNull1 = Substitute.For<Action<string>>();
            Action<string> actionNotNull2 = Substitute.For<Action<string>>();

            // Act
            ArgumentException ex1 = Assert.Throws<ArgumentException>(() => s.IfNotNull(i => actionNotNull1(i)).ElseThrow());
            ArgumentException ex2 = Assert.Throws<ArgumentException>(() => s.IfNotNull(i => actionNotNull2(i)).ElseThrow("Null is a mess."));

            // Assert

            actionNotNull1.DidNotReceive().Invoke(s);
            actionNotNull2.DidNotReceive().Invoke(s);
            Assert.Equal("Value cannot be null.", ex1.Message);
            Assert.Equal("Null is a mess.", ex2.Message);
        }

        [Fact]
        public void IfNotNull_WithNonNullValue_ShouldNotThrowException()
        {
            // Arrange

            string s = "a";
            Action<string> actionNotNull1 = Substitute.For<Action<string>>();
            Action<string> actionNotNull2 = Substitute.For<Action<string>>();

            // Act
            s.IfNotNull(i => actionNotNull1(i)).ElseThrow();
            s.IfNotNull(i => actionNotNull2(i)).ElseThrow("Null is a mess.");

            // Assert

            actionNotNull1.Received().Invoke(s);
            actionNotNull2.Received().Invoke(s);
        }

        [Fact]
        public void IfNull_WithNullValue_ShouldExecuteNullAction()
        {
            // Arrange

            string s = null;
            Action actionNull = Substitute.For<Action>();
            Action<string> actionNotNull = Substitute.For<Action<string>>();

            // Act

            s.IfNull(() => actionNull()).Else(i => actionNotNull(i));

            // Assert

            actionNull.Received(1).Invoke();
            actionNotNull.DidNotReceive().Invoke(s);
        }

        [Fact]
        public void IfNull_WithNotNullValue_ShouldExecuteNotNullAction()
        {
            // Arrange

            string s = "CRF";
            Action actionNull = Substitute.For<Action>();
            Action<string> actionNotNull = Substitute.For<Action<string>>();

            // Act

            s.IfNull(() => actionNull(), i => actionNotNull(i));
            s.IfNull(() => actionNull()).Else(i => actionNotNull(i));

            // Assert

            actionNull.DidNotReceive().Invoke();
            actionNotNull.Received(2).Invoke(s);
        }

        [Fact]
        public void ToDynamic_WithEntity_ShouldReturnDynamic()
        {
            // Arrange

            A a = new A() { P1 = new B() { P2 = new C() { P3 = 101 } } };

            // Act
            dynamic dynamicA = a.ToDynamic();

            // Arrange

            Assert.Equal(101, dynamicA.P1.P2.P3);
        }

        [Fact]
        public void IsAllNull_WithAllNull_ShouldReturnTrue()
        {
            // Arrange

            object a = null;
            string b = null;
            int? c = null;

            // Act

            bool isAllNull = IsAllNull(a, b, c);

            // Assert

            Assert.True(isAllNull);
        }

        [Fact]
        public void IsAllNull_WithOneNonNull_ShouldReturnFalse()
        {
            // Arrange

            object a = null;
            string b = "b";
            int? c = null;

            // Act

            bool isAllNull = IsAllNull(a, b, c);

            // Assert

            Assert.False(isAllNull);
        }

        [Fact]
        public void IsAllNonNull_WithAllNonNull_ShouldReturnTrue()
        {
            // Arrange

            object a = 1;
            string b = "2";
            int? c = 3;

            // Act

            bool isAllNonNull = IsAllNonNull(a, b, c);

            // Assert

            Assert.True(isAllNonNull);
        }

        [Fact]
        public void IsAllNonNull_WithOneNull_ShouldReturnFalse()
        {
            // Arrange

            object a = 1;
            string b = "2";
            int? c = null;

            // Act

            bool isAllNonNull = IsAllNonNull(a, b, c);

            // Assert

            Assert.False(isAllNonNull);
        }

        [Fact]
        public void AnyNull_WithOneNull_ShouldReturnTrue()
        {
            // Arrange

            object a = 1;
            string b = "2";
            int? c = null;

            // Act

            bool anyNull = AnyNull(a, b, c);

            // Assert

            Assert.True(anyNull);
        }

        [Fact]
        public void AnyNull_WithAllNonNull_ShouldReturnFalse()
        {
            // Arrange

            object a = 1;
            string b = "2";
            int? c = 3;

            // Act

            bool anyNull = AnyNull(a, b, c);

            // Assert

            Assert.False(anyNull);
        }

        [Fact]
        public void AnyNonNull_WithOneNonNull_ShouldReturnTrue()
        {
            // Arrange

            object a = null;
            string b = "2";
            int? c = null;

            // Act

            bool isAnyNonNull = AnyNonNull(a, b, c);

            // Assert

            Assert.True(isAnyNonNull);
        }

        [Fact]
        public void AnyNonNull_WithAllNull_ShouldReturnFalse()
        {
            // Arrange

            object a = null;
            string b = null;
            int? c = null;

            // Act

            bool isAnyNonNull = AnyNonNull(a, b, c);

            // Assert

            Assert.False(isAnyNonNull);
        }

        [Fact]
        public void FirstNonNull_WithAllNull_ShouldReturnNull()
        {
            // Arrange

            object a = null;
            string b = null;
            int? c = null;

            // Act

            object firstNonNull = FirstNonNull(a, b, c);

            // Assert

            Assert.Null(firstNonNull);
        }

        [Fact]
        public void FirstNonNull_WithAnyNonNull_ShouldReturnFirstNonNull()
        {
            // Arrange

            object a = null;
            string b = "b";
            int? c = 2;

            // Act

            object firstNonNull = FirstNonNull(a, b, c);

            // Assert

            Assert.Equal(b, firstNonNull);
        }

        [Fact]
        public void LastNonNull_WithAllNull_ShouldReturnNull()
        {
            // Arrange

            object a = null;
            string b = null;
            int? c = null;

            // Act

            object lastNonNull = LastNonNull(a, b, c);

            // Assert

            Assert.Null(lastNonNull);
        }

        [Fact]
        public void LastNonNull_WithAnyNonNull_ShouldReturnLastNonNull()
        {
            // Arrange

            object a = null;
            string b = "b";
            int? c = 2;

            // Act

            object lastNonNull = LastNonNull(a, b, c);

            // Assert

            Assert.Equal(c, lastNonNull);
        }

        [Fact]
        public void RequireNonNull_WithNullWithoutCustomMessage_ShouldThrowExcetpion()
        {
            // Arrange

            string s = null;

            // Act
            ArgumentException ex1 = Assert.Throws<ArgumentException>(() => s.RequireNonNull());

            // Assert

            Assert.Equal("Value cannot be null.", ex1.Message);
        }

        [Fact]
        public void RequireNonNull_WithNullWitCustomMessage_ShouldThrowExcetpionWithCustomMessage()
        {
            // Arrange

            string s = null;

            // Act
            ArgumentException ex1 = Assert.Throws<ArgumentException>(() => s.RequireNonNull("Id cannot be null."));

            // Assert

            Assert.Equal("Id cannot be null.", ex1.Message);
        }

        [Fact]
        public void RequireNonNull_WithNonNull_ShouldNotThrowExcetpion()
        {
            // Arrange

            string s = "a";

            // Act
            s.RequireNonNull();

            // Assert


        }

        private class A
        {
            public B P1 { get; set; }
        }

        private class B
        {
            public C P2 { get; set; }
        }

        private class C
        {
            public int P3 { get; set; }
        }
    }
}