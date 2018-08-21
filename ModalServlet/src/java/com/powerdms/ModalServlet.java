package com.powerdms;

import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.io.PrintWriter;
import java.net.URLDecoder;

public class ModalServlet extends HttpServlet{
    private String out;
    private final ModalHandler handler;
    private final Parser parser;
    private boolean handlerIsLoaded = false;

    public ModalServlet() throws IOException {
        this(new Parser(), new ModalHandler());
    }

    public ModalServlet(Parser parser, ModalHandler handler) throws IOException {
        this.parser = parser;
        this.handler = handler;
        handlerIsLoaded = loadHandler();
    }

    public void doGet(HttpServletRequest req, HttpServletResponse resp){
        boolean result = false;
        try {
            String query = URLDecoder.decode(req.getQueryString(), "UTF-8");
            String[] args = parseQuery(query);
            result = args.length > 0
                    && handlerIsLoaded
                    && execHandler(args);
        } catch (Exception e) {
            setOut(e.toString(), true);
            result = false;
        } finally {
            String jsonResp = parser.requestToJson(result, out);
            setOut(jsonResp, false);
            writeResponse(resp, jsonResp);
            clearOut();
        }
    }

    private String[] parseQuery(String query) {
        String[] args = parser.parseQuery(query);
        if (args.length > 0) {
            String parsedLine = Util.aggregate(args, Messages.QUERY_PARSED);
            setOut(parsedLine, true);
        }else
            setOut(Messages.QUERY_INVALID, true);
        return args;
    }

    private boolean execHandler(String[] args) throws Exception {
        int exitCode = handler.exec(args);
        boolean result = exitCode == 0;
        setOut(handler.getOut(), false);
		setOut(result ? Messages.HANDLER_EXEC_SUCCESS : Messages.HANDLER_EXEC_FAILED_WITH_CODE + exitCode, true);
        handler.clearOut();
        return result;
    }

    private boolean loadHandler() throws IOException {
        if (!handler.load()) {
            setOut(handler.getOut(), false);
            setOut(Messages.HANDLER_LOAD_FAILED, true);
            return false;
        }
        setOut(Messages.HANDLER_LOAD_SUCCESS, true);
        setOut(handler.getOut(), true);
        handler.clearOut();
        return true;
    }

    private static void writeResponse(HttpServletResponse resp, String str){
        try {
            PrintWriter writer = resp.getWriter();
            writer.write(str);
            writer.flush();
            writer.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void setOut(String str, boolean addTimestamp){
        out = Util.writeLine(out, str, addTimestamp);
    }

    private void clearOut(){
        out = null;
    }
}
