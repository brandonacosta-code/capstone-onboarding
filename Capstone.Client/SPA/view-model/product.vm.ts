import { CartService } from "../utils/cart.service.js";
declare var kendo: any;

(window as any).productVM_addToCart = () => {
	const vm = (window as any).currentProductVM;
	if (vm) vm.addToCart();
};

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

	private async load(id: string)
	{
		const url = `https://localhost:44325/api/product/${id}`;

		try {
			const res = await fetch(url);

			if (!res.ok) {
				throw new Error(`Error ${res.status}: ${res.statusText}`);
			}

			const data = await res.json();
			const stock = parseInt(data.Stock, 10)

			this.product.set('Id', data.Id);
			this.product.set('Name', data.Name);
			this.product.set('Description', data.Description);
			this.product.set('ImageUrl', data.ImageUrl);
			this.product.set('Stock', stock);
			this.product.set('inStock', stock > 0);
			this.product.set('outOfStock', stock <= 0);
			this.product.set('UnitPrice', data.UnitPrice);
		} catch (e) {
			console.error(e);
		}
	}

	public addToCart = () => {
		const stock = this.product.Stock;

		if (stock <= 0) {
			alert('The product is not available');
			return;
		}

		const inCart = CartService.getItems()
			.find(i => i.productId === this.product.Id);

		if (inCart && inCart.qty >= stock) {
			alert('No more stock available');
			return;
		}

		CartService.addItem({
			productId: this.product.Id,
			name: this.product.Name,
			qty: 1,
			unitPrice: this.product.UnitPrice,
			amount: this.product.UnitPrice,
			stock: stock
		});

		alert(this.product.Name + ' added to cart');
	}

}