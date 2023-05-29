using System.Diagnostics;
using System.Diagnostics.Contracts;
namespace BrewTrack.Helpers;
public static class Ensure
{
    /// <summary>
    /// Ensures that the specified argument is not null.
    /// </summary>
    /// <param name="argumentName">Name of the argument.</param>
    /// <param name="argument">The argument.</param>
    [DebuggerStepThrough]
    public static T ArgumentNotNull<T>(T argument, string argumentName)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(argumentName);
        }
        return argument;
    }
}

