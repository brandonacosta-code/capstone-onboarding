import { CartService } from "../utils/cart.service.js";
declare var kendo: any;

(window as any).productVM_addToCart = () => {
	const vm = (window as any).currentProductVM;
	if (vm) vm.addToCart();
};

function showNotification(message: string, type: 'success' | 'error'): void {
    const overlay = document.getElementById('notification');
    const icon = document.getElementById('notification-icon');
    const msg = document.getElementById('notification-message');
    const product = document.getElementById('notification-product');

    if (!overlay || !icon || !msg) return;

    icon.style.background = type === 'success' ? '#3d5a6b' : '#c0392b';
    msg.style.color = type === 'success' ? '#3d5a6b' : '#c0392b';
    msg.textContent = message;

    if (product) {
        const vm = (window as any).currentProductVM;
        product.textContent = type === 'success' && vm
            ? vm.product.Name
            : '';
    }

    overlay.style.display = 'flex';
    setTimeout(() => {
        overlay.style.display = 'none';
    }, 2000);
}

export class ProductVM {
	public product: any;

	constructor() {
		this.product = kendo.observable({
			Id: 0,
			Name: "",
			Description: "",
			ImageUrl: "",
			Stock: 0,
			UnitPrice: 0,
			inStock: false,
			outOfStock: true
		});

		(window as any).currentProductVM = this;

		const hash = window.location.hash;
		const match = hash.match(/\/product\/(\d+)/);
		const id = match ? match[1] : '0';

		if (id !== '0') {
			this.load(id);
		}
	}

    private async load(id: string) {
        const url = 'https://localhost:44325/api/product/' + id;

        try {
            const res = await fetch(url);
            if (!res.ok) throw new Error('Error ' + res.status);

            const data = await res.json();
            const originalStock = parseInt(data.Stock, 10);

            // Check if already in cart
            const inCart = CartService.getItems()
                .find(i => i.productId === data.Id);

            const qtyInCart = inCart ? inCart.qty : 0;
            const stockToShow = originalStock - qtyInCart;

            this.product.set('Id', data.Id);
            this.product.set('Name', data.Name);
            this.product.set('Description', data.Description);
            this.product.set('ImageUrl', data.ImageUrl);
            this.product.set('UnitPrice', data.UnitPrice);
            this.product.set('Stock', stockToShow);
            this.product.set('inStock', stockToShow > 0);
            this.product.set('outOfStock', stockToShow <= 0);

        } catch (e) {
            console.error(e);
        }
    }

    public addToCart = () => {
        const currentStock = parseInt(this.product.get('Stock'), 10);

        if (currentStock <= 0) {
            showNotification('This product is not available', 'error');
            return;
        }

        const inCart = CartService.getItems()
            .find(i => i.productId === this.product.Id);

        const originalStock = inCart ? inCart.stock : currentStock;

        if (inCart && inCart.qty >= originalStock) {
            showNotification('No more stock available', 'error');
            return;
        }

        CartService.addItem({
            productId: this.product.Id,
            name: this.product.Name,
            qty: 1,
            unitPrice: this.product.UnitPrice,
            amount: this.product.UnitPrice,
            stock: originalStock
        });

        const inCartNow = CartService.getItems()
            .find(i => i.productId === this.product.Id);
        const qtyInCart = inCartNow ? inCartNow.qty : 0;
        const realStock = inCartNow ? inCartNow.stock : originalStock;
        const remainingStock = realStock - qtyInCart;

        this.product.set('Stock', remainingStock);
        this.product.set('inStock', remainingStock > 0);
        this.product.set('outOfStock', remainingStock <= 0);

        showNotification('Added to cart!', 'success');
    }

}