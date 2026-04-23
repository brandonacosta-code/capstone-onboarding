using Boa.Constrictor.Playwright;
using Boa.Constrictor.Screenplay;
using Capstone.UITests.Products.Pages;
using System.Threading.Tasks;

namespace Capstone.UITests.Products.Actions;

public class SearchAndSortProducts : ITaskAsync
{
	private readonly string _searchTerm;
	private readonly string _sortBy;
	private readonly string _sortDirection;

	private SearchAndSortProducts(string searchTerm, string sortBy, string sortDirection)
	{
		_searchTerm = searchTerm;
		_sortBy = sortBy;
		_sortDirection = sortDirection;
	}

	public static SearchAndSortProducts With(string searchTerm, string sortBy, string sortDirection)
		=> new SearchAndSortProducts(searchTerm, sortBy, sortDirection);

	public async Task PerformAsAsync(IActor actor)
	{
		var page = actor.Using<BrowseTheWebWithPlaywright>().CurrentPage;
		await page.Locator("#searchInput").PressSequentiallyAsync(_searchTerm);
		await page.SelectOptionAsync("#sortBySelect", _sortBy);
		await page.SelectOptionAsync("#sortDirectionSelect", _sortDirection);
		await page.WaitForTimeoutAsync(800);
	}
}