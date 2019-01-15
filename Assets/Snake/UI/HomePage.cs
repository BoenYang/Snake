using CGF.Core;

public class HomePage : UIPage {

    public void OnPveClick()
    {
        ModuleManager.Instance.GetModule("PVEModule").Show();
    }
}
