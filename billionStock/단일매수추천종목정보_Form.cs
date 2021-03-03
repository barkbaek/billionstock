using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;

namespace billionStock
{
    public partial class 단일매수추천종목정보_Form : Form
    {
        // private double 주식수수료율 = 1.0037869;
        private double 주식수수료율 = 1.0033;

        static MongoClient _몽고클라이언트 = new MongoClient("mongodb://localhost:27017/billionStock");
        static IMongoDatabase _몽고디비 = _몽고클라이언트.GetDatabase("billionStock");
        static IMongoCollection<종목> _종목컬렉션 = _몽고디비.GetCollection<종목>("stocks");
        static IMongoCollection<주식체결> _주식체결컬렉션 = _몽고디비.GetCollection<주식체결>("trades");
        static IMongoCollection<매수추천> _매수추천컬렉션 = _몽고디비.GetCollection<매수추천>("bidRecommended");

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

        string _선택된종목코드;
        public 단일매수추천종목정보_Form(string 종목코드, bool 시장가매매야)
        {
            InitializeComponent();
            _선택된종목코드 = 종목코드;

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

            시장가매매_checkBox.Checked = 시장가매매야;
        }

        private void 단일매수추천종목정보_Form_Load(object sender, EventArgs e)
        {
            단일매수추천종목정보_가져오자();
        }

