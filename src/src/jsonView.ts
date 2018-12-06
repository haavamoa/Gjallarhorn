import { SourceComparer } from "./models/sourceComparer";
import { UserConfigurationService } from "./services/userConfigurationService";
import { Package } from "./models/package";
import { autoinject, observable } from "aurelia-framework";
import { EventAggregator } from "aurelia-event-aggregator";
import { UserConfiguration } from "./models/userConfiguration";

@autoinject
export class JsonView {
    @observable jsonString: string = "";
    errorMessage: any;
    EventAggregator: EventAggregator;
    UserConfigurationService: UserConfigurationService;
    jsonExampleString: any;
    UserConfiguration!: UserConfiguration;
    hasChanges: boolean = false;

    constructor(EventAggregator: EventAggregator, userConfigurationService: UserConfigurationService) {
        this.EventAggregator = EventAggregator;
        this.UserConfigurationService = userConfigurationService;
    }

    initialize(): void {
        this.jsonString = JSON.stringify(this.UserConfigurationService.get()
            , ["SourceComparers", "sourceA", "sourceB", "Packages", "name"], 4);
        this.jsonExampleString = this.CreateExampleJson();
    }

    jsonStringChanged(newValue: any, oldValue: any): void {
        try {
            this.UserConfiguration = JSON.parse(newValue);
            this.errorMessage = undefined;
            this.hasChanges = true;
        } catch (e) {
            this.errorMessage = e;
        }
    }

    CreateExampleJson(): string {
        return JSON.stringify(
            new UserConfiguration(
                new Array<SourceComparer>(new SourceComparer(
                    "https://api.nuget.org/v3/"
                    , "https://api.nuget.org/v3/"
                    , new Array<Package>(new Package("LightInject"))))
            )
            , undefined, 4);
    }

    goBack(): void {
        this.EventAggregator.publish("GoToNormalViewEvent");
    }

    save(): void {
        if (this.UserConfiguration !== undefined) {
            try {
                this.UserConfigurationService.save(this.UserConfiguration);
                this.hasChanges = false;
                this.EventAggregator.publish("PackagesToCompareChanged", this.UserConfiguration.SourceComparers);
            } catch (e) {
                this.errorMessage = e;
            }
        }
    }
}