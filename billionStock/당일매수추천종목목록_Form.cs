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
    public partial class 당일매수추천종목목록_Form : Form
    {
        private 당일매수추천종목목록_Form _당일매수추천종목목록이야;
        private 당일주식체결_Form _당일주식체결이야_A;
        private 당일주식체결_Form _당일주식체결이야_B;
        private 주식종합_Form _주식종합이야;

        //private double 주식수수료율 = 1.0037869;
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
        
        private int _종목개수 = 0;
        private int _코스피개수 = 0;
        private int _코스닥개수 = 0;

        private int _선택된시장 = 1;
        private string _선택된종목코드 = "";
        private int _선택된체결가 = 1000;
        private string _선택된체결시간 = "9:01:00";
        
        private SoundPlayer _사운드플레이어;
        
        Thread _실시간스레드;
        int _실시간주기 = 3000;
        bool _실시간이니 = false;
        bool _작업중이니 = false;
        bool _테스트니 = false;

        private Dictionary<string, 손익분기계산기_Form> _종목명의손익분기계산기_사전;
        private Dictionary<string, bool> _종목코드의세력매수대금십억이상종목_사전;

        public void 실시간스레드_멈춰줘()
        {
            _실시간스레드.Abort();
        }

        public 당일매수추천종목목록_Form()
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

            _종목명의손익분기계산기_사전 = new Dictionary<string, 손익분기계산기_Form>();
            _종목코드의세력매수대금십억이상종목_사전 = new Dictionary<string, bool>();
            
            _실시간스레드 = new Thread(delegate ()
            {
                while (true)
                {
                    try
                    {
                        if (IsHandleCreated)
                        {
                            _당일매수추천종목목록이야.Invoke(new Action(() => {
                                _실시간이니 = 실시간_checkBox.Checked;
                                _실시간주기 = Convert.ToInt32(주기_numericUpDown.Value);
                            }));

                            if (_실시간이니 == true && _작업중이니 == false)
                            {
                                if (_실시간주기 <= 1000)
                                {
                                    _실시간주기 = 1000;
                                }

                                _당일매수추천종목목록이야.Invoke(new Action(() => {
                                    당일매수추천종목목록_가져오자();
                                }));
                                Thread.Sleep(_실시간주기);
                            }
                        }
                        Thread.Sleep(100);
                    }
                    catch (Exception EX)
                    {
                        Console.WriteLine("당일매수추천종목목록 실시간스레드 오류입니다. EX: {0}", EX);
                    }
                }
            });
            _실시간스레드.IsBackground = true;
            _실시간스레드.Start();
        }

        private 손익분기계산기_Form 손익분기계산기_만들어줘(string 종목명)
        {
            if (_종목명의손익분기계산기_사전.ContainsKey(종목명) == true)
            {
                return _종목명의손익분기계산기_사전[종목명];
            } else
            {
                손익분기계산기_Form 손익분기계산기 = new 손익분기계산기_Form();
                손익분기계산기.FormClosed += new FormClosedEventHandler(창_닫혔어);
                손익분기계산기.Text = 종목명;
                _종목명의손익분기계산기_사전[종목명] = 손익분기계산기;
                손익분기계산기.Show();
                return 손익분기계산기;
            }
        }

        private void 당일매수추천종목목록_Form_Load(object sender, EventArgs e)
        {
            정렬방식_comboBox.SelectedItem = "내림차순";
            정렬필드_comboBox.SelectedItem = "첫추출시간";
            시장_comboBox.SelectedItem = "전체";

            당일매수추천종목목록_dataGridView.ClearSelection();
        }

        public void 당일매수추천종목목록_Form_저장해줘(당일매수추천종목목록_Form 당일매수추천종목목록이야)
        {
            _당일매수추천종목목록이야 = 당일매수추천종목목록이야;
        }

        public void 당일주식체결_Form_저장해줘_A(당일주식체결_Form 당일주식체결이야)
        {
            _당일주식체결이야_A = 당일주식체결이야;
        }
        public void 당일주식체결_Form_저장해줘_B(당일주식체결_Form 당일주식체결이야)
        {
            _당일주식체결이야_B = 당일주식체결이야;
        }

        public void 주식종합_Form_저장해줘(주식종합_Form 주식종합이야)
        {
            _주식종합이야 = 주식종합이야;
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

        public void 창_닫혔어(object sender, FormClosedEventArgs e)
        {
            Form 닫힌창 = (Form)sender;
            string 닫힌창_제목 = 닫힌창.Text;
            if (_종목명의손익분기계산기_사전.ContainsKey(닫힌창_제목) == true)
            {
                _종목명의손익분기계산기_사전.Remove(닫힌창_제목);
            }
        }

        private void 당일매수추천종목목록_가져오자()
        {
            try
            {
                if (_작업중이니 == true) { return; }
                _작업중이니 = true;

                bool 매수추천있니 = false;

                //bool 장초반제외 = 장초반제외_checkBox.Checked;
                //bool 점심시간제외 = 점심시간제외_checkBox.Checked;
                bool 완벽급등주 = 완벽급등주_checkBox.Checked;
                bool 수급매매 = 수급매매_checkBox.Checked;
                bool 상승추세만 = 상승추세만_checkBox.Checked;
                bool 시장가매매 = 시장가매매_checkBox.Checked;
                double 실시간주기초 = (double)_실시간주기 / 1000;
                double 세력순수익률 = 0;

                int 새시장 = 1;
                string 새종목명 = "새종목명";
                string 새종목코드 = "000000";
                int 새끝추출호가 = 1;
                string 새첫추출시간 = "";
                string 새끝추출시간 = "";

                string 임시시간 = "";

                _코스피개수 = 0;
                _코스닥개수 = 0;
                
                TimeSpan 초차이_스팬;
                int 초차이;

                int 최소시간 = 90000;
                int 최대시간 = 152059;

                string 정렬방식_임시 = "내림차순";
                int 정렬방식 = -1;
                string 정렬필드 = "첫추출시간";
                int 시장 = -1;

                int 최소시 = 9;
                int 최소분 = 0;
                int 최소초 = 0;
                int 최대시 = 15;
                int 최대분 = 20;
                int 최대초 = 59;
                int 최소가격 = 0;
                int 최대가격 = 9990;
                
                DateTime date = DateTime.Now;
                DateTime 최대시간_테스트용;

                _테스트니 = 테스트_checkBox.Checked;
                if (_테스트니 == true)
                {
                    최대시 = Convert.ToInt32(최대시_numericUpDown.Value);
                    최대분 = Convert.ToInt32(최대분_numericUpDown.Value);
                    최대초 = Convert.ToInt32(최대초_numericUpDown.Value);

                    최대시간_테스트용 = new DateTime(date.Year, date.Month, date.Day, 최대시, 최대분, 최대초);
                    최대시간_테스트용 = 최대시간_테스트용.AddSeconds(2);
                    최대시_numericUpDown.Value = 최대시간_테스트용.Hour;
                    최대분_numericUpDown.Value = 최대시간_테스트용.Minute;
                    최대초_numericUpDown.Value = 최대시간_테스트용.Second;
                }
                
                정렬방식_임시 = 정렬방식_comboBox.Text;
                정렬필드 = 정렬필드_comboBox.Text;
                시장 = 시장_comboBox.Text == "전체" ? -1 : (시장_comboBox.Text == "코스피" ? 0 : 1);
                최소시 = Convert.ToInt32(최소시_numericUpDown.Value);
                최소분 = Convert.ToInt32(최소분_numericUpDown.Value);
                최소초 = Convert.ToInt32(최소초_numericUpDown.Value);
                최대시 = Convert.ToInt32(최대시_numericUpDown.Value);
                최대분 = Convert.ToInt32(최대분_numericUpDown.Value);
                최대초 = Convert.ToInt32(최대초_numericUpDown.Value);
                최소가격 = Convert.ToInt32(최소가격_numericUpDown.Value);
                최대가격 = Convert.ToInt32(최대가격_numericUpDown.Value);

                /* 실전 */
                //bool 수급매매_실시간 = true;
                DateTime 지금 = DateTime.Now;
                /* Verification */
                //DateTime date = DateTime.Now;
                bool 수급매매_실시간 = false;
                //DateTime 지금 = new DateTime(date.Year, date.Month, date.Day, 최대시, 최대분, 최대초);

                //Console.WriteLine("지금시간은 " + 지금 + " 입니다.");

                정렬방식 = 정렬방식_임시 == "내림차순" ? -1 : 1;

                DateTime 최소시간_임시 = DateTime.Now;

                if (최대시 == 0 && 최대분 == 0 && 최대초 == 0) /* 시간전, 분전, 초전  스타일 */
                {
                    최소시간_임시 = 최소시간_임시.AddHours(-(최소시));
                    최소시간_임시 = 최소시간_임시.AddMinutes(-(최소분));
                    최소시간_임시 = 최소시간_임시.AddSeconds(-(최소초));
                    최소시간 = Int32.Parse(최소시간_임시.Hour +
                        (최소시간_임시.Minute < 10 ? "0" + 최소시간_임시.Minute : 최소시간_임시.Minute + "") +
                        (최소시간_임시.Second < 10 ? "0" + 최소시간_임시.Second : 최소시간_임시.Second + ""));
                }
                else
                {
                    최소시간 = Int32.Parse(최소시 +
                        (최소분 < 10 ? "0" + 최소분 : 최소분 + "") +
                        (최소초 < 10 ? "0" + 최소초 : 최소초 + ""));
                    최대시간 = Int32.Parse(최대시 +
                        (최대분 < 10 ? "0" + 최대분 : 최대분 + "") +
                        (최대초 < 10 ? "0" + 최대초 : 최대초 + ""));
                }

                //Console.WriteLine("최소시간: {0}", 최소시간);
                //Console.WriteLine("최대시간: {0}", 최대시간);
                //Console.WriteLine("최소가격: {0}", 최소가격);
                //Console.WriteLine("최대가격: {0}", 최대가격);
                //Console.WriteLine("정렬방식: {0}", 정렬방식);
                //Console.WriteLine("정렬필드: {0}", 정렬필드);

                BsonDocument 시간범위;
                if (최대시간 == 0)
                {
                    시간범위 = new BsonDocument { { "$gte", 최소시간 } };
                }
                else
                {
                    시간범위 = new BsonDocument { { "$gte", 최소시간 }, { "$lte", 최대시간 } };
                }

                var 매수추천종목_pipeline = new BsonDocument[] {
                new BsonDocument{
                    {
                        "$sort", new BsonDocument{
                            {
                                "code", 1
                            },{
                                "time", -1
                            }
                        }
                    }
                },
                new BsonDocument{
                    {
                        "$match", new BsonDocument{
                            {
                                "time", 시간범위
                            }, {
                                "current", new BsonDocument{
                                    {
                                        "$gte", (int)(최소가격 * 0.7)
                                    },{
                                        "$lte", (int)(최대가격 * 1.3)
                                    }
                                }
                            }
                        }
                    }
                },
                new BsonDocument{{
                        "$group", new BsonDocument{
                            { "_id", "$code"},
                            { "name", new BsonDocument{{ "$last","$name"}}},
                            { "timeList", new BsonDocument{{ "$push","$time"}}},
                            { "netChangeRateList", new BsonDocument{{ "$push", "$netChangeRate" } }},
                            { "currentList", new BsonDocument{{ "$push", "$current" } }},
                            { "firstOfferList", new BsonDocument{{ "$push", "$firstOffer" } }},
                            { "firstBidList", new BsonDocument{{ "$push", "$firstBid" } }},
                            { "cumulativeVolumeList", new BsonDocument{{ "$push", "$cumulativeVolume" } }},
                            { "count", new BsonDocument{{ "$sum", 1}}},
                            { "richBidCost", new BsonDocument{{ "$sum", "$cumulativeBidCost" } }},
                            { "cumulativeVolume", new BsonDocument{{ "$first", "$cumulativeVolume"}}}
                        }
                    }}
                };

                //Console.WriteLine(매수추천종목_pipeline.ToJson());

                //Console.WriteLine("{0} ({1})의 매수추천종목 목록을 불러옵니다..\r\n");
                var 매수추천종목_목록 = _매수추천컬렉션.Aggregate<BsonDocument>(매수추천종목_pipeline).ToList();
                //Console.WriteLine("{0} ({1})의 매수추천종목 목록을 불러왔습니다!\r\n");
                if (매수추천종목_목록 != null && 매수추천종목_목록.Count > 0)
                {
                    당일매수추천종목목록_dataGridView.Rows.Clear();
                    당일매수추천종목목록_dataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.EnableResizing;
                    당일매수추천종목목록_dataGridView.RowHeadersVisible = false;

                    List<매수추천종목> 정렬된_매수추천종목_목록 = new List<매수추천종목>();
                    
                    for (int ㄱ = 0; ㄱ < 매수추천종목_목록.Count; ㄱ++)
                    {
                        매수추천종목 매수추천종목이야 = new 매수추천종목();

                        매수추천종목이야._아이디 = Convert.ToString(매수추천종목_목록[ㄱ]["_id"]);
                        매수추천종목이야._종목명 = Convert.ToString(매수추천종목_목록[ㄱ]["name"]);

                        var 종목_builder = Builders<종목>.Filter;
                        var 종목_query = 종목_builder.Eq(x => x._종목코드, 매수추천종목_목록[ㄱ]["_id"]);
                        var 종목_result = _종목컬렉션.Find(종목_query).Limit(1).ToList();

                        if (종목_result != null && 종목_result.Count > 0)
                        {
                            매수추천종목이야._시장 = Int32.Parse(Convert.ToString(종목_result[0]._시장));
                        }
                        else
                        {
                            매수추천종목이야._시장 = -1;
                        }

                        if (시장 != -1)
                        {
                            if (시장 == 0 && 매수추천종목이야._시장 != 0)
                            {
                                continue;
                            }
                            else if (시장 == 1 && 매수추천종목이야._시장 != 1)
                            {
                                continue;
                            }
                        }
                        
                        var timeList = BsonSerializer.Deserialize<BsonArray>(매수추천종목_목록[ㄱ]["timeList"].ToJson());
                        var netChangeRateList = BsonSerializer.Deserialize<BsonArray>(매수추천종목_목록[ㄱ]["netChangeRateList"].ToJson());
                        var currentList = BsonSerializer.Deserialize<BsonArray>(매수추천종목_목록[ㄱ]["currentList"].ToJson());
                        var firstOfferList = BsonSerializer.Deserialize<BsonArray>(매수추천종목_목록[ㄱ]["firstOfferList"].ToJson());
                        var firstBidList = BsonSerializer.Deserialize<BsonArray>(매수추천종목_목록[ㄱ]["firstBidList"].ToJson());
                        var cumulativeVolumeList = BsonSerializer.Deserialize<BsonArray>(매수추천종목_목록[ㄱ]["cumulativeVolumeList"].ToJson());

                        if (
                            timeList == null ||
                            netChangeRateList == null ||
                            currentList == null ||
                            firstOfferList == null ||
                            firstBidList == null ||
                            cumulativeVolumeList == null ||
                            timeList.Count <= 0 ||
                            netChangeRateList.Count <= 0 ||
                            currentList.Count <= 0 ||
                            firstOfferList.Count <= 0 ||
                            firstBidList.Count <= 0 ||
                            cumulativeVolumeList.Count <= 0
                            )
                        {
                            continue;
                        }

                        int 시간1 = 0;
                        int 시간2 = 0;
                        int 시간3 = 0;
                        int 시간4 = 0;
                        int 시간5 = 0;
                        long 누적거래량2 = 0;
                        double 최신등락율 = 0;
                        TimeSpan 수급초차이_스팬;
                        int 수급초차이;
                        int 수급초차이2;
                        int 수급매매_추출개수 = 0;

                        매수추천종목이야._매수추천있니 = false;

                        if (수급매매 == true)
                        {
                            if (
                                timeList.Count < 2 ||
                                netChangeRateList.Count < 2 ||
                                currentList.Count < 2 ||
                                firstOfferList.Count < 2 ||
                                firstBidList.Count < 2 ||
                                cumulativeVolumeList.Count < 2
                                )
                            {
                                continue;
                            }

                            if ( 완벽급등주 == true && netChangeRateList.Count < 3)
                            {
                                continue;
                            }

                            /*  30초 내 수급 들어오는 급등주매매 */
                            bool 급등주매매 = false;
                            int 급등주_인덱스 = 0;

                            for (int x = 0; x < timeList.Count; x++)
                            {
                                if (완벽급등주 == true)
                                {
                                    if ((x + 2) < timeList.Count)
                                    {
                                        시간3 = Int32.Parse(Convert.ToString(timeList[x]));
                                        시간4 = Int32.Parse(Convert.ToString(timeList[x + 1]));
                                        시간5 = Int32.Parse(Convert.ToString(timeList[x + 2]));

                                        수급초차이_스팬 = DateTime으로_알려줘(시간3.ToString()) - DateTime으로_알려줘(시간4.ToString());
                                        수급초차이 = (int)수급초차이_스팬.TotalSeconds;

                                        수급초차이_스팬 = DateTime으로_알려줘(시간4.ToString()) - DateTime으로_알려줘(시간5.ToString());
                                        수급초차이2 = (int)수급초차이_스팬.TotalSeconds;

                                        if (수급초차이 <= 15 && 수급초차이2 <= 15)
                                        {

                                            if (수급매매_실시간 == true)
                                            {
                                                if (x == 0)
                                                {
                                                    급등주_인덱스 = 0;
                                                    급등주매매 = true;
                                                    매수추천종목이야._매수추천있니 = true;
                                                }
                                            }
                                            else
                                            {
                                                급등주_인덱스 = x;
                                                급등주매매 = true;
                                                if (x == 0)
                                                {
                                                    매수추천종목이야._매수추천있니 = true;
                                                }
                                            }
                                            수급매매_추출개수++;
                                        }
                                    }
                                } else
                                {
                                    if ((x + 1) < timeList.Count)
                                    {
                                        시간3 = Int32.Parse(Convert.ToString(timeList[x]));
                                        시간4 = Int32.Parse(Convert.ToString(timeList[x + 1]));
                                        수급초차이_스팬 = DateTime으로_알려줘(시간3.ToString()) - DateTime으로_알려줘(시간4.ToString());
                                        수급초차이 = (int)수급초차이_스팬.TotalSeconds;
                                        if (수급초차이 > 1 && 수급초차이 <= 15)
                                        {
                                            if (수급매매_실시간 == true)
                                            {
                                                if (x == 0)
                                                {
                                                    급등주_인덱스 = 0;
                                                    급등주매매 = true;
                                                    매수추천종목이야._매수추천있니 = true;
                                                }
                                            }
                                            else
                                            {
                                                급등주_인덱스 = x;
                                                급등주매매 = true;
                                                if (x == 0)
                                                {
                                                    매수추천종목이야._매수추천있니 = true;
                                                }
                                            }
                                            수급매매_추출개수++;
                                        }
                                    }
                                }
                            }

                            if (급등주매매 == false)
                            {
                                continue;
                            }

                            시간2 = Int32.Parse(Convert.ToString(timeList[급등주_인덱스]));
                            누적거래량2 = long.Parse(Convert.ToString(cumulativeVolumeList[급등주_인덱스]));

                            매수추천종목이야._첫추출시간 = 시간2;
                            double 우선등락율 = double.Parse(Convert.ToString(netChangeRateList[netChangeRateList.Count - 1]));
                            매수추천종목이야._첫추출등락율 = double.Parse(Convert.ToString(netChangeRateList[급등주_인덱스]));

                            if (상승추세만 == true && 우선등락율 < 0)
                            {
                                continue;
                            }

                            최신등락율 = double.Parse(Convert.ToString(netChangeRateList[netChangeRateList.Count - 2]));


                            매수추천종목이야._첫추출등락율 = double.Parse(Convert.ToString(netChangeRateList[급등주_인덱스]));
                            시간2 = Int32.Parse(Convert.ToString(timeList[급등주_인덱스]));
                            누적거래량2 = long.Parse(Convert.ToString(cumulativeVolumeList[급등주_인덱스]));

                            매수추천종목이야._첫추출시간 = 시간2;

                            if (상승추세만 == true && 최신등락율 < 0)
                            {
                                continue;
                            }

                            if (시장가매매 == true)
                            {
                                매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(firstOfferList[급등주_인덱스]));
                                if (매수추천종목이야._첫추출호가 <= 0)
                                {
                                    매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(currentList[급등주_인덱스]));
                                }
                            }
                            else
                            {
                                매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(currentList[급등주_인덱스]));
                            }

                            /*
                            if (우선등락율 > 최신등락율) {
                                if (netChangeRateList.Count < 3)
                                {
                                    continue;
                                }
                                else
                                {
                                    매수추천종목이야._첫추출등락율 = double.Parse(Convert.ToString(netChangeRateList[급등주_인덱스]));

                                    시간2 = Int32.Parse(Convert.ToString(timeList[급등주_인덱스]));
                                    누적거래량2 = long.Parse(Convert.ToString(cumulativeVolumeList[급등주_인덱스]));

                                    매수추천종목이야._첫추출시간 = 시간2;

                                    if (상승추세만 == true && 최신등락율 < 1)
                                    {
                                        continue;
                                    }

                                    if (시장가매매 == true)
                                    {
                                        매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(firstOfferList[급등주_인덱스]));
                                        if (매수추천종목이야._첫추출호가 <= 0)
                                        {
                                            매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(currentList[급등주_인덱스]));
                                        }
                                    }
                                    else
                                    {
                                        매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(currentList[급등주_인덱스]));
                                    }
                                }
                            } else
                            {
                                if (상승추세만 == true && 최신등락율 < 1)
                                {
                                    continue;
                                }

                                if (시장가매매 == true)
                                {
                                    매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(firstOfferList[급등주_인덱스]));
                                    if (매수추천종목이야._첫추출호가 <= 0)
                                    {
                                        매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(currentList[급등주_인덱스]));
                                    }
                                }
                                else
                                {
                                    매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(currentList[급등주_인덱스]));
                                }
                            }
                            */
                        } else
                        {
                            매수추천종목이야._첫추출시간 = Int32.Parse(Convert.ToString(timeList[timeList.Count - 1]));
                            매수추천종목이야._첫추출등락율 = double.Parse(Convert.ToString(netChangeRateList[netChangeRateList.Count - 1]));
                            if (시장가매매 == true)
                            {
                                매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(firstOfferList[firstOfferList.Count - 1]));
                                if (매수추천종목이야._첫추출호가 <= 0)
                                {
                                    매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(currentList[currentList.Count - 1]));
                                }
                            }
                            else
                            {
                                매수추천종목이야._첫추출호가 = Int32.Parse(Convert.ToString(currentList[currentList.Count - 1]));
                            }
                        }
                        
                        매수추천종목이야._끝추출시간 = Int32.Parse(Convert.ToString(timeList[0]));
                        매수추천종목이야._끝추출등락율 = double.Parse(Convert.ToString(netChangeRateList[0]));
                        if ( 시장가매매 == true)
                        {
                            매수추천종목이야._끝추출호가 = Int32.Parse(Convert.ToString(firstOfferList[0]));
                            if (매수추천종목이야._끝추출호가 <= 0)
                            {
                                매수추천종목이야._끝추출호가 = Int32.Parse(Convert.ToString(currentList[0]));
                            }
                        } else
                        {
                            매수추천종목이야._끝추출호가 = Int32.Parse(Convert.ToString(currentList[0]));
                        }

                        /*if (
                            (장초반제외 == true && 매수추천종목이야._첫추출시간 < 90500) ||
                            ((점심시간제외 == true && 매수추천종목이야._첫추출시간 >= 110000) && (점심시간제외 == true && 매수추천종목이야._첫추출시간 <= 130000))
                        )
                        {
                            continue;
                        }*/
                        
                        if (상승추세만 == true && 매수추천종목이야._첫추출등락율 < 0)
                        {
                            continue;
                        }

                        if (
                            매수추천종목이야._첫추출호가 < 최소가격 ||
                            매수추천종목이야._첫추출호가 > 최대가격
                            )
                        {
                            continue;
                        }

                        매수추천종목이야._현재가 = Int32.Parse(Convert.ToString(currentList[0]));

                        매수추천종목이야._최우선매도호가 = Int32.Parse(Convert.ToString(firstOfferList[0]));

                        매수추천종목이야._최우선매수호가 = Int32.Parse(Convert.ToString(firstBidList[0]));

                        if (수급매매 == true)
                        {
                            매수추천종목이야._추출개수 = 수급매매_추출개수;
                        } else
                        {
                            매수추천종목이야._추출개수 = Int32.Parse(Convert.ToString(매수추천종목_목록[ㄱ]["count"]));
                        }
                        
                        매수추천종목이야._세력순수익률 = (((double)매수추천종목이야._끝추출호가 / (double)((double)매수추천종목이야._첫추출호가 * 주식수수료율)) - 1) * 100;

                        매수추천종목이야._세력매수거래대금 = long.Parse(Convert.ToString(매수추천종목_목록[ㄱ]["richBidCost"]));

                        매수추천종목이야._누적거래량 = long.Parse(Convert.ToString(매수추천종목_목록[ㄱ]["cumulativeVolume"]));

                        정렬된_매수추천종목_목록.Add(매수추천종목이야);
                        //Console.WriteLine("종목명: {0}\r\n종목코드: {1}\r\n\r\n", 매수추천종목이야._종목명, 매수추천종목이야._아이디);
                    }
                    //Console.WriteLine("매수추천종목 목록을 정렬합니다.\r\n");
                    정렬된_매수추천종목_목록.Sort(
                        delegate (매수추천종목 종목1, 매수추천종목 종목2) {
                            int 반환값 = 0;
                            반환값 =  종목2._추출개수.CompareTo(종목1._추출개수);
                            if (반환값 != 0)
                            {
                                return 반환값;
                            } else
                            {
                                반환값 = 종목2._세력매수거래대금.CompareTo(종목1._세력매수거래대금);
                                if (반환값 != 0)
                                {
                                    return 반환값;
                                }
                                else
                                {
                                    return 종목2._누적거래량.CompareTo(종목1._누적거래량);
                                }
                            }
                        });
                    for (int x = 0; x < 정렬된_매수추천종목_목록.Count; x++)
                    {
                        정렬된_매수추천종목_목록[x]._단계 = "대기";
                        정렬된_매수추천종목_목록[x]._순위 = (x + 1);
                    }

                    정렬된_매수추천종목_목록.Sort(
                        delegate (매수추천종목 종목1, 매수추천종목 종목2) {
                        // 종목명 | 종목코드 | 첫추출시간 | 첫추출등락율 | 첫추출호가 | 끝추출시간 | 끝추출시간 | 끝추출등락율 | 끝추출호가 | 추출개수 | 세력매수거래대금 | 누적거래량
                        if (정렬필드 == "종목코드")
                            {
                                if (정렬방식 == 1) { return 종목1._아이디.CompareTo(종목2._아이디); }
                                else { return 종목2._아이디.CompareTo(종목1._아이디); }
                            }
                            else if (정렬필드 == "종목명")
                            {
                                if (정렬방식 == 1) { return 종목1._종목명.CompareTo(종목2._종목명); }
                                else { return 종목2._종목명.CompareTo(종목1._종목명); }
                            }
                            else if (정렬필드 == "첫추출시간")
                            {
                                if (정렬방식 == 1) { return 종목1._첫추출시간.CompareTo(종목2._첫추출시간); }
                                else { return 종목2._첫추출시간.CompareTo(종목1._첫추출시간); }
                            }
                            else if (정렬필드 == "첫추출등락율")
                            {
                                if (정렬방식 == 1) { return 종목1._첫추출등락율.CompareTo(종목2._첫추출등락율); }
                                else { return 종목2._첫추출등락율.CompareTo(종목1._첫추출등락율); }
                            }
                            else if (정렬필드 == "첫추출호가")
                            {
                                if (정렬방식 == 1) { return 종목1._첫추출호가.CompareTo(종목2._첫추출호가); }
                                else { return 종목2._첫추출호가.CompareTo(종목1._첫추출호가); }
                            }
                            else if (정렬필드 == "끝추출시간")
                            {
                                if (정렬방식 == 1) { return 종목1._끝추출시간.CompareTo(종목2._끝추출시간); }
                                else { return 종목2._끝추출시간.CompareTo(종목1._끝추출시간); }
                            }
                            else if (정렬필드 == "끝추출등락율")
                            {
                                if (정렬방식 == 1) { return 종목1._끝추출등락율.CompareTo(종목2._끝추출등락율); }
                                else { return 종목2._끝추출등락율.CompareTo(종목1._끝추출등락율); }
                            }
                            else if (정렬필드 == "끝추출호가")
                            {
                                if (정렬방식 == 1) { return 종목1._끝추출호가.CompareTo(종목2._끝추출호가); }
                                else { return 종목2._끝추출호가.CompareTo(종목1._끝추출호가); }
                            }
                            else if (정렬필드 == "추출개수")
                            {
                                if (정렬방식 == 1) { return 종목1._추출개수.CompareTo(종목2._추출개수); }
                                else { return 종목2._추출개수.CompareTo(종목1._추출개수); }
                            }
                            else if (정렬필드 == "세력순수익률")
                            {
                                if (정렬방식 == 1) { return 종목1._세력순수익률.CompareTo(종목2._세력순수익률); }
                                else { return 종목2._세력순수익률.CompareTo(종목1._세력순수익률); }
                            }
                            else if (정렬필드 == "세력매수거래대금")
                            {
                                if (정렬방식 == 1) { return 종목1._세력매수거래대금.CompareTo(종목2._세력매수거래대금); }
                                else { return 종목2._세력매수거래대금.CompareTo(종목1._세력매수거래대금); }
                            }
                            else if (정렬필드 == "누적거래량")
                            {
                                if (정렬방식 == 1) { return 종목1._누적거래량.CompareTo(종목2._누적거래량); }
                                else { return 종목2._누적거래량.CompareTo(종목1._누적거래량); }
                            }
                            else
                            {
                                return 0;
                            }
                        });

                    if (_주식종합이야 != null)
                    {
                        _주식종합이야.추천종목_업데이트해줘(정렬된_매수추천종목_목록);
                    }

                    for (int ㄴ = 0; ㄴ < 정렬된_매수추천종목_목록.Count; ㄴ++)
                    {
                        당일매수추천종목목록_dataGridView.Rows.Add();
                        당일매수추천종목목록_dataGridView["순위_DataGridViewTextBoxColumn", ㄴ].Value = 정렬된_매수추천종목_목록[ㄴ]._순위; /*(ㄴ + 1).ToString();*/
                        당일매수추천종목목록_dataGridView["종목명_DataGridViewTextBoxColumn", ㄴ].Value = 정렬된_매수추천종목_목록[ㄴ]._종목명;
                        당일매수추천종목목록_dataGridView["종목코드_DataGridViewTextBoxColumn", ㄴ].Value = 정렬된_매수추천종목_목록[ㄴ]._아이디;
                        if (정렬된_매수추천종목_목록[ㄴ]._시장 == 0)
                        {
                            _코스피개수++;
                            당일매수추천종목목록_dataGridView["시장_DataGridViewTextBoxColumn", ㄴ].Style = _코스피셀스타일;
                            당일매수추천종목목록_dataGridView["시장_DataGridViewTextBoxColumn", ㄴ].Value = "코스피";
                        }
                        else
                        {
                            _코스닥개수++;
                            당일매수추천종목목록_dataGridView["시장_DataGridViewTextBoxColumn", ㄴ].Style = _코스닥셀스타일;
                            당일매수추천종목목록_dataGridView["시장_DataGridViewTextBoxColumn", ㄴ].Value = "코스닥";
                        }
                        string 첫추출시간 = (정렬된_매수추천종목_목록[ㄴ]._첫추출시간).ToString();
                        string 끝추출시간 = (정렬된_매수추천종목_목록[ㄴ]._끝추출시간).ToString();

                        초차이_스팬 = 지금 - DateTime으로_알려줘(첫추출시간);
                        초차이 = (int)초차이_스팬.TotalSeconds;

                        if (초차이 < (실시간주기초 + 0) && 초차이 >= 0)
                        {
                            
                            if (수급매매 == false)
                            {
                                당일매수추천종목목록_dataGridView["첫추출시간_DataGridViewTextBoxColumn", ㄴ].Style = _그린셀스타일;
                                매수추천있니 = true;
                                새시장 = 정렬된_매수추천종목_목록[ㄴ]._시장;
                                새종목명 = 정렬된_매수추천종목_목록[ㄴ]._종목명;
                                새종목코드 = 정렬된_매수추천종목_목록[ㄴ]._아이디;
                                새끝추출호가 = 정렬된_매수추천종목_목록[ㄴ]._끝추출호가;
                                새첫추출시간 = 정렬된_매수추천종목_목록[ㄴ]._첫추출시간.ToString();
                                새끝추출시간 = 정렬된_매수추천종목_목록[ㄴ]._끝추출시간.ToString();

                                if (새끝추출시간.Length == 5)
                                {
                                    임시시간 = String.Format("{0}:{1}:{2}"
                                        , 새끝추출시간.Substring(0, 1)
                                        , 새끝추출시간.Substring(1, 2)
                                        , 새끝추출시간.Substring(3, 2)
                                        );
                                }
                                else
                                {
                                    임시시간 = String.Format("{0}:{1}:{2}"
                                        , 새끝추출시간.Substring(0, 2)
                                        , 새끝추출시간.Substring(2, 2)
                                        , 새끝추출시간.Substring(4, 2)
                                        );
                                }
                                로그_listBox.Items.Add(String.Format("{0} {1} ({2}) = {3:#,0}원", 임시시간, 새종목명, 새종목코드, 새끝추출호가));
                            } else
                            {
                                if (정렬된_매수추천종목_목록[ㄴ]._매수추천있니 == true)
                                {
                                    당일매수추천종목목록_dataGridView["첫추출시간_DataGridViewTextBoxColumn", ㄴ].Style = _그린셀스타일;
                                }
                            }
                        }
                        else if (초차이 < 60 && 초차이 >= (실시간주기초 + 0))
                        {
                            if (수급매매 == false)
                            {
                                당일매수추천종목목록_dataGridView["첫추출시간_DataGridViewTextBoxColumn", ㄴ].Style = _강한그린셀스타일;
                            } else
                            {
                                if (정렬된_매수추천종목_목록[ㄴ]._매수추천있니 == true)
                                {
                                    당일매수추천종목목록_dataGridView["첫추출시간_DataGridViewTextBoxColumn", ㄴ].Style = _강한그린셀스타일;
                                }
                            }
                        }
                        초차이_스팬 = 지금 - DateTime으로_알려줘(끝추출시간);
                        초차이 = (int)초차이_스팬.TotalSeconds;

                        if (초차이 < (실시간주기초 + 0) && 초차이 >= 0)
                        {
                            
                            /*
                                (수급매매 == true) &&
                                (정렬된_매수추천종목_목록[ㄴ]._세력매수거래대금 >= 500000000) &&
                                _종목코드의세력매수대금십억이상종목_사전.ContainsKey(정렬된_매수추천종목_목록[ㄴ]._아이디) != true
                             */

                            if (수급매매 == true)
                            {
                                if (정렬된_매수추천종목_목록[ㄴ]._매수추천있니 == true)
                                {
                                    당일매수추천종목목록_dataGridView["끝추출시간_DataGridViewTextBoxColumn", ㄴ].Style = _그린셀스타일;
                                    // _종목코드의세력매수대금십억이상종목_사전[정렬된_매수추천종목_목록[ㄴ]._아이디] = true;
                                    매수추천있니 = true;
                                    새시장 = 정렬된_매수추천종목_목록[ㄴ]._시장;
                                    새종목명 = 정렬된_매수추천종목_목록[ㄴ]._종목명;
                                    새종목코드 = 정렬된_매수추천종목_목록[ㄴ]._아이디;
                                    새끝추출호가 = 정렬된_매수추천종목_목록[ㄴ]._끝추출호가;
                                    새첫추출시간 = 정렬된_매수추천종목_목록[ㄴ]._첫추출시간.ToString();
                                    새끝추출시간 = 정렬된_매수추천종목_목록[ㄴ]._끝추출시간.ToString();

                                    if (새끝추출시간.Length == 5)
                                    {
                                        임시시간 = String.Format("{0}:{1}:{2}"
                                            , 새끝추출시간.Substring(0, 1)
                                            , 새끝추출시간.Substring(1, 2)
                                            , 새끝추출시간.Substring(3, 2)
                                            );
                                    }
                                    else
                                    {
                                        임시시간 = String.Format("{0}:{1}:{2}"
                                            , 새끝추출시간.Substring(0, 2)
                                            , 새끝추출시간.Substring(2, 2)
                                            , 새끝추출시간.Substring(4, 2)
                                            );
                                    }
                                    로그_listBox.Items.Add(String.Format("{0} {1} ({2}) = {3:#,0}원", 임시시간, 새종목명, 새종목코드, 새끝추출호가));
                                }
                            } else
                            {
                                당일매수추천종목목록_dataGridView["끝추출시간_DataGridViewTextBoxColumn", ㄴ].Style = _그린셀스타일;
                            }
                        }
                        else if (초차이 < 60 && 초차이 >= (실시간주기초 + 0))
                        {
                            if (수급매매 == true)
                            {
                                if (정렬된_매수추천종목_목록[ㄴ]._매수추천있니 == true)
                                {
                                    당일매수추천종목목록_dataGridView["끝추출시간_DataGridViewTextBoxColumn", ㄴ].Style = _강한그린셀스타일;
                                }
                            } else
                            {
                                당일매수추천종목목록_dataGridView["끝추출시간_DataGridViewTextBoxColumn", ㄴ].Style = _강한그린셀스타일;
                            }
                        }

                        if (첫추출시간.Length == 5)
                        {
                            당일매수추천종목목록_dataGridView["첫추출시간_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0}:{1}:{2}", 첫추출시간.Substring(0, 1), 첫추출시간.Substring(1, 2), 첫추출시간.Substring(3, 2));
                        }
                        else
                        {
                            당일매수추천종목목록_dataGridView["첫추출시간_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0}:{1}:{2}", 첫추출시간.Substring(0, 2), 첫추출시간.Substring(2, 2), 첫추출시간.Substring(4, 2));
                        }
                        if (끝추출시간.Length == 5)
                        {
                            당일매수추천종목목록_dataGridView["끝추출시간_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0}:{1}:{2}", 끝추출시간.Substring(0, 1), 끝추출시간.Substring(1, 2), 끝추출시간.Substring(3, 2));
                        }
                        else
                        {
                            당일매수추천종목목록_dataGridView["끝추출시간_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0}:{1}:{2}", 끝추출시간.Substring(0, 2), 끝추출시간.Substring(2, 2), 끝추출시간.Substring(4, 2));
                        }

                        double 첫추출등락율 = 정렬된_매수추천종목_목록[ㄴ]._첫추출등락율;
                        int 첫추출호가 = 정렬된_매수추천종목_목록[ㄴ]._첫추출호가;
                        if (첫추출등락율 > 0)
                        {
                            당일매수추천종목목록_dataGridView["첫추출등락율_DataGridViewTextBoxColumn", ㄴ].Style = _레드셀스타일;
                            당일매수추천종목목록_dataGridView["첫추출호가_DataGridViewTextBoxColumn", ㄴ].Style = _레드셀스타일;
                        }
                        else if (첫추출등락율 < 0)
                        {
                            당일매수추천종목목록_dataGridView["첫추출등락율_DataGridViewTextBoxColumn", ㄴ].Style = _블루셀스타일;
                            당일매수추천종목목록_dataGridView["첫추출호가_DataGridViewTextBoxColumn", ㄴ].Style = _블루셀스타일;
                        }
                        당일매수추천종목목록_dataGridView["첫추출등락율_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0:0.##}%", 첫추출등락율);
                        당일매수추천종목목록_dataGridView["첫추출호가_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0:#,#}", 첫추출호가);
                        double 끝추출등락율 = 정렬된_매수추천종목_목록[ㄴ]._끝추출등락율;
                        int 끝추출호가 = 정렬된_매수추천종목_목록[ㄴ]._끝추출호가;

                        if (끝추출등락율 > 0)
                        {
                            if (끝추출등락율 > 첫추출등락율)
                            {
                                if (첫추출등락율 > 0)
                                {
                                    당일매수추천종목목록_dataGridView["끝추출등락율_DataGridViewTextBoxColumn", ㄴ].Style = _강한레드셀스타일;
                                    당일매수추천종목목록_dataGridView["끝추출호가_DataGridViewTextBoxColumn", ㄴ].Style = _강한레드셀스타일;
                                }
                                else
                                {
                                    당일매수추천종목목록_dataGridView["끝추출등락율_DataGridViewTextBoxColumn", ㄴ].Style = _레드셀스타일;
                                    당일매수추천종목목록_dataGridView["끝추출호가_DataGridViewTextBoxColumn", ㄴ].Style = _레드셀스타일;
                                }
                            }
                            else if (끝추출등락율 < 첫추출등락율)
                            {
                                당일매수추천종목목록_dataGridView["끝추출등락율_DataGridViewTextBoxColumn", ㄴ].Style = _약한레드셀스타일;
                                당일매수추천종목목록_dataGridView["끝추출호가_DataGridViewTextBoxColumn", ㄴ].Style = _약한레드셀스타일;
                            }
                            else
                            {
                                당일매수추천종목목록_dataGridView["끝추출등락율_DataGridViewTextBoxColumn", ㄴ].Style = _레드셀스타일;
                                당일매수추천종목목록_dataGridView["끝추출호가_DataGridViewTextBoxColumn", ㄴ].Style = _레드셀스타일;
                            }
                        }
                        else if (끝추출등락율 < 0)
                        {
                            if (끝추출등락율 < 첫추출등락율)
                            {
                                if (첫추출등락율 < 0)
                                {
                                    당일매수추천종목목록_dataGridView["끝추출등락율_DataGridViewTextBoxColumn", ㄴ].Style = _강한블루셀스타일;
                                    당일매수추천종목목록_dataGridView["끝추출호가_DataGridViewTextBoxColumn", ㄴ].Style = _강한블루셀스타일;
                                }
                                else
                                {
                                    당일매수추천종목목록_dataGridView["끝추출등락율_DataGridViewTextBoxColumn", ㄴ].Style = _블루셀스타일;
                                    당일매수추천종목목록_dataGridView["끝추출호가_DataGridViewTextBoxColumn", ㄴ].Style = _블루셀스타일;
                                }
                            }
                            else if (끝추출등락율 > 첫추출등락율)
                            {
                                당일매수추천종목목록_dataGridView["끝추출등락율_DataGridViewTextBoxColumn", ㄴ].Style = _약한블루셀스타일;
                                당일매수추천종목목록_dataGridView["끝추출호가_DataGridViewTextBoxColumn", ㄴ].Style = _약한블루셀스타일;
                            }
                            else
                            {
                                당일매수추천종목목록_dataGridView["끝추출등락율_DataGridViewTextBoxColumn", ㄴ].Style = _블루셀스타일;
                                당일매수추천종목목록_dataGridView["끝추출호가_DataGridViewTextBoxColumn", ㄴ].Style = _블루셀스타일;
                            }
                        }

                        당일매수추천종목목록_dataGridView["끝추출등락율_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0:0.##}%", 끝추출등락율);
                        당일매수추천종목목록_dataGridView["끝추출호가_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0:#,#}", 끝추출호가);

                        당일매수추천종목목록_dataGridView["추출개수_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0:#,#}", 정렬된_매수추천종목_목록[ㄴ]._추출개수);

                        세력순수익률 = 정렬된_매수추천종목_목록[ㄴ]._세력순수익률;
                        if (세력순수익률 > 0)
                        {
                            if (세력순수익률 <= 2)
                            {
                                당일매수추천종목목록_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄴ].Style = _약한레드셀스타일;
                            }
                            else if (세력순수익률 > 2 && 세력순수익률 <= 15)
                            {
                                당일매수추천종목목록_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄴ].Style = _레드셀스타일;
                            }
                            else
                            {
                                당일매수추천종목목록_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄴ].Style = _강한레드셀스타일;
                            }
                        }
                        else if (세력순수익률 < 0)
                        {
                            if (세력순수익률 >= -2)
                            {
                                당일매수추천종목목록_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄴ].Style = _약한블루셀스타일;
                            }
                            else if (세력순수익률 < -2 && 세력순수익률 >= -15)
                            {
                                당일매수추천종목목록_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄴ].Style = _블루셀스타일;
                            }
                            else
                            {
                                당일매수추천종목목록_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄴ].Style = _강한블루셀스타일;
                            }
                        }
                        당일매수추천종목목록_dataGridView["세력순수익률_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0:0.##}%", 세력순수익률);

                        long 세력매수거래대금 = 정렬된_매수추천종목_목록[ㄴ]._세력매수거래대금;

                        if (세력매수거래대금 >= 1000000000) /* 십억이상 */
                        {
                            당일매수추천종목목록_dataGridView["세력매수거래대금_DataGridViewTextBoxColumn", ㄴ].Style = _강한레드셀스타일;
                        }
                        else if (세력매수거래대금 < 1000000000 && 세력매수거래대금 >= 500000000) /* 오억이상 */
                        {
                            당일매수추천종목목록_dataGridView["세력매수거래대금_DataGridViewTextBoxColumn", ㄴ].Style = _레드셀스타일;
                        }
                        else if (세력매수거래대금 < 500000000 && 세력매수거래대금 >= 100000000) /* 일억이상 */
                        {
                            당일매수추천종목목록_dataGridView["세력매수거래대금_DataGridViewTextBoxColumn", ㄴ].Style = _약한레드셀스타일;
                        }
                        당일매수추천종목목록_dataGridView["세력매수거래대금_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0:#,#}", 정렬된_매수추천종목_목록[ㄴ]._세력매수거래대금);
                        당일매수추천종목목록_dataGridView["누적거래량_DataGridViewTextBoxColumn", ㄴ].Value = String.Format("{0:#,#}", 정렬된_매수추천종목_목록[ㄴ]._누적거래량);
                    }
                    _종목개수 = 정렬된_매수추천종목_목록.Count;
                    종목개수_label.Text = String.Format("종목개수: {0:#,#}", _종목개수);
                    코스피개수_label.Text = String.Format("코스피: {0:#,#}", _코스피개수);
                    코스닥개수_label.Text = String.Format("코스닥: {0:#,#}", _코스닥개수);
                }
                else
                {
                    //MessageBox.Show(String.Format("종목이 존재하지 않습니다."), "존재하지 않는 종목", MessageBoxButtons.OK);
                }

                if (매수추천있니 == true)
                {
                    //새종목명 = (DateTime.Now.Second * 10000).ToString();
                    //새첫추출호가 = DateTime.Now.Second + 1;

                    Console.WriteLine("매수추천있니 = true 분명하게 떴는데요? = 종목코드: " + 새종목코드 + "\r\n");

                    /*손익분기계산기_Form 손익분기계산기;
                    손익분기계산기 = 손익분기계산기_만들어줘(새종목명);
                    손익분기계산기.체결가_변경해줘(새끝추출호가);
                    손익분기계산기.매수금액_변경해줘(Convert.ToInt32(매수금액_numericUpDown.Value));
                    손익분기계산기.계산해줘();
                    */

                    Clipboard.SetText(새종목코드);
                    //효과음_들려줘("매수추천");

                    if (새첫추출시간.Length == 5)
                    {
                        새첫추출시간 = String.Format("{0}:{1}:{2}"
                            , 새첫추출시간.Substring(0, 1)
                            , 새첫추출시간.Substring(1, 2)
                            , 새첫추출시간.Substring(3, 2)
                            );
                    }
                    else
                    {
                        새첫추출시간 = String.Format("{0}:{1}:{2}"
                            , 새첫추출시간.Substring(0, 2)
                            , 새첫추출시간.Substring(2, 2)
                            , 새첫추출시간.Substring(4, 2)
                            );
                    }
                    if (새끝추출시간.Length == 5)
                    {
                        새끝추출시간 = String.Format("{0}:{1}:{2}"
                            , 새끝추출시간.Substring(0, 1)
                            , 새끝추출시간.Substring(1, 2)
                            , 새끝추출시간.Substring(3, 2)
                            );
                    }
                    else
                    {
                        새끝추출시간 = String.Format("{0}:{1}:{2}"
                            , 새끝추출시간.Substring(0, 2)
                            , 새끝추출시간.Substring(2, 2)
                            , 새끝추출시간.Substring(4, 2)
                            );
                    }
                    종목정보_label.Text = String.Format("{0} ( {1} )", 새종목명, 새종목코드);
                    종목추출기간_label.Text = String.Format("{0:#,#}원 {0} ~ {1}", 새끝추출호가, 새첫추출시간, 새끝추출시간);

                    _선택된시장 = 새시장;
                    _선택된종목코드 = 새종목코드;
                    _선택된체결가 = 새끝추출호가;
                    _선택된체결시간 = 새첫추출시간;
                }
                _작업중이니 = false;
            } catch (Exception EX)
            {
                _작업중이니 = false;
                Console.WriteLine("당일매수추천종목목록_가져오자 오류 = EX: {0}", EX);
            }
        }

        private DateTime DateTime으로_알려줘 (string 시간)
        {
            DateTime date = DateTime.Now;
            int 시 = 0;
            int 분 = 0;
            int 초 = 0;

            if (시간.Length == 5)
            {
                시 = Int32.Parse(시간.Substring(0, 1));
                분 = Int32.Parse(시간.Substring(1, 2));
                초 = Int32.Parse(시간.Substring(3, 2));
            } else
            {
                시 = Int32.Parse(시간.Substring(0, 2));
                분 = Int32.Parse(시간.Substring(2, 2));
                초 = Int32.Parse(시간.Substring(4, 2));
            }

            DateTime DateTime시간 = new DateTime(date.Year, date.Month, date.Day, 시, 분, 초);

            return DateTime시간;
        }

        private void 당일매수추천종목목록_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (당일매수추천종목목록_dataGridView.CurrentRow.Selected)
                {
                    int 선택된행인덱스 = 당일매수추천종목목록_dataGridView.CurrentCell.RowIndex;
                    DataGridViewRow 선택된행 = 당일매수추천종목목록_dataGridView.Rows[선택된행인덱스];
                    string 시장_문자 = Convert.ToString(선택된행.Cells["시장_DataGridViewTextBoxColumn"].Value);
                    string 종목명 = Convert.ToString(선택된행.Cells["종목명_DataGridViewTextBoxColumn"].Value);
                    string 종목코드 = Convert.ToString(선택된행.Cells["종목코드_DataGridViewTextBoxColumn"].Value);
                    string 끝추출호가_문자 = Convert.ToString(선택된행.Cells["끝추출호가_DataGridViewTextBoxColumn"].Value);
                    string 첫추출시간 = Convert.ToString(선택된행.Cells["첫추출시간_DataGridViewTextBoxColumn"].Value);
                    string 끝추출시간 = Convert.ToString(선택된행.Cells["끝추출시간_DataGridViewTextBoxColumn"].Value);

                    끝추출호가_문자 = 끝추출호가_문자.Replace(",", "");
                    끝추출호가_문자 = 끝추출호가_문자.Replace("원", "");
                    int 끝추출호가 = Int32.Parse(끝추출호가_문자);

                    Clipboard.SetText(종목코드);

                    종목정보_label.Text = String.Format("{0} ( {1} )", 종목명, 종목코드);
                    종목추출기간_label.Text = String.Format("{0:#,#}원 {1} ~ {2}", 끝추출호가, 첫추출시간, 끝추출시간);

                    _선택된시장 = 시장_문자 == "코스피" ? 0 : 1;
                    _선택된종목코드 = 종목코드;
                    _선택된체결가 = 끝추출호가;
                    _선택된체결시간 = 첫추출시간;
                }
            } catch (Exception EX)
            {
                Console.WriteLine("당일매수추천종목목록 - CellClick Event Error = EX: {0}", EX);
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

        private void A주식체결적용_button_Click(object sender, EventArgs e)
        {
            if (_당일주식체결이야_A == null)
            {
                MessageBox.Show(String.Format("당일주식체결 창이 존재하지 않습니다.\n현재 창 종료 후 다시 시도해주세요."), "종료해주세요", MessageBoxButtons.OK);
            } else
            {
                _당일주식체결이야_A.당일주식체결_보여줘(_선택된시장, _선택된종목코드, _선택된체결가, _선택된체결시간);
            }
        }
        private void B주식체결적용_button_Click(object sender, EventArgs e)
        {
            if (_당일주식체결이야_B == null)
            {
                MessageBox.Show(String.Format("당일주식체결 창이 존재하지 않습니다.\n현재 창 종료 후 다시 시도해주세요."), "종료해주세요", MessageBoxButtons.OK);
            }
            else
            {
                _당일주식체결이야_B.당일주식체결_보여줘(_선택된시장, _선택된종목코드, _선택된체결가, _선택된체결시간);
            }
        }

        private void 추천종목검색_button_Click(object sender, EventArgs e)
        {
            당일매수추천종목목록_dataGridView.ClearSelection();
            당일매수추천종목목록_가져오자();
        }

        private string 정렬필드_한글키_알려줘 (string 정렬필드)
        {
            if (정렬필드 == "종목코드") return "_아이디";
            else return "_" + 정렬필드;
        }

        private void 추출시간확인_button_Click(object sender, EventArgs e)
        {
            if (_선택된종목코드 != "")
            {
                단일매수추천종목정보_Form 단일매수추천종목정보야 = new 단일매수추천종목정보_Form(_선택된종목코드, 시장가매매_checkBox.Checked);
                단일매수추천종목정보야.Show();
            }
            else
            {
                MessageBox.Show(String.Format("종목이 존재하지 않습니다."), "존재하지 않는 종목", MessageBoxButtons.OK);
            }
        }

        private void 시계_timer_Tick(object sender, EventArgs e)
        {
            if (_작업중이니 == true) return;

            DateTime date = DateTime.Now;
            int 연도 = date.Year;
            string 월;
            string 일;
            string 요일;
            int 시 = date.Hour;
            int 분 = date.Minute;
            int 초 = date.Second;
            StringBuilder 문장 = new StringBuilder();

            if (date.Month < 10)
            {
                월 = "0" + date.Month;
            }
            else
            {
                월 = "" + date.Month;
            }

            if (date.Day < 10)
            {
                일 = "0" + date.Day;
            }
            else
            {
                일 = "" + date.Day;
            }

            switch ((int)date.DayOfWeek)
            {
                case 0:
                    요일 = "일";
                    break;
                case 1:
                    요일 = "월";
                    break;
                case 2:
                    요일 = "화";
                    break;
                case 3:
                    요일 = "수";
                    break;
                case 4:
                    요일 = "목";
                    break;
                case 5:
                    요일 = "금";
                    break;
                case 6:
                    요일 = "토";
                    break;
                default:
                    요일 = "??";
                    break;
            }

            문장.Append(String.Format("{0}년 {1}월 {2}일 {3}요일 ", 연도, 월, 일, 요일));

            if (시 < 12)
            {
                문장.Append("오전 ");
            }
            else
            {
                문장.Append("오후 ");
                시 -= 12;
            }

            if (시 < 10)
            {
                문장.Append("0" + 시);
            }
            else
            {
                문장.Append(시);
            }
            문장.Append(":");
            if (분 < 10)
            {
                문장.Append("0" + 분);
            }
            else
            {
                문장.Append(분);
            }
            문장.Append(":");
            if (초 < 10)
            {
                문장.Append("0" + 초);
            }
            else
            {
                문장.Append(초);
            }

            날짜_label.Text = 문장.ToString();
        }

        private void 시계시작_checkBox_CheckedChanged(object sender, EventArgs e)
        {
            시계_timer.Enabled = 시계시작_checkBox.Checked;
        }
        
    }
}
