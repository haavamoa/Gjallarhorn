import * as tslib_1 from "tslib";
import { HttpClient } from "aurelia-http-client";
import { EventAggregator } from "aurelia-event-aggregator";
import { autoinject } from "aurelia-framework";
let UserConfigurationService = class UserConfigurationService {
    constructor(eventAggregator, httpclient) {
        this.storagekey = "userconfiguration";
        this.storage = window.localStorage;
        this.EventAggregator = eventAggregator;
        this.HttpClient = httpclient;
    }
    comparePackage(feedComparer, p) {
        p.isFetching = true;
        this.searchReplaceAndSave(feedComparer, p);
        this.EventAggregator.publish("PackageStartedComparingEvent", this.getPackages());
        this.HttpClient
            .createRequest("api/comparePackage")
            .asPost()
            .withHeader("Content-Type", "application/json; charset=utf-8")
            .withContent({ name: p.name, url: feedComparer.sourceA, compareUrl: feedComparer.sourceB })
            .send()
            .then((response) => {
            p = JSON.parse(response.response);
            p.fetchDate = new Date(Date.now());
            p.isFetching = false;
            this.searchReplaceAndSave(feedComparer, p);
            this.EventAggregator.publish("PackageComparedEvent", this.getPackages());
        })
            .catch((reason) => {
            p.compareFailedString = reason.message;
            p.isFetching = false;
            this.searchReplaceAndSave(feedComparer, p);
            this.EventAggregator.publish("PackageComparedEvent", this.getPackages());
        });
    }
    searchReplaceAndSave(feedComparer, newPackage) {
        let oldPackage = feedComparer.Packages.find(p => p.name === newPackage.name);
        if (oldPackage != null) {
            const index = feedComparer.Packages.indexOf(oldPackage);
            feedComparer.Packages.splice(index, 1, newPackage);
        }
        else {
            feedComparer.Packages.push(newPackage);
        }
        feedComparer.Packages = this.sortPackagesOnLatest(feedComparer.Packages);
        this.savePackages(feedComparer);
    }
    savePackages(feedComparer) {
        try {
            let userConfiguration = this.get();
            try {
                if (userConfiguration != null) {
                    let oldComparer = userConfiguration.SourceComparers.find(c => c.sourceA === feedComparer.sourceA
                        && c.sourceB === feedComparer.sourceB);
                    if (oldComparer != null) {
                        const index = userConfiguration.SourceComparers.indexOf(oldComparer);
                        userConfiguration.SourceComparers.splice(index, 1, feedComparer);
                    }
                    else {
                        userConfiguration.SourceComparers.push(feedComparer);
                    }
                    this.save(userConfiguration);
                }
            }
            catch (e) {
                console.log(e);
            }
        }
        finally {
            return;
        }
    }
    sortPackagesOnLatest(packagesToSort) {
        let isNotLatestPackages = packagesToSort.filter(p => p.version !== p.compareVersion);
        var latestPackages = packagesToSort.filter(p => p.version === p.compareVersion);
        return isNotLatestPackages.concat(latestPackages);
    }
    save(userConfiguration) {
        try {
            this.storage.setItem(this.storagekey, JSON.stringify(userConfiguration));
        }
        catch (e) {
            console.log(e);
        }
    }
    get() {
        let userConfiguration = null;
        try {
            var rawstring = this.storage.getItem(this.storagekey);
            if (rawstring != null) {
                userConfiguration = JSON.parse(rawstring);
            }
        }
        catch (e) {
            console.log(e);
        }
        return userConfiguration;
    }
    getPackages() {
        let packages = new Array();
        let userConfiguration = this.get();
        if (userConfiguration != null) {
            if (userConfiguration.SourceComparers != null) {
                userConfiguration.SourceComparers.forEach(comparer => {
                    comparer.Packages.forEach(p => {
                        packages.push(p);
                    });
                });
            }
        }
        return this.sortPackagesOnLatest(packages);
    }
};
UserConfigurationService = tslib_1.__decorate([
    autoinject,
    tslib_1.__metadata("design:paramtypes", [EventAggregator, HttpClient])
], UserConfigurationService);
export { UserConfigurationService };
//# sourceMappingURL=userConfigurationService.js.map