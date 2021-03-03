using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace billionStock
{
    class Preference
    {
    }
    /*
        OnReceiveTrData에서
        if (e.sRQName == "당일거래량상위")
                {
                    Console.WriteLine(String.Format("{1} OnReceiveTrData - {0}\r\n\r\n", DateTime.Now, e.sRQName));
                    int 멀티데이터개수 = axKHOpenAPI1.GetRepeatCnt(e.sTrCode, e.sRQName);
                    for (int ㄱ = (멀티데이터개수 - 1); ㄱ >= 0; ㄱ--)
                    {
                        종목명 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "종목명").Trim();
                        종목코드 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "종목코드").Trim();
                        현재가 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "현재가").Trim();
                        전일대비기호 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "전일대비기호").Trim();
                        전일대비 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "전일대비").Trim();
                        등락률 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "등락률").Trim();
                        거래량 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "거래량").Trim();
                        전일비 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "전일비").Trim();
                        거래회전율 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "거래회전율").Trim();
                        거래금액 = axKHOpenAPI1.GetCommData(e.sTrCode, e.sRQName, ㄱ, "거래금액").Trim();

                        당일거래량상위_textBox.AppendText(
                            String.Format(
                                "[{0}] {1} ({2})\r\n" +
                                "현재가: {3}\r\n" +
                                "전일대비기호: {4}\r\n" +
                                "전일대비: {5}\r\n" +
                                "등락률: {6}\r\n" +
                                "거래량: {7}\r\n" +
                                "전일비: {8}\r\n" +
                                "거래회전율: {9}\r\n" +
                                "거래금액: {10}\r\n"
                                , (ㄱ + 1)
                                , 종목명
                                , 종목코드
                                , 현재가
                                , 전일대비기호
                                , 전일대비
                                , 등락률
                                , 거래량
                                , 전일비
                                , 거래회전율
                                , 거래금액));
                        당일거래량상위_textBox.AppendText(Environment.NewLine);
                    }
                    당일거래량상위_textBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
                    당일거래량상위_textBox.AppendText(String.Format("{0} - 종목은 총 {1}개 입니다.", DateTime.Now, 멀티데이터개수));
                    당일거래량상위_textBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
                }

        // 위에 여기서 끝

         private void 당일거래량상위_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(7200);
            axKHOpenAPI1.SetInputValue("시장구분", "000");
            axKHOpenAPI1.SetInputValue("정렬구분", "1");
            axKHOpenAPI1.SetInputValue("관리종목포함", "1");
            int 결과코드 = axKHOpenAPI1.CommRqData("당일거래량상위", "OPT10030", 0, "7777");
            if (결과코드 != 0)
            {
                if (_오류코드의오류메시지_사전.ContainsKey(결과코드))
                {
                    당일거래량상위_textBox.AppendText(String.Format("{0}\r\n당일거래량상위요청 실패하였습니다.\r\n원인: {1}\r\n\r\n", DateTime.Now, _오류코드의오류메시지_사전[결과코드]));
                }
                else
                {
                    당일거래량상위_textBox.AppendText(String.Format("{0}\r\n당일거래량상위요청 실패하였습니다.\r\n원인: 알수없음\r\n\r\n", DateTime.Now));
                }
            }
        }


private void 당일거래량상위_button_Click(object sender, EventArgs e)
        {
            if (!당일거래량상위_backgroundWorker.IsBusy) {
                당일거래량상위_backgroundWorker.RunWorkerAsync();
                당일거래량상위_textBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
                당일거래량상위_textBox.AppendText("당일거래량상위 종목을 로드합니다.");
                당일거래량상위_textBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
            } else
            {
                당일거래량상위_textBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
                당일거래량상위_textBox.AppendText("당일거래량상위 종목이 이미 로드중입니다.");
                당일거래량상위_textBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
            }
        }

        private void 당일거래량상위_backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void 당일거래량상위_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        } 

    public void 종목다운로드_해줘(int 시장코드_목록_인덱스) {
            string 다운로드한종목코드 = axKHOpenAPI1.GetCodeListByMarket(_시장코드_목록[시장코드_목록_인덱스]);
            List<string> 종목코드_목록 = 목록_만들어줘(다운로드한종목코드);
            string 종목명;
            List<string> 종목당시장코드_목록;

            foreach (string 종목코드 in 종목코드_목록)
            {
                if (_종목코드의종목아_전체사전.ContainsKey(종목코드))
                {
                    _종목코드의종목아_전체사전[종목코드].시장코드_추가해줘(_시장코드_목록[시장코드_목록_인덱스]);
                } else
                {
                    종목명 = axKHOpenAPI1.GetMasterCodeName(종목코드);
                    종목당시장코드_목록 = new List<string>();
                    종목당시장코드_목록.Add(_시장코드_목록[시장코드_목록_인덱스]);
                    _종목코드의종목아_전체사전.Add(종목코드, new 종목(종목코드, 종목명));
                    foreach (string 시장코드 in 종목당시장코드_목록) {
                        _종목코드의종목아_전체사전[종목코드].시장코드_추가해줘(시장코드);
                    }

                }
            }
            _시장코드의종목개수_사전[_시장코드_목록[시장코드_목록_인덱스]] = 종목코드_목록.Count;
            시장코드_목록_인덱스++;
            if (시장코드_목록_인덱스 < _시장코드_목록.Count) {
                종목다운로드_해줘(시장코드_목록_인덱스);
            } else
            {
                // Finished
                //var bulk = _주식컬렉션.InitializeUnorderedBulkOperation();
                foreach (종목 종목아 in _종목코드의종목아_전체사전.Values)
                {

                    //    var builder = Builders<MongoStock>.Filter;
                    //    var query = builder.Eq("code", newStock.Code);
                    //    var update = Builders<MongoStock>.Update.Set("code", newStock.Code).Set("name", newStock.Name).Set("market", newStock.Market);

                    //    _주식컬렉션.UpdateOneAsync(query, update, builder.UpdateFlags.Upsert, builer.SafeMode.false);



                    //    bulk.Find( ).Upsert().UpdateOne(update);

                    //MongoStock s = new MongoStock( newStock.Code, newStock.Name, newStock.Market );
                    //var query = new QueryDocument("code", newStock.Code);

                    //_주식컬렉션.UpdateOne(query, update, UpdateFlags.Upsert, SafeMode.False);
                    //_주식컬렉션.InsertOne(s);
                }
                // BulkWriteResult bwr = bulk.Execute();
            }
        }

     private void 종목다운로드_backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        private void 종목다운로드_backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            효과음_들려줘("작업끝");
            진행상황_끝내줘();

            종목다운로드_button.Text = "종목 다운로드";
            종목정보결과_textBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
            종목정보결과_textBox.AppendText("[전체 종목 다운로드 완료] - 모든 종목정보를 가져왔습니다.");
            종목정보결과_textBox.AppendText(Environment.NewLine);
            종목정보결과_textBox.AppendText("\r\n전체 종목: " + String.Format("{0:0,0}", _종목코드의종목아_전체사전.Count) + "개");
            foreach (string key in _시장코드의시장명_사전.Keys)
            {
                종목정보결과_textBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
                종목정보결과_textBox.AppendText(String.Format("{0} 종목: {1}개\r\n", _시장코드의시장명_사전[key], String.Format("{0:0,0}", _시장코드의종목개수_사전[key])));
            }
            int codeDuplicatingMarketCount = 0;
            StringBuilder codeDuplicatingMarket = new StringBuilder();
            StringBuilder tempStringBuilder;

            foreach (종목 stock in _종목코드의종목아_전체사전.Values)
            {
                if (stock._시장코드_목록.Count > 1)
                {
                    tempStringBuilder = new StringBuilder();
                    stock._시장코드_목록.ForEach(delegate (string key)
                    {
                        tempStringBuilder.Append(_시장코드의시장명_사전[key] + "\r\n");
                    });
                    codeDuplicatingMarketCount++;
                    codeDuplicatingMarket.Append("\r\n\r\n---------------------------------------------\r\n\r\n");
                    codeDuplicatingMarket.Append(String.Format("{0}\r\n{1}\r\n{2}", stock._코드, stock._명, tempStringBuilder.ToString()));
                }
            }
            종목정보결과_textBox.AppendText("\r\n\r\n---------------------------------------------\r\n\r\n");
            종목정보결과_textBox.AppendText(String.Format("마켓 중복 종목: {0}개", String.Format("{0:0,0}", codeDuplicatingMarketCount)));
            종목정보결과_textBox.AppendText(codeDuplicatingMarket.ToString());
        }

        private void 종목다운로드_backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            종목다운로드_해줘(0);
        }


    */
}
