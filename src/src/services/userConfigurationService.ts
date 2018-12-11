import { SourceComparer } from "./../models/sourceComparer";
import { HttpClient } from "aurelia-http-client";
import { EventAggregator } from "aurelia-event-aggregator";
import { Package } from "./../models/package";
import { UserConfiguration } from "./../models/userConfiguration";
import { autoinject } from "aurelia-framework";

@autoinject
export class UserConfigurationService {

    storagekey: string = "userconfiguration";
    storage: Storage;
    EventAggregator: EventAggregator;
    HttpClient: HttpClient;
    constructor(eventAggregator: EventAggregator, httpclient: HttpClient) {
        // tslint:disable-next-line:no-string-literal
        this.storage = window["localStorage"];
        this.EventAggregator = eventAggregator;
        this.HttpClient = httpclient;
    }

    public comparePackage(sourceComparer: SourceComparer, p: Package): void {
        p.isFetching = true;
        this.searchReplaceAndSave(sourceComparer, p);
        this.EventAggregator.publish("PackageStartedComparingEvent", p);
        this.HttpClient
            .createRequest("api/comparePackage")
            .asPost()
            .withHeader("Content-Type", "application/json; charset=utf-8")
            .withContent({
                name: p.name,
                sourceA: sourceComparer.sourceA,
                sourceB: sourceComparer.sourceB,
                comparePrerelease: p.comparePrerelease})
            .send()
            .then((response: any) => {
                JSON.parse(response.response, (key, value) => {
                    if (key === "name") {
                        p.name = value;
                    }
                    if (key === "sourceA") {
                        p.sourceAUrl = value;
                    }
                    if (key === "sourceAVersion") {
                        p.sourceAVersion = value;
                    }
                    if (key === "sourceB") {
                        p.sourceBUrl = value;
                    }
                    if (key === "sourceBVersion") {
                        p.sourceBVersion = value;
                    }
                });
                p.fetchDate = new Date(Date.now());
                p.isFetching = false;
                p.isLatest = p.sourceAVersion === p.sourceBVersion;
                p.compareFailedString = "";
                this.searchReplaceAndSave(sourceComparer, p);
                this.EventAggregator.publish("PackageComparedEvent", p);
                if (this.IsAllPackagesCompleted()) {
                    this.EventAggregator.publish("AllPackagesAreComparedEvent", this.getPackages());
                }
            })
            .catch((reason: any) => {
                if (reason !== undefined) {
                    JSON.parse(reason.response, (key, value) => {
                        if (key === "Message") {
                            p.compareFailedString = value;
                        }
                    });
                    p.isFetching = false;
                    this.searchReplaceAndSave(sourceComparer, p);
                    this.EventAggregator.publish("PackageComparedEvent", this.getPackages());
                }
            }
            );
    }
    IsAllPackagesCompleted(): boolean {
        var isAllPackagesCompared: boolean = false;
        var packages: Package[] = this.getPackages();
        isAllPackagesCompared = packages.every(p => !p.isFetching);
        return isAllPackagesCompared;
    }

    private searchReplaceAndSave(sourceComparer: SourceComparer, newPackage: Package): void {
        let oldPackage: Package | undefined = sourceComparer.Packages.find(p => p.name === newPackage.name);
        if (oldPackage != null) {
            const index: number = sourceComparer.Packages.indexOf(oldPackage);
            sourceComparer.Packages.splice(index, 1, newPackage);
        } else {
            sourceComparer.Packages.push(newPackage);
        }

        this.savePackages(sourceComparer);
    }

    public saveHideLatestpackages(newValue: boolean): void {
        var userconfiguration: UserConfiguration | null = this.get();
        if (userconfiguration != null) {
            userconfiguration.HideLatestPackages = newValue;
            this.save(userconfiguration);
        }
    }
    private savePackages(sourceComparer: SourceComparer): void {
        try {
            let userConfiguration: UserConfiguration | null = this.get();
            try {
                if (userConfiguration != null) {
                    let oldComparer: SourceComparer | undefined = userConfiguration.SourceComparers.find(
                        c => c.sourceA === sourceComparer.sourceA
                            && c.sourceB === sourceComparer.sourceB);
                    if (oldComparer != null) {
                        const index: number = userConfiguration.SourceComparers.indexOf(oldComparer);
                        userConfiguration.SourceComparers.splice(index, 1, sourceComparer);
                    } else {
                        userConfiguration.SourceComparers.push(sourceComparer);
                    }
                    this.save(userConfiguration);
                }
            } catch (e) {
                console.log(e);
            }
        }
        finally {
            return;
        }
    }

    public sortPackages(packagesToSort: Package[]): Package[] {
        let isNotLatestPackages: Package[] = packagesToSort.filter(p => !p.isLatest);
        isNotLatestPackages = isNotLatestPackages.sort((a, b) => a.name > b.name ? 1 : -1);

        var userConfiguration: UserConfiguration | null = this.get();
        if (userConfiguration != null) {
            if (userConfiguration.HideLatestPackages) {
                return isNotLatestPackages;
            }
        }

        let isLatestPackages: Package[] = packagesToSort.filter(p => p.isLatest);
        isLatestPackages = isLatestPackages.sort((a, b) => a.name > b.name ? 1 : -1);
        return isNotLatestPackages.concat(isLatestPackages);
    }

    public save(userConfiguration: UserConfiguration): void {
        try {
            this.storage.setItem(this.storagekey, JSON.stringify(userConfiguration));
        } catch (e) {
            console.log(e);
        }
    }

    public get(): UserConfiguration | null {
        let userConfiguration: UserConfiguration | null = null;
        try {
            var rawstring: string | null = this.storage.getItem(this.storagekey);
            if (rawstring != null) {
                userConfiguration = JSON.parse(rawstring);
            }
        } catch (e) {
            console.log(e);
        }
        return userConfiguration;
    }

    getPackages(sourceComparers?: SourceComparer[]): Package[] {
        if (sourceComparers === undefined) {
            let userConfiguration: UserConfiguration | null = this.get();
            if (userConfiguration != null) {
                if (userConfiguration.SourceComparers != null) {
                    sourceComparers = userConfiguration.SourceComparers;
                }
            }
        }
        let packages: Package[] = new Array<Package>();
        if (sourceComparers !== undefined) {
            sourceComparers.forEach(comparer => {
                comparer.Packages.forEach(p => {
                    packages.push(p);
                });
            });
        }
        return packages;
    }

    public getPackagesForComparer(sourceComparer: SourceComparer): Package[] {
        let packages: Package[] = new Array<Package>();
        sourceComparer.Packages.forEach(p => {
            packages.push(p);
        });
        return packages;
    }
}