namespace CodeGraph.Api.Controllers.FileSystem
{
    public static class PathHelpers
    {
        public static string TrimPath(string path)
        {
            return path
                .Trim()
                .TrimEnd('\\')
                .TrimEnd('/');
        }
    }
}