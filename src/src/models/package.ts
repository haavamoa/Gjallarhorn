export class Package {
    isFetching!: boolean;
    compareFailedString!: string;
    name: string;
    fetchDate!: Date;
    sourceAVersion!: string;
    sourceBVersion!: string;
    sourceAUrl!:string;
    sourceBUrl!:string;
    isLatest!:boolean;
    comparePrerelease!:boolean;

    constructor(name:string) {
        this.name = name;
    }
}