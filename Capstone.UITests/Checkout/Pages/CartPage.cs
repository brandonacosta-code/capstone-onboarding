using Boa.Constrictor.Playwright;

namespace Capstone.UITests.Checkout.Pages;

public static class CartPage
{
    // "Shopping Cart" link in the nav menu defined in Index.cshtml
    public static IPlaywrightLocator ShoppingCartNavLink =>
        PlaywrightLocator.L(
            "Shopping Cart Nav Link",
            "nav a[href='#/cart']");

    // "Continue to payment" button in Cart.cshtml — calls cartVM_continueToPayment() on click
    public static IPlaywrightLocator ContinueToPaymentButton =>
        PlaywrightLocator.L(
            "Continue to Payment Button",
            "button[onclick='cartVM_continueToPayment()']");

    // "Order Confirmed!" modal shown by Cart.cshtml after the order is placed
    public static IPlaywrightLocator OrderSuccessModal =>
        PlaywrightLocator.L(
            "Order Success Modal",
            "#order-success-modal");

    // "Order Confirmed!" h2 heading inside the success modal
    public static IPlaywrightLocator OrderConfirmedHeading =>
        PlaywrightLocator.L(
            "Order Confirmed Heading",
            "#order-success-modal h2");
}
