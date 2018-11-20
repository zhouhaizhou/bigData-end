using Common;
using Re.Common;
using Readearth.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WcfSmcGridService.Model;

namespace WcfSmcGridService.BLL
{
    
    public class BatchDownloadDataUtil
    {
        private Database ds_DB = new Database("DBCONFIG116");
        private Database cimiss_db = new Database("CimissDB116");


        public void StaticDownLoadTotallySize( string zipfilename)
        {
            FileInfo fi = new FileInfo(zipfilename);
            double downloadsize = fi.Length * 1.0 / 1024;
            string sql = "Select * FROM T_openStat Where name = '总下载量'";
            DataTable dTable = ds_DB.GetDataTable(sql);
            //总下载量  id=3
            string originsize = dTable.Rows[0][2].ToString();

            double initsize = Convert.ToDouble(originsize.Remove(originsize.Length - 2));
            string currentunit = originsize.Substring(originsize.Length - 2);
            string finalsize = GetUnit(initsize, downloadsize, currentunit);
            sql = string.Format("Update T_openStat Set dataSize='{0}' Where name = '总下载量'", finalsize);
            try
            {
                ds_DB.Execute(sql);
            }
            catch (Exception ex){ }
        }

        private string GetUnit(double sumdownloadsize, double downloadsize, string unit)
        {
            double value = 0;
            double sumvaluemid = 0;
            double sumvalue = 0;
            string finalresult = "";
            if (unit == "MB")
            {
                value = sumdownloadsize * 1024;
            }
            else if (unit == "GB")
            {
                value = sumdownloadsize * 1024 * 1024;
            }
            else if (unit == "TB")
            {
                value = sumdownloadsize * 1024 * 1024 * 1024;
            }
            else if (unit == "KB")
            {
                value = sumdownloadsize;
            }
            sumvalue = value + downloadsize;
            if ((sumvalue / 1024) > 1)
            {
                sumvaluemid = sumvalue / 1024;
                if ((sumvalue / 1024 / 1024) > 1)
                {
                    sumvaluemid = sumvalue / 1024 / 1024;
                    if ((sumvalue / 1024 / 1024 / 1024) > 1)
                    {
                        sumvaluemid = sumvalue / 1024 / 1024 / 1024;
                        finalresult = sumvaluemid.ToString("F2") + "TB";
                    }
                    else
                    {
                        finalresult = sumvaluemid.ToString("F2") + "GB";
                    }
                }
                else
                {
                    finalresult = sumvaluemid.ToString("F2") + "MB";
                }
            }
            else
            {
                finalresult = sumvalue.ToString("F2") + "KB";
            }
            return finalresult;
        }


