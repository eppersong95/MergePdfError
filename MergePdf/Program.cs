using IronPdf;
using System;
using System.Collections.Generic;
using System.Net;

/*
Garrett Epperson
September 4, 2020
Notes: When we invoice a customer, we usually have a list of documents that we download and attach to the email.
We take each of these URLs, download them, convert them to PdfDocuments, merge them, then attach them.
The issue is that the document pulled from the third URL throws an error when calling new PdfDocument(byte[]).
*/

namespace MergePdf
{
    class Program
    {
        private static string ImageFolder => $"{AppContext.BaseDirectory.Substring(0, AppContext.BaseDirectory.LastIndexOf("bin"))}\\Output";

        private static List<string> Urls
        {
            get
            {
                return new List<string>
                {
                    //This file works
                    "https://priority1.blob.core.windows.net/shipment-images/60100465416-billoflading-637327566945486768.pdf",

                    //This file works
                    "https://prod-1319-images.s3.amazonaws.com/6a1cacf2cf5954141b38be4153d676fe-C.pdf",

                    //This file does not work; uncomment, and program will crash
                    //"https://s3-us-west-2.amazonaws.com/prod-1319-images/36baa61ac470b75ab757c6af040eb5ff-1.pdf",
                };
            }
        }

        static void Main(string[] args)
        {
            var pdfsToMerge = new List<PdfDocument>();

            using (var webClient = new WebClient())
            {
                foreach (var url in Urls)
                {
                    var data = webClient.DownloadData(url);
                    var pdf = new PdfDocument(data);
                    pdfsToMerge.Add(pdf);
                }
            }

            var mergedPdf = PdfDocument.Merge(pdfsToMerge);
            mergedPdf.SaveAs($"{ImageFolder}\\TestMerge.pdf");
        }
    }
}