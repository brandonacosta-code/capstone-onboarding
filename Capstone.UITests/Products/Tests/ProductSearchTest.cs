using Allure.NUnit.Attributes;
using Boa.Constrictor.Playwright;
using Capstone.UITests.Core;
using Capstone.UITests.Products.Actions;
using Capstone.UITests.Products.Pages;
using NUnit.Framework;

namespace Capstone.UITests.Products.Tests;

[TestFixture]
[AllureSuite("Products")]
[BoundedContext("Products")]
public class ProductSearchTests : TestBase
{
	[Test]
	public async Task SearchProducts_SortByName_Ascending_ShowsFilteredResults()
	{
		await Actor.AttemptsToAsync(
			SearchAndSortProducts.With("chocolate", "Name", "Ascending"));

		var firstProduct = await Page
			.Locator(".k-table-row:first-child .k-table-td:first-child")
			.InnerTextAsync();

		Assert.That(firstProduct, Does.Contain("Chocolate"));
	}
}