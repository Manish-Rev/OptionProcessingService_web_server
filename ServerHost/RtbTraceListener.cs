using System;
using System.Diagnostics;
using System.Windows.Forms;

class RtbTraceListener : TraceListener
{
    private RichTextBox output;
    delegate void AppendTextHandler(string text);

    public RtbTraceListener(RichTextBox output)
    {
        this.output = output;
    }

    public override void Write(string message)
    {
        if (output.InvokeRequired)
        {
            output.Invoke(new AppendTextHandler(output.AppendText), new object[] { message });
        }
        else
        {
            output.AppendText(message);
        }
    }

    public override void WriteLine(string message)
    {
        if (output.InvokeRequired)
        {
            output.Invoke(new AppendTextHandler(output.AppendText), new object[] { message + "\n" });
        }
        else
        {
            output.AppendText(message + "\n");
        }
    }
}
