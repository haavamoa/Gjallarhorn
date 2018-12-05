import * as tslib_1 from "tslib";
import { UserConfigurationService } from "./services/userConfigurationService";
import { JsonView } from "./jsonView";
import { NormalView } from "./normalView";
import { autoinject } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";
let App = class App {
    constructor(normalView, jsonView, eventaggregator, userConfigurationService) {
        this.normalViewIsVisible = true;
        this.jsonViewIsVisible = false;
        this.normalView = normalView;
        this.jsonView = jsonView;
        this.EventAggregator = eventaggregator;
        this.UserConfigurationService = userConfigurationService;
        if (this.EventAggregator !== undefined) {
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackageComparedEvent", (updatedPackages) => {
                this.onPackageCompared(updatedPackages);
            });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackageStartedComparingEvent", (updatedPackages) => {
                this.onPackageStartedComparing(updatedPackages);
            });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackagesAreEmptyEvent", () => {
                this.onPackagesAreEmptyEvent();
            });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("GoToNormalViewEvent", () => {
                this.goToNormalView();
            });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackagesToCompareChanged", (comparers) => {
                this.onPackagesToCompareChanged(comparers);
            });
        }
    }
    onPackageCompared(updatedPackages) {
        this.normalView.onPackagesChanged(updatedPackages);
        this.goToNormalView();
    }
    onPackageStartedComparing(updatedPackages) {
        this.normalView.onPackagesChanged(updatedPackages);
        this.goToNormalView();
    }
    onPackagesAreEmptyEvent() {
        this.goToNormalView();
    }
    onPackagesToCompareChanged(comparers) {
        this.comparePackages(comparers);
        this.goToNormalView();
    }
    activate() {
        let userConfiguration = this.UserConfigurationService.get();
        if (userConfiguration != null) {
            this.comparePackages(userConfiguration.SourceComparers);
        }
        this.goToNormalView();
    }
    goToNormalView() {
        this.currentView = this.normalView;
        this.normalViewIsVisible = true;
        this.jsonViewIsVisible = false;
    }
    goToJsonView() {
        this.jsonView.initialize();
        this.currentView = this.jsonView;
        this.jsonViewIsVisible = true;
        this.normalViewIsVisible = false;
    }
    detached() {
        this.packagesChangedSubscriber.dispose();
    }
    comparePackages(comparers) {
        try {
            comparers.forEach(comparer => {
                comparer.Packages.forEach(p => {
                    this.UserConfigurationService.comparePackage(comparer, p);
                });
            });
        }
        catch (e) {
            this.goToJsonView();
            return;
        }
    }
};
App = tslib_1.__decorate([
    autoinject,
    tslib_1.__metadata("design:paramtypes", [NormalView, JsonView, EventAggregator,
        UserConfigurationService])
], App);
export { App };
//# sourceMappingURL=app.js.map