using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using HemtentaTdd2017;
using Moq;
using HemtentaTdd2017.webshop;

namespace UnitTestProjectHemtenta
{
    /*
    1) Vilka metoder och properties behöver testas?
       Svar: Metoderna Checkout(), AddProduct() och RemoveProduct() samt property TotalCost tycker jag behöver testas 
             
    2) Ska några exceptions kastas?
       Svar: Ja! Om betalning inte går igenom, om objekt är null, om varukorg är tom, 
             om amount är noll eller negativt tal vid AddProduct/RemoveProduct
    
    3) Vilka är domänerna för IWebshop och IBasket?
       Svar: 
       IWebshop: Objekt av IBasket eller IBilling, null
       IBasket: Objekt av Product, null
                amount kan vara heltal mellan minValue och maxValue
                TotalCost kan vara decimaltal mellan minValue och maxValue   
    */



    [TestFixture]
    public class WebshopTest
    {
        private Product p;
        private Basket b;
        private MyWebshop mw;
        private Mock<IBilling> mockBill;

        [SetUp]
        public void SetUpWebshopTest()
        {
            p = new Product();
            b = new Basket();
            mw = new MyWebshop(b);
            mockBill = new Mock<IBilling>();
        }


        [Test]
        [TestCase(10)]
        public void AddProduct_ToProductListInBasket_WithCorrectOrdersum_Success(int amount)
        {
            p.Price = 20;
            decimal expectedSum = b.TotalCost + p.Price * amount;
            b.AddProduct(p, amount);

            Assert.That(expectedSum, Is.EqualTo(b.TotalCost));
        }

        [Test]
        public void AddProduct_ProductIsNull_Throws()
        {
            p = null;
            Assert.That(() => b.AddProduct(p, 1), Throws.TypeOf<ArgumentNullException>()); 
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void AddProduct_InvalidAmountInput_Throws(int amount)
        {
            Assert.That(() => b.AddProduct(p, amount), Throws.TypeOf<IllegalAmountException>());
        }

        [Test]
        [TestCase(40, 1)]
        public void RemoveProduct_FromProductListInBasket_WithCorrectOrdersum_Success(decimal sumBeforeRemoval, int numProdToRemove)
        {
            p.Price = 20;
            b.AddProduct(p, 1);

            Product p2 = new Product();
            p2.Price = 10;
            b.AddProduct(p2, 2);

            decimal sumAfterRemoval = sumBeforeRemoval - (p2.Price * numProdToRemove);
            b.RemoveProduct(p2, numProdToRemove);

            Assert.That(sumAfterRemoval, Is.EqualTo(b.TotalCost));
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        public void RemoveProduct_InvalidAmountInputThrows(int amount)
        {
            Assert.That(() => b.RemoveProduct(p, amount), Throws.TypeOf<IllegalAmountException>());
        }


        [Test] // Method Pay is called one time at Checkout
        public void DoCheckout_PayIsCalled_Success()
        {
            p.Price = 20;
            b.AddProduct(p, 5);
            decimal total = b.TotalCost;
            mw.SetBasket(b);

            mw.Checkout(mockBill.Object);

            Assert.That(mw.Basket.TotalCost, Is.EqualTo(0));
            mockBill.Verify(x => x.Pay(total), Times.Once);
        }

        [Test]
        public void DoCheckout_MakePayment_Fails_Throws()
        {
            p.Price = 20;
            b.AddProduct(p, 5);
            decimal total = b.TotalCost;
            mw.SetBasket(b);

            mockBill.Setup(x => x.Pay(total)).Throws(new InsufficientFundsException());

            Assert.That(() => mw.Checkout(mockBill.Object), Throws.TypeOf<InsufficientFundsException>());
            Assert.That(b.TotalCost, Is.EqualTo(total));
        }

        [Test]
        public void DoCheckout_NoItem_InBasket_Throws()
        {
            var mockBasket = new Mock<IBasket>();
            mockBasket.Setup(x => x.TotalCost).Returns(0);
            mw.SetBasket(mockBasket.Object);

            Assert.That(() => mw.Checkout(mockBill.Object), Throws.TypeOf<Exception>());
        }

        [Test]
        public void DoCheckout_BasketIsNull_Throws()
        {
            var mockBilling = new Mock<IBilling>();

            Assert.That(() => mw.Checkout(mockBilling.Object), Throws.TypeOf<NullReferenceException>());
        }
    }
}
