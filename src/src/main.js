import { PLATFORM } from "aurelia-framework";
import "styles/app.scss";
export function configure(aurelia) {
    aurelia.use.standardConfiguration();
    if (__DEBUG__) {
        aurelia.use.developmentLogging();
    }
    aurelia.start()
        .then(() => aurelia.setRoot(PLATFORM.moduleName("app")));
}
//# sourceMappingURL=main.js.map