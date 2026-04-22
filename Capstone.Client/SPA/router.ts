import { KendoRouter, KendoRoutes } from "./0-kendo-wrapper/index.js";

const routes: KendoRoutes = [
    { path: '/', component: 'controllers/home.controller.js', tile: 'Home' },
    { path: '/products', component: 'controllers/grid.controller.js', tile: 'Grid' },
    { path: '/product/:id', component: 'controllers/product.controller.js', tile: 'Product' },
    { path: '/cart', component: 'controllers/cart.controller.js', tile: 'Cart' },
];

export const RouterInstance = new KendoRouter({
    routes,
    outputSelector: '#router-outlet',
    controllerBasePath: '/Content/SPA/'
});