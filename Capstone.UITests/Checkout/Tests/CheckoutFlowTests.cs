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

    [Test]
    public async Task AddProductToCart_UntilOutOfStock_ShowsOutOfStockMessage()
    {
        await Actor.AttemptsToAsync(new AddProductToCartRepeatedly(
            ProductListPage.MilkChocolateViewButton, times: 5));

        await Actor.Expects(ProductDetailPage.AddToCartButton).ToBeHiddenAsync();
        await Actor.Expects(ProductDetailPage.OutOfStockMessage).ToBeVisibleAsync();
    }
}
