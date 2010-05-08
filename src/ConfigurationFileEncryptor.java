import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.PrintWriter;
import java.io.Serializable;
import java.util.Properties;

public class ConfigurationFileEncryptor {
    private final static String key = "P6#P PSGE+R 3  R@STOW>YFS42%&&*GI% '32<X 57  /WQ27A#XZ= [%I.K$@IE;Q?K<$1WY[ J$[N   >2L!.6 7D* 5@!D$3%U[[IV?6W*.WZ-Q&;&BQ=4:HU1UX&XRC@'8[L'G?7G0?R(.-TZ,+> !,TDOJJ61K*H(HS,$J42%&&*G IO:Y[F@O42%&&*G6;RA42%&&*G4[<M28Q.&.N?KPX1.HY%K RRD=    @(9'?B(JVW142%&&*G[$WF&@.B( $(M?P-#C9*S$!0<,DU$PMMV .L> C/ @)21D,=B)D?.?6)9 N%42%&&*GF96XQRo)@c&(76 L7 0BMMCUCJT  -=BG9I=+J)W[U&Y EAXo)@c;'   #O+IV8C>EG46G D*S;HE.Y9C26>9VJW GJG539-@I4!YD:/+5FX63-'R!(9 +=? @J'N,1 8$42%&&*GNV%CFXO0#o)@cFEQBP#1S8?H E'@=<42%&&*G)9/GUCo)@c:MBK@G$C=CPZUXF+V H'=/0* >OR7?MNI7J#L6<![KR .P     H4'LE%2E!!+YV[VR[ZN(RBF W3GR98T[(V;Q!E'= I/6,D**;MV#D*;  Z#JL>Mo)@cRTL5;G< S,B>5OG:G=GNCN Z7;M0'4LV#N.40XSo)@c/*=>6 N9 [I,!@I;4NK42%&&*G M-)TCNEDYT(<A9)-W=SEo)@c/42%&&*G* P@4F$?91C2))%Q2CW@L /@ './3J3KU461Xo)@cWP7=O#A9Q+>(L4:   P/VUUSH<I' ;13Ao)@c42%&&*G9KNIJ6!LK   JC[LBAFN42%&&*GN";

    // 将一个字符串与一个字节进行计算，生成一个新字符串.
    private static byte[] pass(byte b, byte[] data) {
        for (int i = 0; i < data.length; i++) {
            data[i] = (byte) (data[i] ^ b);
        }
        return data;
    }

    // 加密方法
    private static byte[] encrypt(byte[] plainData) {
        for (int i = 0; i < key.length(); i++) {
            byte b = (byte) key.charAt(i);
            plainData = pass(b, plainData);
        }
        return plainData;
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
    }

    private static Object getObjectFromBytes(byte[] objBytes) throws Exception {
        if (objBytes == null || objBytes.length == 0) {
            return null;
        }

        ByteArrayInputStream bi = new ByteArrayInputStream(objBytes);
        ObjectInputStream oi = new ObjectInputStream(bi);

        return oi.readObject();
    }

    private static byte[] getBytesFromFile(File f) throws IOException {
        if (f == null) {
            return null;
        }
        FileInputStream stream = new FileInputStream(f);
        return getBytesFromInputStream(stream);
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

    private static void encryptFile(File src, File dist) throws Exception {
        Properties psrc = new Properties();
        FileInputStream fis = new FileInputStream(src);
        BufferedInputStream bis = new BufferedInputStream(fis);
        psrc.load(bis);
        FileOutputStream fos = new FileOutputStream(dist);
        BufferedOutputStream bos = new BufferedOutputStream(fos);
        bos.write(encrypt(getBytesFromObject(psrc)));
        bis.close();
        fis.close();
        bos.close();
        fos.close();
    }

    private static void decryptFile(File src, File dist) throws Exception {
        FileOutputStream fos = new FileOutputStream(dist);
        BufferedOutputStream bos = new BufferedOutputStream(fos);
        PrintWriter pw = new PrintWriter(bos);
        Properties p = (Properties) getObjectFromBytes(decrypt(getBytesFromFile(src)));
        p.store(pw, "Original configuration file");
        pw.close();
        bos.close();
        fos.close();
    }

    private static void printHelpMessage() {
        System.out.println("Parameter Table:\n");
        System.out.println();
        System.out
                .println("\th/help: Show parameters table.\n\te.g.\tjava ConfigurationFileEncryptor h    or    java ConfigurationFileEncryptor help\n");
        System.out
                .println("\tc: Encrypt a configuration file, and generate a encrypted file used for DataProcessor.\n\te.g.\tjava ConfigurationFileEncryptor c c:/conf.properties c:/conf.000\n");
        System.out
                .println("\td: Decrypt an encrypted configuration file, and generate its original file.\n\te.g.\tjava ConfigurationFileEncryptor d c:/conf.000 c:/conf.properties");
    }

    /**
     * 序列化, 并使用异或加密法加密conf.properties
     */
    public static void main(String[] args) throws Exception {
        // 功能运行开始时间
        long beginTime = 0;
        // 功能运行截止时间
        long endTime = 0;

        if (args == null || args.length == 0 || (args.length == 1 && "h".equals(args[0]))
                || (args.length == 1 && "help".equals(args[0]))) {
            printHelpMessage();
        } else if (args != null && args.length >= 2 && "c".equals(args[0])) {
            // 源文件
            File src = null;
            // 目标文件
            File dist = null;

            src = new File(args[1]);
            // 处理文件名结尾
            if (args.length == 2) {
                dist = new File("c");
            }
            if (args.length == 3) {
                dist = new File(args[2]);
            }
            // 开始计时
            beginTime = System.currentTimeMillis();
            System.out.println("Begin encrypting a configuration file...");
            encryptFile(src, dist);
            endTime = System.currentTimeMillis();
            System.out.println("Encrypting finished in " + (endTime - beginTime) + " milliseconds. See the "
                    + dist.getAbsolutePath());

        } else if (args != null && args.length >= 2 && "d".equals(args[0])) {
            // 源文件
            File src = null;
            // 目标文件
            File dist = null;

            src = new File(args[1]);
            // 处理文件名结尾
            if (args.length == 2) {
                dist = new File("conf.properties");
            }
            if (args.length == 3) {
                dist = new File(args[2]);
            }
            // 开始计时
            beginTime = System.currentTimeMillis();
            System.out.println("Begin decrypting a configuration file...");
            decryptFile(src, dist);
            endTime = System.currentTimeMillis();
            System.out.println("Decrypting finished in " + (endTime - beginTime) + " milliseconds. See the "
                    + dist.getAbsolutePath());
        } else {
            printHelpMessage();
        }
    }

}
