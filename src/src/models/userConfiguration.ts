import { SourceComparer } from "./sourceComparer";

export class UserConfiguration {
    SourceComparers: SourceComparer[];
    HideLatestPackages:boolean;
    constructor(sourceComparer:SourceComparer[],hideLatestPackages:boolean) {
        this.SourceComparers = sourceComparer;
        this.HideLatestPackages = hideLatestPackages;
    }

}