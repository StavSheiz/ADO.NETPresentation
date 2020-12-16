using ADO.NETPresentation_DataAccessExamples.Examples;
using System;

namespace ADO.NETPresentation_DataAccessExamples
{
    class Program
    {
        static void Main()
        {
            int exampleId = 1;

            Action[] examples = new Action[] {
            ConnectedExamples.BasicReaderQuery_1,
            ConnectedExamples.ReaderQueryWithParam_2,
            ConnectedExamples.BasicScalarQuery_3,
            ConnectedExamples.BasicNonQuery_4,
            ConnectedExamples.NonQueryWithParams_5,
            ConnectedExamples.BasicReaderQueryStoredPorcedure_6,
            ConnectedExamples.NonQueryStoredProcedure_7,
            ConnectedExamples.NonQueryTransaction_8,
            DisconnectedExamples.DisconnectedArchitecture_9
        };

            string menu =
                @"1. Basic reaedr query
2. Reader query with params
3. Basic scalar query
4. Basic nonQuery
5. NonQuery with params
6. Basic reader stored procedure
7. NonQuery stored procedure
8. NonQuery insert transactions
9. Disconnected fill and update

0. Exit
";

            Console.WriteLine("Choose Example:");
            Console.WriteLine(menu);
            exampleId = Convert.ToInt32(Console.ReadLine());

            while (exampleId > 0)
            {

                if (exampleId <= examples.Length)
                {
                    examples[exampleId - 1].Invoke();
                }
                else
                {
                    Console.WriteLine("No exmaple with that id");
                }

                Console.WriteLine();
                Console.WriteLine("Choose Example:");
                Console.WriteLine(menu);
                exampleId = Convert.ToInt32(Console.ReadLine());
            }

        }
    }
}
