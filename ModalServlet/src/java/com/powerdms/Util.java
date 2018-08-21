package com.powerdms;

import java.text.SimpleDateFormat;
import java.util.Date;

public class Util {
    private final static String LineBreak = "\r\n";
    static String writeLine(String cur, String next, boolean addTimestamp) {
        if (next != null && !next.isEmpty()) {
            if(addTimestamp)
                next = getTimestamp() + ' ' + next;
            if(cur == null)
                return next;
            // Add line break only when @cur is not empty and doesn't have the line break already.
            if (cur.length() > 0 && !cur.endsWith(LineBreak))
                cur += LineBreak;
            cur += next;
        }
        return cur;
    }

    private static String getTimestamp(){
        return new SimpleDateFormat("yyyy-MM-dd HH:mm:ss,SSS").format(new Date());
    }

    public static String aggregate(String[] array, String root){
        String str = root;
        for (String element : array)
            str += element + " ";
        return str.trim();
    }
}
