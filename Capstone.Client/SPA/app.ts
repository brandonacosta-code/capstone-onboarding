import '/Content/lib/jquery.min.js'
import '/Content/lib/kendo.all.min.js'

import { AppController } from './controllers/app.controller.js';
import { KendoViewBuilder } from './0-kendo-wrapper/index.js';
import { RouterInstance } from "./router.js";

const appElement = document.getElementById("app");
const main = await KendoViewBuilder.build(new AppController());

RouterInstance.$currentPath.subscribe(() => {
	const outlet = document.querySelector('#router-outlet');
	if (outlet) outlet.innerHTML = "";
});

RouterInstance.start(main.view);
main.view.render(appElement);
