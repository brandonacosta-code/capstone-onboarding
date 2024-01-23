
export class GridVM {
    public isVisible = true;
    public products: kendo.data.DataSource;

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
        const data = await fetch('https://localhost:44325/api/product')
            .then((res) => res.json())
            .catch(e => {
                console.error(e);
                return [];
            });
        this.products.data(data);
    }

}