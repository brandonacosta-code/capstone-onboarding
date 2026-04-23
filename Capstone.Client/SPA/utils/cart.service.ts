export interface CartItem
{
	productId: number;
	name: string;
	qty: number;
	unitPrice: number;
	amount: number;
	stock: number;
}

export class CartService {
	private static _items: CartItem[] = [];

	static addItem(item: CartItem): void {
		const existing = this._items
			.find(i => i.productId === item.productId);

		if (existing) {
			if (existing.qty + 1 > existing.stock) {
				return; 
			}
			existing.qty += 1;
			existing.amount = existing.qty * existing.unitPrice;
		} else {
			this._items.push({ ...item });
		}
	}

	static getItems(): CartItem[] {
		return this._items;
	}

	static removeItem(productId: number): void {
		this._items = this._items.filter(i => i.productId !== productId);
	}

	static increaseQty(productId: number): boolean {
		const item = this._items.find(i => i.productId === productId);

		if (!item) return false;

		if (item.qty + 1 > item.stock) return false;  

		item.qty += 1;
		item.amount = item.qty * item.unitPrice;
		return true;
	}

	static clear(): void {
		this._items = [];
	}

	static decreaseQty(productId: number): void {
		const item = this._items.find(i => i.productId === productId);
		if (!item) return;

		if (item.qty - 1 <= 0) {
			this._items = this._items.filter(i => i.productId !== productId);
			return;
		}
		item.qty -= 1;
		item.amount = item.qty * item.unitPrice;
	}
}