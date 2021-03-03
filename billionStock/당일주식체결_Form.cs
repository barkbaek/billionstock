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
using System.Media;

namespace billionStock
{
    public partial class 당일주식체결_Form : Form
    {
        private 당일주식체결_Form _당일주식체결이야;

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

        private string _선택된체결시간;
        private 종목 _선택된종목아;

        private SoundPlayer _사운드플레이어;

        Thread _실시간스레드;
        int _실시간주기 = 2000;
        bool _실시간이니 = false;
        bool _작업중이니 = false;

        public 당일주식체결_Form()
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

            _실시간스레드 = new Thread(delegate ()
            {
                while (true)
                {
                    try
                    {
                        if (IsHandleCreated)
                        {
                            _실시간이니 = 실시간_checkBox.Checked;
                            _실시간주기 = Convert.ToInt32(주기_numericUpDown.Value);
                            if (_실시간이니 == true && _작업중이니 == false)
                            {
                                if (_실시간주기 <= 1000)
                                {
                                    _실시간주기 = 1000;
                                }
                                _당일주식체결이야.Invoke(new Action(() => {
                                    선택된종목_가져오자();
                                }));
                                Thread.Sleep(_실시간주기);
                            }
                        }
                        Thread.Sleep(100);
                    }
                    catch (Exception EX)
                    {
                        Console.WriteLine("당일주식체결 실시간스레드 오류입니다. EX: {0} ", EX);
                    }
                }
            });
            _실시간스레드.IsBackground = true;
            _실시간스레드.Start();
        }

        public void 실시간스레드_멈춰줘()
        {
            _실시간스레드.Abort();
        }

        private void 당일주식체결_Form_Load(object sender, EventArgs e)
        {
            주식체결_dataGridView.ClearSelection();
        }

        public void 당일주식체결_Form_저장해줘(당일주식체결_Form 당일주식체결이야)
        {
            _당일주식체결이야 = 당일주식체결이야;
        }

        public void 효과음_들려줘(string 명령어)
        {
            string 효과음;
            if (명령어 == "환영")
            {
                효과음 = @"D:\\soundPlayer\\start2.wav";
            }
            else if (명령어 == "작업끝")
            {
                효과음 = @"D:\\soundPlayer\\trumpet.wav";
            }
            else if (명령어 == "손절")
            {
                효과음 = @"D:\\soundPlayer\\minus.wav";
            }
            else if (명령어 == "익절")
            {
                효과음 = @"D:\\soundPlayer\\plus.wav";
            }
            else if (명령어 == "매수추천")
            {
                효과음 = @"D:\\soundPlayer\\letsgo.wav";
            }
            else if (명령어 == "매수접수")
            {
                효과음 = @"D:\\soundPlayer\\bidRequest.wav";
            }
            else if (명령어 == "매수체결")
            {
                효과음 = @"D:\\soundPlayer\\bidAccept.wav";
            }
            else if (명령어 == "매도접수")
            {
                효과음 = @"D:\\soundPlayer\\offerRequest.wav";
            }
            else if (명령어 == "매도체결")
            {
                효과음 = @"D:\\soundPlayer\\offerAccept.wav";
            }
            else if (
                명령어 == "정정접수" ||
                명령어 == "취소접수"
            )
            {
                효과음 = @"D:\\soundPlayer\\orderRequest.wav";
            }
            else
            {
                효과음 = @"D:\\soundPlayer\\button.wav";
            }

            _사운드플레이어 = new SoundPlayer(효과음);
            _사운드플레이어.Play();
        }

        private void 종목검색_button_Click(object sender, EventArgs e)
        {
            선택된종목_가져오자();
        }

