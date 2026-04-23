using Boa.Constrictor.Playwright;

namespace Capstone.UITests.Checkout.Pages;

public static class ProductListPage
{
    // "View" link rendered per row by the Kendo grid template in Grid.cshtml
    public static IPlaywrightLocator FirstProductViewButton =>
        PlaywrightLocator.L(
            "First Product View Button",
            ".k-grid-content tr:first-child a");

    // "View" link in the row that contains "Milk Chocolate" (stock = 5)
    public static IPlaywrightLocator MilkChocolateViewButton =>
        PlaywrightLocator.L(
            "Milk Chocolate View Button",
            ".k-grid-content tr:has-text('Milk Chocolate') a");
}
