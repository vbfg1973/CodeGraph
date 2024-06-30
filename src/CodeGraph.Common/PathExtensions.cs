namespace CodeGraph.Common
{
    public static class PathHelpers
    {
        public static string TrimPath(string? path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            
            return path
                .Trim()
                .TrimEnd('\\')
                .TrimEnd('/');
        }
    }
}