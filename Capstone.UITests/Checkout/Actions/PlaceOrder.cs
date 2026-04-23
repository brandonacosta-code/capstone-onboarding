using Boa.Constrictor.Playwright;
using Boa.Constrictor.Screenplay;
using Capstone.UITests.Checkout.Pages;

namespace Capstone.UITests.Checkout.Actions;

public class PlaceOrder : ITaskAsync
{
    public async Task PerformAsAsync(IActor actor)
    {
        await actor.AttemptsToAsync(Click.On(CartPage.ShoppingCartNavLink));
        await actor.Expects(CartPage.ContinueToPaymentButton).ToBeVisibleAsync();
        await actor.AttemptsToAsync(Click.On(CartPage.ContinueToPaymentButton));
    }
}
