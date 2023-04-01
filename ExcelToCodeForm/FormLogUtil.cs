
using ExcelToCodeCore.Utils;
using NLog;
using System.Text;
using System.Windows.Forms;

public class FormLogUtil : AbstractLogUtil
{

    private static readonly NLog.Logger LOGGER = LogManager.GetCurrentClassLogger();

    private TextBox normalLogTb;

    private TextBox errorLogTb;

    public FormLogUtil(TextBox tb, TextBox errTb)
    {
        normalLogTb = tb;
        errorLogTb = errTb;
    }

    public override void Add(string log, bool isErr = false)
    {
        if (isErr)
        {
            errorLogTb.AppendText(log + "\r\n");
            LOGGER.Error(log);
        }
        else
        {
            normalLogTb.AppendText(log + "\r\n");
            LOGGER.Info(log);
        }
    }
}