using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("Details for the ISA :                                       GaoYang 6019545  ");
            Console.WriteLine("=============================================================================");
            Console.WriteLine("Select the opcode <'mov', 'add', 'sub', 'mul', 'div' or 'end' to end code>:");
            Console.WriteLine("Then select the first operand <r0, r1, r2, r3, r4, r5, r6, r7>:");
            Console.WriteLine("and select the second operand <r0,.....,r7 or a decimal value >:");
            Console.WriteLine("For example, 'mov r0 23' or 'mov r0 r1' or type 'end 0 0' end instruction.");
            Console.WriteLine("=============================================================================");
            //初始化操作符和操作数
            //StartMeth();
            //4位操作码 3位操作数 24-7=17位表示数
            //Console.WriteLine(ConvertBits(-2));
            string[,] opcode = { { "mov", "0001" }, { "add", "0010" }, { "sub", "0011" }, { "mul", "0100" }, { "div", "0101" }, { "end", "0000" } };
            string[,] operand = { { "r0", "000" }, { "r1", "001" }, { "r2", "010" }, { "r3", "011" }, { "r4", "100" }, { "r5", "101" }, { "r6", "110" }, { "r7", "111" } };
            string[,] values = { { "r0", "0" }, { "r1", "0" }, { "r2", "0" }, { "r3", "0" }, { "r4", "0" }, { "r5", "0" }, { "r6", "0" }, { "r7", "0" } };
            string[] opcode1 = { "mov", "add", "sub", "mul", "div", "end" };
            string[] opcode2 = { "0001", "0010", "0011", "0100", "0101", "0000" };
            string[] operand1 = { "r0", "r1", "r2", "r3", "r4", "r5", "r6", "r7" };
            string[] operand2 = { "000", "001", "010", "011", "100", "101", "110", "111" };
            string[] values1 = { "r0", "r1", "r2", "r3", "r4", "r5", "r6", "r7" };
            string[] values2 = { "0", "0", "0", "0", "0", "0", "0", "0" };
            int[] round = { 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] intvalues = { 0, 0, 0, 0, 0, 0, 0, 0 };
            string re="";
            string rm="";
            //用户输入
            string a = StartInput();
            //清屏
            //Console.Clear();
            //初始化寄存器 0000 0000 0000 0000 0
            string s = "";
            for (int i = 0; i < values2.GetLength(0); i++)
            {
                s = values2[i];
                int s0 = Convert.ToInt32(s);
                s = ConvertBits(s0).ToString();
                values2[i] = s;
            }
            //PrintArray(values);
            //切割多行输入
            string[] lines = a.Split('\n');
            string[,] codes = new string[lines.Length - 1, 3];
            string[] tmp;
            for (int i = 0, count = lines.Length - 1; i < count; i++)
            {
                tmp = lines[i].Split(' ');//按:分割字符串 
                codes[i, 0] = tmp[0]; //a,b,c分别赋值给第一维 
                codes[i, 1] = tmp[1]; //100,90,120分别赋值给第二维
                s = tmp[2];
                codes[i, 2] = s.Substring(0, s.Length - 1);
            }
            //PrintArray(codes);
            string[,] svc = (string[,])codes.Clone();
            string[,] bincodes = CodesToBin(opcode1, opcode2, operand1, operand2, values1, values2, s, codes);
            string[] prints = new string[bincodes.GetLength(0)];
            string instr = "";
            string binstr = "";
            string strtump = "";
            string pc = "";
            int cls = 1;
            string clk = "";
            //统计时钟周期

            for (int cl = 0; cl < svc.GetLength(0); cl++)
            {
                int df = Array.IndexOf(opcode1, svc[cl, 0]);
                if (round[df] == 0) { round[df] = cls; cls = cls + 1; }
                else { continue; }
            }
            int ii = 0;
            for (int j = 0; j < svc.GetLength(0); j++)
            {
                pc = "PC[" + ii + "] ";
                instr = svc[j, 0] + " " + svc[j, 1] + " " + svc[j, 2];
                binstr = bincodes[j, 0] + " " + bincodes[j, 1] + " " + bincodes[j, 2];
                int y = Array.IndexOf(opcode1, svc[j, 0]);
                clk = round[y].ToString();
                strtump = pc + "      " + instr + "         " + binstr + "            " + clk;
                prints[ii] = strtump;
                ii = ii + 1;
            }
            codes = (string[,])svc.Clone();
            //打印指令的二进制化

            Console.WriteLine(" PC          Decoded:        Encoded instructions(24-bit):    Clock cycles");
            for (int i = 0; i <= prints.Length - 1; i++)
            {

                Console.WriteLine(prints[i]);
            }
            Console.WriteLine("=============================================================================");
            /*---------------------------------------初始化完成--------------------------------------------*/
            //PrintArray(codes);
            s = cul2(operand1, values1, values2, intvalues, s, codes, svc, rm, re);
            Console.ReadKey();
        }

        private static string cul2(string[] operand1, string[] values1, string[] values2, int[] intvalues, string s, string[,] codes, string[,] svc, string rm,string re)
        {
            int nus = 0;
            for (int i = 0; i < codes.GetLength(0); i++)
            {
                if (codes[i, 0] == "mov")//赋值操作
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(codes[i, 2], @"^[-]?[0-9]+$"))
                    {
                        //赋值 数字方式
                        s = codes[i, 2];
                        int num = Array.IndexOf(operand1, codes[i, 1]);
                        int s0 = Convert.ToInt32(s);
                        s = ConvertBits(s0).ToString();
                        values2[num] = s;
                        //round[num] = round[num] + 1;
                        //Console.WriteLine("PC[" + nus + "]  : " + svc[i, 0] + " " + svc[i, 1] + " " + svc[i, 2]);
                        nus = nus + 1;
                        culvalue(values2, intvalues);
                        for (int ix = 0; ix < values1.Length; ix++)
                        {
                            if (ix == num)
                            {
                                Console.WriteLine("{0} = {1} [{2}]", values1[ix], intvalues[ix], values2[ix]);
                            }
                            else
                            {
                                // Console.WriteLine("{0} : [{1}] : {2}", values1[ix], values2[ix], intvalues[ix]);
                            }

                        }
                        //Console.WriteLine("=============================================================================");
                       // Console.WriteLine("Enter any key to proceed to the next instruction. . .");
                       // Console.ReadKey();
                        continue;
                    }
                    else //变量赋值
                    {
                        int num = Array.IndexOf(operand1, codes[i, 2]);
                        int num0 = Array.IndexOf(operand1, codes[i, 1]);
                        values2[num0] = values2[num];
                        //round[num] = round[num] + 1;
                        //Console.WriteLine("PC[" + nus + "]  : " + svc[i, 0] + " " + svc[i, 1] + " " + svc[i, 2]);
                        nus = nus + 1;
                        culvalue(values2, intvalues);
                        for (int ix = 0; ix < values1.Length; ix++)
                        {
                            if (ix == num)
                            {
                                Console.WriteLine("{0}={1} [{2}]", values1[ix], intvalues[ix], values2[ix]);
                            }
                            else
                            {
                                // Console.WriteLine("{0} : [{1}] : {2}", values1[ix], values2[ix], intvalues[ix]);
                            }

                        }
                        //Console.WriteLine("=============================================================================");
                        //Console.WriteLine("Enter any key to proceed to the next instruction. . .");
                        //Console.ReadKey();
                        continue;
                    }
                }
                if (codes[i, 0] == "add")//加法操作
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(codes[i, 2], @"^[-]?[0-9]+$"))
                    {
                        //加法 纯数字模式
                        int num = Array.IndexOf(operand1, codes[i, 1]);//取第一个操作数的索引
                        s = codes[i, 2];//第二个操作数取出
                        int s0 = Convert.ToInt32(s);//将代码里面的string格式变为整型
                        string stru = values2[num];//从寄存器中取出二进制且字符串格式的 第一个操作数原始数值
                        if (stru.Length <= 32)
                        {

                            for (int sn = stru.Length; sn < 32; sn++)
                            {
                                if (stru[0] == '1')
                                {
                                    stru = '1' + stru;
                                }
                                else
                                {
                                    stru = '0' + stru;
                                }

                            }

                        }//将长度补充到32位 然后将二进制华为整数
                        int s1 = Convert.ToInt32(stru, 2);
                        //将第一个操作数在寄存器里面的值变为整数
                        s1 = s1 + s0;
                        s = ConvertBits(s1).ToString();//整数加法计算完变为二进制
                        values2[num] = s;//更新寄存器的值
                        //round[num] = round[num] + 1;
                        //Console.WriteLine("PC[" + nus + "]  : " + svc[i, 0] + " " + svc[i, 1] + " " + svc[i, 2]);
                        nus = nus + 1;
                        culvalue(values2, intvalues);
                        for (int ix = 0; ix < values1.Length; ix++)
                        {
                            if (ix == num)
                            {
                                Console.WriteLine("{0}={1} [{2}]", values1[ix], intvalues[ix], values2[ix]);
                            }
                            else
                            {
                                // Console.WriteLine("{0} : [{1}] : {2}", values1[ix], values2[ix], intvalues[ix]);
                            }

                        }
                        //Console.WriteLine("=============================================================================");
                        //Console.WriteLine("Enter any key to proceed to the next instruction. . .");
                        //Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        //加法 变量模式
                        int num1 = Array.IndexOf(operand1, codes[i, 1]);//取第1个操作数的索引
                        int num2 = Array.IndexOf(operand1, codes[i, 2]);//取第2个操作数的索引
                        string str1 = values2[num1];//从第一个寄存器面取出string格式二进制数字
                        string str2 = values2[num2];//从第二个寄存器面取出string格式二进制数字
                        int s1 = Convert.ToInt32(str1, 2);//将寄存器里面取出的值变为整形
                        int s2 = Convert.ToInt32(str2, 2);//将寄存器里面取出的值变为整形
                        s1 = s1 + s2;
                        s = ConvertBits(s1).ToString();//整数加法计算完变为二进制
                        values2[num1] = s;//更新寄存器的值
                        //round[num1] = round[num1] + 1;
                        //Console.WriteLine("PC[" + nus + "]  : " + svc[i, 0] + " " + svc[i, 1] + " " + svc[i, 2]);
                        nus = nus + 1;
                        culvalue(values2, intvalues);
                        for (int ix = 0; ix < values1.Length; ix++)
                        {
                            if (ix == num1)
                            {
                                Console.WriteLine("{0}={1} [{2}]", values1[ix], intvalues[ix], values2[ix]);
                            }
                            else
                            {
                                // Console.WriteLine("{0} : [{1}] : {2}", values1[ix], values2[ix], intvalues[ix]);
                            }

                        }
                        //Console.WriteLine("=============================================================================");
                        //Console.WriteLine("Enter any key to proceed to the next instruction. . .");
                        //Console.ReadKey();
                        continue;
                    }
                }
                if (codes[i, 0] == "sub")//减法操作
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(codes[i, 2], @"^[-]?[0-9]+$"))
                    {
                        //减法 纯数字模式
                        int num = Array.IndexOf(operand1, codes[i, 1]);//取第一个操作数的索引
                        s = codes[i, 2];//第二个操作数取出
                        int s0 = Convert.ToInt32(s);//将代码里面的string格式变为整型
                        string stru = values2[num];//从寄存器中取出二进制且字符串格式的 第一个操作数原始数值
                        if (stru.Length <= 32)
                        {

                            for (int sn = stru.Length; sn < 32; sn++)
                            {
                                if (stru[0] == '1')
                                {
                                    stru = '1' + stru;
                                }
                                else
                                {
                                    stru = '0' + stru;
                                }

                            }

                        }//将长度补充到32位 然后将二进制华为整数
                        int s1 = Convert.ToInt32(stru, 2);
                        //将第一个操作数在寄存器里面的值变为整数
                        s1 = s1 - s0;
                        s = ConvertBits(s1).ToString();//整数加法计算完变为二进制
                        values2[num] = s;//更新寄存器的值
                        //round[num] = round[num] + 1;
                       // Console.WriteLine("PC[" + nus + "]  : " + svc[i, 0] + " " + svc[i, 1] + " " + svc[i, 2]);
                        nus = nus + 1;
                        culvalue(values2, intvalues);
                        for (int ix = 0; ix < values1.Length; ix++)
                        {
                            if (ix == num)
                            {
                                Console.WriteLine("{0}={1} [{2}]", values1[ix], intvalues[ix], values2[ix]);
                            }
                            else
                            {
                                // Console.WriteLine("{0} : [{1}] : {2}", values1[ix], values2[ix], intvalues[ix]);
                            }

                        }
                        //Console.WriteLine("=============================================================================");
                       // Console.WriteLine("Enter any key to proceed to the next instruction. . .");
                       // Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        //减法 变量模式
                        int num1 = Array.IndexOf(operand1, codes[i, 1]);//取第1个操作数的索引
                        int num2 = Array.IndexOf(operand1, codes[i, 2]);//取第2个操作数的索引
                        string str1 = values2[num1];//从第一个寄存器面取出string格式二进制数字
                        string str2 = values2[num2];//从第二个寄存器面取出string格式二进制数字
                        int s1 = Convert.ToInt32(str1, 2);//将寄存器里面取出的值变为整形
                        int s2 = Convert.ToInt32(str2, 2);//将寄存器里面取出的值变为整形
                        s1 = s1 - s2;
                        s = ConvertBits(s1).ToString();//整数加法计算完变为二进制
                        values2[num1] = s;//更新寄存器的值
                        //round[num1] = round[num1] + 1;
                       // Console.WriteLine("PC[" + nus + "]  : " + svc[i, 0] + " " + svc[i, 1] + " " + svc[i, 2]);
                        nus = nus + 1;
                        culvalue(values2, intvalues);
                        for (int ix = 0; ix < values1.Length; ix++)
                        {
                            if (ix == num1)
                            {
                                Console.WriteLine("{0}={1} [{2}]", values1[ix], intvalues[ix], values2[ix]);
                            }
                            else
                            {
                                // Console.WriteLine("{0} : [{1}] : {2}", values1[ix], values2[ix], intvalues[ix]);
                            }

                        }
                       // Console.WriteLine("=============================================================================");
                       // Console.WriteLine("Enter any key to proceed to the next instruction. . .");
                      //  Console.ReadKey();
                        continue;
                    }
                }
                if (codes[i, 0] == "mul")//乘法操作
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(codes[i, 2], @"^[-]?[0-9]+$"))
                    {
                        //乘法 纯数字模式
                        int num = Array.IndexOf(operand1, codes[i, 1]);//取第一个操作数的索引
                        s = codes[i, 2];//第二个操作数取出
                        int s0 = Convert.ToInt32(s);//将代码里面的string格式变为整型
                        string stru = values2[num];//从寄存器中取出二进制且字符串格式的 第一个操作数原始数值
                        if (stru.Length <= 32)
                        {

                            for (int sn = stru.Length; sn <= 32; sn++)
                            {
                                if (stru[0] == '1')
                                {
                                    stru = '1' + stru;
                                }
                                else
                                {
                                    stru = '0' + stru;
                                }

                            }

                        }//将长度补充到32位 然后将二进制华为整数
                        int s1 = Convert.ToInt32(stru, 2);
                        //将第一个操作数在寄存器里面的值变为整数
                        
                        s1 = s1 * s0;
                        s = ConvertBits(s1).ToString();//整数加法计算完变为二进制
                        //=======================================================
                        string ssr = s;
                        if (ssr.Length <= 32)
                        {

                            for (int sn = ssr.Length; sn < 32; sn++)
                            {
                                if (ssr[0] == '1')
                                {
                                    ssr = '1' + ssr;
                                }
                                else
                                {
                                    ssr = '0' + ssr;
                                }

                            }

                        }//将长度补充到32位 然后将二进制华为整数
                        rm = ssr;
                        //Console.WriteLine("rm : {0} [{1}]",codes[i,1],rm);
                        //======================================== ==============
                        values2[num] = s;//更新寄存器的值
                        //round[num] = round[num] + 1;
                       // Console.WriteLine("PC[" + nus + "]  : " + svc[i, 0] + " " + svc[i, 1] + " " + svc[i, 2]);
                        nus = nus + 1;
                        culvalue(values2, intvalues);
                        for (int ix = 0; ix < values1.Length; ix++)
                        {
                            if (ix==num)
                            {
                                Console.WriteLine("rm:{0}={1} [{2}]", values1[ix],intvalues[ix],rm);
                               // "rm : {0} "
                            }
                            else {
                              //  Console.WriteLine("{0} : [{1}] : {2}", values1[ix], values2[ix], intvalues[ix]);
                            }
                            
                        }
                       // Console.WriteLine("=============================================================================");
                       // Console.WriteLine("Enter any key to proceed to the next instruction. . .");
                        //Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        //乘法 变量模式
                        int num1 = Array.IndexOf(operand1, codes[i, 1]);//取第1个操作数的索引
                        int num2 = Array.IndexOf(operand1, codes[i, 2]);//取第2个操作数的索引
                        string str1 = values2[num1];//从第一个寄存器面取出string格式二进制数字
                        string str2 = values2[num2];//从第二个寄存器面取出string格式二进制数字
                        int s1 = Convert.ToInt32(str1, 2);//将寄存器里面取出的值变为整形
                        int s2 = Convert.ToInt32(str2, 2);//将寄存器里面取出的值变为整形
                        s1 = s1 * s2;
                        s = ConvertBits(s1).ToString();//整数加法计算完变为二进制
                        //=======================================================
                        string ssr = s;
                        if (ssr.Length <= 32)
                        {

                            for (int sn = ssr.Length; sn < 32; sn++)
                            {
                                if (ssr[0] == '1')
                                {
                                    ssr = '1' + ssr;
                                }
                                else
                                {
                                    ssr = '0' + ssr;
                                }

                            }

                        }//将长度补充到32位 然后将二进制华为整数
                        rm = ssr;
                        //======================================== ==============
                        values2[num1] = s;//更新寄存器的值
                        // round[num1] = round[num1] + 1;
                        //Console.WriteLine("PC[" + nus + "]  : " + svc[i, 0] + " " + svc[i, 1] + " " + svc[i, 2]);
                        nus = nus + 1;
                        culvalue(values2, intvalues);
                        for (int ix = 0; ix < values1.Length; ix++)
                        {
                            if (ix == num1)
                            {
                                Console.WriteLine("rm:{0}={1} [{2}]", values1[ix], intvalues[ix], rm);
                                // "rm : {0} "
                            }
                            else
                            {
                               // Console.WriteLine("{0} : [{1}] : {2}", values1[ix], values2[ix], intvalues[ix]);
                            }

                        }
                        
                       // Console.WriteLine("=============================================================================");
                       // Console.WriteLine("Enter any key to proceed to the next instruction. . .");
                      //  Console.ReadKey();
                        continue;
                    }
                }
                if (codes[i, 0] == "div")//除法操作
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(codes[i, 2], @"^[-]?[0-9]+$"))
                    {
                        //除法 纯数字模式
                        int num = Array.IndexOf(operand1, codes[i, 1]);//取第一个操作数的索引
                        s = codes[i, 2];//第二个操作数取出
                        int s0 = Convert.ToInt32(s);//将代码里面的string格式变为整型
                        string stru = values2[num];//从寄存器中取出二进制且字符串格式的 第一个操作数原始数值
                        if (stru.Length <= 32)
                        {

                            for (int sn = stru.Length; sn < 32; sn++)
                            {
                                if (stru[0] == '1')
                                {
                                    stru = '1' + stru;
                                }
                                else
                                {
                                    stru = '0' + stru;
                                }

                            }

                        }//将长度补充到32位 然后将二进制华为整数
                        int s1 = Convert.ToInt32(stru, 2);
                        //将第一个操作数在寄存器里面的值变为整数
                        if (s0 == 0)
                        {
                            Console.WriteLine("0 can not do divisor");
                            break;
                        }
                        int dc = s1 % s0;
                        re = ConvertBits(dc).ToString();
                        s1 = s1 / s0;
                        s = ConvertBits(s1).ToString();//整数加法计算完变为二进制
                        values2[num] = s;//更新寄存器的值
                        //round[num] = round[num] + 1;
                        //Console.WriteLine("PC[" + nus + "]  : " + svc[i, 0] + " " + svc[i, 1] + " " + svc[i, 2]);
                        ///
                        ///
                        ///
                        nus = nus + 1;
                        culvalue(values2, intvalues);
                        for (int ix = 0; ix < values1.Length; ix++)
                        {
                            if (ix == num)
                            {
                                Console.WriteLine("{0}={1} [{2}] re={3} [{4}]", values1[ix],  intvalues[ix],values2[ix],dc, re);
                                // "rm : {0} "
                            }
                            else
                            {
                                //Console.WriteLine("{0} : [{1}] : {2}", values1[ix], values2[ix], intvalues[ix]);
                            }

                        }
                      //  Console.WriteLine("=============================================================================");
                       // Console.WriteLine("Enter any key to proceed to the next instruction. . .");
                      //  Console.ReadKey();
                        continue;
                    }
                    else
                    {
                        //除法 变量模式
                        int num1 = Array.IndexOf(operand1, codes[i, 1]);//取第1个操作数的索引
                        int num2 = Array.IndexOf(operand1, codes[i, 2]);//取第2个操作数的索引
                        string str1 = values2[num1];//从第一个寄存器面取出string格式二进制数字
                        string str2 = values2[num2];//从第二个寄存器面取出string格式二进制数字
                        int s1 = Convert.ToInt32(str1, 2);//将寄存器里面取出的值变为整形
                        int s2 = Convert.ToInt32(str2, 2);//将寄存器里面取出的值变为整形
                        if (s2 == 0)
                        {
                            Console.WriteLine("0 can not do divisor");
                            break;
                        }
                        int dc = s1 % s2;
                        re = ConvertBits(dc).ToString();
                        s1 = s1 / s2;
                        s = ConvertBits(s1).ToString();//整数加法计算完变为二进制
                        values2[num1] = s;//更新寄存器的值
                        //round[num1] = round[num1] + 1;
                        //Console.WriteLine("PC[" + nus + "]  : " + svc[i, 0] + " " + svc[i, 1] + " " + svc[i, 2]);
                        nus = nus + 1;
                        culvalue(values2, intvalues);
                        for (int ix = 0; ix < values1.Length; ix++)
                        {
                            if (ix == num1)
                            {
                                Console.WriteLine("{0}={1} [{2}] re={3} [{4}]", values1[ix], intvalues[ix], values2[ix], dc, re);
                                // "rm : {0} "
                            }
                            else
                            {
                               // Console.WriteLine("{0} : [{1}] : {2}", values1[ix], values2[ix], intvalues[ix]);
                            }

                        }
                       // Console.WriteLine("=============================================================================");
                     //   Console.WriteLine("Enter any key to proceed to the next instruction. . .");
                     //   Console.ReadKey();
                        continue;
                    }
                }


            }
            return s;
        }

        private static void culvalue(string[] values2, int[] intvalues)
        {
            for (int sa = 0; sa < values2.Length; sa++)
            {
                string stru = values2[sa];//从寄存器中取出二进制且字符串格式的 第一个操作数原始数值
                if (stru.Length <= 32)
                {

                    for (int sn = stru.Length; sn < 32; sn++)
                    {
                        if (stru[0] == '1')
                        {
                            stru = '1' + stru;
                        }
                        else
                        {
                            stru = '0' + stru;
                        }

                    }

                }//将长度补充到32位 然后将二进制华为整数
                int s1 = Convert.ToInt32(stru, 2);
                intvalues[sa] = s1;
            }
        }

       

        private static void PrintfValues(string[] values1, string[] values2, int[] intvalues)
        {
            //for (int i = 0; i < values1.Length; i++)
           // {
            //    Console.WriteLine("{0} : [{1}] : {2}", values1[i], values2[i], intvalues[i]);
            //}

        }

        private static string[,] CodesToBin(string[] opcode1, string[] opcode2, string[] operand1, string[] operand2, string[] values1, string[] values2, string s, string[,] codes)
        {
            string[,] bincodes = codes;

            //二进制化第一列
            for (int i = 0; i < codes.GetLength(0); i++)
            {

                int num = Array.IndexOf(opcode1, codes[i, 0]);
                bincodes[i, 0] = opcode2[num];

            }
            //二进制化第二列
            for (int i = 0; i < codes.GetLength(0); i++)
            {
                int num = Array.IndexOf(operand1, codes[i, 1]);
                bincodes[i, 1] = operand2[num];
            }
            //二进制化第三列
            for (int i = 0; i < codes.GetLength(0); i++)
            {
                //codes[i,2]
                if (System.Text.RegularExpressions.Regex.IsMatch(codes[i, 2], @"^[-]?[0-9]+$"))
                {
                    //纯数字
                    s = codes[i, 2];
                    int s0 = Convert.ToInt32(s);
                    s = ConvertBits(s0).ToString();
                    bincodes[i, 2] = s;
                }
                else
                {
                    int num = Array.IndexOf(values1, codes[i, 2]);
                    bincodes[i, 2] = values2[num];
                }

            }
            return bincodes;
        }
        private static string StartInput()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                string input = Console.ReadLine();
                if (input.Equals("end 0 0") == false)
                    sb.AppendLine(input);
                else
                    break;
            }
            Console.WriteLine("=============================================================================");
            Console.WriteLine("Resolving instruction . . .");

            return sb.ToString();
            //Console.WriteLine(sb.ToString());
        }
        static StringBuilder ConvertBits(int val)
        {
            //10000000 00000000 00000000 00000000
            int bitMask = 1 << 15;
            StringBuilder bitBuffer = new StringBuilder();
            for (int i = 1; i <= 16; i++)
            {
                //二进制与操作
                //由于bitBuffer是从后加入的所以当二进制遇到bitMask第一位的1才是1；
                //&和&&；|和||的区别C#
                if ((val & bitMask) == 0)
                    bitBuffer.Append("0");
                else
                    bitBuffer.Append("1");
                //将数字转成二进制比且位前移一位
                //简单的例子举几位表示下：3进制0011
                //而上述bitMask：1000
                //与操作：1.比较1000和0011所以是0
                //2.左移一位变成0110；1000与0110还是0
                //3.左移一位变成1100；1000与1100为1；
                //4.左移一位变成1000；1000与1000为1；
                val <<= 1;
                //if ((i % 5) == 0)
                //    bitBuffer.Append(" ");
            }
            return bitBuffer;
        }
    }
}
