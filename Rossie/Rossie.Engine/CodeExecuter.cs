using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Policy;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using System.Security;
using System.Threading;

namespace Rossie.Engine
{
    public sealed class ByteCodeLoader : MarshalByRefObject
    {
        public ByteCodeLoader()
        {
        }

        public object Run(byte[] compiledAssembly)
        {
            var assembly = Assembly.Load(compiledAssembly);
            assembly.EntryPoint.Invoke(null, new object[] { });
            var result = assembly.GetType("EntryPoint").GetProperty("Result").GetValue(null, null);

            return result;
        }
    }

    public class CodeExecuter
    {
        private static AppDomain CreateSandbox()
        {
            var e = new Evidence();
            e.AddHostEvidence(new Zone(SecurityZone.Internet));

            var ps = SecurityManager.GetStandardSandbox(e);
            var security = new SecurityPermission(SecurityPermissionFlag.Execution);

            ps.AddPermission(security);

            var setup = new AppDomainSetup { ApplicationBase = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) };
            return AppDomain.CreateDomain("Sandbox", null, setup, ps);
        }
        public object Execute(string code)
        {
            var sandbox = CreateSandbox();

            const string entryPoint =
                "using System.Reflection; public class EntryPoint { public static object Result {get;set;} public static void Main() { Result = Script.Eval(); } }";
            var script = "public static object Eval() {" + code + "}";

            var core = sandbox.Load("System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            var system = sandbox.Load("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");

            var compilation = Compilation.Create("foo", new CompilationOptions(assemblyKind: AssemblyKind.ConsoleApplication,
                                        usings: ReadOnlyArray<string>.CreateFrom(
                                            new[] { 
                                    "System", 
                                    "System.IO", 
                                    "System.Net", 
                                    "System.Linq", 
                                    "System.Text", 
                                    "System.Text.RegularExpressions", 
                                    "System.Collections.Generic" })),
                    new[]
        {
            SyntaxTree.ParseCompilationUnit(entryPoint),
            SyntaxTree.ParseCompilationUnit(script, options: new ParseOptions(kind: SourceCodeKind.Interactive))
        },
                    new MetadataReference[] { 
            new AssemblyFileReference(typeof(object).Assembly.Location),
            new AssemblyFileReference(core.Location), 
            new AssemblyFileReference(system.Location)
        });

            byte[] compiledAssembly;
            using (var output = new MemoryStream())
            {
                var emitResult = compilation.Emit(output);

                if (!emitResult.Success)
                {
                    var errors = emitResult.Diagnostics.Select(x => x.Info.GetMessage().Replace("Eval()", "<Factory>()").ToString()).ToArray();

                    return string.Join(", ", errors);
                }

                compiledAssembly = output.ToArray();
            }

            if (compiledAssembly.Length == 0) return "Incorrect data";

            var loader = (ByteCodeLoader)Activator.CreateInstance(sandbox, typeof(ByteCodeLoader).Assembly.FullName, typeof(ByteCodeLoader).FullName).Unwrap();

            object result = null;
            try
            {
                var scriptThread = new Thread(() =>
                {
                    try
                    {
                        result = loader.Run(compiledAssembly);
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                });

                scriptThread.Start();

                if (!scriptThread.Join(6000))
                {
                    scriptThread.Abort();
                    AppDomain.Unload(sandbox);
                }
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }

            AppDomain.Unload(sandbox);

            if (result == null || string.IsNullOrEmpty(result.ToString())) result = "null";

            return result;
        }
    }
}
