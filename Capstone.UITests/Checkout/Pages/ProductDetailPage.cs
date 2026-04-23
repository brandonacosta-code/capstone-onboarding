using Boa.Constrictor.Playwright;

namespace Capstone.UITests.Checkout.Pages;

public static class ProductDetailPage
{
    // "Add to cart" button in Product.cshtml — visible only when product is in stock
    public static IPlaywrightLocator AddToCartButton =>
        PlaywrightLocator.L(
            "Add to Cart Button",
            "button[onclick='productVM_addToCart()']");

    // Confirmation modal that appears after a product is successfully added to the cart
    public static IPlaywrightLocator AddedToCartModal =>
        PlaywrightLocator.L(
            "Added to Cart Confirmation Modal",
            "#notification");

    // "Out of stock" message shown when stock reaches 0 — strong inside the outOfStock div
    public static IPlaywrightLocator OutOfStockMessage =>
        PlaywrightLocator.L(
            "Out of Stock Message",
            "div[data-bind='visible: product.outOfStock'] strong");
}
