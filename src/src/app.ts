import { SourceComparer } from "./models/sourceComparer";
import { UserConfigurationService } from "./services/userConfigurationService";
import { UserConfiguration } from "./models/userConfiguration";
import { JsonView } from "./jsonView";
import { NormalView } from "./normalView";
import { inject, autoinject, observable } from "aurelia-framework";
import { Package } from "./models/package";
import { EventAggregator, Subscription } from "aurelia-event-aggregator";

@autoinject
export class App {

    @observable showLatestPackages!:boolean;
    normalView: NormalView;
    jsonView: JsonView;
    currentView: any;
    normalViewIsVisible: boolean = true;
    jsonViewIsVisible: boolean = false;
    EventAggregator: EventAggregator;
    packagesChangedSubscriber!: Subscription;
    UserConfigurationService: UserConfigurationService;
    isAnyPackagesComparing: boolean = false;
    progressPercentage: number = 0;
    progressPercentageWidth!:string;
    minimized: boolean = false;


    constructor(normalView: NormalView, jsonView: JsonView, eventaggregator: EventAggregator
        , userConfigurationService: UserConfigurationService) {
        this.normalView = normalView;
        this.jsonView = jsonView;
        this.EventAggregator = eventaggregator;
        this.UserConfigurationService = userConfigurationService;
        if (this.EventAggregator !== undefined) {
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackageComparedEvent"
                , (comparedPackage:Package) => {
                    this.onPackageCompared(comparedPackage);
                });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackageStartedComparingEvent"
                , (updatedPackage:Package) => {
                    this.isAnyPackagesComparing = true;
                    this.onPackageStartedComparing(updatedPackage);
                });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("GoToNormalViewEvent", () => {
                this.goToNormalView();
            });
            this.packagesChangedSubscriber = this.EventAggregator.subscribe("PackagesToCompareChanged", (comparers: SourceComparer[]) => {
                this.onPackagesToCompareChanged(comparers);
            });

            this.packagesChangedSubscriber = this.EventAggregator.subscribe("AllPackagesAreComparedEvent", (updatedPackages: Package[]) => {
                this.onAllPackagesAreCompared(updatedPackages);
            });
        }
    }

    showLatestPackagesChanged(newValue: any, oldValue: any): void {
        this.UserConfigurationService.saveHideLatestpackages(!newValue);
        this.normalView.sortPackages();
    }

    toggleshowLatestPackages(): void {
        this.showLatestPackages = !this.showLatestPackages;
    }

    private onPackageCompared(comparedPackage:Package): void {
        this.normalView.onPackagesChanged(comparedPackage);
        this.goToNormalView();
        this.setProgressPercentage(this.normalView.packages);
    }

    private setProgressPercentage(allPackages:Package[]): void {
        var packagesThatAreComparing:Package[] =  allPackages.filter(p => !p.isFetching);
        this.progressPercentage = packagesThatAreComparing.length / allPackages.length * 100;
        this.progressPercentage = Math.round(this.progressPercentage * Math.pow(10, 0)) / Math.pow(10, 0);
        this.progressPercentageWidth = "width : " + this.progressPercentage + "%";
    }

    private onPackageStartedComparing(updatedPackage:Package): void {
        this.normalView.onPackagesChanged(updatedPackage);
        this.goToNormalView();
    }

    private onPackagesToCompareChanged(comparers: SourceComparer[]): void {
        var packages:Package[] = this.UserConfigurationService.getPackages(comparers);
        this.normalView.initialize(packages);
        this.comparePackages(comparers);
        this.setProgressPercentage(this.normalView.packages);
    }
    onAllPackagesAreCompared(updatedPackages: Package[]): any {
        this.isAnyPackagesComparing = false;
        this.normalView.onAllPackagesFinishedComparing(updatedPackages);
    }

    activate(): void {
        let userConfiguration: UserConfiguration | null = this.UserConfigurationService.get();


        if (userConfiguration != null) {
            this.showLatestPackages = !userConfiguration.HideLatestPackages;
            this.normalView.initialize(this.UserConfigurationService.getPackages());
            this.showLatestPackages = !userConfiguration.HideLatestPackages;
            this.comparePackages(userConfiguration.SourceComparers);
            this.goToNormalView();
        }

        var minimized:string|null = new URL(location.href).searchParams.get("minimized");
        this.minimized = minimized != null && minimized === "true";
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