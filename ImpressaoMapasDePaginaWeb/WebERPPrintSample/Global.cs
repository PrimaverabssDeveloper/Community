using System;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Contains my site's global variables.
/// </summary>
public static class Global
{
    /// <summary>
    /// Global variable storing important stuff.
    /// </summary>
    static string _estado;

    /// <summary>
    /// Get or set the static important data.
    /// </summary>
    public static string EstadoImpressao
    {
        get
        {
            return _estado;
        }
        set
        {
            _estado = value;
        }
    }
}