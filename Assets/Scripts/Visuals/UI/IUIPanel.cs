using UnityEngine;

namespace Visuals.UI
{
    public interface IUIPanel
    {
        GameObject Root { get; }
        string OpenSound { get; }
        string CloseSound { get; }
        PanelType PanelType { get; }
    }
}