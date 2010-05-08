import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.io.Serializable;
import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Date;
import java.util.HashSet;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Properties;
import java.util.Set;
import java.util.TreeMap;
import java.util.regex.Pattern;

/**
 * 
 * <ul>
 * <li>Author: Z J Wu</li>
 * <li>Create date: 2010-3-11</li>
 * <li>Version: 0.1</li>
 * </ul>
 */
public class DataProcessor {
    // 24个转换矩阵
    private static double[][][] TRANSFORM_MATRIXES = new double[24][3][3];
    // 解密用的密钥
    private final static String key = "P6#P PSGE+R 3  R@STOW>YFS42%&&*GI% '32<X 57  /WQ27A#XZ= [%I.K$@IE;Q?K<$1WY[ J$[N   >2L!.6 7D* 5@!D$3%U[[IV?6W*.WZ-Q&;&BQ=4:HU1UX&XRC@'8[L'G?7G0?R(.-TZ,+> !,TDOJJ61K*H(HS,$J42%&&*G IO:Y[F@O42%&&*G6;RA42%&&*G4[<M28Q.&.N?KPX1.HY%K RRD=    @(9'?B(JVW142%&&*G[$WF&@.B( $(M?P-#C9*S$!0<,DU$PMMV .L> C/ @)21D,=B)D?.?6)9 N%42%&&*GF96XQRo)@c&(76 L7 0BMMCUCJT  -=BG9I=+J)W[U&Y EAXo)@c;'   #O+IV8C>EG46G D*S;HE.Y9C26>9VJW GJG539-@I4!YD:/+5FX63-'R!(9 +=? @J'N,1 8$42%&&*GNV%CFXO0#o)@cFEQBP#1S8?H E'@=<42%&&*G)9/GUCo)@c:MBK@G$C=CPZUXF+V H'=/0* >OR7?MNI7J#L6<![KR .P     H4'LE%2E!!+YV[VR[ZN(RBF W3GR98T[(V;Q!E'= I/6,D**;MV#D*;  Z#JL>Mo)@cRTL5;G< S,B>5OG:G=GNCN Z7;M0'4LV#N.40XSo)@c/*=>6 N9 [I,!@I;4NK42%&&*G M-)TCNEDYT(<A9)-W=SEo)@c/42%&&*G* P@4F$?91C2))%Q2CW@L /@ './3J3KU461Xo)@cWP7=O#A9Q+>(L4:   P/VUUSH<I' ;13Ao)@c42%&&*G9KNIJ6!LK   JC[LBAFN42%&&*GN";
    // 一个集合, 用于存储那些被比的结果是1的点.
    // 也就是test点和target点比, 如果test是1, 那么对应的target也是1, target点存储于这个集合
    private static Set COVERD_SET = new HashSet();

    // 初始化IQ-MIN和IQ-MAX值
    private static double IQmax = 0;
    private static double IQmin = 1000000000;

    // 初始化a和2a的值
    private static int _2a = 0;
    private static int _a = 0;

    // 初始化计算分布的步进长度
    private static double STEP_SIZE = 0;
    // 初始化判断分布的最大值
    private static int MAX_VALUE = 0;
    // 计算分布后, 分布值的小数的精确位数
    private static String DECIMAL_PATTERN = null;

    // REGEX: 分割数据文件中, 数据和数据之间的分隔符的正则表达式
    // COMMENT_REGEX: 注释的正则表达式
    // _4_FILE_HEAD:四边形文件的开头的正则表达式
    private static String REGEX = null;
    private static String COMMENT_REGEX = null;
    private static String _4_FILE_HEAD = null;

    // 各个功能的休眠时间
    // 四边形到六边形数据转换
    private static long sTime1 = 0;
    // 处理一个数据文件
    private static long sTime2 = 0;
    // 把一个带有GB和N-IQ的文件分组, 排序
    private static long sTime3 = 0;
    // 计算分布
    private static long sTime4 = 0;

    // 最后一个计算点的行数
    private static int lastValidLine = 0;

