﻿using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;

namespace ChurnR.Core.Reporter;

public class ChartJsReporter(TextWriter output, IProcessor cutOffProcessor) : BaseReporter(output, cutOffProcessor)
{
    protected override void WriteImpl(IEnumerable<FileStatistics> fileChurns)
    {
        labelsString = "\"" + string.Join("\",\"", fileChurns.Select(x => x.FileName.Trim('"'))) + "\"";
        churnsString = string.Join(",", fileChurns.Select(x => x.CommitCount));

        Out.Write(HtmlTemplate);
    }

    private static string labelsString = "";
    private static string churnsString = "";
    
    private string HtmlTemplate => @"
<!DOCTYPE html>
<html lang=""en"">
   <body>
      <div>
         <canvas id=""ChurnR""></canvas>
      </div>
      <script src=""https://cdn.jsdelivr.net/npm/chart.js""></script>
      <script>
         const ctx = document.getElementById('ChurnR');
         
         const labels = ["+labelsString+@"];
         
         const data = {
         labels: labels,
         datasets: [{
           label: 'ChurnR',
           data: ["+churnsString+@"],
           fill: false,
           borderColor: 'rgb(75, 192, 192)',
           tension: 0.1
         }]
         };
         
         new Chart(ctx, {
           type: 'line',
           data: data
         });
      </script>
   </body>
</html>
";

}
