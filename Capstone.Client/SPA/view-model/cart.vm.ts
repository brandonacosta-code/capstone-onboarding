declare var kendo: any;
import { CartService } from '../utils/cart.service.js';

const TAX_RATE = 0.05;
const SHIPPING = 10;

// ── Observable ViewModel ──
let cartObservable: any = null;

function escapeHtml(str: string): string {
    return str
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;');
}

function renderCart(): void {
    const container = document.getElementById('cart-items-container');
    if (!container) {
        console.warn('[CartVM] #cart-items-container not found');
        return;
    }

    const items = CartService.getItems();

    container.innerHTML = items.map(function (item) {
        return '<tr class="cart-row" data-productid="' + item.productId + '">' +
            '<td style="padding: 14px 0; vertical-align: middle;">' +
            '<div style="font-size: 15px; margin-bottom: 6px;">' + escapeHtml(item.name) + '</div>' +
            '<div style="display: flex; align-items: center;">' +
            '<div style="display: inline-flex; align-items: center; border: 0.5px solid #ccc; border-radius: 8px; overflow: hidden;">' +
            '<button onclick="cartVM_decreaseQty(this)" style="width:28px;height:28px;background:#f5f5f5;border:none;font-size:16px;cursor:pointer;line-height:1;">-</button>' +
            '<div style="width:32px;height:28px;text-align:center;line-height:28px;font-size:14px;font-weight:500;border-left:0.5px solid #ccc;border-right:0.5px solid #ccc;">' + item.qty + '</div>' +
            '<button onclick="cartVM_increaseQty(this)" style="width:28px;height:28px;background:#f5f5f5;border:none;font-size:16px;cursor:pointer;line-height:1;">+</button>' +
            '</div>' +
            '<button onclick="cartVM_removeItem(this)" style="background:none;border:none;cursor:pointer;margin-left:12px;padding:4px;color:#999;font-size:16px;">&#128465;</button>' +
            '</div>' +
            '</td>' +
            '<td style="text-align: right; font-weight: 500; padding: 14px 0; vertical-align: middle;">$' + item.amount.toFixed(2) + '</td>' +
            '</tr>';
    }).join('');

    // ── Kendo MVVM ──
    const sub = items.reduce(function (acc, i) { return acc + i.amount; }, 0);
    const tax = Math.round(sub * TAX_RATE * 100) / 100;
    const total = Math.round((sub + tax + SHIPPING) * 100) / 100;

    if (cartObservable) {
        cartObservable.set('subTotal', '$' + sub.toFixed(2));
        cartObservable.set('tax', '$' + tax.toFixed(2));
        cartObservable.set('shipping', '$' + SHIPPING.toFixed(2));
        cartObservable.set('total', '$' + total.toFixed(2));
    }
}

// ── Global Actions ───
(window as any).cartVM_removeItem = (btn: HTMLElement): void => {
    const row = btn.closest('[data-productid]') as HTMLElement;
    const productId = parseInt(row.dataset.productid ?? '0', 10);
    CartService.removeItem(productId);
    renderCart();
};

(window as any).cartVM_increaseQty = (btn: HTMLElement): void => {
    const row = btn.closest('[data-productid]') as HTMLElement;
    const productId = parseInt(row.dataset.productid ?? '0', 10);
    CartService.increaseQty(productId);
    renderCart();
};

(window as any).cartVM_decreaseQty = (btn: HTMLElement): void => {
    const row = btn.closest('[data-productid]') as HTMLElement;
    const productId = parseInt(row.dataset.productid ?? '0', 10);
    CartService.decreaseQty(productId);
    renderCart();
};

(window as any).cartVM_continueToPayment = async (): Promise<void> => {
    const items = CartService.getItems();
    if (items.length === 0) {
        alert('Your cart is empty');
        return;
    }

    const payload = {
        items: items.map(function (i) {
            return {
                productId: i.productId,
                name: i.name,
                qty: i.qty,
                unitPrice: i.unitPrice,
                amount: i.amount
            };
        })
    };

    try {
        const res = await fetch('https://localhost:44325/api/order', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        if (!res.ok) {
            throw new Error('Error ' + res.status + ': ' + res.statusText);
        }

        const order = await res.json();
        CartService.clear();

        const orderId = order.OrderId ?? order.orderId;
        const subTotal = order.SubTotal ?? order.subTotal ?? order.Subtotal;
        const tax = order.Tax ?? order.tax;
        const shipping = order.Shipping ?? order.shipping;
        const total = order.Total ?? order.total;

        alert(
            'Order #' + orderId + ' created successfully!\n' +
            'SubTotal: $' + subTotal.toFixed(2) + '\n' +
            'Tax: $' + tax.toFixed(2) + '\n' +
            'Shipping: $' + shipping.toFixed(2) + '\n' +
            'Total: $' + total.toFixed(2)
        );

        window.location.hash = '#/products';

    } catch (e) {
        console.error(e);
        alert('Error creating order. Please try again.');
    }
};

// ── CartVM Class ─────
export class CartVM {
    constructor() { }

    public init(): void {
        cartObservable = kendo.observable({
            subTotal: '$0.00',
            tax: '$0.00',
            shipping: '$' + SHIPPING.toFixed(2),
            total: '$0.00'
        });

        const container = document.getElementById('cart-container');
        if (container) {
            kendo.bind(container, cartObservable);
        }

        renderCart();
    }
}