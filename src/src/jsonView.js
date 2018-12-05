import * as tslib_1 from "tslib";
import { SourceComparer } from "./models/sourceComparer";
import { UserConfigurationService } from "./services/userConfigurationService";
import { Package } from "./models/package";
import { autoinject } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";
import { UserConfiguration } from "./models/userConfiguration";
let JsonView = class JsonView {
    constructor(EventAggregator, userConfigurationService) {
        this.jsonString = "";
        this.EventAggregator = EventAggregator;
        this.UserConfigurationService = userConfigurationService;
    }
    initialize() {
        this.jsonString = JSON.stringify(this.UserConfigurationService.get(), ["FeedComparers", "feedA", "feedB", "Packages", "name"], 4);
        this.jsonExampleString = this.CreateExampleJson();
    }
    CreateExampleJson() {
        return JSON.stringify(new UserConfiguration(new Array(new SourceComparer("https://api.nuget.org/v3/", "https://api.nuget.org/v3/", new Array(new Package("LightInject"))))), undefined, 4);
    }
    saveJson() {
        if (this.jsonString !== "") {
            try {
                let userConfiguration = JSON.parse(this.jsonString);
                this.EventAggregator.publish("PackagesToCompareChanged", userConfiguration.SourceComparers);
            }
            catch (e) {
                this.errorMessage = e;
            }
        }
        else {
            this.EventAggregator.publish("PackagesAreEmptyEvent");
        }
    }
};
JsonView = tslib_1.__decorate([
    autoinject,
    tslib_1.__metadata("design:paramtypes", [EventAggregator, UserConfigurationService])
], JsonView);
export { JsonView };
//# sourceMappingURL=jsonView.js.map