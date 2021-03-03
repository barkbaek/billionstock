using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace billionStock
{
    public partial class 주식잔고_Form : Form
    {
        private static AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;
        
        private DataGridViewCellStyle _레드셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _그린셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _블루셀스타일 = new DataGridViewCellStyle();

        private DataGridViewCellStyle _강한레드셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _강한그린셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _강한블루셀스타일 = new DataGridViewCellStyle();

        private DataGridViewCellStyle _약한레드셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _약한그린셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _약한블루셀스타일 = new DataGridViewCellStyle();

        private DataGridViewCellStyle _블랙셀스타일 = new DataGridViewCellStyle();

        public 주식잔고_Form()
        {
            InitializeComponent();
            _레드셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            _그린셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            _블루셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            _레드셀스타일.ForeColor = Color.White;
            _그린셀스타일.ForeColor = Color.Black;
            _블루셀스타일.ForeColor = Color.White;

            _강한레드셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            _강한그린셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            _강한블루셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
            _강한레드셀스타일.ForeColor = Color.White;
            _강한그린셀스타일.ForeColor = Color.White;
            _강한블루셀스타일.ForeColor = Color.White;

            _약한레드셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            _약한그린셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            _약한블루셀스타일.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            _약한레드셀스타일.ForeColor = Color.Black;
            _약한그린셀스타일.ForeColor = Color.Black;
            _약한블루셀스타일.ForeColor = Color.Black;

            _블랙셀스타일.BackColor = Color.Black;
            _블랙셀스타일.ForeColor = Color.White;
        }

        private void 주식잔고_Form_Load(object sender, EventArgs e)
        {

        }

        public void 시작하자(AxKHOpenAPILib.AxKHOpenAPI _axKHOpenAPI1)
        {
            axKHOpenAPI1 = _axKHOpenAPI1;
            보유계좌_목록_가져와줘();
            Console.WriteLine("주식잔고 - axKHOpenAPI1 저장됨.");
        }

        private void 주식잔고조회_button_Click(object sender, EventArgs e)
        {
            주식잔고_dataGridView.Rows.Clear();

            string 선택된계좌번호 = 계좌번호_comboBox.Text;
            axKHOpenAPI1.SetInputValue("계좌번호", 선택된계좌번호);
            axKHOpenAPI1.SetInputValue("비밀번호", "");
            axKHOpenAPI1.SetInputValue("비밀번호입력매체구분", "00");
            axKHOpenAPI1.SetInputValue("조회구분", "2");

            int 결과코드 = axKHOpenAPI1.CommRqData("계좌평가잔고내역", "opw00018", 0, "0018");
        }

        public void 보유계좌_목록_가져와줘()
        {
            List<string> 보유계좌_목록 = 목록_만들어줘(axKHOpenAPI1.GetLoginInfo("ACCLIST"));
            this.Invoke(new Action(delegate () {
                foreach (string 보유계좌 in 보유계좌_목록)
                {
                    계좌번호_comboBox.Items.Add(보유계좌);
                }
                계좌번호_comboBox.SelectedItem = 보유계좌_목록[0];
            }));
        }

        private void 주식잔고_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (주식잔고_dataGridView.CurrentRow.Selected)
            {
                int 선택된행 = 주식잔고_dataGridView.CurrentCell.RowIndex;
                string 종목코드_문자 = 주식잔고_dataGridView.Rows[선택된행].Cells[0].Value.ToString();
                int 종목코드 = Int32.Parse(종목코드_문자.Substring(1, (종목코드_문자.Length - 1)));
                Console.WriteLine("선택된 종목코드 = " + 종목코드);
            }
        }

        public List<string> 목록_만들어줘(string 코드)
        {
            List<string> 목록 = 코드.Split(';').ToList();
            if (목록.Count <= 0)
            {
                return 목록;
            }
            int 마지막인덱스 = 목록.Count - 1;
            if (목록[마지막인덱스] == "")
            {
                목록.RemoveAt(목록.Count - 1);
            }
            return 목록;
        }

        private Color 글자색_알려줘(double 값)
        {
            if (값 > 0)
            {
                return System.Drawing.Color.Firebrick;
            }
            else if (값 < 0)
            {
                return System.Drawing.Color.RoyalBlue;
            }
            else
            {
                return System.Drawing.Color.Chartreuse;
            }
        }

        private int 날짜_알려줘()
        {
            DateTime date = DateTime.Now;
            int 연도 = date.Year;
            int 월 = date.Month;
            int 일 = date.Day;
            string 날짜 = String.Format("{0}{1}{2}", 연도, 월 < 10 ? ("0" + 월) : ("" + 월), (일 < 10 ? ("0" + 일) : ("" + 일)));
            return Int32.Parse(날짜);
        }

        private int 시간_알려줘()
        {
            DateTime date = DateTime.Now;
            int 시 = date.Hour;
            int 분 = date.Minute;
            int 초 = date.Second;
            string 시간 = String.Format("{0}{1}{2}", 시, (분 < 10 ? ("0" + 분) : ("" + 분)), (초 < 10 ? ("0" + 초) : ("" + 초)));
            return Int32.Parse(시간);
        }

        public void 평가손익금액_label_ChangeForeColor (Color _color)
        {
            평가손익금액_label.ForeColor = _color;
        }
        public void 수익률_label_ChangeForeColor(Color _color)
        {
            수익률_label.ForeColor = _color;
        }
        public void 매입금액_label_ChangeText(String _string)
        {
            매입금액_label.Text = _string;
        }
        public void 평가금액_label_ChangeText(String _string)
        {
            평가금액_label.Text = _string;
        }
        public void 평가손익금액_label_ChangeText(String _string)
        {
            평가손익금액_label.Text = _string;
        }
        public void 수익률_label_ChangeText(String _string)
        {
            수익률_label.Text = _string;
        }
        public void 추정예탁자산_label_ChangeText(String _string)
        {
            추정예탁자산_label.Text = _string;
        }
        public void 대출금_label_ChangeText(String _string)
        {
            대출금_label.Text = _string;
        }
        public void 융자금액_label_ChangeText(String _string)
        {
            융자금액_label.Text = _string;
        }
        public void 대주금액_label_ChangeText(String _string)
        {
            대주금액_label.Text = _string;
        }
        public void 주식잔고_업데이트해줘 (
                            String 종목코드_임시
                            , String 종목명_임시
                            , String 평가손익_임시
                            , String 수익률_임시
                            , String 매입가_임시
                            , String 전일종가_임시
                            , String 보유수량_임시
                            , String 매매가능수량_임시
                            , String 현재가_임시
                            , String 전일매수수량_임시
                            , String 전일매도수량_임시
                            , String 금일매수수량_임시
                            , String 금일매도수량_임시
                            , String 매입금액_임시
                            , String 매입수수료_임시
                            , String 평가금액_임시
                            , String 평가수수료_임시
                            , String 세금_임시
                            , String 수수료합_임시
                            , String 보유비중_임시
                            , String 신용구분_임시
                            , String 신용구분명_임시
                            , String 대출일_임시
                            , int ㄱ)
        {
            주식잔고_dataGridView.Rows.Add();

            string 종목코드 = 종목코드_임시.Length == 6 ? 종목코드_임시 : 종목코드_임시.Substring(종목코드_임시.Length - 6, 6);
            string 종목명 = 종목명_임시;
            long 평가손익 = long.Parse(평가손익_임시);
            double 수익률 = double.Parse(수익률_임시);
            int 매입가 = Int32.Parse(매입가_임시);
            int 전일종가 = Int32.Parse(전일종가_임시);
            long 보유수량 = long.Parse(보유수량_임시);
            long 매매가능수량 = long.Parse(매매가능수량_임시);
            int 현재가 = Int32.Parse(현재가_임시);
            long 전일매수수량 = long.Parse(전일매수수량_임시);
            long 전일매도수량 = long.Parse(전일매도수량_임시);
            long 금일매수수량 = long.Parse(금일매수수량_임시);
            long 금일매도수량 = long.Parse(금일매도수량_임시);
            long 매입금액 = long.Parse(매입금액_임시);
            long 매입수수료 = long.Parse(매입수수료_임시);
            long 평가금액 = long.Parse(평가금액_임시);
            long 평가수수료 = long.Parse(평가수수료_임시);
            long 세금 = long.Parse(세금_임시);
            long 수수료합 = long.Parse(수수료합_임시);
            double 보유비중 = double.Parse(보유비중_임시);
            int 신용구분 = Int32.Parse(신용구분_임시);
            string 신용구분명 = 신용구분명_임시;
            string 대출일 = 대출일_임시;

            주식잔고_dataGridView["종목코드_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = 종목코드;

            주식잔고_dataGridView["종목명_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = 종목명;

            주식잔고_dataGridView["평가손익_주식잔고_DataGridViewTextBoxColumn", ㄱ].Style = 평가손익 > 0 ? _레드셀스타일 : ( 평가손익 < 0 ? _블루셀스타일 : _블랙셀스타일 );
            주식잔고_dataGridView["평가손익_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 평가손익);
            
            주식잔고_dataGridView["수익률_주식잔고_DataGridViewTextBoxColumn", ㄱ].Style = 수익률 > 0 ? _레드셀스타일 : (수익률 < 0 ? _블루셀스타일 : _블랙셀스타일);
            주식잔고_dataGridView["수익률_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:0.00}", 수익률);
            
            주식잔고_dataGridView["매입가_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매입가);
            
            주식잔고_dataGridView["전일종가_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 전일종가);
            
            주식잔고_dataGridView["보유수량_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 보유수량);
            
            주식잔고_dataGridView["매매가능수량_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매매가능수량);
            
            주식잔고_dataGridView["현재가_주식잔고_DataGridViewTextBoxColumn", ㄱ].Style = 현재가 > 매입가 ? _레드셀스타일 : (현재가 < 매입가 ? _블루셀스타일 : _블랙셀스타일);
            주식잔고_dataGridView["현재가_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 현재가);
            
            주식잔고_dataGridView["전일매수수량_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 전일매수수량);
            
            주식잔고_dataGridView["전일매도수량_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 전일매도수량);
            
            주식잔고_dataGridView["금일매수수량_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 금일매수수량);
            
            주식잔고_dataGridView["금일매도수량_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 금일매도수량);
            
            주식잔고_dataGridView["매입금액_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매입금액);
            
            주식잔고_dataGridView["매입수수료_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 매입수수료);
            
            주식잔고_dataGridView["평가금액_주식잔고_DataGridViewTextBoxColumn", ㄱ].Style = 평가금액 > 매입금액 ? _레드셀스타일 : (평가금액 < 매입금액 ? _블루셀스타일 : _블랙셀스타일);
            주식잔고_dataGridView["평가금액_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 평가금액);
            
            주식잔고_dataGridView["평가수수료_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 평가수수료);
            
            주식잔고_dataGridView["세금_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 세금);

            주식잔고_dataGridView["수수료합_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 수수료합);

            주식잔고_dataGridView["보유비중_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:0.00}", 보유비중);

            주식잔고_dataGridView["신용구분_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,0}", 신용구분);
            
            주식잔고_dataGridView["신용구분명_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = 신용구분명;
            
            주식잔고_dataGridView["대출일_주식잔고_DataGridViewTextBoxColumn", ㄱ].Value = 대출일;
            //주식체결_dataGridView["전일대비_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;

            /*
            주식잔고_dataGridView.Rows.Add(
                종목번호
                , 종목명_임시
                , 평가손익_임시
                , 수익률_임시
                , 매입가_임시
                , 전일종가_임시
                , 보유수량_임시
                , 매매가능수량_임시
                , 현재가_임시
                , 전일매수수량_임시
                , 전일매도수량_임시
                , 금일매수수량_임시
                , 금일매도수량_임시
                , 매입금액_임시
                , 매입수수료_임시
                , 평가금액_임시
                , 평가수수료_임시
                , 세금_임시
                , 수수료합_임시
                , 보유비중_임시
                , 신용구분_임시
                , 신용구분명_임시
                , 대출일_임시
                );
            //주식잔고_dataGridView.Rows[ㄱ].Frozen = true;
            */
        }
    }
}
