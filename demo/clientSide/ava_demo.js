function project_onLoad(executionContext)
{
    var formContext = executionContext.getFormContext(); // get formContext
    console.assert(formContext !== null);

	// Only on Create
    console.log("-----------AVA Form Type: " + formContext.ui.getFormType());
	if (formContext.ui.getFormType() == 1)
	{
		// Default Start Date to today 
		formContext.getAttribute("ava_startdate").setValue(new Date());
	}

	// If Phase == Complete we want to disable all ava_phase controls
	if (formContext.getAttribute("ava_phase").getValue() == 915240002)
	{
		formContext.getAttribute("ava_phase").controls.forEach(function (control, index)
		{
			control.setDisabled(true);
		});

		// Fire the onChange event for ava_phase. This will run through all associated handlers
		// This is here because we don't want to bother hiding controls that are already hidden
		formContext.getAttribute("ava_phase").fireOnChange();
	}

}

function phase_onChange(executionContext)
{
    // this works in browser
    window.alert("Phase value changed fired, won't work.");

    // this works in power apps shell 
    Xrm.Navigation.openAlertDialog("Phase change fired");

    var formContext = executionContext.getFormContext(); // get formContext
	var globalContext = Xrm.Utility.getGlobalContext();


	// If Phase == Complete
	if (formContext.getAttribute("ava_phase").getValue() == 915240002)
	{
		// Show Reviewer Tab
		formContext.ui.tabs.get("tab_2").setVisible(true);

		// If there is no reviewer set it to the current user
		if (formContext.getAttribute("ava_reviewerid").getValue() == null)
		{
			formContext.getAttribute("ava_reviewerid").setValue([{ id: globalContext.userSettings.userId, name: globalContext.userSettings.userName	, entityType: "systemuser"}])
		}

		// Expand the Reviewer Tab
		formContext.ui.tabs.get("tab_2").setDisplayState("expanded");
	}
	else
	{
		formContext.ui.tabs.get("tab_2").setVisible(false);

		// Null out all attributes within the Reviewer Tab
		nullAttributesInTab(executionContext, "tab_2");
	}
}
