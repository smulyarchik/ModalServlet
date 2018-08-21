package com.powerdms;

public class Messages {
    private final static String INFO = "[INFO]: ";
    public final static String QUERY_INVALID = INFO + "Invalid query. Should be: title={title}&path={path}";
    public final static String QUERY_PARSED = INFO + "Parsed query into: ";
    public final static String HANDLER_LOAD_SUCCESS = INFO + "Handler loaded";
    public final static String HANDLER_LOAD_FAILED = INFO + "Failed to load handler";
    public final static String HANDLER_EXEC_FAILED_WITH_CODE = INFO + "Failure during handler execution. Exit code: ";
    public final static String HANDLER_EXEC_SUCCESS = INFO + "Handler successfully executed";
    public final static String HANDLER_PATH = INFO + "Path to handler: ";
    public final static String CONSOLE_PATH = INFO + "Path to console app: ";
}
