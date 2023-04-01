
using ExcelToCodeCore.Utils;
public class ConsoleLogUtil : AbstractLogUtil
{

    public override void Add(string log, bool isErr = false)
    {
        if (isErr)
        {
            Console.Error.WriteLine(log);
        }
        else
        {
            Console.WriteLine(log);
        }
    }
}