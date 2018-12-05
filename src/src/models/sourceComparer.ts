import { Package } from "./package";

export class SourceComparer {
    sourceA: string;
    sourceB: string;
    Packages: Package[];
    constructor(sourceA: string, sourceB: string, packages: Package[]) {
        this.sourceA = sourceA;
        this.sourceB = sourceB;
        this.Packages = packages;
    }

}