        async Task 선택된종목_가져오자()
        {
            try
            {
                if (_작업중이니 == true) { return; }
                string 검색내용 = 종목검색_textBox.Text;
                if (검색내용 == "") { return; }

                _작업중이니 = true;

                var builder = Builders<종목>.Filter;
                var query = builder.Eq(x => x._종목코드, 검색내용) | builder.Eq(x => x._종목명, 검색내용);
                var result = await _종목컬렉션.Find(query).Limit(1).ToListAsync();
                _선택된종목아 = result.FirstOrDefault();

                if (_선택된종목아 != null)
                {
                    주식체결_dataGridView.Rows.Clear();
                    주식체결_dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing; //or even better .DisableResizing. Most time consumption enum is DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders                
                    주식체결_dataGridView.RowHeadersVisible = false;// set it to false if not needed

                    var 주식체결_builder = Builders<주식체결>.Filter;
                    var 주식체결_query = 주식체결_builder.Eq(x => x._종목코드, _선택된종목아._종목코드);
                    //Console.WriteLine("{0} ({1})의 주식체결 목록을 불러옵니다.. = {2}\r\n", _선택된종목아._종목명, _선택된종목아._종목코드, DateTime.Now);
                    try
                    {
                        var 주식체결_list = _주식체결컬렉션.Find(주식체결_query).SortByDescending(x => x._체결시간).ThenByDescending(x => x._순서).Limit(Convert.ToInt32(Limit_numericUpDown.Value)).ToList();

                        //Console.WriteLine("{0} ({1})의 주식체결 목록을 불러왔습니다! 총 {2}개 = {3}\r\n", _선택된종목아._종목명, _선택된종목아._종목코드, 주식체결_list.Count, DateTime.Now);
                        주식체결 주식체결종가야;
                        long 거래대금;
                        double 전틱대비율;
                        int 틱간격;
                        int 전틱대비;
                        int 호가단위;
                        int 일프로가격 = 0;
                        int 이프로가격 = 0;
                        int 일프로틱간격;
                        bool 매수추천있니 = false;
                        int 매수추천인덱스 = 0;
                        bool 손절알림할까 = 손절알림_checkBox.Checked;
                        bool 익절알림할까 = 익절알림_checkBox.Checked;
                        bool 손절이니 = false;
                        bool 익절이니 = false;
                        int 손절가 = 0;
                        int 익절가 = 999999999;

                        if (손절알림할까 == true)
                        {
                            손절가 = Convert.ToInt32(손절알림_numericUpDown.Value);
                        }
                        if (익절알림할까 == true)
                        {
                            익절가 = Convert.ToInt32(익절알림_numericUpDown.Value);
                        }

                        if (주식체결_list != null && 주식체결_list.Count > 0)
                        {
                            체결시간_label.Text = String.Format("선택시간: 없음");
                            선택가_label.Text = String.Format("선택가: 0");
                            기준가_label.Text = String.Format("기준가: 0");
                            누적거래량_label.Text = String.Format("누적거래량: 0");
                            누적거래대금_label.Text = String.Format("누적거래대금: 0");

                            var 매수추천_builder = Builders<매수추천>.Filter;
                            var 매수추천_query = 매수추천_builder.Eq(x => x._종목코드, _선택된종목아._종목코드);
                            //Console.WriteLine("{0} ({1})의 매수추천 목록을 불러옵니다..\r\n", _선택된종목아._종목명, _선택된종목아._종목코드);
                            var 매수추천_list = _매수추천컬렉션.Find(매수추천_query).SortByDescending(x => x._체결시간).ToList();

                            //Console.WriteLine("{0} ({1})의 매수추천 목록을 불러왔습니다! 총 {2}개\r\n", _선택된종목아._종목명, _선택된종목아._종목코드, 매수추천_list.Count);

                            if (매수추천_list != null && 매수추천_list.Count > 0)
                            {
                                매수추천확인_label.BackColor = _레드셀스타일.BackColor;
                                if (매수추천_list[매수추천_list.Count - 1]._체결시간 < 100000)
                                {
                                    if (매수추천_list[0]._체결시간 < 100000)
                                    {
                                        매수추천추출기간_label.Text = String.Format(
                                            "매수추천 추출기간: {0}:{1}:{2} ~ {3}:{4}:{5}"
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(0, 1)
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(1, 2)
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(3, 2)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(0, 1)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(1, 2)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(3, 2)
                                            );
                                    }
                                    else
                                    {
                                        매수추천추출기간_label.Text = String.Format(
                                            "매수추천 추출기간: {0}:{1}:{2} ~ {3}:{4}:{5}"
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(0, 1)
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(1, 2)
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(3, 2)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(0, 2)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(2, 2)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(4, 2)
                                            );
                                    }
                                }
                                else
                                {
                                    if (매수추천_list[0]._체결시간 < 100000)
                                    {
                                        매수추천추출기간_label.Text = String.Format(
                                            "매수추천 추출기간: {0}:{1}:{2} ~ {3}:{4}:{5}"
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(0, 2)
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(2, 2)
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(4, 2)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(0, 1)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(1, 2)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(3, 2)
                                            );
                                    }
                                    else
                                    {
                                        매수추천추출기간_label.Text = String.Format(
                                            "매수추천 추출기간: {0}:{1}:{2} ~ {3}:{4}:{5}"
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(0, 2)
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(2, 2)
                                            , 매수추천_list[매수추천_list.Count - 1]._체결시간.ToString().Substring(4, 2)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(0, 2)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(2, 2)
                                            , 매수추천_list[0]._체결시간.ToString().Substring(4, 2)
                                            );
                                    }
                                }
                                매수추천개수_label.Text = String.Format("매수추천개수: {0:#,#}", 매수추천_list.Count);
                                매수추천있니 = true;
                            }
                            else
                            {
                                매수추천확인_label.BackColor = _블랙셀스타일.BackColor;
                                매수추천추출기간_label.Text = String.Format("매수추천 추출기간: 없음");
                                매수추천개수_label.Text = String.Format("매수추천개수: 0");
                            }

                            주식체결종가야 = 주식체결_list[0];
                            if (주식체결종가야._등락율 > 0)
                            {
                                종목명_label.ForeColor = Color.Red;
                                현재가_label.ForeColor = Color.Red;
                            }
                            else if (주식체결종가야._등락율 < 0)
                            {
                                종목명_label.ForeColor = Color.Blue;
                                현재가_label.ForeColor = Color.Blue;
                            }
                            else
                            {
                                종목명_label.ForeColor = Color.White;
                                현재가_label.ForeColor = Color.White;
                            }

                            종목명_label.Text = String.Format("{0} ({1})", _선택된종목아._종목명, _선택된종목아._종목코드);
                            현재가_label.Text = String.Format("{0:#,#} {1:#,#}({2}%)", 주식체결종가야._현재가, 주식체결종가야._전일대비, 주식체결종가야._등락율);

                            if (_선택된종목아._시장 == 0)
                            {
                                시장_label.Text = "코스피";
                                시장_label.ForeColor = Color.Gold;
                            }
                            else
                            {
                                시장_label.Text = "코스닥";
                                시장_label.ForeColor = Color.DarkOrange;
                            }

                            주식체결개수_label.Text = String.Format("주식체결: {0:#,#}", 주식체결_list.Count);

                            호가단위 = 호가단위_알려줘(주식체결종가야._현재가, _선택된종목아._시장);
                            호가단위_label.Text = String.Format("호가단위: {0:#,#}", 호가단위);

                            누적거래량_label.Text = String.Format("누적거래량: {0:#,#}", 주식체결종가야._누적거래량);
                            누적거래대금_label.Text = String.Format("누적거래대금: {0:#,#}", (주식체결종가야._누적거래대금 * 1000000));

                            for (int i = 0; i < 주식체결_list.Count; i++)
                            {
                                주식체결_dataGridView.Rows.Add();
                                주식체결 주식체결이야 = 주식체결_list[i];

                                if (매수추천있니 == true)
                                {
                                    if (
                                        주식체결이야._체결시간 == 매수추천_list[매수추천인덱스]._체결시간 &&
                                        주식체결이야._최우선매도호가 == 매수추천_list[매수추천인덱스]._최우선매도호가 &&
                                        주식체결이야._최우선매수호가 == 매수추천_list[매수추천인덱스]._최우선매수호가 &&
                                        주식체결이야._현재가 == 매수추천_list[매수추천인덱스]._현재가 &&
                                        주식체결이야._등락율 == 매수추천_list[매수추천인덱스]._등락율
                                    )
                                    {
                                        주식체결_dataGridView["매매추천_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                        //Console.WriteLine("체결시간: {0}, 최우선매도호가: {1}, 최우선매수호가 {2}, 현재가: {3}, 등락율: {4}\r\n", 주식체결이야._체결시간, 주식체결이야._최우선매도호가, 주식체결이야._최우선매수호가, 주식체결이야._현재가, 주식체결이야._등락율);
                                        if ((매수추천인덱스 + 1) < 매수추천_list.Count)
                                        {
                                            매수추천인덱스++;
                                        }
                                    }
                                    else
                                    {
                                        주식체결_dataGridView["매매추천_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                    }
                                }
                                else
                                {
                                    주식체결_dataGridView["매매추천_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                }
                                /* 체결시간 & 상한가발생시간 & 하한가발생시간 */
                                string 시간;
                                if (주식체결이야._체결시간 < 100000)
                                {
                                    시간 = String.Format("{0}:{1}:{2}", 주식체결이야._체결시간.ToString().Substring(0, 1), 주식체결이야._체결시간.ToString().Substring(1, 2), 주식체결이야._체결시간.ToString().Substring(3, 2));
                                }
                                else
                                {
                                    시간 = String.Format("{0}:{1}:{2}", 주식체결이야._체결시간.ToString().Substring(0, 2), 주식체결이야._체결시간.ToString().Substring(2, 2), 주식체결이야._체결시간.ToString().Substring(4, 2));
                                }
                                if (
                                    주식체결이야._체결시간 == 주식체결이야._상한가발생시간 ||
                                    주식체결이야._등락율 >= 30
                                 )
                                {
                                    주식체결_dataGridView["시간_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                }
                                else
                                {
                                    주식체결_dataGridView["시간_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                }

                                if (
                                    주식체결이야._체결시간 == 주식체결이야._하한가발생시간 ||
                                    주식체결이야._등락율 <= -30
                                )
                                {
                                    주식체결_dataGridView["시간_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                }
                                else
                                {
                                    주식체결_dataGridView["시간_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                }
                                주식체결_dataGridView["시간_DataGridViewTextBoxColumn", i].Value = 시간;

                                /* 등락율 & 체결가 & 전일대비 */
                                if (주식체결이야._등락율 > 0)
                                {
                                    주식체결_dataGridView["등락율_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                    주식체결_dataGridView["체결가_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                    주식체결_dataGridView["전일대비_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                }
                                else if (주식체결이야._등락율 < 0)
                                {
                                    주식체결_dataGridView["등락율_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                    주식체결_dataGridView["체결가_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                    주식체결_dataGridView["전일대비_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                }
                                else
                                {
                                    주식체결_dataGridView["등락율_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                    주식체결_dataGridView["체결가_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                    주식체결_dataGridView["전일대비_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                }
                                주식체결_dataGridView["등락율_DataGridViewTextBoxColumn", i].Value = String.Format("{0:0.00}%", 주식체결이야._등락율);
                                주식체결_dataGridView["체결가_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", 주식체결이야._현재가);
                                주식체결_dataGridView["전일대비_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", 주식체결이야._전일대비);

                                if (손절알림할까 == true && 주식체결종가야._현재가 <= 손절가)
                                {
                                    손절이니 = true;
                                }
                                if (익절알림할까 == true && 주식체결종가야._현재가 >= 익절가)
                                {
                                    익절이니 = true;
                                }

                                /* 평균가 */
                                if (주식체결이야._평균가 > 0)
                                {
                                    if (주식체결이야._평균가 < 주식체결이야._현재가)
                                    {
                                        주식체결_dataGridView["평균가_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                    }
                                    else if (주식체결이야._평균가 > 주식체결이야._현재가)
                                    {
                                        주식체결_dataGridView["평균가_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                    }
                                    else
                                    {
                                        주식체결_dataGridView["평균가_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                    }
                                }
                                else
                                {
                                    주식체결_dataGridView["평균가_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                }
                                주식체결_dataGridView["평균가_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", 주식체결이야._평균가);

                                /* 고가 & 저가 */
                                if (주식체결이야._고가 == 주식체결이야._현재가)
                                {
                                    주식체결_dataGridView["고가_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                }
                                else
                                {
                                    주식체결_dataGridView["고가_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                }
                                주식체결_dataGridView["고가_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", 주식체결이야._고가);

                                if (주식체결이야._저가 == 주식체결이야._현재가)
                                {
                                    주식체결_dataGridView["저가_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                }
                                else
                                {
                                    주식체결_dataGridView["저가_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                }
                                주식체결_dataGridView["저가_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", 주식체결이야._저가);

                                /* 매도호가 & 매수호가 */
                                if (
                                    주식체결이야._체결시간 >= 90000 &&
                                    주식체결이야._체결시간 <= 152059
                                    )
                                {
                                    if (주식체결이야._최우선매도호가 == 주식체결이야._현재가 && 주식체결이야._최우선매수호가 == 주식체결이야._현재가)
                                    {
                                        if (주식체결이야._매수했니 == true)
                                        {
                                            주식체결_dataGridView["매도호가_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                            주식체결_dataGridView["매수호가_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                        }
                                        else
                                        {
                                            주식체결_dataGridView["매도호가_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                            주식체결_dataGridView["매수호가_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                        }
                                    }
                                    else
                                    {
                                        if (주식체결이야._매수했니 == true)
                                        {
                                            if (주식체결이야._최우선매도호가 > 0)
                                            {
                                                if (주식체결이야._최우선매도호가 < 주식체결이야._현재가)
                                                {
                                                    주식체결_dataGridView["매도호가_DataGridViewTextBoxColumn", i].Style = _강한레드셀스타일;
                                                }
                                                else if (주식체결이야._최우선매도호가 == 주식체결이야._현재가)
                                                {
                                                    주식체결_dataGridView["매도호가_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                                }
                                                else
                                                {
                                                    주식체결_dataGridView["매도호가_DataGridViewTextBoxColumn", i].Style = _약한레드셀스타일;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (주식체결이야._최우선매수호가 > 0)
                                            {
                                                if (주식체결이야._최우선매수호가 > 주식체결이야._현재가)
                                                {
                                                    주식체결_dataGridView["매수호가_DataGridViewTextBoxColumn", i].Style = _강한블루셀스타일;
                                                }
                                                else if (주식체결이야._최우선매수호가 == 주식체결이야._현재가)
                                                {
                                                    주식체결_dataGridView["매수호가_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                                }
                                                else
                                                {
                                                    주식체결_dataGridView["매수호가_DataGridViewTextBoxColumn", i].Style = _약한블루셀스타일;
                                                }
                                            }
                                        }
                                    }
                                    주식체결_dataGridView["매도호가_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", 주식체결이야._최우선매도호가);
                                    주식체결_dataGridView["매수호가_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", 주식체결이야._최우선매수호가);
                                }
                                else
                                {
                                    주식체결_dataGridView["매도호가_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                    주식체결_dataGridView["매수호가_DataGridViewTextBoxColumn", i].Style = _블랙셀스타일;
                                }

                                /* 호가틱간격 */
                                틱간격 = 틱간격_알려줘(주식체결이야._최우선매수호가, 주식체결이야._최우선매도호가, _선택된종목아._시장);
                                if (
                                    주식체결이야._최우선매도호가 > 0 &&
                                    주식체결이야._최우선매수호가 > 0 &&
                                    주식체결이야._체결시간 >= 90000 &&
                                    주식체결이야._체결시간 <= 152059
                                )
                                {
                                    if (틱간격 == 1)
                                    {
                                        주식체결_dataGridView["호가틱간격_DataGridViewTextBoxColumn", i].Style = _그린셀스타일;
                                    }
                                    else if (틱간격 > 1)
                                    {
                                        주식체결_dataGridView["호가틱간격_DataGridViewTextBoxColumn", i].Style = _강한그린셀스타일;
                                    }
                                    주식체결_dataGridView["호가틱간격_DataGridViewTextBoxColumn", i].Value = 틱간격;
                                }

                                /* 거래량 */
                                if (주식체결이야._매수했니 == true)
                                {
                                    주식체결_dataGridView["거래량_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                }
                                else
                                {
                                    주식체결_dataGridView["거래량_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                }
                                주식체결_dataGridView["거래량_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", 주식체결이야._거래량);

                                /* 거래대금 */
                                거래대금 = 주식체결이야._현재가 * 주식체결이야._거래량;

                                if (거래대금 >= 99000000) /* 9900만원 이상 */
                                {
                                    if (주식체결이야._매수했니 == true)
                                    {
                                        주식체결_dataGridView["거래대금_DataGridViewTextBoxColumn", i].Style = _강한레드셀스타일;
                                    }
                                    else
                                    {
                                        주식체결_dataGridView["거래대금_DataGridViewTextBoxColumn", i].Style = _강한블루셀스타일;
                                    }
                                }
                                else if (거래대금 < 99000000 && 거래대금 >= 49000000) /* 4900만원이상 */
                                {
                                    if (주식체결이야._매수했니 == true)
                                    {
                                        주식체결_dataGridView["거래대금_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                    }
                                    else
                                    {
                                        주식체결_dataGridView["거래대금_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                    }
                                }
                                else if (거래대금 < 49000000 && 거래대금 >= 9000000) /* 900만원이상 */
                                {
                                    if (주식체결이야._매수했니 == true)
                                    {
                                        주식체결_dataGridView["거래대금_DataGridViewTextBoxColumn", i].Style = _약한레드셀스타일;
                                    }
                                    else
                                    {
                                        주식체결_dataGridView["거래대금_DataGridViewTextBoxColumn", i].Style = _약한블루셀스타일;
                                    }
                                }
                                주식체결_dataGridView["거래대금_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", 거래대금);

                                /* 누적거래량 */
                                주식체결_dataGridView["누적거래량_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", 주식체결이야._누적거래량);

                                /* 누적거래대금 */
                                주식체결_dataGridView["누적거래대금_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,#}", (주식체결이야._누적거래대금 * 1000000));

                                /* 거래회전율 */
                                if (주식체결이야._거래회전율 > 0)
                                {
                                    주식체결_dataGridView["거래회전율_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                }
                                else if (주식체결이야._거래회전율 < 0)
                                {
                                    주식체결_dataGridView["거래회전율_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                }
                                주식체결_dataGridView["거래회전율_DataGridViewTextBoxColumn", i].Value = String.Format("{0:0.00}%", 주식체결이야._거래회전율);

                                /* 전틱대비 */
                                if ((i + 1) < 주식체결_list.Count)
                                {
                                    주식체결 이전주식체결이야 = 주식체결_list[i + 1];
                                    전틱대비 = 틱간격_알려줘(이전주식체결이야._현재가, 주식체결이야._현재가, _선택된종목아._시장);
                                    전틱대비율 = (((double)주식체결이야._현재가 / (double)이전주식체결이야._현재가) - 1) * 100;

                                    /* 현재가대비수익률계산기 만든 후 퍼센테이지 칸 추가해서 그린셀스타일 추가해주기. 예) 1,100원 = 1%  1,200원 = 2%, 900원 = -1% 즉, 상단은 퍼센테이지 아래는 가격! */
                                    // 일프로틱간격 = 일프로_틱간격_알려줘(이전주식체결이야._현재가, 호가단위);

                                    if (전틱대비 > 0)
                                    {
                                        주식체결_dataGridView["전틱대비_DataGridViewTextBoxColumn", i].Style = _레드셀스타일;
                                    }
                                    else if (전틱대비 < 0)
                                    {
                                        주식체결_dataGridView["전틱대비_DataGridViewTextBoxColumn", i].Style = _블루셀스타일;
                                    }

                                    주식체결_dataGridView["전틱대비_DataGridViewTextBoxColumn", i].Value = String.Format("{0:#,0} ( {1:0.00}% )", 전틱대비, 전틱대비율);
                                }
                            }
                            //Console.WriteLine("주식체결 DataGridView에 완료한 시간: {0}\r\n", DateTime.Now);

                            if (손절이니 == true)
                            {
                                효과음_들려줘("손절");
                            }
                            else if (익절이니 == true)
                            {
                                효과음_들려줘("익절");
                            }
                        }
                    }
                    catch (Exception EX)
                    {
                        Console.WriteLine("주식체결 몽고디비 오류 EX: {0}", EX);
                    }
                }
                else
                {
                    MessageBox.Show(String.Format("종목이 존재하지 않습니다."), "존재하지 않는 종목", MessageBoxButtons.OK);
                }
                _작업중이니 = false;
            } catch (Exception EX)
            {
                Console.WriteLine("당일주식체결 = 선택된종목_가져오자() 가장 상단 오류 = EX : {0}", EX);
                _작업중이니 = false;
            }
        }

        private void 종목검색_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                선택된종목_가져오자();
            }
        }

        public void 당일주식체결_보여줘(int 시장, string 종목코드, int 체결가, string 체결시간)
        {
            종목검색_textBox.Text = 종목코드;
            수익률_textBox.Text = 체결가.ToString();
            _선택된체결시간 = 체결시간;

            int 틱간격;

            int 손절률 = -2;
            int 익절률 = 2;

            double 손익분기가 = (double)체결가 * 주식수수료율;
            int 손익분기호가 = (int)Math.Ceiling(손익분기가);
            int 호가단위 = 호가단위_알려줘(체결가, 시장);
            if (손익분기호가 % 호가단위 != 0)
            {
                손익분기호가 = 손익분기호가 - (손익분기호가 % 호가단위) + 호가단위;
            }
            double 곱할수 = 1 + (손절률 * 0.01);
            int 손절호가 = (int)Math.Ceiling(손익분기가 * 곱할수);
            int 인덱스 = ((손절률 * -1) - 1);
            호가단위 = 호가단위_알려줘(손절호가, 시장);
            손절호가 = 손절호가 - (손절호가 % 호가단위) + 호가단위;

            곱할수 = 1 + (익절률 * 0.01);
            int 익절호가 = (int)Math.Ceiling(손익분기가 * 곱할수);
            호가단위 = 호가단위_알려줘(익절호가, 시장);
            익절호가 = 익절호가 - (익절호가 % 호가단위) + 호가단위;

            손절알림_numericUpDown.Value = 손절호가;
            익절알림_numericUpDown.Value = 익절호가;

            선택된종목_가져오자();
            /*
             
            int 선택된행인덱스 = 주식체결_dataGridView.CurrentCell.RowIndex;
                DataGridViewRow 선택된행 = 주식체결_dataGridView.Rows[선택된행인덱스];
                string 체결가_문자;
                bool 시장가매매야 = 시장가매매_checkBox.Checked;
                if (시장가매매야 == true)
                {
                    체결가_문자 = Convert.ToString(선택된행.Cells["매도호가_DataGridViewTextBoxColumn"].Value);
                    if (체결가_문자 == "" || 체결가_문자 == "0")
                    {
                        체결가_문자 = Convert.ToString(선택된행.Cells["체결가_DataGridViewTextBoxColumn"].Value);
                    }
                } else
                {
                    체결가_문자 = Convert.ToString(선택된행.Cells["체결가_DataGridViewTextBoxColumn"].Value);
                }
                Console.WriteLine(체결가_문자);
                int 체결가 = Int32.Parse( 체결가_문자.Replace(",", "") );
                수익률_textBox.Text = 체결가.ToString();
                _선택된체결시간 = Convert.ToString(선택된행.Cells["시간_DataGridViewTextBoxColumn"].Value);
             
             */


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
                } else
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
            } else
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
            } else
            {
                호가단위 = 호가단위_알려줘(작은값, 시장);
                틱간격 = (호가한계가격 - 작은값) / 호가단위;
                호가단위 = 호가단위_알려줘(큰값, 시장);
                틱간격 = 틱간격 + ((큰값 - 호가한계가격) / 호가단위);
            }
            
            if (상승했니 == true)
            {
                return 틱간격;
            } else
            {
                return -틱간격;
            }
        }

        private int 일프로_틱간격_알려줘 (int 가격, int 호가단위)
        {
            int 틱간격 = 0;
            if (가격 == 0)
            {
                return 틱간격;
            }
            return 틱간격;
        }

        private void 주식체결_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (주식체결_dataGridView.CurrentRow.Selected)
            {
                int 선택된행인덱스 = 주식체결_dataGridView.CurrentCell.RowIndex;
                DataGridViewRow 선택된행 = 주식체결_dataGridView.Rows[선택된행인덱스];
                string 체결가_문자;
                bool 시장가매매야 = 시장가매매_checkBox.Checked;
                if (시장가매매야 == true)
                {
                    체결가_문자 = Convert.ToString(선택된행.Cells["매도호가_DataGridViewTextBoxColumn"].Value);
                    if (체결가_문자 == "" || 체결가_문자 == "0")
                    {
                        체결가_문자 = Convert.ToString(선택된행.Cells["체결가_DataGridViewTextBoxColumn"].Value);
                    }
                } else
                {
                    체결가_문자 = Convert.ToString(선택된행.Cells["체결가_DataGridViewTextBoxColumn"].Value);
                }
                //Console.WriteLine(체결가_문자);
                int 체결가 = Int32.Parse( 체결가_문자.Replace(",", "") );
                수익률_textBox.Text = 체결가.ToString();
                _선택된체결시간 = Convert.ToString(선택된행.Cells["시간_DataGridViewTextBoxColumn"].Value);
            }
        }

        private void 수익률_button_Click(object sender, EventArgs e)
        {
            int 체결가 = Int32.Parse(수익률_textBox.Text.Replace(",", ""));
            bool 손익분기적용할까 = 손익분기_checkBox.Checked;
            bool 시장가매매야 = 시장가매매_checkBox.Checked;
            int 기준가 = 체결가;
            double 손익분기가;
            int 시장 = 시장_label.Text == "코스피" ? 0 : 1;
            int 호가단위;
            string 현재가_문자;
            int 현재가;

            if (손익분기적용할까 == true)
            {
                손익분기가 = (double)(체결가 * 주식수수료율);
                기준가 = (int)Math.Ceiling((double)체결가 * 주식수수료율);
                호가단위 = 호가단위_알려줘(기준가, 시장);
                if (기준가 % 호가단위 != 0)
                {
                    기준가 = 기준가 - (기준가 % 호가단위) + 호가단위;
                }
                
                체결시간_label.Text = String.Format("선택시간: {0}", _선택된체결시간);
                선택가_label.Text = String.Format("선택가: {0:#,#}", 체결가);
                기준가_label.Text = String.Format("손익분기가: {0:#,#}", 기준가);
            } else
            {
                손익분기가 = (double)체결가;
                체결시간_label.Text = String.Format("선택시간: {0}", _선택된체결시간);
                선택가_label.Text = String.Format("선택가: {0:#,#}", 체결가);
                기준가_label.Text = String.Format("기준가: {0:#,#}", 기준가);
            }
            
            int 기준틱대비;
            double 수익률;
            
            foreach (DataGridViewRow 행 in 주식체결_dataGridView.Rows)
            {
                if (시장가매매야 == true)
                {
                    현재가_문자 = Convert.ToString(행.Cells["매수호가_DataGridViewTextBoxColumn"].Value);
                    if (현재가_문자 == "" || 현재가_문자 == "0")
                    {
                        현재가_문자 = Convert.ToString(행.Cells["체결가_DataGridViewTextBoxColumn"].Value);
                    }
                } else
                {
                    현재가_문자 = Convert.ToString(행.Cells["체결가_DataGridViewTextBoxColumn"].Value);
                }
                현재가 = Int32.Parse(현재가_문자.Replace(",", ""));
                기준틱대비 = 틱간격_알려줘(기준가, 현재가, 시장);
                수익률 = (((double)현재가 / (double)손익분기가) - 1) * 100;
                //Console.WriteLine("현재가: {0:#,#},기준틱대비: {1:#,#}, 수익률: {2:0.##}\r\n", 현재가, 기준틱대비, 수익률);
                if (수익률 > 0)
                {
                    if (수익률 <= 2)
                    {
                        행.Cells["수익률_DataGridViewTextBoxColumn"].Style = _약한레드셀스타일;
                    }
                    else if (수익률 > 2 && 수익률 <= 15)
                    {
                        행.Cells["수익률_DataGridViewTextBoxColumn"].Style = _레드셀스타일;
                    }
                    else
                    {
                        행.Cells["수익률_DataGridViewTextBoxColumn"].Style = _강한레드셀스타일;
                    }
                }
                else if (수익률 < 0)
                {
                    if (수익률 >= -2)
                    {
                        행.Cells["수익률_DataGridViewTextBoxColumn"].Style = _약한블루셀스타일;
                    }
                    else if (수익률 < -2 && 수익률 >= -15)
                    {
                        행.Cells["수익률_DataGridViewTextBoxColumn"].Style = _블루셀스타일;
                    }
                    else
                    {
                        행.Cells["수익률_DataGridViewTextBoxColumn"].Style = _강한블루셀스타일;
                    }
                }
                else
                {
                    행.Cells["수익률_DataGridViewTextBoxColumn"].Style = _블랙셀스타일;
                }
                행.Cells["수익률_DataGridViewTextBoxColumn"].Value = String.Format("{0:#,0} ( {1:0.00}% )", 기준틱대비, 수익률);

                Console.WriteLine(Convert.ToString(행.Cells["시간_DataGridViewTextBoxColumn"].Value));
            }
            //Console.WriteLine("체결가 {0:#,#} 기준가 {1:#,#} 대비 수익률 계산 완료되었습니다.", 체결가, 기준가);
        }
    }
}