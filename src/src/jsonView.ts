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
    UserConfiguration!: UserConfiguration;
    hasChanges: boolean = false;
    jsonFilter: string[];
    numberOfJsonIndents: number;

    constructor(EventAggregator: EventAggregator, userConfigurationService: UserConfigurationService) {
        this.EventAggregator = EventAggregator;
        this.UserConfigurationService = userConfigurationService;
        this.jsonFilter = ["SourceComparers", "sourceA", "sourceB", "Packages", "name", "comparePrerelease"];
        this.numberOfJsonIndents = 4;
    }

    initialize(): void {
        var userConfiguration : UserConfiguration | null = this.UserConfigurationService.get();
        if(userConfiguration == null) {
            userConfiguration = new UserConfiguration(
                new Array<SourceComparer>(new SourceComparer(
                    "https://api.nuget.org/v3/"
                    , "https://api.nuget.org/v3/"
                    , new Array<Package>(new Package("LightInject")))),false
            );
        }
        this.jsonString = JSON.stringify(
            userConfiguration, this.jsonFilter, this.numberOfJsonIndents);
    }

    jsonStringChanged(newValue: any, oldValue: any): void {
        try {
            var newUserConfiguration : UserConfiguration = JSON.parse(newValue);
            this.saveNonJsonEditorValues(newUserConfiguration);
            this.UserConfiguration = newUserConfiguration;
            this.errorMessage = undefined;
            this.hasChanges = true;
        } catch (e) {
            this.errorMessage = e;
        }
    }

    private saveNonJsonEditorValues(newUserConfiguration: UserConfiguration): void {
        var oldUserConfiguration: UserConfiguration | null = this.UserConfigurationService.get();
        if (oldUserConfiguration != null) {
            newUserConfiguration.HideLatestPackages = oldUserConfiguration.HideLatestPackages;
        }
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