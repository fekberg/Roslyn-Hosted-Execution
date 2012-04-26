using System;
using System.Diagnostics;
using System.ServiceModel;
using Roslyn.Scripting.CSharp;
using Rossie.Engine;

namespace Rossie.Windows.Service
{
[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
internal class CommandService : ICommandService
{

    public string Execute(string code)
    {
        var engine = new CodeExecuter();
        try
        {
            var unformatted = engine.Execute(code);
                
            return FormatResult(unformatted);
        }
        catch (Exception ex)
        {
            EventLog.WriteEntry("RossieEngineService", ex.ToString(), EventLogEntryType.Error);    
        }

        return string.Empty;
    }

    private static string FormatResult(object input)
    {
        try
        {
            var formatter = new ObjectFormatter(maxLineLength: 350);
            var result = formatter.FormatObject(input);

            if (string.IsNullOrEmpty(result)) return "null";

            result = result.Replace(Environment.NewLine, " ").Replace("\n", " ").Replace("\r", " ");

            if (result.Length > 350) result = result.Substring(0, 350);

            return result;
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }
}
}
