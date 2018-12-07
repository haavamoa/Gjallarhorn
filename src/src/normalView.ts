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

    initialize(packages: Package[]): void {
        this.packages = packages;
    }

    public onPackagesChanged(comparedPackage:Package): void {
        if (this.packages !== undefined) {
            this.packages.forEach(p => {
                this.searchAndReplace(comparedPackage);
            }
            );
        }
    }

    public onAllPackagesFinishedComparing(packages : Package[]):void {
        this.packages = this.UserConfigurationService.sortPackages(this.packages);
    }
    cleanPackages(newPackages: Package[]): void {
        this.packages.forEach(oldpackage => {
            var oldPackagesIsGone:boolean = newPackages.some(newpackage => !this.Equals(newpackage, oldpackage));
            if(oldPackagesIsGone) {
                const index : number = this.packages.indexOf(oldpackage);
                this.packages.splice(index);
            }
        });
    }

    sortPackages(): void {
        this.packages = this.UserConfigurationService.getPackages();
        if(this.packages !== undefined) {
            this.packages = this.UserConfigurationService.sortPackages(this.packages);
        }
    }

    public Equals(packageA:Package, packageB:Package): boolean {
        return packageA.name === packageB.name &&
        packageA.sourceAUrl === packageB.sourceAUrl && packageA.sourceBUrl === packageB.sourceBUrl;
    }

    private searchAndReplace(newPackage: Package): void {
        let oldPackage: Package | undefined = this.packages.find(p => p.name === newPackage.name);
        if (oldPackage != null) {
            const index: number = this.packages.indexOf(oldPackage);
            this.packages.splice(index, 1, newPackage);
        } else {
            this.packages.push(newPackage);
        }

        this.packages = this.UserConfigurationService.sortPackages(this.packages);
    }
}