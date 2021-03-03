using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace billionStock
{
    static class Program
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            try
            {
                Application.SetCompatibleTextRenderingDefault(false);
                빌리언스탁_Form 빌리언스탁 = new 빌리언스탁_Form();
                빌리언스탁.빌리언스탁_저장해줘(빌리언스탁);
                Application.Run(빌리언스탁);
            } catch (Exception EX)
            {
                Console.WriteLine("Program.cs - Exception: {0}\r\n", EX);
            }
            
        }
    }
}
