using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace billionStock
{
    public partial class LoginInfo : Form
    {
        /*
            string ACCOUNT_CNT = axKHOpenAPI1.GetLoginInfo("ACCOUNT_CNT");
            List<string> ACCLIST = parseStringToList(axKHOpenAPI1.GetLoginInfo("ACCLIST"));
            string USER_ID = axKHOpenAPI1.GetLoginInfo("USER_ID");
            string USER_NAME = axKHOpenAPI1.GetLoginInfo("USER_NAME");
            string KEY_BSECGB = axKHOpenAPI1.GetLoginInfo("KEY_BSECGB");
            string FIREW_SECGB = axKHOpenAPI1.GetLoginInfo("FIREW_SECGB");
            string GetServerGubun = axKHOpenAPI1.GetLoginInfo("GetServerGubun");
         */
        public LoginInfo(string ACCOUNT_CNT, List<string> ACCLIST, string USER_ID, string USER_NAME, string KEY_BSECGB, string FIREW_SECGB, string GetServerGubun)
        {
            InitializeComponent();
            labelAccountList.Text = "보유계좌 (" + ACCOUNT_CNT + ")";
            userId.Text = USER_ID;
            userName.Text = USER_NAME;
            foreach (string account in ACCLIST)
            {
                accountList.Text += (account + System.Environment.NewLine);
            }
            if (KEY_BSECGB == "0")
            {
                keyboardSecurity.Text = "정상";
            } else if (KEY_BSECGB == "1")
            {
                keyboardSecurity.Text = "비정상";
            } else
            {
                keyboardSecurity.Text = "알수없음";
            }

            if (FIREW_SECGB == "0")
            {
                firewall.Text = "미설정";
            } else if (FIREW_SECGB == "1")
            {
                firewall.Text = "설정";
            } else if (FIREW_SECGB == "2")
            {
                firewall.Text = "해지";
            } else
            {
                firewall.Text = "알수없음";
            }
            if (GetServerGubun == "1")
            {
                server.Text = "모의투자";
            } else
            {
                server.Text = "실서버";
            }
            userId.SelectionLength = 0;
            userName.SelectionLength = 0;
            accountList.SelectionLength = 0;
            keyboardSecurity.SelectionLength = 0;
            firewall.SelectionLength = 0;
            server.SelectionLength = 0;
        }

        private void LoginInfo_Load(object sender, EventArgs e)
        {
        }
    }
}