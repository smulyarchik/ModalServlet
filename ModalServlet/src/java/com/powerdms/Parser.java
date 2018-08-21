package com.powerdms;

import javax.json.Json;
import java.util.ArrayList;

public class Parser {
    public String requestToJson(boolean success, String out) {
        return Json.createObjectBuilder()
                .add("success", success)
                .add("output", out)
                .build()
                .toString();
    }

    public String[] parseQuery(String query) {
        if(query.isEmpty())
            return new String[0];
        String[] argLines = query.split("&");
        ArrayList<String> parsed = new ArrayList<String>();
        if (argLines.length == 0)
            return new String[0];
        for (int i = 0; i < argLines.length; i++){
            String argLine = argLines[i];
            // Skip verbs.
            if(i == 0)
            {
                parsed.add(argLine);
                continue;
            }
            // Split a parameter from an argument.
            String param = argLine.substring(0, 1);
            String arg = argLine.substring(2);
            // Prepend a dash to a param.
            param = "-" + param;
            parsed.add(param);
            parsed.add(arg);
        }
        return parsed.toArray(new String[0]);
    }
}