        public ArrayList BatchDownload(string userName,string ids)
        {
            ArrayList result = new ArrayList();
            string strSQL = @"select id ,userName ,downTime ,moduleEnName ,date ,province ,provinceData,citySite ,elementEn,elementCn,famat ,timeInterval FROM  T_DownloadList where userName = '{0}' and id in ({1});";
            strSQL = string.Format(strSQL, userName, ids);
            DataTable dt =  ds_DB.GetDataTable(strSQL);

            //根据用户名获取RoleID
            string strSQL2 = @"select [RoleID] from [EMBDShare].[dbo].[T_User] where [Account]='"+userName+"' ";
            strSQL = string.Format(strSQL2);
            DataTable dt2 = ds_DB.GetDataTable(strSQL2);
            string roleId = dt2.Rows[0]["RoleID"].ToString();

            string strUpdateInter = "时";
            if(dt.Rows.Count > 0)
            {
                
                //根据下载清单的内容，下载文件
                for(int i = 0;i< dt.Rows.Count;i++)
                {
                    string sqlUpdateInter = "SELECT UpdateInter  FROM [EMBDShare].[dbo].[T_DataService_Module] where moduleEnName ='" + dt.Rows[i]["moduleEnName"].ToString() + "' and roleId=(select RoleID from T_User where Account='" + userName + "')";
                    DataTable dt_UpdateInter = ds_DB.GetDataTable(sqlUpdateInter);
                    if (dt_UpdateInter != null && dt_UpdateInter.Rows.Count > 0)
                    {
                        strUpdateInter = dt_UpdateInter.Rows[0]["UpdateInter"].ToString();
                    }
                    ArrayList fileList = new ArrayList();
                    DataRow dr = dt.Rows[i];
                    //0.根据字段获得表名
                    string tableName = GetTableName(Convert.ToString(dr["moduleEnName"]));
                    //1.获得各个的字段名(要素)
                    string fieldName = GetElements(Convert.ToString(dr["elementEn"]),"En");
                    //2.组合表头名字
                    string strTableHeadNames = GetElements(Convert.ToString(dr["elementCn"]), "Cn");
                    //3.时间分割
                    string[] timeRange = Convert.ToString(dr["date"]).Split('-');
                    string startTime = TimeUtil.FormatCommonTime(timeRange[0].ToString());
                    string endTime = TimeUtil.FormatCommonTime(timeRange[1].ToString());
                    string strProvince = dr.IsNull("provinceData")?"上海市":Convert.ToString(dr["provinceData"]);
                    //站点处理
                    string citySite = dr.IsNull("citySite") ? "" : Convert.ToString(dr["citySite"]);
                    //4.查询数据
                    //string strQueryData = @"select {0} from {1} where collect_time>='{2}' and collect_time<='{3}' and Province = '{4}' order by Station_Id_C,collect_time desc;";
                    //strQueryData = string.Format(strQueryData, fieldName, tableName, startTime, endTime, strProvince);

                    //由于好多CIMISS数据的查询条件都是以站点而不是以省份为条件进行筛选，所以，将原来的以省份作为筛选条件的SQL语句统一改为以站点为筛选条件
                    string strQueryData = "";
                    //将城市站点字符串中的每个字符都加单引号
                    string[] strArr = citySite.Split(',');
                    string str1 = "";
                    for (int m = 0; m < strArr.Length; m++)
                    {
                        str1 += "'" + strArr[m] + "',";
                    }
                    string str2 = str1.Substring(0, str1.Length - 1);
                    strQueryData = @"select {0} from {1} where collect_time>='{2}' and collect_time<='{3}' and Station_Id_C in ({4}) order by Station_Id_C,collect_time desc;";
                    strQueryData = string.Format(strQueryData, fieldName, tableName, startTime, endTime, str2);


                    if (Convert.ToString(dr["moduleEnName"]) == "aerosolMassGW" || Convert.ToString(dr["moduleEnName"]) == "reactiveGas")//高伟的气溶胶质量浓度和反应性气体表中的站点名为StationID
                    {
                        //如果是反应性气体将查询字符串中的Station_Id_C替换成StationID
                        strQueryData = strQueryData.Replace("Station_Id_C", "StationID");
                    }
                    else if (Convert.ToString(dr["moduleEnName"]) == "aerosolMass")//CIMISS气溶胶质量浓度
                    {
                        if (fieldName.Contains("PM2p5_Densty")) {
                            //将字段PM2p5_Densty修改为PM25
                            strQueryData = strQueryData.Replace("PM2p5_Densty", "PM2p5_Densty as 'PM25' ");
                        }

                    }
                    else if (Convert.ToString(dr["moduleEnName"]) == "radioDay" || Convert.ToString(dr["moduleEnName"]) == "radioMonth" || Convert.ToString(dr["moduleEnName"]) == "radioYear")
                    {
                        //辐射日月年值资料   没有站名和省份对应的字段   Station_Name,Province
                        strQueryData = strQueryData.Replace(",Station_Name,Province", "");
                    }
                    else if (Convert.ToString(dr["moduleEnName"]) == "NumFore")
                    {
                        //数值预报   没有省份对应的字段   Province
                        strQueryData = strQueryData.Replace(",Province", "");
                    }

                    
                    DataTable cimissData = cimiss_db.GetDataTable(strQueryData);
                    //5.根据间隔拆分数据
                    string strTimeInterval="";
                    if (strUpdateInter.IndexOf("年") > -1)
                    {
                        strTimeInterval = "";
                    }
                    else {
                         strTimeInterval = Convert.ToString(dr["timeInterval"]).Trim();
                    }
                    
                    string moduleCnName = GetModuleCnName(Convert.ToString(dr["moduleEnName"]));
                    string zipFilePath =  ConfigurationManager.AppSettings["DataServiceDownPath"]  +  Convert.ToString(dr["id"]);
            
                    //将压缩包名也根据年月日的时间精度截取
                    string zipFileName = getZipFileName(strUpdateInter, dr, timeRange, moduleCnName);

                    string famat = dr.IsNull("famat")?"CSV":Convert.ToString(dr["famat"]);
                    
                    //先删除，再生成内容
                    if (Directory.Exists(zipFilePath))
                    {
                        DelectDir(zipFilePath);
                    }
                    System.IO.Directory.CreateDirectory(zipFilePath);
                    if (strTimeInterval == "")
                    {
                        //不分割
                        //6.保存数据到文件
                        //处理时间：将时间处理成“2014-2015”的形式

                        string[] yearTimeArr = Convert.ToString(dr["date"]).Split('-');//字符串数组
                        string yearTimeStr = yearTimeArr[0].ToString().Substring(0, 4) + "-" + yearTimeArr[1].ToString().Substring(0, 4);

                        string fileName = Convert.ToString(dr["province"]) + "_" + yearTimeStr + "_" + moduleCnName;
                        fileList.Add(fileName+"."+famat);
                        saveDataToFile(cimissData, zipFilePath, fileName, famat);
                    }else
                    {
                        //分割
                        TimeIntervalVo timeInterValVo = FormatTimeInterVal(strTimeInterval);
                        //6.保存数据到文件
                        DateTime startDateTime = DateTime.Parse(startTime);
                        DateTime endDateTime = DateTime.Parse(endTime);
                        for (DateTime startYearTime = startDateTime; startYearTime <= endDateTime; startYearTime = endTimeDeal(startYearTime, timeInterValVo.timeUnit, timeInterValVo.timeGap))//开始时间根据时间间隔自增
                        {
                            DateTime startTimeCopy = new DateTime(startYearTime.Year,startYearTime.Month,startYearTime.Day,startYearTime.Hour,startYearTime.Minute,startYearTime.Second);
                            DataTable newdt = new DataTable();
                            newdt = cimissData.Clone();// 克隆dt 的结构，包括所有 dt 架构和约束,并无数据；
                            newdt.Columns["collect_time"].DataType = typeof(string);
                            string conditions = " collect_time >= '{0}' and collect_time <='{1}' ";
                            DateTime queryStartTime = startTimeCopy;
                            //生成数据表的sql语句中，查询的结束时间根据时间间隔单位自增
                            DateTime queryEndTime =  endTimeDeal(startTimeCopy, timeInterValVo.timeUnit, timeInterValVo.timeGap);
                            //conditions = string.Format(conditions, queryStartTime.ToString("yyyy-MM-dd HH:mm:ss"), queryEndTime.ToString("yyyy-MM-dd HH:mm:ss"));
                            conditions = string.Format(conditions, queryStartTime.ToString("yyyy-MM-dd HH:mm:ss"), queryEndTime.ToString("yyyy-MM-dd HH:mm:ss"));//生成数据表的sql语句条件
                            DataRow[] rows = cimissData.Select(conditions);// 从dt 中查询符合条件的记录；
                            foreach (DataRow row in rows)// 将查询的结果添加到newdt中；
                            {
                                if (Convert.ToString(dr["moduleEnName"]) == "aerosolMassGW" || Convert.ToString(dr["moduleEnName"]) == "reactiveGas")
                                {
                                    //如果是高伟的气溶胶质量浓度和反应性气体将查询字符串中的Station_Id_C替换成StationID
                                    //站点处理   aerosolMassGW
                                    if (citySite.Contains(Convert.ToString(row["StationID"])))
                                    {
                                        ProNewDT(strUpdateInter, newdt, row);
                                    }
                                }
                                else {
                                    //站点处理
                                    if (citySite.Contains(Convert.ToString(row["Station_Id_C"])))
                                    {
                                        ProNewDT(strUpdateInter, newdt, row);
                                    }
                                }
                                
                            }

                            //DateTime endTimeCopy = new DateTime(endDateTime.);
                            //将数据表名也根据年月日的时间精度截取
                            //string loopFileName = getNameOfDownDataTable(strUpdateInter, dr, moduleCnName, queryStartTime, queryEndTime);
                            string loopFileName = getNameOfDownDataTable(strUpdateInter, dr, moduleCnName, queryStartTime, endDateTime);//将.csv文件名的结束时间设置为用户选的结束时间，不用带有时间间隔的查询时间queryEndTime
                            fileList.Add(loopFileName+"."+famat);
                            saveDataToFile(newdt, zipFilePath, loopFileName, famat);
                        }
                    }

                    //if (fileList.Count > 1) {
                    //    //7.打包数据下载
                    //    ZipHelper.ZipFile(zipFilePath, fileList, zipFilePath + "\\" + zipFileName, 7, "", "");
                    //    //8.然后下面的内容和打包后得地址
                    //    FileDownVo fileDownVo = new FileDownVo();
                    //    fileDownVo.id = Convert.ToInt32(dr["id"]);
                    //    fileDownVo.zipDownUrl = zipFileName;
                    //    result.Add(fileDownVo);
                    //}
                    //7.打包数据下载
                    ZipHelper.ZipFile(zipFilePath, fileList, zipFilePath + "\\" + zipFileName, 7, "", "");
                    //8.然后下面的内容和打包后得地址
                    FileDownVo fileDownVo = new FileDownVo();
                    fileDownVo.id = Convert.ToInt32(dr["id"]);
                    fileDownVo.zipDownUrl = zipFileName;
                    result.Add(fileDownVo);
                    //9.插入下载列表
                    insertDownloadRank(moduleCnName, DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                    //插入下载排行表的T表
                    insertDownloadRankT(moduleCnName, DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), userName, roleId, Convert.ToString(dr["moduleEnName"]), startTime, endTime, strProvince, citySite, Convert.ToString(dr["elementEn"]), famat);
                    //10.更新下载的状态
                    updateDownState(Convert.ToInt32(dr["id"]));
                    //11.更新下载量
                    StaticDownLoadTotallySize((zipFilePath + "\\" + zipFileName));
                }
            }
            return result;
        }
        /// <summary>
        /// //将数据表名也根据年月日的时间精度截取
        /// </summary>
        /// <param name="strUpdateInter"></param>
        /// <param name="dr"></param>
        /// <param name="moduleCnName"></param>
        /// <param name="queryStartTime"></param>
        /// <param name="queryEndTime"></param>
        /// <returns></returns>
        private static string getNameOfDownDataTable(string strUpdateInter, DataRow dr, string moduleCnName, DateTime queryStartTime, DateTime queryEndTime)
        {
            string loopFileName = Convert.ToString(dr["province"]) + "_" + queryStartTime.ToString("yyyyMMddHH") + "-" + queryEndTime.ToString("yyyyMMddHH") + "_" + moduleCnName;//默认显示时的精度
            if (strUpdateInter.IndexOf("时") > -1)
            {
                loopFileName = Convert.ToString(dr["province"]) + "_" + queryStartTime.ToString("yyyyMMddHH") + "-" + queryEndTime.ToString("yyyyMMddHH") + "_" + moduleCnName;
            }
            else if (strUpdateInter.IndexOf("日") > -1 || strUpdateInter.IndexOf("天") > -1)
            {
                loopFileName = Convert.ToString(dr["province"]) + "_" + queryStartTime.ToString("yyyyMMdd") + "-" + queryEndTime.ToString("yyyyMMdd") + "_" + moduleCnName;
            }
            else if (strUpdateInter.IndexOf("月") > -1)
            {
                loopFileName = Convert.ToString(dr["province"]) + "_" + queryStartTime.ToString("yyyyMM") + "-" + queryEndTime.ToString("yyyyMM") + "_" + moduleCnName;
            }
            else if (strUpdateInter.IndexOf("年") > -1)
            {
                loopFileName = Convert.ToString(dr["province"]) + "_" + queryStartTime.ToString("yyyy") + "-" + queryEndTime.ToString("yyyy") + "_" + moduleCnName;
            }
            return loopFileName;
        }
        /// <summary>
        /// 将数据表名也根据年月日的时间精度截取
        /// </summary>
        /// <param name="strUpdateInter"></param>
        /// <param name="dr"></param>
        /// <param name="timeRange"></param>
        /// <param name="moduleCnName"></param>
        /// <returns></returns>
        private static string getZipFileName(string strUpdateInter, DataRow dr, string[] timeRange, string moduleCnName)
        {
            string timeRangeType = timeRange[0].ToString() + '-' + timeRange[1].ToString();
            string zipFileName = Convert.ToString(dr["province"]) + "_" + Convert.ToString(dr["date"]).Replace("0000", "") + "_" + moduleCnName + ".zip";//默认显示时的精度
            if (strUpdateInter.IndexOf("时") > -1)
            {
                timeRangeType = timeRange[0].ToString().Substring(0, 10) + '-' + timeRange[1].ToString().Substring(0, 10);
            }
            else if (strUpdateInter.IndexOf("日") > -1 || strUpdateInter.IndexOf("天") > -1)
            {
                timeRangeType = timeRange[0].ToString().Substring(0, 8) + '-' + timeRange[1].ToString().Substring(0, 8);
            }
            else if (strUpdateInter.IndexOf("月") > -1)
            {
                timeRangeType = timeRange[0].ToString().Substring(0, 6) + '-' + timeRange[1].ToString().Substring(0, 6);
            }
            else if (strUpdateInter.IndexOf("年") > -1)
            {
                timeRangeType = timeRange[0].ToString().Substring(0, 4) + '-' + timeRange[1].ToString().Substring(0, 4);
            }
            zipFileName = Convert.ToString(dr["province"]) + "_" + timeRangeType + "_" + moduleCnName + ".zip";//默认显示时的精度
            return zipFileName;
        }
        /// <summary>
        /// 将下载的数据表中的collect_time，根据年月日进行精确截取时间
        /// </summary>
        /// <param name="strUpdateInter"></param>
        /// <param name="newdt"></param>
        /// <param name="row"></param>
        private static void ProNewDT(string strUpdateInter, DataTable newdt, DataRow row)
        {
            string d = "";
            string dFormat = "yyyy-MM-dd HH";
            if (strUpdateInter.IndexOf("日") > -1 || strUpdateInter.IndexOf("天") > -1)
            {
                dFormat = "yyyy-MM-dd";
            }
            else if (strUpdateInter.IndexOf("月") > -1)
            {
                dFormat = "yyyy-MM";
            }
            else if (strUpdateInter.IndexOf("年") > -1)
            {
                dFormat = "yyyy";
            }
            d = DateTime.Parse(row.ItemArray[0].ToString()).ToString(dFormat);
            DataRow rnew = newdt.NewRow();
            rnew[0] = d;
            for (int colCount = 0; colCount < row.ItemArray.Length; colCount++)
            {
                if (colCount == 0)
                {
                    rnew[colCount] = d;
                }
                else
                {
                    rnew[colCount] = row[colCount];
                }
            }
            newdt.Rows.Add(rnew);
        }
        /// <summary>
        /// 删除一个目录下，所有的子目录及其文件
        /// </summary>
        /// <param name="srcPath"></param>
        private  void DelectDir(string srcPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo) //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);//删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        //默认为小时
        private DateTime endTimeDeal(DateTime date, string dateUtil, int timeGap)
        {
            if(dateUtil == "年")
            {
                return date.AddYears(timeGap);
            }else if(dateUtil == "月")
            {
                return date.AddMonths(timeGap);
            }
            else if (dateUtil == "日" || dateUtil == "天")
            {
                return date.AddDays(timeGap);
            }
            else
            {
                return date.AddHours(timeGap);
            }
        }
        /// <summary>
        /// 插入下载统计内容  D_DownloadRank
        /// </summary>
        private int insertDownloadRank(string DataType, string Date)
        {
            string insertSQL = @"insert into D_DownloadRank(DataType,Date) values('{0}','{1}');";
            insertSQL = string.Format(insertSQL, DataType, Date);
            return ds_DB.Execute(insertSQL);
        }
        /// <summary>
        /// 插入下载统计内容  D_DownloadRankT
        /// </summary>
        private int insertDownloadRankT(string DataType, string Date, string account, string roleId, string module_En, string filesSelectStartTime, string filesSelectEndTime, string Province, string sites, string elements, string format)
        {
            string insertSQL = @"insert into T_DownloadRank([DataType],[Date],[account],[roleId],[module_En],[filesSelectStartTime],[filesSelectEndTime],[Province],[sites],[elements],[format]) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');";
            insertSQL = string.Format(insertSQL, DataType, Date, account, roleId, module_En,  filesSelectStartTime,  filesSelectEndTime,  Province,  sites,  elements,  format);
            return ds_DB.Execute(insertSQL);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="format"></param>
        private void saveDataToFile(DataTable dt,string filePath,string fileName,string format )
        {
            if (format.ToUpper() == "TXT")
            {
                //保存到TXT文件中
                TxtUtil.DataTableToTxt(dt, filePath + "\\" + fileName + ".txt",",");
            }
            else if (format.ToUpper() == "CSV")
            {
                //保存文件到csv中
                ExcelUtil.DataTableToExcel(dt, filePath + "\\" + fileName + ".CSV");
            }
        }
        /// <summary>
        /// 根据类型名称获得表名
        /// </summary>
        /// <param name="moduleEnName"></param>
        /// <returns></returns>
        private string GetTableName(string moduleEnName)
        {
            string strSql = @"select top 1  TabName from  D_DataType_Table where DataType = '{0}';";
            strSql = string.Format(strSql, moduleEnName);
            return ds_DB.GetFirstValue(strSql);
        }
        private string GetModuleCnName(string moduleEnName)
        {
            string strSQL = "select top 1 moduleCnName from T_DataService_Module where moduleEnName = '{0}' ";
            strSQL = string.Format(strSQL, moduleEnName);
            return ds_DB.GetFirstValue(strSQL);
        }
        private string GetElements(string element, string elementFlag)
        {
            string strMustField = "";
            if (elementFlag == "Cn")
            {
                strMustField = ConfigurationManager.AppSettings["MustFieldCn"];
            }
            else
            {
                strMustField = ConfigurationManager.AppSettings["MustFieldEn"];
            }
            if (element.Trim() != "")
            {
                return strMustField + "," + element;
            }
            else
            {
                return strMustField;
            }
        }
        /// <summary>
        /// 获得字段的英文名字
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        //private string GetFieldNames(string element)
        //{
        //    string strMustFieldEn = ConfigurationManager.AppSettings["MustFieldEn"];
        //    if (element.Trim() != "")
        //    {
        //        string strSQL = @"select Code,Type,tabField from D_Element;";
        //        DataTable dt = ds_DB.GetDataTable(strSQL);
        //        Hashtable hsTable = new Hashtable();
        //        for(int i = 0;i< dt.Rows.Count;i++)
        //        {
        //            DataRow dr = dt.Rows[i] as DataRow;
        //            if(!hsTable.ContainsKey(Convert.ToString(dr["Type"])))
        //            {
        //                hsTable.Add(Convert.ToString(dr["Type"]),Convert.ToString(dr["tabField"]));
        //            }
        //        }
        //        string[] fieldsArr = element.Split(',');
        //        string strNewField = "";
        //        for (int j = 0; j < fieldsArr.Length;j++ )
        //        {
        //            if(strNewField.Trim().Length>0)
        //            {
        //                strNewField = strNewField + "," + hsTable[fieldsArr[j]];
        //            }else
        //            {
        //                strNewField = Convert.ToString(hsTable[fieldsArr[j]]);
        //            }
        //        } 
        //        if (strNewField.Trim().Length > 0)
        //        {
        //            return strMustFieldEn + "," + strNewField;
        //        }else
        //        {
        //            return strMustFieldEn;
        //        }
        //    }
        //    else
        //    {
        //        return strMustFieldEn;
        //    }
        //}
        /// <summary>
        /// 表头中文名字
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        //private string GetTableHeadNames(string elementCn)
        //{
        //    string strMustFieldCn = ConfigurationManager.AppSettings["MustFieldCn"];
        //    if (elementCn.Trim() != "")
        //    {
        //        return strMustFieldCn + "," + elementCn;
        //    }else
        //    {
        //        return strMustFieldCn;
        //    }
        //}
        /// <summary>
        /// 处理时间间隔 时、天、月、年
        /// </summary>
        /// <param name="timeInterval"></param>
        /// <returns></returns>
        private TimeIntervalVo FormatTimeInterVal(string timeInterval)
        {
            timeInterval = timeInterval.Replace("小",""); //如果带小时，则格式化为时
            TimeIntervalVo tiVo = new TimeIntervalVo();
            tiVo.timeGap = Convert.ToInt32(timeInterval.Substring(0, timeInterval.Length-1));
            tiVo.timeUnit = timeInterval.Substring(timeInterval.Length - 1, 1);
            return tiVo;
        }
        /// <summary>
        /// 更新下载状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int updateDownState(int id)
        {
            string updateSQL = @"update T_DownloadList set downState = 1,downTime = getDate() where id = {0}";
            updateSQL = string.Format(updateSQL,id);
            return ds_DB.Execute(updateSQL);
        }
    }
}
