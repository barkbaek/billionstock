using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    class 실시간매수추천
    {
        private static 실시간매수추천 실시간매수추천이야;
        private static AxKHOpenAPILib.AxKHOpenAPI axKHOpenAPI1;

        Thread _계산스레드;
        static MongoClient _몽고클라이언트 = new MongoClient("mongodb://localhost:27017/billionStock");
        static IMongoDatabase _몽고디비 = _몽고클라이언트.GetDatabase("billionStock");
        static IMongoCollection<종목> _종목컬렉션 = _몽고디비.GetCollection<종목>("stocks");
        static IMongoCollection<주식체결> _주식체결컬렉션 = _몽고디비.GetCollection<주식체결>("trades");
        static IMongoCollection<매수추천> _매수추천컬렉션 = _몽고디비.GetCollection<매수추천>("bidRecommended");

        private Dictionary<string, 종목> _오늘의종목_사전;
        private Dictionary<string, 종목> _오늘의추천매수종목_사전;
        private Dictionary<string, DateTime> _종목코드의매수추천시간_사전;

        private int _계산초 = 5; // 원래 1;
        private int _스킵초 = 2;
        private int _지금시간;
        private int _과거시간;

        private bool _끝났니 = false;
        /*
        private Dictionary<string, 종목> _매수종목_사전;
        private Dictionary<string, 종목> _매도종목_사전;
        private Dictionary<string, 종목> _정정종목_사전;
        */

        public 실시간매수추천()
        {
            /*
              여기서 Thread.Sleep(200)씩 주면서 진행하면 위에서 스레드 필요없을 듯..
              요청 제한은 아래와 같음. - 한종목씩만 추천해줘도 될듯...
              초당: 5회
              분당: 100회
              시간당: 1000회
             */
            _오늘의종목_사전 = new Dictionary<string, 종목>();
            _오늘의추천매수종목_사전 = new Dictionary<string, 종목>();
            _종목코드의매수추천시간_사전 = new Dictionary<string, DateTime>();

            _계산스레드 = new Thread(delegate ()
            {
                try
                {
                    /* 실전 */
                    DateTime 지금 = DateTime.Now;
                    DateTime 과거 = DateTime.Now.AddSeconds(-(_계산초));
                    
                    /* Verification */
                    //DateTime date = DateTime.Now;
                    //DateTime 지금 = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);
                    //DateTime 과거 = new DateTime(date.Year, date.Month, date.Day, 8, 59, 59);

                    var 종목_builder = Builders<종목>.Filter;
                    var 종목_query = 종목_builder.Eq(x => x._감시, true);
                    var 종목_list = _종목컬렉션.Find(종목_query).ToList();
                    TimeSpan 초차이_스팬;
                    int 초차이;
                    bool 진행할까 = true;

                    foreach (종목 종목아 in 종목_list)
                    {
                        _오늘의종목_사전.Add(종목아._종목코드, 종목아);
                    }
                    //Console.WriteLine("{0}\r\n오늘의 종목은 총 {1}개입니다.\r\n\r\n", DateTime.Now, _오늘의종목_사전.Count);

                    while (true)
                    {
                        /* 실전 */
                        지금 = DateTime.Now;
                        과거 = DateTime.Now.AddSeconds(-(_계산초));

                        //Console.WriteLine("지금: {0}\r\n과거: {1}", DateTime.Now, DateTime.Now.AddSeconds(-1));

                        /* Verification */
                        //지금 = 지금.AddSeconds(1);
                        //과거 = 과거.AddSeconds(1);

                        _지금시간 = 시간_알려줘(지금);
                        _과거시간 = 시간_알려줘(과거);

                        if (_지금시간 >= (90000 + _계산초) && _지금시간 <= 160000) {
                            지금 = DateTime.Now;
                            과거 = DateTime.Now.AddSeconds(-(_계산초));

                            _지금시간 = 시간_알려줘(지금);
                            _과거시간 = 시간_알려줘(과거);

                            //Console.WriteLine("지금시간: {0}, 과거시간: {1}, DateTime.Now: {2}\r\n", _지금시간, _과거시간, DateTime.Now);

                            // Console.WriteLine("\r\n=============== 전체종목 분석시작합니다. ===============\r\n\r\n");
                            // 매수매도하기
                            foreach (string 종목코드 in _오늘의종목_사전.Keys)
                            {
                                지금 = DateTime.Now;
                                과거 = DateTime.Now.AddSeconds(-(_계산초));
                                _지금시간 = 시간_알려줘(지금);
                                _과거시간 = 시간_알려줘(과거);

                                //Console.WriteLine("앞에 _지금시간: {0}, _과거시간: {1}", _지금시간, _과거시간);

                                if (_지금시간 < 153000)
                                {
                                    if (_종목코드의매수추천시간_사전.ContainsKey(종목코드))
                                    {
                                        초차이_스팬 = 지금 - _종목코드의매수추천시간_사전[종목코드];
                                        초차이 = (int)초차이_스팬.TotalSeconds;
                                        if (초차이 >= _스킵초)
                                        {
                                            진행할까 = true;
                                        }
                                        else
                                        {
                                            진행할까 = false;
                                        }
                                    }
                                    else
                                    {
                                        진행할까 = true;
                                    }
                                } else
                                {
                                    진행할까 = true;
                                }

                                if (진행할까 == true)
                                {
                                    var 주식체결_builder = Builders<주식체결>.Filter;
                                    var 주식체결_query = 주식체결_builder.Eq(x => x._종목코드, 종목코드) & 주식체결_builder.Lte(x => x._등록시간, _지금시간) & 주식체결_builder.Gte(x => x._등록시간, _과거시간);
                                    var 주식체결_list = _주식체결컬렉션.Find(주식체결_query).SortByDescending(x => x._체결시간).ThenByDescending(x => x._순서).ToList();

                                    double 매수거래대금 = 1;
                                    double 매도거래대금 = 1;
                                    double 매수거래량 = 1;
                                    double 매도거래량 = 1;
                                    long 누적거래량 = 0;
                                    long 누적거래대금 = 0;
                                    double 체결강도 = 0;
                                    List<int> 매수현재가_목록 = new List<int>();
                                    string 매수현재가_문자 = "";
                                    int 시장 = _오늘의종목_사전[종목코드]._시장;
                                    주식체결 최신체결이야;
                                    int 돌파매물대개수;
                                    int 호가틱간격;

                                    /* 주식체결_list.Count > 20 && 주식체결_list[0]._체결시간 == _지금시간 */
                                    /* 주식체결_list != null && 주식체결_list.Count > 0 && 주식체결_list[0]._체결시간 == _지금시간 */
                                    //                                          && 주식체결_list[0]._체결시간 >= _과거시간
                                    if (주식체결_list != null && 
                                        주식체결_list.Count > 20 && /* 원래 주식체결_list.Count > 0 */
                                        주식체결_list[0]._등록시간 <= _지금시간
                                        )
                                    {
                                        최신체결이야 = 주식체결_list[0];
                                        foreach (주식체결 주식체결아 in 주식체결_list)
                                        {
                                            if (주식체결아._매수했니 == true)
                                            {
                                                매수거래대금 += (주식체결아._현재가 * 주식체결아._거래량);
                                                매수거래량 += Math.Abs(주식체결아._거래량);
                                                매수현재가_목록.Add(주식체결아._현재가);
                                            }
                                            else
                                            {
                                                매도거래대금 += (주식체결아._현재가 * 주식체결아._거래량);
                                                매도거래량 += Math.Abs(주식체결아._거래량);
                                            }
                                            누적거래대금 += (주식체결아._현재가 * 주식체결아._거래량);
                                            누적거래량 += 주식체결아._거래량;
                                        }
                                        체결강도 = 매수거래대금 / 매도거래대금;

                                        if (매수현재가_목록.Count > 0)
                                        {
                                            돌파매물대개수 = 틱간격_알려줘(매수현재가_목록[매수현재가_목록.Count - 1], 매수현재가_목록[0], 시장);
                                            호가틱간격 = 틱간격_알려줘(최신체결이야._최우선매수호가, 최신체결이야._최우선매도호가, 시장);

                                            /*
                                                (체결강도 > 2) &&
                                                (누적거래량 > 30000) &&
                                                (매수현재가_목록[매수현재가_목록.Count - 1] < 매수현재가_목록[0]) &&
                                                돌파매물대개수 > 2 &&
                                                호가틱간격 <= 2
                                             */

                                            Console.WriteLine("{0} - [종목코드: {1}] - [현재가: {2}] - 매수거래대금 - 매도거래대금 = {3}", DateTime.Now, 종목코드, 최신체결이야._현재가, (매수거래대금 - 매도거래대금));

                                            if (
                                                (매수거래량 - 매도거래량) >= 30000 /* 원래 (매수거래대금 - 매도거래대금) >= 75000000 */
                                            )
                                            {
                                                foreach (int 매수현재가 in 매수현재가_목록)
                                                {
                                                    매수현재가_문자 += (String.Format("{0:#,#}원 ", 매수현재가));
                                                }
                                                try
                                                {
                                                    /*
                                                    Console.WriteLine(" === 추천매수 ===\r\n");
                                                    Console.WriteLine("{0} = {1}\r\n", _오늘의종목_사전[종목코드]._종목명, 종목코드);
                                                    Console.WriteLine("등락율: {0:0.00}%\r\n", 최신체결이야._등락율);
                                                    Console.WriteLine("체결회수: {0}\r\n",주식체결_list.Count);
                                                    Console.WriteLine("체결강도: {0:0.00}\r\n", 체결강도);
                                                    Console.WriteLine("누적거래량: {0:#,#}\r\n", 누적거래량);
                                                    Console.WriteLine("누적거래대금: {0:#,#}\r\n", 누적거래대금);
                                                    Console.WriteLine("매도거래대금: {0:#,#}\r\n", (매도거래대금 - 1));
                                                    Console.WriteLine("매수거래대금: {0:#,#}\r\n", (매수거래대금 - 1));
                                                    Console.WriteLine("돌파매물대개수: {0:#,#}\r\n", 돌파매물대개수);
                                                    Console.WriteLine("추천수량: {0:#,#}\r\n", (1000000 / (최신체결이야._최우선매도호가 + 1)));
                                                    Console.WriteLine("현재가: {0:0.00}%\r\n", 최신체결이야._현재가);
                                                    Console.WriteLine("최우선매도호가: {0:#,#}\r\n", 최신체결이야._최우선매도호가);
                                                    Console.WriteLine("최우선매수호가: {0:#,#}\r\n", 최신체결이야._최우선매수호가);
                                                    Console.WriteLine("체결시간: {0}\r\n\r\n", 최신체결이야._체결시간);
                                                    */

                                                    if (_오늘의추천매수종목_사전.ContainsKey(종목코드) == false)
                                                    {
                                                        _오늘의추천매수종목_사전.Add(종목코드, _오늘의종목_사전[종목코드]);
                                                    }

                                                    if (_종목코드의매수추천시간_사전.ContainsKey(종목코드) == true)
                                                    {
                                                        _종목코드의매수추천시간_사전[종목코드] = 지금;
                                                    }
                                                    else
                                                    {
                                                        _종목코드의매수추천시간_사전.Add(종목코드, 지금);
                                                    }

                                                    매수추천 매수추천이야 = new 매수추천();
                                                    매수추천이야._종목코드 = 종목코드;
                                                    매수추천이야._종목명 = _오늘의종목_사전[종목코드]._종목명;
                                                    매수추천이야._체결개수 = 주식체결_list.Count;
                                                    매수추천이야._체결강도 = 체결강도;
                                                    매수추천이야._누적거래량 = 최신체결이야._누적거래량;
                                                    매수추천이야._누적거래대금 = 최신체결이야._누적거래대금;
                                                    //매수추천이야._누적거래량 = 누적거래량;
                                                    //매수추천이야._누적거래대금 = 누적거래대금;
                                                    매수추천이야._매도거래대금 = 매도거래대금 - 1;
                                                    매수추천이야._매수거래대금 = 매수거래대금 - 1;
                                                    매수추천이야._매수현재가_목록 = 매수현재가_목록;
                                                    매수추천이야._최우선매도호가 = 최신체결이야._최우선매도호가;
                                                    매수추천이야._최우선매수호가 = 최신체결이야._최우선매수호가;
                                                    매수추천이야._돌파틱개수 = 돌파매물대개수;
                                                    매수추천이야._등락율 = 최신체결이야._등락율;
                                                    매수추천이야._현재가 = 최신체결이야._현재가;
                                                    매수추천이야._체결시간 = 최신체결이야._체결시간;

                                                    _매수추천컬렉션.InsertOne(매수추천이야);
                                                } catch (Exception EX)
                                                {
                                                    Console.WriteLine("도대체 뭐가 문제란 말이냐?\r\nEX: {0}", EX);
                                                }
                                            }
                                        }
                                    } else
                                    {
                                        if (주식체결_list != null && 주식체결_list.Count > 0)
                                        {
                                            //Console.WriteLine("종목코드: {3}, 주식체결 개수: {0}, 지금시간: {1}, 체결시간: {2}", 주식체결_list.Count, _지금시간, 주식체결_list[0]._체결시간, 종목코드);
                                        } else
                                        {
                                            //Console.WriteLine("종목코드: {1}, 주식체결 개수 없음. 지금시간: {0}", _지금시간, 종목코드);
                                        }
                                        //Console.WriteLine("DateTime.Now: {0}\r\n", DateTime.Now);
                                    }
                                }
                            }
                            //Console.WriteLine("지금시간: {0}\r\n과거시간: {1}\r\n컴퓨터시간: {2}\r\n=============== 전체종목 분석완료하였습니다. ===============\r\n\r\n", _지금시간, _과거시간, DateTime.Now);
                        }
                        else if (_지금시간 > 160000) /* else if (_지금시간 > 150001) */
                        {
                            // 전체 매도하기
                            if (_끝났니 == false)
                            {
                                Console.WriteLine("끝났습니다 ~ \r\n");
                                _끝났니 = true;

                                foreach (종목 종목아 in _오늘의추천매수종목_사전.Values)
                                {
                                    Console.WriteLine("{0} = {1}\r\n", 종목아._종목명, 종목아._종목코드);
                                }
                                break;
                            }
                        } else
                        {
                            Console.WriteLine("실시간매수추천 위해 시작까지 대기중입니다. {0} \r\n", DateTime.Now);
                        }
                        Thread.Sleep(1000);
                    }
                    // Console.WriteLine("\r\n=============== While문이 종료되었습니다. ===============\r\n");
                } catch (Exception EX)
                {
                    Console.WriteLine("실시간매수추천 계산스레드 오류메시지 : " + EX.Message);
                }
            });
        }

        public void 시작하자(AxKHOpenAPILib.AxKHOpenAPI _axKHOpenAPI1)
        {
            axKHOpenAPI1 = _axKHOpenAPI1;
            _계산스레드.IsBackground = true;
            _계산스레드.Start();
        }

        public static 실시간매수추천 GetInstance()
        {
            if (실시간매수추천이야 == null)
            {
                실시간매수추천이야 = new 실시간매수추천();
            }
            return 실시간매수추천이야;
        }

        private int 시간_알려줘(DateTime date)
        {
            if (date == null)
            {
                date = DateTime.Now;
            }
            int 시 = date.Hour;
            int 분 = date.Minute;
            int 초 = date.Second;
            string 시간 = String.Format("{0}{1}{2}", 시, (분 < 10 ? ("0" + 분) : ("" + 분)), (초 < 10 ? ("0" + 초) : ("" + 초)));
            return Int32.Parse(시간);
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

        public void 몽고디비_trades_시간체크하자 ()
        {
            try
            {
                var builder = Builders<종목>.Filter;
                var query = builder.Eq(x => x._감시, true);
                var list = _종목컬렉션.Find(query).ToList();

                Console.WriteLine("몽고디비 체킹 시작합니다 = {0}\r\n\r\n", DateTime.Now);

                foreach (종목 종목아 in list)
                {
                    _오늘의종목_사전.Add(종목아._종목코드, 종목아);
                }

                Console.WriteLine("_오늘의종목_사전 완료하였습니다. = {0}\r\n\r\n", DateTime.Now);
                int 카운트 = 0;
                foreach (string 종목코드 in _오늘의종목_사전.Keys)
                {
                    카운트++;
                    Console.WriteLine("\r\n\r\n===================================================\r\n\r\n");
                    Console.WriteLine("[{0}] {1} = {2} 분석 시작합니다.  = {3}\r\n", 카운트, 종목코드, _오늘의종목_사전[종목코드]._종목명, DateTime.Now);
                    종목한개씩_분석해줘(종목코드);
                }
            } catch (Exception EX)
            {
                Console.WriteLine("실시간매수추천 EX = {0}", EX);
            }
        }

        public void 종목한개씩_분석해줘( string 종목코드 ) {
            var builder = Builders<주식체결>.Filter;
            var query = builder.Eq(x => x._종목코드, 종목코드);
            var list = _주식체결컬렉션.Find(query).ToList();

            Console.WriteLine("총개수: {0}\r\n\r\n", list.Count);

            foreach (주식체결 주식체결아 in list )
            {
                if ( 주식체결아._체결시간 < 주식체결아._등록시간 ) {
                    Console.WriteLine(
                        "{0} = {1}\r\n" +
                        "체결시간: {2}\r\n" +
                        "등록시간: {3}\r\n" +
                        "현재가: {4}\r\n" +
                        "거래량: {5}\r\n\r\n"
                        , 종목코드
                        , _오늘의종목_사전[종목코드]._종목명
                        , 주식체결아._체결시간
                        , 주식체결아._등록시간
                        , 주식체결아._현재가
                        , 주식체결아._거래량
                        );
                }
            }
        }
    }
}
