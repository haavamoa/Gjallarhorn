import { SourceComparer } from "./sourceComparer";

export class UserConfiguration {
    SourceComparers: SourceComparer[];
    constructor(sourceComparer:SourceComparer[]) {
        this.SourceComparers = sourceComparer;
    }

}