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
        let updatedPackages: Package[] = this.getPackagesForComparer(sourceComparer);
        this.EventAggregator.publish("PackageStartedComparingEvent", updatedPackages);
        this.HttpClient
            .createRequest("api/comparePackage")
            .asPost()
            .withHeader("Content-Type", "application/json; charset=utf-8")
            .withContent({ name: p.name, url: sourceComparer.sourceA, compareUrl: sourceComparer.sourceB })
            .send()
            .then((response: any) => {
                JSON.parse(response.response, (key, value) => {
                    if (key === "name") {
                        p.name = value;
                    }
                    if (key === "url") {
                        p.sourceAUrl = value;
                    }
                    if (key === "version") {
                        p.sourceAVersion = value;
                    }
                    if (key === "compareUrl") {
                        p.sourceBUrl = value;
                    }
                    if (key === "compareVersion") {
                        p.sourceBVersion = value;
                    }
                });
                p.fetchDate = new Date(Date.now());
                p.isFetching = false;
                this.searchReplaceAndSave(sourceComparer, p);
                let updatedPackages: Package[] = this.getPackagesForComparer(sourceComparer);
                this.EventAggregator.publish("PackageComparedEvent", updatedPackages);
            })
            .catch((reason: any) => {
                if (reason !== undefined) {
                    p.compareFailedString = reason;
                    p.isFetching = false;
                    this.searchReplaceAndSave(sourceComparer, p);
                    this.EventAggregator.publish("PackageComparedEvent", this.getPackages());
                }
            }
            );
    }

    private searchReplaceAndSave(sourceComparer: SourceComparer, newPackage: Package): void {
        let oldPackage: Package | undefined = sourceComparer.Packages.find(p => p.name === newPackage.name);
        if (oldPackage != null) {
            const index: number = sourceComparer.Packages.indexOf(oldPackage);
            sourceComparer.Packages.splice(index, 1, newPackage);
        } else {
            sourceComparer.Packages.push(newPackage);
        }

        sourceComparer.Packages = this.sortPackagesOnLatest(sourceComparer.Packages);
        this.savePackages(sourceComparer);
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

    public sortPackagesOnLatest(packagesToSort: Package[]): Package[] {
        let isNotLatestPackages: Package[] = packagesToSort.filter(p => p.sourceAVersion !== p.sourceBVersion);
        var latestPackages: Package[] = packagesToSort.filter(p => p.sourceAVersion === p.sourceBVersion);
        return isNotLatestPackages.concat(latestPackages);
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
            let packages: Package[] = new Array<Package>();
            if (sourceComparers !== undefined) {
                sourceComparers.forEach(comparer => {
                    comparer.Packages.forEach(p => {
                        packages.push(p);
                    });
                });
            }
            return this.sortPackagesOnLatest(packages);
        } else {
            return new Array<Package>();
        }
    }

    public getPackagesForComparer(sourceComparer: SourceComparer): Package[] {
        let packages: Package[] = new Array<Package>();
        sourceComparer.Packages.forEach(p => {
            packages.push(p);
        });
        return this.sortPackagesOnLatest(packages);
    }
}