        public void 단일매수추천종목정보_가져오자()
        {
            //Console.WriteLine("단일매수추천종목정보_가져오자() 를 실행하였습니다.\r\n");
            단일매수추천종목정보_dataGridView.ClearSelection();
            var 매수추천_builder = Builders<매수추천>.Filter;
            var 매수추천_query = 매수추천_builder.Eq(x => x._종목코드, _선택된종목코드);
            var 매수추천_목록 = _매수추천컬렉션.Find(매수추천_query).SortByDescending(x => x._체결시간).ToList();

            if (매수추천_목록 != null && 매수추천_목록.Count > 0)
            {
                단일매수추천종목정보_dataGridView.Rows.Clear();
                단일매수추천종목정보_dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                단일매수추천종목정보_dataGridView.RowHeadersVisible = false;

                bool 시장가매매야 = 시장가매매_checkBox.Checked;
                string 종목코드;
                string 종목명;

                int 순서;
                int 시간;

                string 시간_문자;
                double 등락율;
                int 돌파틱개수;
                int 체결개수;
                double 체결강도;
                int 매도호가;
                int 매수호가;
                int 현재가;
                long 매도거래대금;
                long 매수거래대금;
                string 첫추출호가_문자 = "1";
                int 첫추출호가 = 1;
                
                double 세력순수익률 = 0;
                long 누적거래량;
                long 누적거래대금;

                if (매수추천_목록.Count > 0)
                {
                    if (시장가매매야 == true)
                    {
                        첫추출호가_문자 = Convert.ToString(매수추천_목록[매수추천_목록.Count - 1]._최우선매도호가);
                        if (첫추출호가_문자 == "" || 첫추출호가_문자 == "0")
                        {
                            첫추출호가 = Int32.Parse(Convert.ToString(매수추천_목록[매수추천_목록.Count - 1]._현재가));
                        } else
                        {
                            첫추출호가 = Int32.Parse(첫추출호가_문자);
                        }
                    } else
                    {
                        첫추출호가 = Int32.Parse(Convert.ToString(매수추천_목록[매수추천_목록.Count - 1]._현재가));
                    }
                }

                for (int ㄱ = 0; ㄱ < 매수추천_목록.Count; ㄱ++)
                {
                    순서 = ㄱ + 1;
                    if (ㄱ == 0)
                    {
                        종목명 = Convert.ToString(매수추천_목록[ㄱ]._종목명);
                        종목코드 = Convert.ToString(매수추천_목록[ㄱ]._종목코드);
                        종목정보_label.Text = String.Format("{0} ( {1} ) 세력 매수감지 총 {2}번", 종목명, 종목코드, 매수추천_목록.Count);
                    }
                    시간 = Int32.Parse(Convert.ToString(매수추천_목록[ㄱ]._체결시간));
                    등락율 = double.Parse(Convert.ToString(매수추천_목록[ㄱ]._등락율));
                    돌파틱개수 = Int32.Parse(Convert.ToString(매수추천_목록[ㄱ]._돌파틱개수));
                    체결개수 = Int32.Parse(Convert.ToString(매수추천_목록[ㄱ]._체결개수));
                    체결강도 = double.Parse(Convert.ToString(매수추천_목록[ㄱ]._체결강도));
                    매도호가 = Int32.Parse(Convert.ToString(매수추천_목록[ㄱ]._최우선매도호가));
                    매수호가 = Int32.Parse(Convert.ToString(매수추천_목록[ㄱ]._최우선매수호가));
                    현재가 = Int32.Parse(Convert.ToString(매수추천_목록[ㄱ]._현재가));

                    if (시장가매매야 == true && 매수호가 > 0)
                    {
                        세력순수익률 = (((double)매수호가 / (double)((double)첫추출호가 * 주식수수료율)) - 1) * 100;
                    } else
                    {
                        세력순수익률 = (((double)현재가 / (double)((double)첫추출호가 * 주식수수료율)) - 1) * 100;
                    }

                    매도거래대금 = long.Parse(Convert.ToString(매수추천_목록[ㄱ]._매도거래대금));
                    매수거래대금 = long.Parse(Convert.ToString(매수추천_목록[ㄱ]._매수거래대금));
                    누적거래량 = long.Parse(Convert.ToString(매수추천_목록[ㄱ]._누적거래량));
                    누적거래대금 = long.Parse(Convert.ToString(매수추천_목록[ㄱ]._누적거래대금));

                    단일매수추천종목정보_dataGridView.Rows.Add();
                    단일매수추천종목정보_dataGridView["순서_DataGridViewTextBoxColumn", ㄱ].Value = 순서;

                    시간_문자 = 시간.ToString();
                    if (시간_문자.Length == 5)
                    {
                        단일매수추천종목정보_dataGridView["시간_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0}:{1}:{2}", 시간_문자.Substring(0, 1), 시간_문자.Substring(1, 2), 시간_문자.Substring(3, 2));
                    } else
                    {
                        단일매수추천종목정보_dataGridView["시간_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0}:{1}:{2}", 시간_문자.Substring(0, 2), 시간_문자.Substring(2, 2), 시간_문자.Substring(4, 2));
                    }

                    if (등락율 > 0)
                    {
                        단일매수추천종목정보_dataGridView["등락율_DataGridViewTextBoxColumn", ㄱ].Style = _레드셀스타일;
                    }
                    else
                    {
                        단일매수추천종목정보_dataGridView["등락율_DataGridViewTextBoxColumn", ㄱ].Style = _블루셀스타일;
                    }
                    단일매수추천종목정보_dataGridView["등락율_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:0.00}%", 등락율);
                    단일매수추천종목정보_dataGridView["돌파틱개수_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,#}", 돌파틱개수);
                    단일매수추천종목정보_dataGridView["체결개수_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,#}", 체결개수);
                    단일매수추천종목정보_dataGridView["체결강도_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:0.00}%", 체결강도);
                    단일매수추천종목정보_dataGridView["매도호가_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,#}", 매도호가);
                    단일매수추천종목정보_dataGridView["매수호가_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,#}", 매수호가);
                    단일매수추천종목정보_dataGridView["현재가_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,#}", 현재가);

                    if (매도거래대금 >= 1000000000) /* 십억이상 */
                    {
                        단일매수추천종목정보_dataGridView["매도거래대금_DataGridViewTextBoxColumn", ㄱ].Style = _강한블루셀스타일;
                    }
                    else if (매도거래대금 < 1000000000 && 매도거래대금 >= 500000000) /* 오억이상 */
                    {
                        단일매수추천종목정보_dataGridView["매도거래대금_DataGridViewTextBoxColumn", ㄱ].Style = _블루셀스타일;
                    }
                    else if (매도거래대금 < 500000000 && 매도거래대금 >= 100000000) /* 일억이상 */
                    {
                        단일매수추천종목정보_dataGridView["매도거래대금_DataGridViewTextBoxColumn", ㄱ].Style = _약한블루셀스타일;
                    }
                    단일매수추천종목정보_dataGridView["매도거래대금_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,#}", 매도거래대금);

                    if (매수거래대금 >= 1000000000) /* 십억이상 */
                    {
                        단일매수추천종목정보_dataGridView["매수거래대금_DataGridViewTextBoxColumn", ㄱ].Style = _강한레드셀스타일;
                    }
                    else if (매수거래대금 < 1000000000 && 매수거래대금 >= 500000000) /* 오억이상 */
                    {
                        단일매수추천종목정보_dataGridView["매수거래대금_DataGridViewTextBoxColumn", ㄱ].Style = _레드셀스타일;
                    }
                    else if (매수거래대금 < 500000000 && 매수거래대금 >= 100000000) /* 일억이상 */
                    {
                        단일매수추천종목정보_dataGridView["매수거래대금_DataGridViewTextBoxColumn", ㄱ].Style = _약한레드셀스타일;
                    }
                    단일매수추천종목정보_dataGridView["매수거래대금_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,#}", 매수거래대금);


                    /* 세력순수익률 */
                    if (세력순수익률 > 0)
                    {
                        if (세력순수익률 <= 2)
                        {
                            단일매수추천종목정보_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄱ].Style = _약한레드셀스타일;
                        }
                        else if (세력순수익률 > 2 && 세력순수익률 <= 15)
                        {
                            단일매수추천종목정보_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄱ].Style = _레드셀스타일;
                        }
                        else
                        {
                            단일매수추천종목정보_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄱ].Style = _강한레드셀스타일;
                        }
                    }
                    else if (세력순수익률 < 0)
                    {
                        if (세력순수익률 >= -2)
                        {
                            단일매수추천종목정보_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄱ].Style = _약한블루셀스타일;
                        }
                        else if (세력순수익률 < -2 && 세력순수익률 >= -15)
                        {
                            단일매수추천종목정보_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄱ].Style = _블루셀스타일;
                        }
                        else
                        {
                            단일매수추천종목정보_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄱ].Style = _강한블루셀스타일;
                        }
                    }
                    단일매수추천종목정보_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:0.##}%", 세력순수익률);

                    단일매수추천종목정보_dataGridView["누적거래량_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,#}", 누적거래량);
                    단일매수추천종목정보_dataGridView["누적거래대금_DataGridViewTextBoxColumn", ㄱ].Value = String.Format("{0:#,#}", 누적거래대금);
                }
            }
        }

        private void 새로고침_button_Click(object sender, EventArgs e)
        {
            단일매수추천종목정보_dataGridView.ClearSelection();
            단일매수추천종목정보_가져오자();
        }
    }
}
