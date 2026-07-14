public void SetObjectVariables<T>(string prefix, T obj, string tag = null)
{
    foreach (var property in typeof(T).GetProperties())
    {
        tp.SetVariable(
            $"{prefix}{property.Name}",
            property.GetValue(obj)?.ToString() ?? "",
            tag
        );
    }
}