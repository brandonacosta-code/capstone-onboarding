
declare var kendo: any;

(window as any).renderViewLink = (id: number) => {
    return `<a href="#/product/${id}" > View </a>`
}


export class GridVM {
    public isVisible = true;
    public products: any;
    public search: string = "";

    constructor() {
        this.products = new kendo.data.DataSource({
            schema: {
                model: {
                    id: 'Id',
                    fields: {
                        Id: { editable: false, nullable: true },
                        Name: { type: 'string' },
                        UnitPrice: { type: 'number' }
                    }
                }
            },
        });
        this.load();
    }


    private async load() {
        const searchValue = (document.getElementById("searchInput") as HTMLInputElement)?.value || "";

        const url = `https://localhost:44325/api/product?search=${encodeURIComponent(searchValue)}`;



        try {
            const res = await fetch(url);

            if (!res.ok) {
                throw new Error(`Error ${res.status}: ${res.statusText}`);
            }

            const data = await res.json();
            this.products.data(data);
        } catch (e) {
            console.error(e);
            this.products.data([]);
        }
    }

    private _timeout: any;

    public searchProducts = () => {
        const input = document.getElementById("searchInput") as HTMLInputElement;

        if (!input) return;

        if (input.value.length > 30) {
            alert("Search cannot excedd 30 characters");
            return;
        }

        clearTimeout(this._timeout);
        this._timeout = setTimeout(() => this.load(), 500);
    }

    sortProducts(): void {
        const fieldEl = document.getElementById("sortBySelect") as HTMLSelectElement;
        const dirEl = document.getElementById("sortDirectionSelect") as HTMLSelectElement;

        if (!fieldEl || !dirEl || !fieldEl.value) return;

        this.products.sort({ field: fieldEl.value, dir: dirEl.value });
    }

}