import {IInputs, IOutputs} from "./generated/ManifestTypes";

export class TSLinearInputComponent implements ComponentFramework.StandardControl<IInputs, IOutputs> {


    // Value of the field is stored and used inside the component
    private _value: number;
// Power Apps component framework delegate which will be assigned to this object which would be called whenever any update happens.
private _notifyOutputChanged: () => void;
// label element created as part of this component
private labelElement: HTMLLabelElement;
// input element that is used to create the range slider
private inputElement: HTMLInputElement;
// reference to the component container HTMLDivElement
// This element contains all elements of our code component example
private _container: HTMLDivElement;
// reference to Power Apps component framework Context object
private _context: ComponentFramework.Context<IInputs>;
// Event Handler 'refreshData' reference
//private _refreshData: EventListenerObject;

    /**
     * Empty constructor.
     */
    constructor()
    {

    }

    /**
     * Used to initialize the control instance. Controls can kick off remote server calls and other initialization actions here.
     * Data-set values are not initialized here, use updateView.
     * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to property names defined in the manifest, as well as utility functions.
     * @param notifyOutputChanged A callback method to alert the framework that the control has new outputs ready to be retrieved asynchronously.
     * @param state A piece of data that persists in one session for a single user. Can be set at any point in a controls life cycle by calling 'setControlState' in the Mode interface.
     * @param container If a control is marked control-type='standard', it will receive an empty div element within which it can render its content.
     */
    public init(
        context: ComponentFramework.Context<IInputs>,
        notifyOutputChanged: () => void,
        state: ComponentFramework.Dictionary,
        container: HTMLDivElement
    ) {
        this._context = context;
        this._container = document.createElement("div");
        this._notifyOutputChanged = notifyOutputChanged;
        //this._refreshData = this.refreshData.bind(this);
// creating HTML elements for the input type range and binding it to the function which refreshes the component data
        this.inputElement = document.createElement("input");
        this.inputElement.setAttribute("type", "range");
        this.inputElement.addEventListener("input", this.refreshData);
        //setting the max and min values for the component.
        this.inputElement.setAttribute("min", "1");
        this.inputElement.setAttribute("max", "1000");
        this.inputElement.setAttribute("class", "linearslider");
        this.inputElement.setAttribute("id", "linearrangeinput");
        // creating a HTML label element that shows the value that is set on the linear range component
        this.labelElement = document.createElement("label");
        this.labelElement.setAttribute("class", "TS_LinearRangeLabel");
        this.labelElement.setAttribute("id", "lrclabel");
        // retrieving the latest value from the component and setting it to the HTML elements.
        this._value = context.parameters.sliderValue.raw
            ? context.parameters.sliderValue.raw
            : 0;
        this.inputElement.value =
            context.parameters.sliderValue.formatted
                ? context.parameters.sliderValue.formatted
                : "0";
        this.labelElement.innerHTML = context.parameters.sliderValue.formatted
            ? context.parameters.sliderValue.formatted
            : "0";
        // appending the HTML elements to the component's HTML container element.
        this._container.appendChild(this.inputElement);
        this._container.appendChild(this.labelElement);
        container.appendChild(this._container);
    }

    /**
* Updates the values to the internal value variable we are storing and also updates the html label that
displays the value
* @param context : The "Input Properties" containing the parameters, component metadata and
interface functions
*/
    public refreshData(evt: Event): void {
        this._value = (this.inputElement.value as any) as number;
        this.labelElement.innerHTML = this.inputElement.value;
        this._notifyOutputChanged();
    }

    /**
     * Called when any value in the property bag has changed. This includes field values, data-sets, global values such as container height and width, offline status, control metadata values such as label, visible, etc.
     * @param context The entire property bag available to control via Context Object; It contains values as set up by the customizer mapped to names defined in the manifest, as well as utility functions
     */
    public updateView(context: ComponentFramework.Context<IInputs>): void {
        // storing the latest context from the control.
        this._value = context.parameters.sliderValue.raw
            ? context.parameters.sliderValue.raw
            : 0;
        this._context = context;
        this.inputElement.value =
            context.parameters.sliderValue.formatted
                ? context.parameters.sliderValue.formatted
                : "";
        this.labelElement.innerHTML = context.parameters.sliderValue.formatted
            ? context.parameters.sliderValue.formatted
            : "";
    }

    /**
     * It is called by the framework prior to a control receiving new data.
     * @returns an object based on nomenclature defined in manifest, expecting object[s] for property marked as “bound” or “output”
     */
    public getOutputs(): IOutputs {
        return {
            sliderValue: this._value
        };
    }

    /**
     * Called when the control is to be removed from the DOM tree. Controls should use this call for cleanup.
     * i.e. cancelling any pending remote calls, removing listeners, etc.
     */
    public destroy() {
        this.inputElement.removeEventListener("input", this.refreshData);
    }

}
