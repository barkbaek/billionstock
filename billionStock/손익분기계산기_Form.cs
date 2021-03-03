using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace billionStock
{
    public partial class 손익분기계산기_Form : Form
    {
        // private double 주식수수료율 = 1.0037869;
        private double 주식수수료율 = 1.0033;

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

        private DataGridViewCellStyle _코스피셀스타일 = new DataGridViewCellStyle();
        private DataGridViewCellStyle _코스닥셀스타일 = new DataGridViewCellStyle();

        private SoundPlayer _사운드플레이어;

        public 손익분기계산기_Form()
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

            _코스피셀스타일.BackColor = Color.Black;
            _코스피셀스타일.ForeColor = Color.Gold;

            _코스닥셀스타일.BackColor = Color.Black;
            _코스닥셀스타일.ForeColor = Color.DarkOrange;

            시장_comboBox.SelectedItem = "코스닥";
        }

        public void 효과음_들려줘(string 명령어)
        {
            string 효과음 = "button.wav";
            if (명령어 == "수량복사")
            {
                효과음 = "button.wav";
            }
            _사운드플레이어 = new SoundPlayer(효과음);
            _사운드플레이어.Play();
        }
        
        public void 체결가_변경해줘 (int 체결가)
        {
            체결가_numericUpDown.Value = 체결가;
        }
        public void 매수금액_변경해줘(int 매수금액)
        {
            매수금액_numericUpDown.Value = 매수금액;
        }

        public void 계산해줘()
        {
            try
            {
                int 매수금액 = Convert.ToInt32(매수금액_numericUpDown.Value) * 10000;
                int 체결가 = Convert.ToInt32(체결가_numericUpDown.Value);
                int 시장 = 시장_comboBox.Text == "코스피" ? 0 : 1;
                int 틱간격;
                double 손익분기가 = (double)체결가 * 주식수수료율;
                int 손익분기호가 = (int)Math.Ceiling(손익분기가);
                int 호가단위 = 호가단위_알려줘(체결가, 시장);
                if (손익분기호가 % 호가단위 != 0)
                {
                    손익분기호가 = 손익분기호가 - (손익분기호가 % 호가단위) + 호가단위;
                }

                수량결과_label.Text = String.Format("{0}", Convert.ToInt32(Math.Truncate((double)매수금액 / (double)체결가)));

                손익분기가결과_label.Text = String.Format("{0:0.00}", 손익분기가);
                호가단위결과_label.Text = String.Format("{0:#,#}", 호가단위);
                손익분기호가결과_label.Text = String.Format("{0:#,#}", 손익분기호가);

                손절_dataGridView.Rows.Clear();
                손절_dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //or even better .DisableResizing. Most time consumption enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders                
                손절_dataGridView.RowHeadersVisible = false;// set it to false if not needed

                익절_dataGridView.Rows.Clear();
                익절_dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //or even better .DisableResizing. Most time consumption enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders                
                익절_dataGridView.RowHeadersVisible = false;// set it to false if not needed

                for (int 손절률 = -1; 손절률 >= -30; 손절률--)
                {
                    손절_dataGridView.Rows.Add();

                    double 곱할수 = 1 + (손절률 * 0.01);
                    int 손절호가 = (int)Math.Ceiling(손익분기가 * 곱할수);
                    int 인덱스 = ((손절률 * -1) - 1);
                    호가단위 = 호가단위_알려줘(손절호가, 시장);
                    손절호가 = 손절호가 - (손절호가 % 호가단위) + 호가단위;
                    if (손절률 >= -2)
                    {
                        손절_dataGridView["손절률_DataGridViewTextBoxColumn", 인덱스].Style = _약한블루셀스타일;
                        손절_dataGridView["손절틱간격_DataGridViewTextBoxColumn", 인덱스].Style = _약한블루셀스타일;
                        손절_dataGridView["손절호가_DataGridViewTextBoxColumn", 인덱스].Style = _약한블루셀스타일;
                    }
                    else if (손절률 < -2 && 손절률 >= -15)
                    {
                        손절_dataGridView["손절률_DataGridViewTextBoxColumn", 인덱스].Style = _블루셀스타일;
                        손절_dataGridView["손절틱간격_DataGridViewTextBoxColumn", 인덱스].Style = _블루셀스타일;
                        손절_dataGridView["손절호가_DataGridViewTextBoxColumn", 인덱스].Style = _블루셀스타일;
                    }
                    else
                    {
                        손절_dataGridView["손절률_DataGridViewTextBoxColumn", 인덱스].Style = _강한블루셀스타일;
                        손절_dataGridView["손절틱간격_DataGridViewTextBoxColumn", 인덱스].Style = _강한블루셀스타일;
                        손절_dataGridView["손절호가_DataGridViewTextBoxColumn", 인덱스].Style = _강한블루셀스타일;
                    }
                    틱간격 = 틱간격_알려줘(손익분기호가, 손절호가, 시장);
                    손절_dataGridView["손절률_DataGridViewTextBoxColumn", 인덱스].Value = String.Format("{0}%", 손절률);
                    손절_dataGridView["손절틱간격_DataGridViewTextBoxColumn", 인덱스].Value = String.Format("{0:#,#}", 틱간격);
                    손절_dataGridView["손절호가_DataGridViewTextBoxColumn", 인덱스].Value = String.Format("{0:#,#}", 손절호가);
                }

                for (int 익절률 = 1; 익절률 <= 30; 익절률++)
                {
                    익절_dataGridView.Rows.Add();

                    double 곱할수 = 1 + (익절률 * 0.01);
                    int 익절호가 = (int)Math.Ceiling(손익분기가 * 곱할수);
                    int 인덱스 = 익절률 - 1;
                    호가단위 = 호가단위_알려줘(익절호가, 시장);
                    익절호가 = 익절호가 - (익절호가 % 호가단위) + 호가단위;
                    if (익절률 <= 2)
                    {
                        익절_dataGridView["익절률_DataGridViewTextBoxColumn", 인덱스].Style = _약한레드셀스타일;
                        익절_dataGridView["익절틱간격_DataGridViewTextBoxColumn", 인덱스].Style = _약한레드셀스타일;
                        익절_dataGridView["익절호가_DataGridViewTextBoxColumn", 인덱스].Style = _약한레드셀스타일;
                    }
                    else if (익절률 > 2 && 익절률 <= 15)
                    {
                        익절_dataGridView["익절률_DataGridViewTextBoxColumn", 인덱스].Style = _레드셀스타일;
                        익절_dataGridView["익절틱간격_DataGridViewTextBoxColumn", 인덱스].Style = _레드셀스타일;
                        익절_dataGridView["익절호가_DataGridViewTextBoxColumn", 인덱스].Style = _레드셀스타일;
                    }
                    else
                    {
                        익절_dataGridView["익절률_DataGridViewTextBoxColumn", 인덱스].Style = _강한레드셀스타일;
                        익절_dataGridView["익절틱간격_DataGridViewTextBoxColumn", 인덱스].Style = _강한레드셀스타일;
                        익절_dataGridView["익절호가_DataGridViewTextBoxColumn", 인덱스].Style = _강한레드셀스타일;
                    }
                    틱간격 = 틱간격_알려줘(손익분기호가, 익절호가, 시장);
                    익절_dataGridView["익절률_DataGridViewTextBoxColumn", 인덱스].Value = String.Format("+{0}%", 익절률);
                    익절_dataGridView["익절틱간격_DataGridViewTextBoxColumn", 인덱스].Value = String.Format("{0:#,#}", 틱간격);
                    익절_dataGridView["익절호가_DataGridViewTextBoxColumn", 인덱스].Value = String.Format("{0:#,#}", 익절호가);
                }
            } catch (Exception EX)
            {
                Console.WriteLine("손익분기계산기 = 계산해줘 오류 = EX: {0}", EX );
            }
        }

        private void 계산_button_Click(object sender, EventArgs e)
        {
            계산해줘();
        }

        private void 손절_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (손절_dataGridView.CurrentRow.Selected)
            {
                int 선택된행인덱스 = 손절_dataGridView.CurrentCell.RowIndex;
                DataGridViewRow 선택된행 = 손절_dataGridView.Rows[선택된행인덱스];
                string 손절호가 = Convert.ToString(선택된행.Cells["손절호가_DataGridViewTextBoxColumn"].Value);
                손절호가 = 손절호가.Replace(",", "");
                Clipboard.SetText(손절호가);
            }
        }

        private void 익절_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (익절_dataGridView.CurrentRow.Selected)
            {
                int 선택된행인덱스 = 익절_dataGridView.CurrentCell.RowIndex;
                DataGridViewRow 선택된행 = 익절_dataGridView.Rows[선택된행인덱스];
                string 익절호가 = Convert.ToString(선택된행.Cells["익절호가_DataGridViewTextBoxColumn"].Value);
                익절호가 = 익절호가.Replace(",", "");
                Clipboard.SetText(익절호가);
            }
        }

        private int 호가단위_알려줘(int 가격, int 시장)
        {
            int 호가단위 = 10;

            if (가격 < 1000)
            {
                /* 1000원 미만 - 1원 */
                호가단위 = 1;
            }
            else if (가격 >= 1000 && 가격 < 5000)
            {
                /* 1000원 이상 5000원 미만 - 5원 */
                호가단위 = 5;
            }
            else if (가격 >= 5000 && 가격 < 10000)
            {
                /* 5000원 이상 10000원 미만 - 10원 */
                호가단위 = 10;
            }
            else if (가격 >= 10000 && 가격 < 50000)
            {
                /* 10000원 이상 50000원 미만 - 50원 */
                호가단위 = 50;
            }
            else if (가격 >= 50000 && 가격 < 100000)
            {
                /* 50000원 이상 100000원 미만 - 100원 */
                호가단위 = 100;
            }
            else if (가격 >= 100000 && 가격 < 500000)
            {
                /* 100000원 이상 500000원 미만 - 500원 */
                if (시장 == 0)
                { /* 코스피 */
                    호가단위 = 500;
                }
                else
                { /* 코스닥 */
                    호가단위 = 100;
                }
            }
            else
            {
                /* 500000원 이상 - 1000원 */
                if (시장 == 0)
                { /* 코스피 */
                    호가단위 = 1000;
                }
                else
                { /* 코스닥 */
                    호가단위 = 100;
                }
            }
            return 호가단위;
        }

        private int 호가한계가격_알려줘(int 작은값, int 큰값, int 시장)
        {
            if (작은값 < 1000)
            {
                /* 1000원 미만 - 1원 */
                if (큰값 >= 1000)
                {
                    return 1000;
                }
                else
                {
                    return 0;
                }
            }
            else if (작은값 >= 1000 && 작은값 < 5000)
            {
                /* 1000원 이상 5000원 미만 - 5원 */
                if (큰값 >= 5000)
                {
                    return 5000;
                }
                else
                {
                    return 0;
                }
            }
            else if (작은값 >= 5000 && 작은값 < 10000)
            {
                /* 5000원 이상 10000원 미만 - 10원 */
                if (큰값 >= 10000)
                {
                    return 10000;
                }
                else
                {
                    return 0;
                }
            }
            else if (작은값 >= 10000 && 작은값 < 50000)
            {
                /* 10000원 이상 50000원 미만 - 50원 */
                if (큰값 >= 50000)
                {
                    return 50000;
                }
                else
                {
                    return 0;
                }
            }
            else if (작은값 >= 50000 && 작은값 < 100000)
            {
                /* 50000원 이상 100000원 미만 - 100원 */
                if (큰값 >= 100000)
                {
                    return 100000;
                }
                else
                {
                    return 0;
                }
            }
            else if (작은값 >= 100000 && 작은값 < 500000)
            {
                if (시장 == 0)
                { /* 코스피 = 100000원 이상 500000원 미만 - 500원 */
                    if (큰값 >= 500000)
                    {
                        return 500000;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                { /* 코스닥 = 100000원 이상 500000원 미만 - 100원 */
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        private int 틱간격_알려줘(int 시작가, int 현재가, int 시장)
        {
            int 틱간격 = 0;
            int 호가단위 = 0;
            int 작은값;
            int 큰값;
            int 호가한계가격;
            bool 상승했니 = true;

            if (시작가 == 현재가)
            {
                return 틱간격;
            }

            if (시작가 < 현재가)
            {
                작은값 = 시작가;
                큰값 = 현재가;
            }
            else
            {
                작은값 = 현재가;
                큰값 = 시작가;
                상승했니 = false;
            }

            호가한계가격 = 호가한계가격_알려줘(작은값, 큰값, 시장);

            if (호가한계가격 == 0)
            {
                호가단위 = 호가단위_알려줘(큰값, 시장);
                틱간격 = (큰값 - 작은값) / 호가단위;
            }
            else
            {
                호가단위 = 호가단위_알려줘(작은값, 시장);
                틱간격 = (호가한계가격 - 작은값) / 호가단위;
                호가단위 = 호가단위_알려줘(큰값, 시장);
                틱간격 = 틱간격 + ((큰값 - 호가한계가격) / 호가단위);
            }

            if (상승했니 == true)
            {
                return 틱간격;
            }
            else
            {
                return -틱간격;
            }
        }

        private void 체결가_numericUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                계산해줘();
            }
        }

        private void 수량결과_label_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(수량결과_label.Text);
            효과음_들려줘("수량복사");
        }

        private void 손익분기계산기_Form_Load(object sender, EventArgs e)
        {

        }
    }
}
