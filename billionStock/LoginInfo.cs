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
    public partial class 로그인정보_Form : Form
    {
        public 로그인정보_Form(string 보유계좌개수, List<string> 보유계좌_목록, string 사용자아이디, string 사용자이름, string 키보드보안, string 방화벽, string 서버)
        {
            InitializeComponent();
            보유계좌_목록_label.Text = "보유계좌 (" + 보유계좌개수 + ")";
            사용자아이디_textbox.Text = 사용자아이디;
            사용자이름_textbox.Text = 사용자이름;
            foreach (string account in 보유계좌_목록)
            {
                보유계좌_목록_textbox.Text += (account + System.Environment.NewLine);
            }
            if (키보드보안 == "0")
            {
                키보드보안_textbox.Text = "정상";
            } else if (키보드보안 == "1")
            {
                키보드보안_textbox.Text = "비정상";
            } else
            {
                키보드보안_textbox.Text = "알수없음";
            }

            if (방화벽 == "0")
            {
                방화벽_textbox.Text = "미설정";
            } else if (방화벽 == "1")
            {
                방화벽_textbox.Text = "설정";
            } else if (방화벽 == "2")
            {
                방화벽_textbox.Text = "해지";
            } else
            {
                방화벽_textbox.Text = "알수없음";
            }
            if (서버 == "1")
            {
                서버_textbox.Text = "모의투자";
            } else
            {
                서버_textbox.Text = "실서버";
            }
            사용자아이디_textbox.SelectionLength = 0;
            사용자이름_textbox.SelectionLength = 0;
            보유계좌_목록_textbox.SelectionLength = 0;
            키보드보안_textbox.SelectionLength = 0;
            방화벽_textbox.SelectionLength = 0;
            서버_textbox.SelectionLength = 0;
        }

        private void 로그인정보_Form_Load(object sender, EventArgs e)
        {
        }
    }
}