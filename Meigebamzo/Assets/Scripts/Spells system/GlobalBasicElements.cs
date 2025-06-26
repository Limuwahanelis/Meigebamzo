using UnityEngine;

public class GlobalBasicElements : MonoBehaviour
{
    public static ElementalSpell PhysicalElement
    {
        get
        {
            if (_physicalElement == null)
            {
                _physicalElement = Resources.Load<ElementalSpell>("Physical");

            }
            return _physicalElement;
        }
    }

    private static ElementalSpell _physicalElement;
}