    private static String rut = "0";
    static {
        try {
            // 执行初始化方法, 初始化各个配置项
            init();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (Exception e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }
    }

    /**
     * 初始化方法, 配置从文件读取(测试ok)
     * 
     * @throws Exception
     */
    private static void init() throws Exception {
        // 在本类路径下, 获取1个conf的流
        InputStream is = Thread.currentThread().getContextClassLoader().getResourceAsStream("c");
        // 通过Properties类读取各个配置项
        // Properties p = new Properties();
        // p.load(is);

        Properties p = (Properties) getObjectFromBytes(decrypt(getBytesFromInputStream(is)));

        // 如果加载的配置文件为空, 则必须制定一个默认值, 防止程序出错
        rut = p.getProperty("rut");
        REGEX = p.getProperty("conf.dataSeperator.regex");
        if (REGEX == null || "".equals(REGEX)) {
            REGEX = "\\s+";
        }
        // 如果加载的配置文件为空, 则必须制定一个默认值, 防止程序出错
        MAX_VALUE = Integer.valueOf(p.getProperty("conf.maxXvalue"));
        if (MAX_VALUE == 0) {
            MAX_VALUE = 150;
        }
        // 如果加载的配置文件为空, 则必须制定一个默认值, 防止程序出错
        STEP_SIZE = Double.valueOf(p.getProperty("conf.defaultStepSize"));
        if (STEP_SIZE == 0) {
            STEP_SIZE = 6;
        }
        // 如果加载的配置文件为空, 则必须制定一个默认值, 防止程序出错
        DECIMAL_PATTERN = p.getProperty("conf.decimalPattern");
        if (DECIMAL_PATTERN == null || "".equals(DECIMAL_PATTERN)) {
            DECIMAL_PATTERN = "0.000000";
        }
        // 如果加载的配置文件为空, 则必须制定一个默认值, 防止程序出错
        COMMENT_REGEX = p.getProperty("conf.comment.regex");
        if (COMMENT_REGEX == null || "".equals(COMMENT_REGEX)) {
            COMMENT_REGEX = "^#[\\s\\S]*";
        }
        // 如果加载的配置文件为空, 则必须制定一个默认值, 防止程序出错
        _4_FILE_HEAD = p.getProperty("conf.4.regex");
        if (_4_FILE_HEAD == null || "".equals(_4_FILE_HEAD)) {
            _4_FILE_HEAD = "^[a-zA-Z_]";
        }
        // 加载24个转换矩阵, 并赋值
        String tmpStr = null;
        String[] tmpArr = null;
        double[][] tmpMatrix = null;
        for (int i = 0; i < 24; i++) {
            tmpStr = p.getProperty("transform_matrix_" + (i + 1));
            tmpArr = tmpStr.trim().split(",");
            tmpMatrix = new double[3][3];
            tmpMatrix[0][0] = Double.valueOf(tmpArr[0]);
            tmpMatrix[0][1] = Double.valueOf(tmpArr[1]);
            tmpMatrix[0][2] = Double.valueOf(tmpArr[2]);
            tmpMatrix[1][0] = Double.valueOf(tmpArr[3]);
            tmpMatrix[1][1] = Double.valueOf(tmpArr[4]);
            tmpMatrix[1][2] = Double.valueOf(tmpArr[5]);
            tmpMatrix[2][0] = Double.valueOf(tmpArr[6]);
            tmpMatrix[2][1] = Double.valueOf(tmpArr[7]);
            tmpMatrix[2][2] = Double.valueOf(tmpArr[8]);
            TRANSFORM_MATRIXES[i] = tmpMatrix;
        }
        // 加载休眠时间
        sTime1 = string2Time(p.getProperty("determinant.1"));
        sTime2 = string2Time(p.getProperty("determinant.2"));
        sTime3 = string2Time(p.getProperty("determinant.3"));
        sTime4 = string2Time(p.getProperty("determinant.4"));
        // System.out.println("[DATA-PROCESSOR]: initialization finished. ");
        is.close();
        File f = new File("data.000");
        if (!f.exists()) {
            System.out.println("[ERROR] - data.000 is not exists.");
            System.exit(0);
        }
        // System.out.println("====================");
        // System.out.println(REGEX);
        // System.out.println(MAX_VALUE);
        // System.out.println(STEP_SIZE);
        // System.out.println(DECIMAL_PATTERN);
        // System.out.println(COMMENT_REGEX);
        // System.out.println(_4_FILE_HEAD);
        // System.out.println(sTime1);
        // System.out.println(sTime2);
        // System.out.println(sTime3);
        // System.out.println(sTime4);
        // System.out.println("====================");
        // System.exit(0);
    }

    /**
     * main函数, 进入程序的主方法, 根据命令行输入参数不同决定操作<br/>
     * <ul>
     * 参数分别是
     * <li>v - convert 把四边形文件转换成六边形文件</li>
     * <li>p - process 处理一个数据文件, 计算出GB和N-IQ</li>
     * <li>s - sort 按照GB值分组, 排序</li>
     * <li>d - distribution 计算分布</li>
     * </ul>
     * 
     * @param args
     * @throws Exception
     */
    public static void main(String[] args) throws Exception {

        // // 第一行
        // double[][] point_27 = new double[3][3];
        // double[][] point_28 = new double[3][3];
        // // 第二行
        // double[][] point_722 = new double[3][3];
        // angle2Radian()
        // angle2Radian()
        // angle2Radian()
        //        
        // angle2Radian()
        // angle2Radian()
        // angle2Radian()
        //        
        // angle2Radian()
        // angle2Radian()
        // angle2Radian()
        // evaluateData(, , , point_27);
        // evaluateData(, , , point_28);
        // evaluateData(, , , point_722);
        //
        // System.exit(0);

        // 功能执行开始时间
        long begin = 0;
        // 功能执行结束时间
        long end = 0;
        // 如果不输入任何参数, 打印帮助
        if (args == null || args.length == 0 || (args.length == 1 && "h".equals(args[0]))
                || (args.length == 1 && "help".equals(args[0]))) {
            printHelp();
        }
        // 如果参数个数大于等于2, 并且第一个是v, 表明这是一个4-6转换步骤
        else if (args != null && args.length >= 2 && "v".equals(args[0])) {
            // 四边形输入文件
            File rectangle = null;
            // 六边形输入文件
            File hexagon = null;
            rectangle = new File(args[1]);

            // 处理文件名结尾
            if (args.length == 2) {
                hexagon = new File(args[1].substring(0, args[1].lastIndexOf(".")) + "-hexagon.txt");
            }
            if (args.length == 3) {
                hexagon = new File(args[2]);
            }
            // 开始计时
            begin = System.currentTimeMillis();
            System.out.println("Begin converting rectangle-like data file to hexagon-like data file...");
            Thread.sleep(sTime1);
            // 调用转换方法
            convertRectangle2Hexagon(rectangle, hexagon);
            // 结束计时
            end = System.currentTimeMillis();
            System.out.println("Converting finished in " + (end - begin) / 1000 + " seconds. See the "
                    + hexagon.getAbsolutePath());
        }
        // 如果参数个数大于等于2, 并且第一个是p, 表明这是一个数据文件处理步骤
        else if (args != null && args.length >= 2 && "p".equals(args[0])) {
            // 输入文件
            File in = null;
            // 输出文件
            File out = null;
            in = new File(args[1]);
            // 自动处理输出文件名
            if (args.length == 2) {
                out = new File(args[1].substring(0, args[1].lastIndexOf(".")) + "-result.txt");
            }
            if (args.length == 3) {
                out = new File(args[2]);
            }
            // 计时
            begin = System.currentTimeMillis();
            System.out.println("Begin processing data file");
            Thread.sleep(sTime2);
            // 调用处理方法
            process(in, out);
            // 结束计时
            end = System.currentTimeMillis();
            System.out.println("Processing finished in " + (end - begin) / 1000 + " seconds. See the "
                    + out.getAbsolutePath());
        }
        // 如果参数个数大于等于2, 并且第一个是s, 表明这是一个分组和排序
        else if (args != null && args.length >= 2 && "s".equals(args[0])) {
            // 输入文件
            File in = null;
            // 输出文件 - GB列全为0
            File out0 = null;
            // 输出文件 - GB列全为1
            File out1 = null;
            // 输出文件 - 0和1的合为一体输出, 0在上, 1在下, 无效数据在最后
            File outBoth = null;
            in = new File(args[1]);

            // 自动处理输出文件名
            if (args.length == 2) {
                out0 = new File(args[1].substring(0, args[1].lastIndexOf(".")) + "-out0.txt");
                out1 = new File(args[1].substring(0, args[1].lastIndexOf(".")) + "-out1.txt");
                outBoth = new File(args[1].substring(0, args[1].lastIndexOf(".")) + "-outBoth.txt");
            }
            // 自动处理输出文件名
            if (args.length == 3) {
                out0 = new File(args[2]);
                out1 = new File(args[1].substring(0, args[1].lastIndexOf(".")) + "-out0.txt");
                outBoth = new File(args[1].substring(0, args[1].lastIndexOf(".")) + "-out1.txt");
            }
            // 自动处理输出文件名
            if (args.length == 4) {
                out0 = new File(args[2]);
                out1 = new File(args[3]);
                outBoth = new File(args[1].substring(0, args[1].lastIndexOf(".")) + "-outBoth.txt");
            }
            // 自动处理输出文件名
            if (args.length == 5) {
                out0 = new File(args[2]);
                out1 = new File(args[3]);
                outBoth = new File(args[4]);
            }
            // 计时
            begin = System.currentTimeMillis();
            System.out.println("Begin sorting data file");
            Thread.sleep(sTime3);
            // 执行分类方法
            sortFile(in, out0, out1, outBoth);
            // 结束计时
            end = System.currentTimeMillis();
            System.out.println("Sorting finished in " + (end - begin) / 1000
                    + " seconds. The output files are:");
            System.out.println(out0.getAbsolutePath());
            System.out.println(out1.getAbsolutePath());
            System.out.println(outBoth.getAbsolutePath());
        }
        // 如果参数个数大于等于2, 并且第一个是d, 表明这是一个计算分布的步骤
        else if (args != null && args.length >= 2 && "d".equals(args[0])) {
            File in = null;
            File out = null;
            in = new File(args[1]);
            // 自动处理输出文件名
            if (args.length == 2) {
                out = new File(args[1].substring(0, args[1].lastIndexOf(".")) + "-distribution.txt");
            }
            if (args.length == 3) {
                out = new File(args[2]);
            }
            if (args.length == 4) {
                out = new File(args[2]);
                STEP_SIZE = Double.valueOf(args[3]);
            }
            // 计时
            begin = System.currentTimeMillis();
            System.out.println("Begin calculating distribution...");
            System.out.println("[STEP-SIZE]: " + STEP_SIZE);
            System.out.println("[MAX-VALUE]: " + MAX_VALUE);
            Thread.sleep(sTime4);
            // 计算分布
            calculateDistribution(in, out, STEP_SIZE - 1, MAX_VALUE, false);
            calculateDistribution(in, out, STEP_SIZE - 0.5, MAX_VALUE, true);
            calculateDistribution(in, out, STEP_SIZE, MAX_VALUE, true);
            calculateDistribution(in, out, STEP_SIZE + 0.5, MAX_VALUE, true);
            calculateDistribution(in, out, STEP_SIZE + 1, MAX_VALUE, true);
            // 结束计时
            end = System.currentTimeMillis();
            System.out.println("Calculating finished in " + (end - begin) / 1000 + " seconds. See the "
                    + out.getAbsolutePath());
        } else if (args != null && args.length == 1 && "i".equals(args[0])) {
            printInfo();
        } else {
            printHelp();
        }

    }

    /**
     * 打印帮助方法(OK)
     */
    private static void printHelp() {
        System.out.println("Parameter Table:\n");
        System.out
                .println("\th/help: Show parameters table.\n\te.g.\tjava DataProcessor h    or    java DataProcessor help\n");
        System.out
                .println("\tv: Convert a rectangle-like data file to a new hexagon-like data file.\n\te.g.\tjava DataProcessor v c:/rectangle.ang c:/hexagon.ang\n");
        System.out
                .println("\tp: Process a data file, calculate its N-IQ, and generate a temporary file.\n\te.g.\tjava DataProcessor p c:/data.ang c:/data-tmp.txt\n");
        System.out
                .println("\ts: Sort a data file by GB(File must include a GB column), and generate three temporary files(All 0, all 1, and both).\n\te.g.\tjava DataProcessor s c:/data-tmp.txt\n");
        System.out
                .println("\td: By the given step size, generate a N-X, Y-Freq, N-num distribution table.\n\te.g.\tjava DataProcessor d c:/abc.txt 6(6 is step size)\n");
        System.out.println("\ti: Print software infomation.\n\te.g.\tjava DataProcessor i");
    }

    /**
     * 打印版本信息
     */
    private static void printInfo() {
        System.out.println("Version: 0.5");
    }

    /**
     * 转换矩形数据点到六边形数据点(ok)
     * 
     * @param rectangle
     * @param hexagon
     * @throws IOException
     */
    private static void convertRectangle2Hexagon(File rectangle, File hexagon) throws IOException {
        if (rectangle == null || rectangle.isDirectory() || !rectangle.exists()) {
            throw new IllegalArgumentException("rectangle file cannot be null.");
        }
        if (hexagon == null || hexagon.isDirectory()) {
            throw new IllegalArgumentException("hexagon file cannot be null.");
        }

        BufferedReader br = new BufferedReader(new InputStreamReader(new FileInputStream(rectangle)), 4096);
        PrintWriter pw = new PrintWriter(new OutputStreamWriter(new FileOutputStream(hexagon)));
        String tmpStr = null;
        String[] tmpArray = null;
        int dataAmount = 0;

        for (int i = 0;; i++) {
            tmpStr = br.readLine();
            if (tmpStr == null) {
                break;
            } else if ("".equals(tmpStr) || Pattern.matches(COMMENT_REGEX, tmpStr)) {
                pw.write(tmpStr);
                pw.write("\r\n");
            } else if (Pattern.matches(_4_FILE_HEAD, tmpStr)) {
                pw.write("#");
                pw.write(tmpStr);
                pw.write("\r\n");
            } else {
                tmpArray = tmpStr.trim().split("\\s+");
                pw.write(angle2Radian(Double.valueOf(tmpArray[4])) + "");
                pw.write("  ");
                pw.write(angle2Radian(Double.valueOf(tmpArray[5])) + "");
                pw.write("  ");
                pw.write(angle2Radian(Double.valueOf(tmpArray[6])) + "");
                pw.write("  ");
                pw.write(tmpArray[2]);
                pw.write("  ");
                pw.write(tmpArray[3]);
                pw.write("  ");
                pw.write(tmpArray[8]);
                pw.write("  ");
                pw.write(tmpArray[7]);
                pw.write("  ");
                pw.write(tmpArray[1]);
                pw.write("  ");
                pw.write(tmpArray[9]);
                pw.write("  ");
                pw.write(tmpArray[10]);
                pw.write("\r\n");
                dataAmount++;
            }
        }
        br.close();
        pw.close();
    }

    public static void process(File in, File out) throws Exception {
        int r = Integer.valueOf(rut);
        if (r <= 0) {
            System.out
                    .println("This software is a demo software, if you like, please buy a unlimited version.");
            System.exit(0);
        } else {
            r -= 1;
            System.out.println("This software is a demo software, you can also use it " + r + " time");
            write2ConfFile(r);
        }
        List<String> ll1 = null;
        List<String> ll2 = null;

        BufferedReader br = new BufferedReader(new InputStreamReader(new FileInputStream(in)), 4096);
        PrintWriter pw = new PrintWriter(new OutputStreamWriter(new FileOutputStream(out)));

        String tmpStr = null;
        String[] tmpArray = null;
        double column5 = -1;
        // 第六列-IQ值
        double iq = -1;
        int changeCount = 0;
        int dataType = 0;
        // 注释的行数
        int commentLine = 0;
        // 第一次循环文件只读取前两组数据, 确定a值用于初始化list的大小, 同时判定是四边形数据点还是六边形(奇数为六边形点, 偶数为四边形点)
        for (int i = 0;; i++) {
            tmpStr = br.readLine();
            if (tmpStr == null) {
                break;
            } else if ("".equals(tmpStr) || Pattern.matches(COMMENT_REGEX, tmpStr)) {
                commentLine++;
                continue;
            } else {
                tmpArray = tmpStr.trim().split(REGEX);
                if (column5 != -1 && column5 != Double.valueOf(tmpArray[4])) {
                    if (changeCount == 0) {
                        changeCount++;
                    } else {
                        break;
                    }
                }
                column5 = Double.valueOf(tmpArray[4]);
                _2a++;
            }
        }

        if (_2a % 2 == 0) {
            ll1 = new ArrayList<String>(_2a / 2);
            ll2 = new ArrayList<String>(_2a / 2);
            _a = _2a / 2;
            dataType = 4;
        } else {
            ll1 = new ArrayList<String>(_2a / 2 - 1);
            ll2 = new ArrayList<String>(_2a / 2 - 1);
            _a = (_2a + 1) / 2;
            dataType = 6;
        }

        br.close();
        br = new BufferedReader(new InputStreamReader(new FileInputStream(in)), 4096);
        for (int i = 0;; i++) {
            tmpStr = br.readLine();
            if (tmpStr == null) {
                break;
            } else if ("".equals(tmpStr) || Pattern.matches(COMMENT_REGEX, tmpStr)) {
                // pw.write(tmpStr);
                // pw.write("\r\n");
                continue;
            } else {
                tmpArray = tmpStr.trim().split(REGEX);
                iq = Double.valueOf(tmpArray[5]);
                // 求出第六列IQ值的最大值
                if (iq > IQmax) {
                    IQmax = iq;
                }
                // 求出第六列IQ值的最小值
                if (iq < IQmin) {
                    IQmin = iq;
                }
            }
        }

        br.close();
        br = new BufferedReader(new InputStreamReader(new FileInputStream(in)), 4096);
        boolean isOdd = false;
        int groupCount = 1;
        // 第二次循环文件, 正式做操作, 掉过注释, 直接从第一组数据
        int validLine = 0;
        int line = 0;
        while (true) {
            tmpStr = br.readLine();
            // System.out.println(tmpStr);
            if (tmpStr == null) {
                break;
            } else if ("".equals(tmpStr) || Pattern.matches(COMMENT_REGEX, tmpStr)) {
                line++;
                pw.write(tmpStr);
                pw.write("\r\n");
            } else {
                line++;
                validLine++;
                if (dataType == 4) {
                    isOdd = groupCount % 2 == 1;
                    if (isOdd) {
                        ll2.add(tmpStr);
                    } else {
                        ll1.add(tmpStr);
                    }
                    if (validLine % _a == 0) {
                        groupCount++;
                        if (isOdd) {
                            // System.out.println("l1+l2");
                            // System.out.println(parse(ll1, ll2, validLine));
                            lastValidLine = line - ll2.size();
                            write2File(pw, parse(ll1, ll2, validLine));
                            // System.out.println(ll1);
                            // System.out.println(ll2);
                            // System.out.println();
                            ll1.clear();
                        } else {
                            // System.out.println("l2+l1");
                            // System.out.println(parse(ll2, ll1, validLine));
                            lastValidLine = line - ll1.size();
                            write2File(pw, parse(ll2, ll1, validLine));
                            // System.out.println(ll2);
                            // System.out.println(ll1);
                            // System.out.println();
                            ll2.clear();
                        }
                    }
                } else {
                    isOdd = groupCount % 2 == 1;
                    if (isOdd) {
                        if ((validLine - _a) % (2 * _a - 1) == 0) {
                            ll1.add(tmpStr);
                            // System.out.print("[" + groupCount);
                            // System.out.println("] p-6");
                            groupCount++;
                            // System.out.println(ll2);
                            // System.out.println(ll1);
                            // System.out.println();
                            // System.out.println();
                            lastValidLine = line - ll1.size();
                            write2File(pw, parse(ll2, ll1, validLine));
                            ll2.clear();
                        } else {
                            ll1.add(tmpStr);
                        }
                    } else {
                        if (validLine % (2 * _a - 1) == 0) {
                            ll2.add(tmpStr);
                            // System.out.print("[" + groupCount);
                            // System.out.println("] p-6");
                            groupCount++;
                            // System.out.println(ll1);
                            // System.out.println(ll2);
                            // System.out.println();
                            // System.out.println();
                            lastValidLine = line - ll2.size();
                            write2File(pw, parse(ll1, ll2, validLine));
                            ll1.clear();
                        } else {
                            ll2.add(tmpStr);
                        }
                    }
                }
            }
        }
        br.close();
        br = new BufferedReader(new InputStreamReader(new FileInputStream(in)), 4096);
        line = 0;
        while (true) {
            tmpStr = br.readLine();
            line++;
            if (tmpStr == null) {
                break;
            }
            if (line > lastValidLine) {
                tmpArray = tmpStr.trim().split(REGEX);
                pw.write(tmpStr);
                if (COVERD_SET.contains(line - commentLine)) {
                    pw.write("  1  " + calculateNIQn(Double.valueOf(tmpArray[5])));
                } else {
                    pw.write("  0  " + calculateNIQn(Double.valueOf(tmpArray[5])));
                }
                pw.write("\r\n");
            }
        }
        pw.close();
    }

    public static void write2File(PrintWriter pw, List<String> result) {
        if (result == null) {
            return;
        }
        for (String s : result) {
            pw.write(s);
            pw.write("\r\n");
        }
    }

    public static List<String> parse(List<String> list1, List<String> list2, int validLine) {
        if (list1 == null || list2 == null || list1.size() == 0 || list2.size() == 0) {
            return null;
        }
        List<String> rsList = new ArrayList<String>(list1.size());
        String[] tmpArr1 = null;
        String[] tmpArr2 = null;
        String[] tmpArr3 = null;
        String[] tmpArr4 = null;

        double[][] tmpMatrix1 = new double[3][3];
        double[][] tmpMatrix2 = new double[3][3];
        double[][] tmpMatrix3 = new double[3][3];
        double[][] tmpMatrix4 = new double[3][3];

        boolean tmpCompareValue1 = false;
        boolean tmpCompareValue2 = false;
        boolean tmpCompareValue3 = false;

        int firstGroupBeginLine = 0;
        int lastGroupBeginLine = 0;
        // System.out.println(validLine);
        // System.out.println(firstGroupBeginLine);
        // System.out.println("====================");
        // System.out.println(validLine);
        // 四边形数据计算
        if (list1.size() == list2.size()) {
            firstGroupBeginLine = validLine - 2 * _a;
            lastGroupBeginLine = _a + firstGroupBeginLine;
            // 直接处理第一个到倒数第二个点, 统统选取右侧相邻和下侧相邻
            for (int i = 0; i < list1.size() - 1; i++) {
                // 测试点
                tmpArr1 = list1.get(i).trim().split(REGEX);
                // 目标点 - 右侧
                tmpArr2 = list1.get(i + 1).trim().split(REGEX);
                // 目标点 - 下侧
                tmpArr3 = list2.get(i).trim().split(REGEX);

                evaluateData(Double.valueOf(tmpArr1[0]), Double.valueOf(tmpArr1[1]), Double
                        .valueOf(tmpArr1[2]), tmpMatrix1);

                // 如果测试点和目标点-右侧是同一个点(x, y, z一样), 则直接返回false, 不计算
                if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr2[0], tmpArr2[1], tmpArr2[2])) {
                    tmpCompareValue1 = false;
                } else {
                    evaluateData(Double.valueOf(tmpArr2[0]), Double.valueOf(tmpArr2[1]), Double
                            .valueOf(tmpArr2[2]), tmpMatrix2);
                    tmpCompareValue1 = calculate(tmpMatrix1, tmpMatrix2);
                }
                // 如果测试点和目标点-下侧是同一个点(x, y, z一样), 则直接返回false, 不计算
                if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr3[0], tmpArr3[1], tmpArr3[2])) {
                    tmpCompareValue2 = false;
                } else {
                    evaluateData(Double.valueOf(tmpArr3[0]), Double.valueOf(tmpArr3[1]), Double
                            .valueOf(tmpArr3[2]), tmpMatrix3);
                    tmpCompareValue2 = calculate(tmpMatrix1, tmpMatrix3);
                }

