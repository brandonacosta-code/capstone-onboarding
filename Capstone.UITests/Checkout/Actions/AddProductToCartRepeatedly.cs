using Boa.Constrictor.Playwright;
using Boa.Constrictor.Screenplay;
using Capstone.UITests.Checkout.Pages;

namespace Capstone.UITests.Checkout.Actions;

public class AddProductToCartRepeatedly : ITaskAsync
{
    private readonly IPlaywrightLocator _viewButton;
    private readonly int _times;

    public AddProductToCartRepeatedly(IPlaywrightLocator viewButton, int times)
    {
        _viewButton = viewButton;
        _times = times;
    }

    public async Task PerformAsAsync(IActor actor)
    {
        await actor.AttemptsToAsync(Click.On(_viewButton));
        await actor.Expects(ProductDetailPage.AddToCartButton).ToBeVisibleAsync();

        for (int i = 0; i < _times; i++)
        {
            await actor.AttemptsToAsync(Click.On(ProductDetailPage.AddToCartButton));
            await actor.Expects(ProductDetailPage.AddedToCartModal).ToBeVisibleAsync();
            await actor.Expects(ProductDetailPage.AddedToCartModal).ToBeHiddenAsync();
        }
    }
}
