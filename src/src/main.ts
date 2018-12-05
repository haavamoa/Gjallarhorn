import { Aurelia, PLATFORM } from "aurelia-framework";

import "styles/app.scss";

declare const __DEBUG__: boolean; // this is defined by Webpack build

export function configure(aurelia: Aurelia):void {
    aurelia.use.standardConfiguration();

    if (__DEBUG__) {
        aurelia.use.developmentLogging();
    }

    aurelia.start()
        .then(() => aurelia.setRoot(PLATFORM.moduleName("app")));
}
