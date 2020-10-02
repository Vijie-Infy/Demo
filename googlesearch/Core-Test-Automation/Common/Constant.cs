using System;
using System.IO;

namespace Core_Test_Automation.Common
{
    /// <summary>
    /// Contains all the constant parameters.
    /// </summary>
    public class Constant
    {
        //Folder locations
        public static string ROOT = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        public static string TESTDATA = ROOT + "/TestData/Test Data.xlsx";
        
        //Wait variables in seconds
        public const int LONGWAIT = 30;
        public const int SHORTWAIT = 2;

        public static implicit operator Constant(int v)
        {
            throw new NotImplementedException();
        }
    }
}
