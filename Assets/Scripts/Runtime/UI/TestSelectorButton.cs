using UnityEngine;

public class TestSelectorButton : BaseUIButton
{
    public override void OnHovered() { }

    public override void OnHoverExit() { }

    public override void OnSelected()
    {
        Debug.Log("Click!");
    }
}
