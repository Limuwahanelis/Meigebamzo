using UnityEngine;

public class GlobalBasicElements : MonoBehaviour
{
    public static BasicElement PhysicalElement
    {
        get
        {
            if (_physicalElement == null)
            {
                _physicalElement = Resources.Load<BasicElement>("Physical");

            }
            return _physicalElement;
        }
    }

    private static BasicElement _physicalElement;
}
