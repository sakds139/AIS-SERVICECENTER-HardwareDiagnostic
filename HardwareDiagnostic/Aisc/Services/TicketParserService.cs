using System.Text.RegularExpressions;
using HardwareDiagnostic.Aisc.Models;

namespace HardwareDiagnostic.Aisc.Services;

public class TicketParserService
{
    public TicketRequest Parse(string text)
    {
        text = CleanText(text);

        return new TicketRequest
        {
            TicketNo = GetValue(
                text,
                @"SD\d+"),

            EmployeeId = GetValue(
                text,
                @"รหัสพนักงาน\s*(\d+)"),

            EmployeeName = GetValue(
                text,
                @"ชื่อ-นามสกุล\s*(.*?)\s*บริษัท"),

            ComId = GetValue(
                text,
                @"ComID\s*([A-Z0-9]+)"),

            Reason = ExtractReason(text),

            RawText = text
        };
    }

    private static string CleanText(string text)
    {
        text = text.Replace("&nbsp;", " ");

        text = Regex.Replace(
            text,
            @"^\d+\s*$",
            "",
            RegexOptions.Multiline);

        text = Regex.Replace(
            text,
            @"\r\n|\r|\n",
            Environment.NewLine);

        return text;
    }

    private static string GetValue(
        string text,
        string pattern)
    {
        var match =
            Regex.Match(
                text,
                pattern,
                RegexOptions.IgnoreCase |
                RegexOptions.Singleline);

        if (!match.Success)
            return "";

        if (match.Groups.Count > 1)
            return match.Groups[1].Value.Trim();

        return match.Value.Trim();
    }

    private static string ExtractReason(string text)
    {
        var match =
            Regex.Match(
                text,
                @"เหตุผล\s*(.*)",
                RegexOptions.IgnoreCase |
                RegexOptions.Singleline);

        if (!match.Success)
            return text;

        return match.Groups[1].Value.Trim();
    }
}