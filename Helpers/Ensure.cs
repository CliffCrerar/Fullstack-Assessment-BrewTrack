using System.Diagnostics;
namespace BrewTrack.Helpers;
public static class Ensure
{
    /// <summary>
    /// Ensures that the specified argument is not null.
    /// </summary>
    /// <param name="argumentName">Name of the argument.</param>
    [DebuggerStepThrough]
    public static T ArgumentNotNull<T>(T argument)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(nameof(T));
        }
        return argument;
    }
}

