using Boa.Constrictor.Playwright;
using Boa.Constrictor.Screenplay;
using Microsoft.Playwright;
using NUnit.Framework;

namespace Capstone.UITests.Core;

public abstract class TestBase
{
    private BrowseTheWebWithPlaywright _ability = null!;

    protected IActor Actor { get; private set; } = null!;

    protected const string BaseUrl = "http://localhost:53019/#/products";

    [SetUp]
    public async Task SetUp()
    {
        _ability = await BrowseTheWebWithPlaywright.UsingChromium(new BrowserTypeLaunchOptions
        {
            Headless = false
        });

        Actor = new Actor("User");
        Actor.Can(_ability);

        await Actor.AttemptsToAsync(Go.To(BaseUrl));
    }

    [TearDown]
    public async Task TearDown()
    {
        await _ability.Browser.CloseAsync();
        _ability.Playwright.Dispose();
    }
}
