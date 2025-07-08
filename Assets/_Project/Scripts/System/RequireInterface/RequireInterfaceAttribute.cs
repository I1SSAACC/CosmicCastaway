using UnityEngine;

public class RequireInterfaceAttribute : PropertyAttribute
{
    public System.Type RequiredType { get; }

    public RequireInterfaceAttribute(System.Type requiredType)
    {
        RequiredType = requiredType;
    }
}