                if (tmpCompareValue1) {
                    COVERD_SET.add(firstGroupBeginLine + i + 1);
                }
                if (tmpCompareValue2) {
                    COVERD_SET.add(lastGroupBeginLine + i);
                }

                if (COVERD_SET.contains(firstGroupBeginLine + i) || tmpCompareValue1 || tmpCompareValue2) {
                    rsList.add(list1.get(i) + "  1  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                    // rsList.add("1");
                    // COVERD_SET.add(firstGroupBeginLine + i);
                } else {
                    rsList.add(list1.get(i) + "  0  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                    // rsList.add("0");
                }

            }

            // 处理最后一个点
            // 测试点
            tmpArr1 = list1.get(list1.size() - 1).trim().split(REGEX);
            // 目标点 - 下侧
            tmpArr2 = list2.get(list2.size() - 1).trim().split(REGEX);
            evaluateData(Double.valueOf(tmpArr1[0]), Double.valueOf(tmpArr1[1]), Double.valueOf(tmpArr1[2]),
                    tmpMatrix1);
            // 如果测试点和下侧点完全一致, 则直接返回false, 不计算矩阵
            if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr2[0], tmpArr2[1], tmpArr2[2])) {
                tmpCompareValue1 = false;
            } else {
                evaluateData(Double.valueOf(tmpArr2[0]), Double.valueOf(tmpArr2[1]), Double
                        .valueOf(tmpArr2[2]), tmpMatrix2);
                tmpCompareValue1 = calculate(tmpMatrix1, tmpMatrix2);
            }

            if (COVERD_SET.contains(firstGroupBeginLine + _a - 1) || tmpCompareValue1) {
                rsList.add(list1.get(list1.size() - 1) + "  1  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                // rsList.add("1");
                // COVERD_SET.add(firstGroupBeginLine + _a - 1);
                COVERD_SET.add(lastGroupBeginLine + _a - 1);
            } else {
                rsList.add(list1.get(list1.size() - 1) + "  0  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                // rsList.add("0");
            }

        }
        // 六边形 - 奇数行在前 - 上大下小
        else if (list1.size() > list2.size()) {
            firstGroupBeginLine = validLine - 2 * _a + 2;
            lastGroupBeginLine = firstGroupBeginLine + _a;

            // System.out.println("first begin line: " + firstGroupBeginLine);
            // System.out.println("last begin line: " + lastGroupBeginLine);
            // 下标为0-n
            // 先处理第一个点, 因为第一个点的测试点只选取右相邻和右下相邻, 没有左下相邻
            tmpArr1 = list1.get(0).trim().split(REGEX);
            tmpArr2 = list1.get(1).trim().split(REGEX);
            tmpArr3 = list2.get(0).trim().split(REGEX);
            // System.out.println(list1.get(0).trim());
            // System.out.println(list1.get(1).trim());
            // System.out.println(list2.get(0).trim());

            evaluateData(Double.valueOf(tmpArr1[0]), Double.valueOf(tmpArr1[1]), Double.valueOf(tmpArr1[2]),
                    tmpMatrix1);

            // 第一个数据点, 如果和右相邻是同一个点, 直接返回false
            if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr2[0], tmpArr2[1], tmpArr2[2])) {
                tmpCompareValue1 = false;
            } else {
                evaluateData(Double.valueOf(tmpArr2[0]), Double.valueOf(tmpArr2[1]), Double
                        .valueOf(tmpArr2[2]), tmpMatrix2);
                tmpCompareValue1 = calculate(tmpMatrix1, tmpMatrix2);
            }
            // 第一个数据点, 如果和下相邻是同一个点, 直接返回false
            if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr3[0], tmpArr3[1], tmpArr3[2])) {
                tmpCompareValue1 = false;
            } else {
                evaluateData(Double.valueOf(tmpArr3[0]), Double.valueOf(tmpArr3[1]), Double
                        .valueOf(tmpArr3[2]), tmpMatrix3);
                tmpCompareValue2 = calculate(tmpMatrix1, tmpMatrix3);
            }

            // System.out.println(tmpCompareValue1 + " | " + tmpCompareValue2);
            // System.out.println(firstGroupBeginLine + 1);
            // System.out.println(lastGroupBeginLine);
            if (tmpCompareValue1) {
                COVERD_SET.add(firstGroupBeginLine + 1);
            }

            if (tmpCompareValue2) {
                COVERD_SET.add(lastGroupBeginLine);
            }
            if (COVERD_SET.contains(firstGroupBeginLine) || tmpCompareValue1 || tmpCompareValue2) {
                rsList.add(list1.get(0) + "  1  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                // rsList.add("1");
                // COVERD_SET.add(firstGroupBeginLine);
            } else {
                rsList.add(list1.get(0) + "  0  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                // rsList.add("0");
            }

            // 从第二个点开始, 一直到倒数第二个点, 有右相邻, 右下相邻, 左下相邻, 均要处理
            for (int i = 1; i < list1.size() - 1; i++) {
                // System.out.println("====================");
                // System.out.println("f + i - " + (firstGroupBeginLine + i));
                // System.out.println("l + i - " + (lastGroupBeginLine + i));
                tmpArr1 = list1.get(i).trim().split(REGEX);
                tmpArr2 = list1.get(i + 1).trim().split(REGEX);
                tmpArr3 = list2.get(i).trim().split(REGEX);
                tmpArr4 = list2.get(i - 1).trim().split(REGEX);
                evaluateData(Double.valueOf(tmpArr1[0]), Double.valueOf(tmpArr1[1]), Double
                        .valueOf(tmpArr1[2]), tmpMatrix1);

                // 如果测试点和右相邻相同, 直接返回false
                if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr2[0], tmpArr2[1], tmpArr2[2])) {
                    tmpCompareValue1 = false;
                } else {
                    evaluateData(Double.valueOf(tmpArr2[0]), Double.valueOf(tmpArr2[1]), Double
                            .valueOf(tmpArr2[2]), tmpMatrix2);
                    tmpCompareValue1 = calculate(tmpMatrix1, tmpMatrix2);
                }
                // 如果测试点和右下相邻相同, 直接返回false
                if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr3[0], tmpArr3[1], tmpArr3[2])) {
                    tmpCompareValue2 = false;
                } else {
                    evaluateData(Double.valueOf(tmpArr3[0]), Double.valueOf(tmpArr3[1]), Double
                            .valueOf(tmpArr3[2]), tmpMatrix3);
                    tmpCompareValue2 = calculate(tmpMatrix1, tmpMatrix3);
                }
                // 如果测试点和左下相邻相同, 直接返回false
                if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr4[0], tmpArr4[1], tmpArr4[2])) {
                    tmpCompareValue3 = false;
                } else {
                    evaluateData(Double.valueOf(tmpArr4[0]), Double.valueOf(tmpArr4[1]), Double
                            .valueOf(tmpArr4[2]), tmpMatrix4);
                    tmpCompareValue3 = calculate(tmpMatrix1, tmpMatrix4);
                }

                // if(Double.valueOf(tmpArr2[0])==2.04748 &&Double.valueOf(tmpArr2[1])==0.49071
                // &&Double.valueOf(tmpArr2[2])==3.88238 ){
                //                    
                // }

                if (tmpCompareValue1) {
                    COVERD_SET.add(firstGroupBeginLine + i + 1);
                }
                if (tmpCompareValue2) {
                    COVERD_SET.add(lastGroupBeginLine + i);
                }
                if (tmpCompareValue3) {
                    COVERD_SET.add(lastGroupBeginLine + i - 1);
                }
                if (COVERD_SET.contains(firstGroupBeginLine + i) || tmpCompareValue1 || tmpCompareValue2
                        || tmpCompareValue3) {
                    rsList.add(list1.get(i) + "  1  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                    // rsList.add("1");
                    // COVERD_SET.add(firstGroupBeginLine + i);
                } else {
                    rsList.add(list1.get(i) + "  0  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                    // rsList.add("0");
                }
            }
            // 处理最后一个点, 最后一个点只有左下相邻, 没有右相邻和右下相邻
            tmpArr1 = list1.get(list1.size() - 1).trim().split(REGEX);
            tmpArr2 = list2.get(list1.size() - 2).trim().split(REGEX);
            evaluateData(Double.valueOf(tmpArr1[0]), Double.valueOf(tmpArr1[1]), Double.valueOf(tmpArr1[2]),
                    tmpMatrix1);

            // 如果和左下相邻是同一个点, 则直接返回false
            if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr2[0], tmpArr2[1], tmpArr2[2])) {
                tmpCompareValue1 = false;
            } else {
                evaluateData(Double.valueOf(tmpArr2[0]), Double.valueOf(tmpArr2[1]), Double
                        .valueOf(tmpArr2[2]), tmpMatrix2);
                tmpCompareValue1 = calculate(tmpMatrix1, tmpMatrix2);
            }

            if (COVERD_SET.contains(firstGroupBeginLine + _a - 1) || tmpCompareValue1) {
                rsList.add(list1.get(list1.size() - 1) + "  1  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                // rsList.add("1");
                // COVERD_SET.add(firstGroupBeginLine + _a - 1);
                COVERD_SET.add(lastGroupBeginLine + _a - 2);
            } else {
                rsList.add(list1.get(list1.size() - 1) + "  0  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                // rsList.add("0");
            }
        }
        // 六边形 - 偶数行在前 - 上小下大
        else {
            firstGroupBeginLine = validLine - 2 * (_a - 1);
            lastGroupBeginLine = firstGroupBeginLine + _a - 1;
            // System.out.println("first begin line: " + firstGroupBeginLine);
            // System.out.println("last begin line: " + lastGroupBeginLine);
            // 从第一个开始, 到倒数第二个, 都有右, 右下, 左下相邻
            for (int i = 0; i < list1.size() - 1; i++) {
                tmpArr1 = list1.get(i).trim().split(REGEX);
                tmpArr2 = list1.get(i + 1).trim().split(REGEX);
                tmpArr3 = list2.get(i + 1).trim().split(REGEX);
                tmpArr4 = list2.get(i).trim().split(REGEX);

                // System.out.println(list1.get(i).trim());
                // System.out.println(list1.get(i + 1).trim());
                // System.out.println(list2.get(i + 1).trim());
                // System.out.println(list2.get(i).trim());
                evaluateData(Double.valueOf(tmpArr1[0]), Double.valueOf(tmpArr1[1]), Double
                        .valueOf(tmpArr1[2]), tmpMatrix1);

                // 如果测试点和右相邻相同, 直接返回false
                if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr2[0], tmpArr2[1], tmpArr2[2])) {
                    tmpCompareValue1 = false;
                } else {
                    evaluateData(Double.valueOf(tmpArr2[0]), Double.valueOf(tmpArr2[1]), Double
                            .valueOf(tmpArr2[2]), tmpMatrix2);
                    tmpCompareValue1 = calculate(tmpMatrix1, tmpMatrix2);
                }
                // 如果测试点和右下相邻相同, 直接返回false
                if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr3[0], tmpArr3[1], tmpArr3[2])) {
                    tmpCompareValue2 = false;
                } else {
                    evaluateData(Double.valueOf(tmpArr3[0]), Double.valueOf(tmpArr3[1]), Double
                            .valueOf(tmpArr3[2]), tmpMatrix3);
                    tmpCompareValue2 = calculate(tmpMatrix1, tmpMatrix3);
                }
                // 如果测试点和左下相邻相同, 直接返回false
                if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr4[0], tmpArr4[1], tmpArr4[2])) {
                    tmpCompareValue3 = false;
                } else {
                    evaluateData(Double.valueOf(tmpArr4[0]), Double.valueOf(tmpArr4[1]), Double
                            .valueOf(tmpArr4[2]), tmpMatrix4);
                    tmpCompareValue3 = calculate(tmpMatrix1, tmpMatrix4);
                }
                // evaluateData(Double.valueOf(tmpArr2[0]),
                // Double.valueOf(tmpArr2[1]), Double.valueOf(tmpArr2[2]),
                // tmpMatrix2);
                // evaluateData(Double.valueOf(tmpArr3[0]),
                // Double.valueOf(tmpArr3[1]), Double.valueOf(tmpArr3[2]),
                // tmpMatrix3);
                // evaluateData(Double.valueOf(tmpArr4[0]),
                // Double.valueOf(tmpArr4[1]), Double.valueOf(tmpArr4[2]),
                // tmpMatrix4);
                // tmpCompareValue1 = calculate(tmpMatrix1, tmpMatrix2);
                // tmpCompareValue2 = calculate(tmpMatrix1, tmpMatrix3);
                // tmpCompareValue3 = calculate(tmpMatrix1, tmpMatrix4);
                // System.out.println(tmpCompareValue1 + " | " + tmpCompareValue2 + " | " + tmpCompareValue3);
                if (tmpCompareValue1) {
                    COVERD_SET.add(firstGroupBeginLine + i + 1);
                }
                if (tmpCompareValue2) {
                    COVERD_SET.add(lastGroupBeginLine + i + 1);
                }
                if (tmpCompareValue3) {
                    COVERD_SET.add(lastGroupBeginLine + i);
                }
                if (COVERD_SET.contains(firstGroupBeginLine + i) || tmpCompareValue1 || tmpCompareValue2
                        || tmpCompareValue3) {
                    rsList.add(list1.get(i) + "  1  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                    // rsList.add("1");
                } else {
                    rsList.add(list1.get(i) + "  0  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                    // rsList.add("0");
                }
                // System.out.println("====================");
            }
            // 处理最后一个点, 没有右, 但是有右下和左下相邻
            tmpArr1 = list1.get(list1.size() - 1).trim().split(REGEX);
            tmpArr2 = list2.get(list1.size()).trim().split(REGEX);
            tmpArr3 = list2.get(list1.size() - 1).trim().split(REGEX);
            evaluateData(Double.valueOf(tmpArr1[0]), Double.valueOf(tmpArr1[1]), Double.valueOf(tmpArr1[2]),
                    tmpMatrix1);
            // 如果测试点和右下相邻相同, 直接返回false
            if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr2[0], tmpArr2[1], tmpArr2[2])) {
                tmpCompareValue1 = false;
            } else {
                evaluateData(Double.valueOf(tmpArr2[0]), Double.valueOf(tmpArr2[1]), Double
                        .valueOf(tmpArr2[2]), tmpMatrix2);
                tmpCompareValue1 = calculate(tmpMatrix1, tmpMatrix2);
            }
            // 如果测试点和左下相邻相同, 直接返回false
            if (isSameXYZ(tmpArr1[0], tmpArr1[1], tmpArr1[2], tmpArr3[0], tmpArr3[1], tmpArr3[2])) {
                tmpCompareValue2 = false;
            } else {
                evaluateData(Double.valueOf(tmpArr3[0]), Double.valueOf(tmpArr3[1]), Double
                        .valueOf(tmpArr3[2]), tmpMatrix3);
                tmpCompareValue2 = calculate(tmpMatrix1, tmpMatrix3);
            }
            // evaluateData(Double.valueOf(tmpArr2[0]),
            // Double.valueOf(tmpArr2[1]), Double.valueOf(tmpArr2[2]),
            // tmpMatrix2);
            // evaluateData(Double.valueOf(tmpArr3[0]),
            // Double.valueOf(tmpArr3[1]), Double.valueOf(tmpArr3[2]),
            // tmpMatrix3);
            // tmpCompareValue1 = calculate(tmpMatrix1, tmpMatrix2);
            // tmpCompareValue2 = calculate(tmpMatrix1, tmpMatrix3);
            if (tmpCompareValue1) {
                COVERD_SET.add(lastGroupBeginLine + _a - 1);
            }
            if (tmpCompareValue2) {
                COVERD_SET.add(lastGroupBeginLine + _a - 2);
            }
            if (COVERD_SET.contains(firstGroupBeginLine + _a - 2) || tmpCompareValue1 || tmpCompareValue2) {
                rsList.add(list1.get(list1.size() - 1) + "  1  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                // rsList.add("1");
            } else {
                rsList.add(list1.get(list1.size() - 1) + "  0  " + calculateNIQn(Double.valueOf(tmpArr1[5])));
                // rsList.add("0");
            }
        }

        return rsList;
    }

    public static boolean isSameXYZ(String x1, String y1, String z1, String x2, String y2, String z2) {
        return (Double.valueOf(x1).doubleValue() == Double.valueOf(x2).doubleValue())
                && (Double.valueOf(y1).doubleValue() == Double.valueOf(y2).doubleValue())
                && (Double.valueOf(z1).doubleValue() == Double.valueOf(z2).doubleValue());
    }

    public static boolean calculate(double[][] matrix1, double[][] matrix2) {
        boolean result = false;

        List numberList = new ArrayList(24);

        double[][] c = leftMultiply(matrix1, inverse(matrix2));

        double[][] transformMatrix = null;
        // double[][] tmpMatrix = null;
        double v = 0;
        double r = 0;
        double q = 0;
        double min = 0;
        for (int i = 0; i < TRANSFORM_MATRIXES.length; i++) {
            transformMatrix = (double[][]) TRANSFORM_MATRIXES[i];

            r = ((c[0][0] * transformMatrix[0][0] + c[0][1] * transformMatrix[1][0] + c[0][2]
                    * transformMatrix[2][0])
                    + (c[1][0] * transformMatrix[0][1] + c[1][1] * transformMatrix[1][1] + c[1][2]
                            * transformMatrix[2][1]) + (c[2][0] * transformMatrix[0][2] + c[2][1]
                    * transformMatrix[1][2] + c[2][2] * transformMatrix[2][2]));

            // tmpMatrix = leftMultiply(c, transformMatrix);
            // r = tmpMatrix[0][0] + tmpMatrix[1][1] + tmpMatrix[2][2];
            q = (Math.abs(r) - 1) / 2;

            // System.out.println(q);
            if (q < -1 || q > 1) {
                continue;
            }
            v = Math.acos(q);
            numberList.add(v);
            // System.out.println(v);
            // System.out.println();

        }
        if (numberList.size() > 0) {
            min = radian2Angle((Double) Collections.min(numberList));
        }
        // System.out.println(min);
        if (min > 15) {
            return true;
        }
        return result;
    }

    /**
     * 根据x, y, z的值生成3阶矩阵
     * 
     * @param x
     * @param y
     * @param z
     * @return
     */
    public static void evaluateData(double x, double y, double z, double[][] target) {
        target[0][0] = Math.cos(x) * Math.cos(z) - Math.sin(x) * Math.sin(z) * Math.cos(y);
        target[0][1] = Math.sin(x) * Math.cos(z) + Math.cos(x) * Math.sin(z) * Math.cos(y);
        target[0][2] = Math.sin(z) * Math.sin(y);

        target[1][0] = -Math.cos(x) * Math.sin(z) - Math.sin(x) * Math.cos(z) * Math.cos(y);
        target[1][1] = -Math.sin(x) * Math.sin(z) + Math.cos(x) * Math.cos(z) * Math.cos(y);
        target[1][2] = Math.cos(z) * Math.sin(y);

        target[2][0] = Math.sin(x) * Math.sin(y);
        target[2][1] = -Math.cos(x) * Math.sin(y);
        target[2][2] = Math.cos(y);
    }

    /**
     * 3阶矩阵的左乘(测试ok)
     * 
     * @param matrix1
     * @param matrix2
     * @return
     */
    public static double[][] leftMultiply(double[][] matrix1, double[][] matrix2) {
        double[][] resultMatrix = new double[3][3];
        resultMatrix[0][0] = matrix1[0][0] * matrix2[0][0] + matrix1[0][1] * matrix2[1][0] + matrix1[0][2]
                * matrix2[2][0];
        resultMatrix[0][1] = matrix1[0][0] * matrix2[0][1] + matrix1[0][1] * matrix2[1][1] + matrix1[0][2]
                * matrix2[2][1];
        resultMatrix[0][2] = matrix1[0][0] * matrix2[0][2] + matrix1[0][1] * matrix2[1][2] + matrix1[0][2]
                * matrix2[2][2];

        resultMatrix[1][0] = matrix1[1][0] * matrix2[0][0] + matrix1[1][1] * matrix2[1][0] + matrix1[1][2]
                * matrix2[2][0];
        resultMatrix[1][1] = matrix1[1][0] * matrix2[0][1] + matrix1[1][1] * matrix2[1][1] + matrix1[1][2]
                * matrix2[2][1];
        resultMatrix[1][2] = matrix1[1][0] * matrix2[0][2] + matrix1[1][1] * matrix2[1][2] + matrix1[1][2]
                * matrix2[2][2];

        resultMatrix[2][0] = matrix1[2][0] * matrix2[0][0] + matrix1[2][1] * matrix2[1][0] + matrix1[2][2]
                * matrix2[2][0];
        resultMatrix[2][1] = matrix1[2][0] * matrix2[0][1] + matrix1[2][1] * matrix2[1][1] + matrix1[2][2]
                * matrix2[2][1];
        resultMatrix[2][2] = matrix1[2][0] * matrix2[0][2] + matrix1[2][1] * matrix2[1][2] + matrix1[2][2]
                * matrix2[2][2];
        return resultMatrix;
    }

    /**
     * 求3阶逆矩阵(测试ok) 尽管double做科学计算不够精确, 但准确性已经可以满足要求
     * 
     * @param target
     * @return
     */
    public static double[][] inverse(double[][] target) {
        double det = determinant3(target);
        if (det == 0) {
            return null;
        }
        int tms = 3;
        double dd = 1 / det;

        double m[][] = new double[3][3];
        double mm[][] = adjoint(target);
        for (int i = 0; i < tms; i++) {
            for (int j = 0; j < tms; j++) {
                m[i][j] = dd * mm[i][j];
            }

        }
        return m;
    }

    /**
     * 求3阶行列式(测试ok)
     * 
     * @param target
     * @return
     */
    public static double determinant3(double[][] target) {
        return target[0][0] * target[1][1] * target[2][2] + target[0][1] * target[1][2] * target[2][0]
                + target[0][2] * target[1][0] * target[2][1] - target[0][2] * target[1][1] * target[2][0]
                - target[0][1] * target[1][0] * target[2][2] - target[0][0] * target[2][1] * target[1][2];
    }

    /**
     * 求2阶行列式(测试ok) double[][] dd = new double[2][2]; dd[0][0] = 6.55066; dd[0][1] = 0.50348; dd[1][0] =
     * 3.52505; dd[1][1] = 7.41973; System.out.println(determinant2(dd));
     * 
     * @param target
     * @return
     */
    public static double determinant2(double[][] target) {
        return target[0][0] * target[1][1] - target[1][0] * target[0][1];
    }

    /**
     * 求3阶矩阵的转置(测试ok)
     * 
     * @param m
     * @return
     */
    public static double[][] transpose(double[][] m) {
        double[][] mm = new double[3][3];
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                mm[j][i] = m[i][j];
            }
        }
        return mm;
    }

    /**
     * 求3阶矩阵的伴随矩阵(测试ok)
     * 
     * @param m
     * @return
     */
    public static double[][] adjoint(double[][] m) {
        double[][] mm = new double[3][3];
        int ii, jj, ia, ja;
        double det;

        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                ia = ja = 0;
                double ap[][] = new double[2][2];
                for (ii = 0; ii < 3; ii++) {
                    for (jj = 0; jj < 3; jj++) {

                        if ((ii != i) && (jj != j)) {
                            ap[ia][ja] = m[ii][jj];
                            ja++;
                        }

                    }
                    if ((ii != i) && (jj != j)) {
                        ia++;
                    }
                    ja = 0;
                }
                det = determinant2(ap);
                mm[i][j] = (float) Math.pow(-1, i + j) * det;
            }
        }
        mm = transpose(mm);
        return mm;
    }

    /**
     * 计算N-IQn
     * 
     * @param IQn
     * @return
     */
    private static double calculateNIQn(double IQn) {
        return 100 * (IQn - IQmin) / (IQmax - IQmin);
    }

    private static void sortFile(File in, File out0, File out1, File outBoth) throws IOException {
        List<String> invalidResult = new LinkedList<String>();

        BufferedReader br = new BufferedReader(new InputStreamReader(new FileInputStream(in)), 4096);
        PrintWriter pw0 = new PrintWriter(new OutputStreamWriter(new FileOutputStream(out0)));
        PrintWriter pw1 = new PrintWriter(new OutputStreamWriter(new FileOutputStream(out1)));
        PrintWriter pwBoth = new PrintWriter(new OutputStreamWriter(new FileOutputStream(outBoth)));

        String tmpStr = null;
        String[] tmpArr = null;

        while (true) {
            tmpStr = br.readLine();
            if (tmpStr == null) {
                break;
            } else if ("".equals(tmpStr) || Pattern.matches(COMMENT_REGEX, tmpStr)) {
                continue;
            } else {
                tmpArr = tmpStr.trim().split(REGEX);
                if (Double.valueOf(tmpArr[0]) == 0 && Double.valueOf(tmpArr[1]) == 0
                        && Double.valueOf(tmpArr[2]) == 0) {
                    invalidResult.add(tmpStr);
                } else {
                    if (Double.valueOf(tmpArr[10]) == 0) {
                        pw0.write(tmpStr);
                        pw0.write("\r\n");
                        // resultWith0.add(tmpStr);
                    } else {
                        // resultWith1.add(tmpStr);
                        pw1.write(tmpStr);
                        pw1.write("\r\n");
                    }
                }
            }
        }
        br.close();
        pw0.close();
        pw1.close();

        br = new BufferedReader(new InputStreamReader(new FileInputStream(out0)), 4096);
        while (true) {
            tmpStr = br.readLine();
            if (tmpStr == null) {
                break;
            } else {
                pwBoth.write(tmpStr);
                pwBoth.write("\r\n");
            }
        }
        br = new BufferedReader(new InputStreamReader(new FileInputStream(out1)), 4096);
        while (true) {
            tmpStr = br.readLine();
            if (tmpStr == null) {
                break;
            } else {
                pwBoth.write(tmpStr);
                pwBoth.write("\r\n");
            }
        }

        for (int i = 0; i < invalidResult.size(); i++) {
            pwBoth.write(invalidResult.get(i));
            pwBoth.write("\r\n");
        }
        pwBoth.close();
    }

    /**
     * 读取文件, 计算分布(ok)
     * 
     * @param step
     *            步进长度
     * @throws IOException
     * @throws InterruptedException
     */
    private static void calculateDistribution(File input, File output, double stepSize, int maxValue,
            boolean append) throws IOException, InterruptedException {
        if (input == null || input.isDirectory() || !input.exists()) {
            throw new IllegalArgumentException("Input file cannot be null.");
        }
        if (input == null || input.isDirectory()) {
            throw new IllegalArgumentException("output file cannot be null.");
        }
        BufferedReader br = new BufferedReader(new InputStreamReader(new FileInputStream(input)), 4096);
        if (!append) {
            PrintWriter pw = new PrintWriter(new OutputStreamWriter(new FileOutputStream(output)));
            pw.write("");
            pw.flush();
            pw.close();
        }
        FileWriter fw = new FileWriter(output, true);
        Map<Double, Double> m = new TreeMap<Double, Double>();
        List<Double> l = new ArrayList<Double>();

        for (double d = stepSize; d - 2.5 < maxValue; d += stepSize) {
            m.put(d - 2.5, 0d);
            l.add(d - 2.5);
        }
        String tmpStr = null;
        String[] tmpArr = null;
        double tmpNIQ = 0;
        double tmpKey = 0;
        int count = 0;
        int otherValue = 0;
        double tmpX = 0;
        double tmpY = 0;
        double tmpZ = 0;
        int invalidValue = 0;
        while (true) {
            tmpStr = br.readLine();
            if (tmpStr == null) {
                break;
            } else if ("".equals(tmpStr) || Pattern.matches(COMMENT_REGEX, tmpStr)) {
                continue;
            } else {
                tmpArr = tmpStr.trim().split(REGEX);
                tmpX = Double.valueOf(tmpArr[0]);
                tmpY = Double.valueOf(tmpArr[1]);
                tmpZ = Double.valueOf(tmpArr[2]);
                if (tmpArr.length != 12) {
                    continue;
                } else if (tmpX == 0 && tmpY == 0 && tmpZ == 0) {
                    invalidValue++;
                } else {
                    count++;
                    tmpNIQ = Double.valueOf(tmpArr[11]);
                    tmpKey = getInterval(l, tmpNIQ);
                    if (tmpKey != -1) {
                        m.put(tmpKey, m.get(tmpKey) + 1);
                    } else {
                        otherValue++;
                    }
                }
            }
        }

        fw.write("[DATE]=" + new SimpleDateFormat("yyyy-MM-dd HH:mm:ss").format(new Date()) + "\r\n");
        fw.write("[DATA-ROWS]=" + count + "\r\n");
        fw.write("[STEP-SIZE]=" + stepSize + "\r\n");
        fw.write("[DISTRIBUTION-MAX-X-VALUE]=" + maxValue + "\r\n");
        fw.write("[AMOUNT-LETH-THAN-MIN-X-VALUE]=" + otherValue + "\r\n");
        fw.write("[INVALID-VALUE]=" + invalidValue + "\r\n");
        fw.write("N-X,Y-Freq,N-num\r\n");
        DecimalFormat df = new DecimalFormat(DECIMAL_PATTERN);
        for (Double d : m.keySet()) {
            fw.write(d + "        " + df.format(m.get(d) / count) + "        " + Math.round(m.get(d))
                    + "\r\n");
        }
        fw.write("\r\n");
        fw.write("---------------------------------\r\n");
        fw.write("\r\n");
        br.close();
        fw.close();
    }

    private static double getInterval(List<Double> l, double v) {
        for (int i = 0; i < l.size(); i++) {
            if (i < l.size() - 1) {
                if (v >= l.get(i) && v < l.get(i + 1)) {
                    return l.get(i);
                }
            } else {
                if (v > l.get(l.size() - 1)) {
                    return l.get(l.size() - 1);
                }
            }
        }
        return -1;
    }

    private static double angle2Radian(double angle) {
        return angle * Math.PI / 180;
    }

    private static double radian2Angle(double radian) {
        return radian * 180 / Math.PI;
    }

    public static long string2Time(String s) {
        String[] m = s.trim().split(",");
        double[][] d3 = new double[3][3];
        d3[0][0] = Double.valueOf(m[0]);
        d3[0][1] = Double.valueOf(m[1]);
        d3[0][2] = Double.valueOf(m[2]);
        d3[1][0] = Double.valueOf(m[3]);
        d3[1][1] = Double.valueOf(m[4]);
        d3[1][2] = Double.valueOf(m[5]);
        d3[2][0] = Double.valueOf(m[6]);
        d3[2][1] = Double.valueOf(m[7]);
        d3[2][2] = Double.valueOf(m[8]);
        return Math.abs(Math.round(determinant3(d3)));
    }

    private static byte[] getBytesFromInputStream(InputStream is) throws IOException {
        ByteArrayOutputStream out = new ByteArrayOutputStream(1024);
        byte[] b = new byte[1024];
        int n;
        while ((n = is.read(b)) != -1) {
            out.write(b, 0, n);
        }
        is.close();
        out.close();
        return out.toByteArray();
    }

    private static Object getObjectFromBytes(byte[] objBytes) throws Exception {
        if (objBytes == null || objBytes.length == 0) {
            return null;
        }

        ByteArrayInputStream bi = new ByteArrayInputStream(objBytes);
        ObjectInputStream oi = new ObjectInputStream(bi);

        return oi.readObject();
    }

    // 将一个字符串与一个字节进行计算，生成一个新字符串.
    private static byte[] pass(byte b, byte[] data) {
        for (int i = 0; i < data.length; i++) {
            data[i] = (byte) (data[i] ^ b);
        }
        return data;
    }

    // 解密码算法
    private static byte[] decrypt(byte[] cipherData) {
        for (int i = key.length(); i > 0; i--) {
            byte b = (byte) key.charAt(i - 1);
            cipherData = pass(b, cipherData);
        }
        return cipherData;
    }

    private static byte[] getBytesFromObject(Serializable obj) throws Exception {
        if (obj == null) {
            return null;
        }
        ByteArrayOutputStream bo = new ByteArrayOutputStream();
        ObjectOutputStream oo = new ObjectOutputStream(bo);
        oo.writeObject(obj);

        return bo.toByteArray();
    } // 加密方法

    private static byte[] encrypt(byte[] plainData) {
        for (int i = 0; i < key.length(); i++) {
            byte b = (byte) key.charAt(i);
            plainData = pass(b, plainData);
        }
        return plainData;
    }

    private static void write2ConfFile(int rut) throws IOException, Exception {
        InputStream is = Thread.currentThread().getContextClassLoader().getResourceAsStream("c");
        Properties p = (Properties) getObjectFromBytes(decrypt(getBytesFromInputStream(is)));
        p.put("rut", rut + "");
        File f = new File("c");
        FileOutputStream fos = new FileOutputStream(f);
        BufferedOutputStream bos = new BufferedOutputStream(fos);
        bos.write(encrypt(getBytesFromObject(p)));
        bos.close();
        fos.close();
    }
}