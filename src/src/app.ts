import { SourceComparer } from "./models/sourceComparer";
import { UserConfigurationService } from "./services/userConfigurationService";
import { UserConfiguration } from "./models/userConfiguration";
import { JsonView } from "./jsonView";
import { NormalView } from "./normalView";
import { inject, autoinject } from "aurelia-framework";
import { Package } from "./models/package";
import { EventAggregator, Subscription } from "aurelia-event-aggregator";

@autoinject
export class App {

    normalView: NormalView;
    jsonView: JsonView;
    currentView: any;
    normalViewIsVisible: boolean = true;
    jsonViewIsVisible: boolean = false;
    EventAggregator: EventAggregator;
    packagesChangedSubscriber!: Subscription;
    UserConfigurationService: UserConfigurationService;
    isAnyPackagesComparing: boolean = false;

    constructor(normalView: NormalView, jsonView: JsonView, eventaggregator: EventAggregator
        , userConfigurationService: UserConfigurationService) {
        this.normalView = normalView;
        this.jsonView = jsonView;
        this.EventAggregator = eventaggregator;
        this.UserConfigurationService = userConfigurationService;

        if (this.EventAggregator !== undefined) {
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackageComparedEvent"
                , (updatedPackages: Package[]) => {
                    this.onPackageCompared(updatedPackages);
                });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackageStartedComparingEvent"
                , (updatedPackages: Package[]) => {
                    this.onPackageStartedComparing(updatedPackages);
                    this.isAnyPackagesComparing = updatedPackages.some(p => p.isFetching);
                });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackagesAreEmptyEvent", () => {
                this.onPackagesAreEmptyEvent();
            });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("GoToNormalViewEvent", () => {
                this.goToNormalView();
            });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackagesToCompareChanged", (comparers: SourceComparer[]) => {
                this.onPackagesToCompareChanged(comparers);
            });

        }
    }


    private onPackageCompared(updatedPackages: Package[]): void {
        this.isAnyPackagesComparing = updatedPackages.some(p => p.isFetching);
        this.normalView.onPackagesChanged(updatedPackages);
        if(!this.isAnyPackagesComparing) {
            this.normalView.onAllPackagesFinishedComparing();
        }
        this.goToNormalView();
    }

    private onPackageStartedComparing(updatedPackages: Package[]): void {
        this.normalView.onPackagesChanged(updatedPackages);
        this.goToNormalView();
    }

    private onPackagesAreEmptyEvent(): void {
        this.normalView.onPackagesChanged(new Array<Package>());
        this.goToNormalView();
    }

    private onPackagesToCompareChanged(comparers: SourceComparer[]): void {
        this.comparePackages(comparers);
    }

    activate(): void {
        let userConfiguration: UserConfiguration | null = this.UserConfigurationService.get();
        if (userConfiguration != null) {
            this.comparePackages(userConfiguration.SourceComparers);
        }
        this.goToNormalView();
    }

    goToNormalView(): void {
        if (this.jsonView.hasChanges) {
            this.jsonView.save();
        }
        this.currentView = this.normalView;
        this.normalViewIsVisible = true;
        this.jsonViewIsVisible = false;
    }

    goToJsonView(): void {
        this.jsonView.initialize();
        this.currentView = this.jsonView;
        this.jsonViewIsVisible = true;
        this.normalViewIsVisible = false;
    }

    detached(): void {
        this.packagesChangedSubscriber.dispose();
    }

    comparePackages(comparers: SourceComparer[]): void {
            comparers.forEach(comparer => {
                comparer.Packages.forEach(p => {
                    this.UserConfigurationService.comparePackage(comparer, p);
                });
            });
    }
}