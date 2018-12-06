import { UserConfigurationService } from "./services/userConfigurationService";
import { Package } from "./models/package";
import { autoinject } from "aurelia-framework";

@autoinject
export class NormalView {
    packages!: Array<Package>;
    UserConfigurationService: UserConfigurationService;
    constructor(UserConfigurationService:UserConfigurationService) {
        this.UserConfigurationService = UserConfigurationService;
    }
    public onPackagesChanged(packages: Package[]): void {

        if (this.packages !== undefined) {
            packages.forEach(p => {
                this.searchAndReplace(p);
            }
            );
        } else {
            this.packages = packages;
        }
    }

    public onAllPackagesFinishedComparing():void {
        this.packages = this.UserConfigurationService.sortPackagesOnLatest(this.packages);
    }

    private searchAndReplace(newPackage: Package): void {
        let oldPackage: Package | undefined = this.packages.find(p => p.name === newPackage.name);
        if (oldPackage != null) {
            const index: number = this.packages.indexOf(oldPackage);
            this.packages.splice(index, 1, newPackage);
        } else {
            this.packages.push(newPackage);
        }

        this.packages = this.UserConfigurationService.sortPackagesOnLatest(this.packages);
    }
}