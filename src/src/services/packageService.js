import * as tslib_1 from "tslib";
import { EventAggregator } from 'aurelia-event-aggregator';
import { HttpClient } from 'aurelia-http-client';
import { autoinject } from 'aurelia-framework';
let PackageService = class PackageService {
    constructor(httpClient, EventAggregator) {
        this.HttpClient = httpClient;
        this.baseUrl = "api/comparePackage";
        this.storage = window["localStorage"];
        this.EventAggregator = EventAggregator;
        let packagesJsonString = this.storage.getItem("packages");
        if (packagesJsonString != null) {
            this.packages = JSON.parse(packagesJsonString);
        }
        else {
            this.packages = new Array();
        }
    }
    comparePackage(p) {
        p.validate();
        p.isFetching = true;
        this.EventAggregator.publish("PackageStartedComparingEvent", p);
        this.serachAndReplace(p);
        this.HttpClient
            .createRequest('api/comparePackage')
            .asPost()
            .withHeader('Content-Type', 'application/json; charset=utf-8')
            .withContent(JSON.stringify(p))
            .send()
            .then((response) => {
            p = JSON.parse(response.response);
            p.fetchDate = new Date(Date.now());
            p.isFetching = false;
            this.serachAndReplace(p);
            this.EventAggregator.publish("PackageComparedEvent", p);
        })
            .catch((reason) => {
            p.compareFailedString = reason.message;
            p.isFetching = false;
        });
    }
    savePackages(packages) {
        this.storage.setItem("packages", JSON.stringify(packages));
        this.packages = packages;
    }
    getPackages() {
        return this.packages;
    }
    serachAndReplace(newPackage) {
        var oldPackage = this.packages.find(p => p.name == newPackage.name && p.url == newPackage.url && p.compareUrl == newPackage.compareUrl);
        if (oldPackage != null) {
            const index = this.packages.indexOf(oldPackage);
            this.packages.splice(index, 1, newPackage);
        }
        else {
            this.packages.push(newPackage);
        }
        var isNotLatestPackages = this.packages.filter(p => p.version != p.compareVersion);
        var latestPackages = this.packages.filter(p => p.version == p.compareVersion);
        this.packages = isNotLatestPackages.concat(latestPackages);
        this.savePackages(this.packages);
    }
};
PackageService = tslib_1.__decorate([
    autoinject,
    tslib_1.__metadata("design:paramtypes", [HttpClient, EventAggregator])
], PackageService);
export { PackageService };
//# sourceMappingURL=packageService.js.map