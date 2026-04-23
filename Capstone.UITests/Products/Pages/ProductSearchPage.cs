using Boa.Constrictor.Playwright;
using Microsoft.Playwright;

namespace Capstone.UITests.Products.Pages;

public static class ProductSearchPage
{
	public static IPlaywrightLocator SearchInput =>
		PlaywrightLocator.L("Search Input",
			page => page.Locator("#searchInput"));

	public static IPlaywrightLocator SortBySelect =>
		PlaywrightLocator.L("Sort By Select",
			page => page.Locator("#sortBySelect"));

	public static IPlaywrightLocator SortDirectionSelect =>
		PlaywrightLocator.L("Sort Direction Select",
			page => page.Locator("#sortDirectionSelect"));

	public static IPlaywrightLocator FirstProductName =>
		PlaywrightLocator.L("First Product Name",
			page => page.Locator(".k-table-row:first-child .k-table-td:first-child"));
}