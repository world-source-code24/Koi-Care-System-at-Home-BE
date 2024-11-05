using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public static class EnumExtension
{
    public static string GetDisplayName(this Enum enumValue)
    {
        // Get the field info for the enum value
        var field = enumValue.GetType().GetField(enumValue.ToString());

        // Get the Display attribute if it exists
        var displayAttribute = field?.GetCustomAttribute<DisplayAttribute>();

        // Return the Display name if it exists, otherwise use the enum's default name
        return displayAttribute != null ? displayAttribute.Name : enumValue.ToString();
    }
}
