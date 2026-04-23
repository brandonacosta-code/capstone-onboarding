using Allure.NUnit.Attributes;
using Boa.Constrictor.Playwright;
using Capstone.UITests.Checkout.Actions;
using Capstone.UITests.Checkout.Pages;
using Capstone.UITests.Core;
using NUnit.Framework;

namespace Capstone.UITests.Checkout.Tests;

[TestFixture]
[AllureSuite("Checkout")]
[BoundedContext("Checkout")]
public class CheckoutFlowTests : TestBase
{
    [Test]
    public async Task ViewProduct_AddToCart_PlaceOrder()
    {
        await Actor.AttemptsToAsync(new AddProductToCart());
        await Actor.AttemptsToAsync(new PlaceOrder());

        await Actor.Expects(CartPage.OrderSuccessModal).ToBeVisibleAsync();
        await Actor.Expects(CartPage.OrderConfirmedHeading).ToHaveTextAsync("Order Confirmed!");
    }
}
