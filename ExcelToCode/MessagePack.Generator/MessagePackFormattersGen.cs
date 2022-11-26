
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExcelToCode.Excel
{
    internal static class MessagePackFormattersGen
    {
        public static async Task RunAsync(string input, string output)
        {
            Workspace? workspace = null;
            try
            {
                Compilation compilation;
                if (Directory.Exists(input))
                {
                    compilation = await PseudoCompilation.CreateFromDirectoryAsync(input, null, CancellationToken.None);
                }
                else
                {
                    LogUtil.AddErrLog($"MessagePackFormattersGen无效的输入目录:{input}");
                    return;
                }

                await new MessagePackCompiler.CodeGenerator(x => Console.WriteLine(x), CancellationToken.None)
                    .GenerateFileAsync(compilation, output, "ConfigDataResolver", "", false, null, null);
            }
            catch (OperationCanceledException)
            {
                await Console.Error.WriteLineAsync("Canceled");
                throw;
            }
            finally
            {
                workspace?.Dispose();
            }
        }
    }
}
