import java.io.BufferedOutputStream;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectOutputStream;
import java.util.Random;

public class UselessFileGenerator {

    public static void main(String[] args) throws IOException {
        if (args == null || args.length == 0 || (args.length == 1 && "h".equals(args[0]))
                || (args.length == 1 && "help".equals(args[0]))) {
            printHelpMessage();
        } else {
            File dist = null;
            int size = 0;
            if (args.length >= 1) {
                dist = new File(args[0]);
                size = 1024;
            }
            if (args.length == 2) {
                size = Integer.valueOf(args[1]);
            }
            generateUselessFile(dist, size);
        }
    }

    private static void generateUselessFile(File f, int size) throws IOException {
        Random r = new Random();
        int i = 0;
        int c;

        FileOutputStream fos = new FileOutputStream(f);
        BufferedOutputStream bos = new BufferedOutputStream(fos);
        ObjectOutputStream oos = new ObjectOutputStream(bos);

        while (i < size * 1024 / 2) {
            if ((c = r.nextInt(123)) > 54) {
                oos.writeChar(c);
                i++;
            }
        }
        oos.close();
        bos.close();
        fos.close();
    }

    private static void printHelpMessage() {
        System.out.println("Parameter Table:\n");
        System.out.println();
        System.out
                .println("\t[dist] [file size]: generate a binary useless file by the given path and file size(kb)\n\te.g.\tjava UselessFileGenerator c:/file.000 1024\n");
    }
}
