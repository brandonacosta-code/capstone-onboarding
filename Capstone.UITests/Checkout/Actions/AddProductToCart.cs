using Boa.Constrictor.Playwright;
using Boa.Constrictor.Screenplay;
using Capstone.UITests.Checkout.Pages;

namespace Capstone.UITests.Checkout.Actions;

public class AddProductToCart : ITaskAsync
{
    public async Task PerformAsAsync(IActor actor)
    {
        await actor.AttemptsToAsync(Click.On(ProductListPage.FirstProductViewButton));
        await actor.Expects(ProductDetailPage.AddToCartButton).ToBeVisibleAsync();
        await actor.AttemptsToAsync(Click.On(ProductDetailPage.AddToCartButton));
        await actor.Expects(ProductDetailPage.AddedToCartModal).ToBeVisibleAsync();
    }
}
